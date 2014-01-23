using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZedGraph;

namespace Graph_practice_2_Rolling_data
{

    public partial class RollingGraph : Form
    {
        // Starting time in millisecs. 
        int tickStart = 0;

        //Integer to use as time coord for plotting out of Plotting_1 array
        int time1 = 0;
        int time2 = 5;

        int COPY_POS = 0;
        int trans_index = 0;

        //TextWriter Data_file = new StreamWriter(@"C:\Users\localadmin\Desktop\Data_file1.txt", true);


        byte[] Data = new byte[100];
        byte[] Plotting_1 = new byte[1];

        public RollingGraph()
        {
            InitializeComponent();
        }


        private void RollingGraph_Load(object sender, EventArgs e)
        {
            CreateGraph(zgc);
            CreateGraph(zgc);
            Console.WriteLine("Load");
        }

        /*creates an array of data to simulate data from experiment, dont think this is used later
        private double[] Data()
        {
            int t = (Environment.TickCount - tickStart);
            double[] Values = new double[1000];
            for(int i=0; i < Values.Length;i++)
                Values[i] = i;
            return Values;
        }*/


        private void CreateGraph(ZedGraph.ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "Plotting arrays of bytes";

            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            // New RollingPairList with 1200 values
            RollingPointPairList list1 = new RollingPointPairList(1200000);

            //Make a new curve
            LineItem curve = myPane.AddCurve("Counts", list1, Color.DarkGoldenrod, SymbolType.None);

            //Timer fort the X axis, defined later
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();

            // Second timer
            timer2.Interval = 100;
            timer2.Enabled = true;
            timer2.Start();

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps (DONT UNDERSTAND)
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 300;
            myPane.XAxis.Scale.MinorStep = 10;
            myPane.XAxis.Scale.MajorStep = 50;

            //Scale axis
            zgc.AxisChange();

            //Save begging item for reference (?)
            tickStart = Environment.TickCount;
            Console.WriteLine("Create Graphs");
        }

        /*private void CreateGraph2(ZedGraph.ZedGraphControl zgc)
        {
            GraphPane myPane2 = zgc.GraphPane;
            myPane2.Title.Text = "Plotting arrays of averaged bytes";

            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            // New RollingPairList with 1200 values
            RollingPointPairList list2 = new RollingPointPairList(1200000);

            //Make a new curve
            LineItem curve2 = myPane2.AddCurve("Counts (Avg ten)", list2, Color.DimGray, SymbolType.None);

            //Timer fort the X axis, defined later

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps (DONT UNDERSTAND)
            myPane2.XAxis.Scale.Min = 0;
            myPane2.XAxis.Scale.Max = 300;
            myPane2.XAxis.Scale.MinorStep = 10;
            myPane2.XAxis.Scale.MajorStep = 50;

            //Scale axis
            zgc.AxisChange();

            //Save begging item for reference (?)
            tickStart = Environment.TickCount;
        }*/

