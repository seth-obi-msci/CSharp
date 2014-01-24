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
        public int time1 = 0;
        double time2 = 0;
        int NumValuesToScreen = 300;
        int COPY_POS = 0;
        int trans_index = 0;
        bool AutoSaveBool = false;
        bool Stop = false;

        //TextWriter Data_file = new StreamWriter(@"C:\Users\localadmin\Desktop\Data_file1.txt", true);

        // The RollingPointPairList is an efficient storage class that always
        // keeps a rolling set of point data without needing to shift any data values
        // New RollingPairList with 1200000 values
        RollingPointPairList list1 = new RollingPointPairList(300);
        RollingPointPairList list2 = new RollingPointPairList(300);

        byte[] ScreenBuffer = new byte[300];
        byte[] UART_Buffer;
        byte[] Plotting_1 = new byte[1];

        
 
        public RollingGraph()
        {
            InitializeComponent();
        }

        private void RollingGraph_Load(object sender, EventArgs e)
        {
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


        public void CreateGraph(ZedGraph.ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "Plotting arrays of bytes";


            //Make a new curve
            LineItem curve = myPane.AddCurve("Counts", list1, Color.DarkGoldenrod, SymbolType.None);
            LineItem curve2 = myPane.AddCurve("Counts (Avg ten)", list2, Color.DimGray, SymbolType.None);
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
            Console.WriteLine("Create Graph");
        }

    
        public void timer1_Tick(object sender, EventArgs e)
        {
            
            int t = (Environment.TickCount - tickStart);
            //Read bytes, gives array of bytes, the data.
            UART_Buffer = FPGA.ReadBytes();

            double UART_Average = SumBytes(UART_Buffer) / UART_Buffer.Length;
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
           if (!Stop)
           {
               //For loop to add data points to the list
               for (int i = 0; i < UART_Buffer.Length; i++)
               {

                   double y = UART_Buffer[i];

                   list.Add(time1, y);
                   time1++;

                   double j = i;
                   double k = UART_Buffer.Length;
                   if (i - 1 < k / 2 && k / 2 <= i)
                   {
                       time2 += k / 2;
                       list2.Add(time2, UART_Average);
                       time2 += k / 2;
                   }
               }

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







            int Diff = ScreenBuffer.Length - COPY_POS;
            bool Condition = UART_Buffer.Length > Diff;




            // Coppies buffer array into a larger Data array. When data array is filled overwrites
            // least recent with most recent. 
            if (!Condition)
            {

                Array.Copy(UART_Buffer, 0, ScreenBuffer, COPY_POS, UART_Buffer.Length);


                //Console.WriteLine("time1%100 is {0}", time1%100);
                COPY_POS = COPY_POS + UART_Buffer.Length;

                //WRITE TO FILE
            }
            else
            {
                Array.Copy(UART_Buffer, 0, ScreenBuffer, COPY_POS, ScreenBuffer.Length - COPY_POS);

                if(AutoSaveBool)
                {
                    SaveToFile(@"C:\Users\localadmin\Desktop\Data_file1.txt", ScreenBuffer);
                }

                /* for (int i = 0; i < Data.Length / 10; i++)
                 {

                     int[] Avg_ten = new int[10];
                     Array.Copy(Data, i * 10, Avg_ten, 0, 10);

                     double Sum = Avg_ten.Sum();
                     double Average = Avg_ten.Sum() / 10.0;
                     Console.WriteLine("Avg_ten={0}", Average);

                     //list2.Add(time2, Average);
                     //time2 = time2 + 10;
                    
                     //trans_index++; // will equal 9 if there are 94 values in data array, since equas 1 to start
                 }
                */

                //WriteToScreen(ScreenBuffer);



                //WriteToScreen(Data);


                /*
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
               */
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

                //Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length);
                Array.Copy(UART_Buffer, ScreenBuffer.Length - COPY_POS, ScreenBuffer, 0, UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS));

                COPY_POS = UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS);




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

        private void SaveBytesButton(object sender, EventArgs e)
        {
            //Button SaveButton = sender as Button;
            if (sender == null) return;
            if (sender != null)
            {
                SaveToFile(@"C:\Users\localadmin\Desktop\Data_file1.txt", ScreenBuffer);
            }
        }

        private void FreshScreenButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                time1 = 0;
                time2 = 0;
                list1.Clear();
                list2.Clear();
                Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length);
            }
            return;
        }

        public void AutoSave(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                AutoSaveBool = true;
            }

        }


        public void SaveToFile(string FileLocation, byte[] DataToSave)
        {
            List<int> Data_Readings = new List<int>(DataToSave.Length);

            foreach (int j in DataToSave)
            {
                string Reading = j.ToString() + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }
            return;
        }

        public void WriteToScreen(byte[] DataToWrite)
        {
            for (int j = 0; j < DataToWrite.Length; j++)
            {
                Console.WriteLine("{0}th element is {1}", j, DataToWrite[j]);
            }
            return;
        }

        public double SumBytes(byte[] DataToSum)
        {
            double total = 0;
            for (int j = 0; j < DataToSum.Length; j++)
            {
                total += DataToSum[j];
            }

            return total;
        }

        private void StopGraph(object sender, EventArgs e)
        {
            if (sender == null) Stop = false;
            if (sender != null)
            {
                Stop = true;
            }
         return;

        }
    
    }



}

       



