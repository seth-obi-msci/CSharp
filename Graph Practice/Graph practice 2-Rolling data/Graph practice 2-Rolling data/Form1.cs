using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Graph_practice_2_Rolling_data
{
    public partial class RollingGraph : Form
    {
        // Starting time in millisecs. 
        int tickStart = 0;

        public RollingGraph()
        {
            InitializeComponent();
        }


        private void RollingGraph_Load(object sender, EventArgs e)
        {
           CreateGraph(zgc);
            Console.WriteLine("Load");
        }

        //creates an array of data to simulate data from experiment, dont think this is used later
        private double[] Data()
        {
            int t = (Environment.TickCount - tickStart);
            double[] Values = new double[1000];
            for(int i=0; i < Values.Length;i++)
                Values[i] = i;
            return Values;
        }
        

        private void CreateGraph(ZedGraph.ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;
            myPane.Title.Text = "Plotting arrays of bytes";

            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            // New RollingPairList with 1200 values
            RollingPointPairList list1 = new RollingPointPairList(1200);

            //Make a new curve
            LineItem curve = myPane.AddCurve("Counts", list1, Color.DarkGoldenrod, SymbolType.None);

            //Timer fort the X axis, defined later
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps (DONT UNDERSTAND)
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 30;
            myPane.XAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 5;

            //Scale axis
            zgc.AxisChange();

            //Save begging tiem for reference (?)
            tickStart = Environment.TickCount;
            Console.WriteLine("Create Graph");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int t = (Environment.TickCount - tickStart);
                //Read bytes, gives array of bytes, the data.
           
            Console.WriteLine("t at tick is {0}", t);   
              byte[] Data = new byte[32];
              byte[] UART_Buffer = FPGA.ReadBytes();
              int UART_Length = UART_Buffer.Length;
              int Data_Length = Data.Length;

             
            //Most recent UART buffer bytes added to first positions in Data array
              for (int i = 0; i < UART_Length; i++)
              {
                  Data[i] = UART_Buffer[i];
                  
              }

              //Loop shifts all values in Data array up by UART buffer size, 
              //except oldest values, which are overwritten
              for (int j = Data_Length; j > UART_Length; j--)
              {
                  Data[j] = Data[j - UART_Length];
              }

              for (int k = 0; k < Data.Length; k++)
              {
                  Console.WriteLine(Data[k]);
              }
             
                Console.WriteLine("Values in Data array {0}",Data.Length);
 
                // Ensures there's at least one curve in GraphPane
                if (zgc.GraphPane.CurveList.Count <= 0)
                    return;

                LineItem curve = zgc.GraphPane.CurveList[0] as LineItem;
                if (curve == null)
                    return;

                //Get the list of points from the selected curve
                IPointListEdit list = curve.Points as IPointListEdit;
                // If list is null then reference at curve.Points doesn't
                // support IPointPairList and can't be modified
                if (list == null)
                    return;
                
                // Think this gives time in milliseconds
                double time = (Environment.TickCount - tickStart)/1000.0;
                
                double y = UART_Buffer[0];

                //Now add data points to the list
                list.Add(time, y);
                Console.WriteLine(Data[0]);
                // Used to control max and min values of the x-axis so that 
                // axis moves along as 
                Scale xScale = zgc.GraphPane.XAxis.Scale;
                if (time > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' (5) of teh max x value
                {
                    xScale.Max = time + xScale.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                    xScale.Min = xScale.Max - 30.0;    //Increase min values of x axis acordingly
                }

                //Rescale axis so that y axis changes also
                zgc.AxisChange();
                //Redraw
                zgc.Invalidate();

                //int t2 = (Environment.TickCount - tickStart);
                //Console.WriteLine("t at end of tick is {0}", t2);
            
        }

       private void RollingGraph_Resize(object sender, EventArgs e)
        {
            SetSize();
            int t3 = (Environment.TickCount - tickStart);
            Console.WriteLine("t in setsize is {0}", t3);
        }

        //Used to set size and location of the graph
        private void SetSize()
        {
            Rectangle formRect = this.ClientRectangle;
            formRect.Inflate(-10,-10);

            if( zgc.Size != formRect.Size)
            {
                zgc.Location=formRect.Location;
                zgc.Size=formRect.Size;
            }
            
       }

    }
}