        public void timer1_Tick(object sender, EventArgs e)
        {
            int t = (Environment.TickCount - tickStart);
            //Read bytes, gives array of bytes, the data.

            byte[] UART_Buffer = FPGA.ReadBytes();
            int UART_Length = UART_Buffer.Length;

            //Console.WriteLine("t at tick is {0}", t);
            Console.WriteLine("Size of Data array {0}", Data.Length);
            Console.WriteLine("Size of Buffer array {0}", UART_Buffer.Length);

            // Ensures there's at least one curve in GraphPane
            if (zgc.GraphPane.CurveList.Count <= 0)
                return;

            LineItem curve = zgc.GraphPane.CurveList[0] as LineItem;
            LineItem curve2 = zgc.GraphPane.CurveList[1] as LineItem;
            if (curve == null || curve2 == null)
                return;

            //Get the list of points from the selected curve
            IPointListEdit list = curve.Points as IPointListEdit;
            IPointListEdit list2 = curve2.Points as IPointListEdit;

            // If list is null then reference at curve.Points doesn't
            // support IPointPairList and can't be modified
            if (list2 == null || list == null)
                return;

            // Think this gives time in milliseconds
            double time = (Environment.TickCount - tickStart) / 1000.0;

            //For loop to add data points to the list
            for (int i = 0; i < UART_Buffer.Length; i++)
            {


                double y = UART_Buffer[i];
                //Console.WriteLine(y);


                list.Add(time1, y);
                time1++;
            }



            // Used to control max and min values of the x-axis so that 
            // axis moves along as 
            Scale xScale = zgc.GraphPane.XAxis.Scale;
            if (time1 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' (5) of teh max x value
            {
                xScale.Max = time1 + xScale.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                xScale.Min = xScale.Max - 300.0;    //Increase min values of x axis acordingly
            }

            //Rescale axis so that y axis changes also
            zgc.AxisChange();
            //Redraw
            zgc.Invalidate();







            int Diff = Data.Length - COPY_POS;
            bool Condition = UART_Length > Diff;




            // Coppies buffer array into a larger Data array. When data array is filled overwrites
            // least recent with most recent. 
            if (!Condition)
            {

                Array.Copy(UART_Buffer, 0, Data, COPY_POS, UART_Length);
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(Data[i]);
                }

                Console.WriteLine("COPY_POS = {0}", COPY_POS);
                COPY_POS = COPY_POS + UART_Length;

                //WRITE TO FILE
            }
            else
            {
                Array.Copy(UART_Buffer, 0, Data, COPY_POS, Data.Length - COPY_POS);
                for (int i = 0; i < Data.Length / 10; i++)
                {

                    int[] Avg_ten = new int[10];
                    Array.Copy(Data, i * 10, Avg_ten, 0, 10);

                    double Sum = Avg_ten.Sum();
                    double Average = Avg_ten.Sum() / 10.0;
                    Console.WriteLine("Avg_ten={0}", Average);

                    list2.Add(time2, Average);
                    time2 = time2 + 10;

                    //trans_index++; // will equal 9 if there are 94 values in data array, since equas 1 to start
                }
                Console.WriteLine("Gets into transfer bit");
                /* for (int i = 0; i < 100; i++)
                 {
                     Console.WriteLine(Data[i]);
                 }
                 */


                List<int> Data_Readings = new List<int>(Data.Length);

                for (int k = 0; k < Data.Length; k++)
                {
                    int Data_Reading = Data[k];
                    Data_Readings.Add(Data_Reading);
                }


                foreach (int j in Data)
                {
                    string Reading = j.ToString() + "\r\n";
                    File.AppendAllText(@"C:\Users\localadmin\Desktop\Data_file1.txt", Reading);
                }
                /* char[] data_string = new ( System.Text.Encoding.GetDecoder(Data));
                 using (Data_file)
                 {
                     Data_file.WriteLine(data_string);
                 }*/



                //System.IO.File.WriteAllBytes(@"C:\Users\localadmin\Desktop\Data_file.txt", Data);
                //byte[] read_data = System.IO.File.ReadAllBytes(@"C:\Users\localadmin\Desktop\Data_file.txt");

                /* 
                 for (int j = 0; j < read_data.Length; j++)
                 {
                     Console.WriteLine("The {0}th element is {1}", j, read_data[j]);
                 }*/

                Array.Clear(Data, 0, Data.Length);
                Array.Copy(UART_Buffer, Data.Length - COPY_POS, Data, 0, UART_Buffer.Length - (Data.Length - COPY_POS));

                COPY_POS = UART_Buffer.Length - (Data.Length - COPY_POS);




            }

            /*Prints Data array to screen
            for (int j = 0; j < Plotting_1.Length; j++)
            {
                Console.WriteLine(Plotting_1[j]);
            }*/


            /* int Data_Length = Data.Length;

             
                
 
            /*  // Ensures there's at least one curve in GraphPane
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
              //Console.WriteLine("t at end of tick is {0}", t2);*/

        }



        // This event is controlled by a second timer. Will try to use it to 
        // plot average counts on different time scale.

        public void timer2_Tick(object sender, EventArgs e)
        {
            /*Console.WriteLine("Timer 2 working");
            int [] Plotting_2 = new int [Plotting_1.Length];

            // Avearges over ten bytes of plotting_1 to make plotting_2
            for (int i = 0; i < Plotting_2.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Plotting_2[i] = Plotting_2[i] + Plotting_1[j]/10;
                }
            }*/

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
            formRect.Inflate(-10, -10);

            if (zgc.Size != formRect.Size)
            {
                zgc.Location = formRect.Location;
                zgc.Size = formRect.Size;
            }

        }

        private void RollingGraph_Load_1(object sender, EventArgs e)
        {

        }

    }

}



