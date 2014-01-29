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

        public int time1 = 0; //Integer to use as time coord for plotting arrays
        public double time2 = 0; //double to allow division for plotting average

        //int NumValuesToScreen = 300; //Number of Values in point pair list and number of elements in ScreenBuffer array
        
        int COPY_POS = 0;  //Pointer to element in ScreenBuffer to copy UART_Buffer to

        int l = 0; //Index for clock divider "for" loop used for byte simulation 

        //Booleans used in the form for various functions 
        bool AutoSaveBool = false;
        bool Stop = false;
        double slider;
        
        // The RollingPointPairList is an efficient storage class that always
        // keeps a rolling set of point data without needing to shift any data values
        // New RollingPairList with 1200000 values
        RollingPointPairList list1 = new RollingPointPairList(30000);
        RollingPointPairList list2 = new RollingPointPairList(30000);

        
        //byte arrays
        byte[] SimulatedBytes = new byte [10];
        byte[] ScreenBuffer = new byte[30000];
        byte[] UART_Buffer;
        byte[] Plotting_1 = new byte[1];

        //Images for fill
         Image Joe = Bitmap.FromFile(@"C:\Users\localadmin\Downloads\70583720.JPG");
         Image graham = Bitmap.FromFile(@"C:\Users\localadmin\Downloads\graham.jpg");

        string FileLocation;

        public RollingGraph()
        {
            InitializeComponent();

        }


        private void RollingGraph_Load(object sender, EventArgs e)
        {
            CreateGraph(zgc,zgc2);
            Console.WriteLine("Load");
        }
        

        public void CreateGraph(ZedGraph.ZedGraphControl zgc, ZedGraph.ZedGraphControl zgc2)
        {

            GraphPane myPane = zgc.GraphPane;
            GraphPane myPane2 = zgc2.GraphPane; 

            myPane.Title.Text = "Plotting arrays of bytes";
            myPane2.Title.Text = "Average of Buffer Counts";


            //Make a new curve
             LineItem curve = myPane.AddCurve("Counts", list1, Color.Black, SymbolType.None);
             TextureBrush joebrush = new TextureBrush(Joe);
             curve.Line.Fill = new Fill(joebrush);
             curve.Line.Width = 3.0F;
            

            LineItem curve2 = myPane2.AddCurve("Counts (Avg ten)", list2, Color.White, SymbolType.None);
            TextureBrush grahambrush = new TextureBrush(graham);
            curve2.Line.Fill = new Fill(grahambrush);
            curve2.Line.Width = 3.0F;
            
            //Timer fort the X axis, defined later
            timer1.Interval = 10; //10 - buffer size increases due to build up but levels out at about 112 bytes.
            timer1.Enabled = true;
            timer1.Start();

            // Second timer
            timer2.Interval = 10;
            timer2.Enabled = true;
            timer2.Start();

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps (DONT UNDERSTAND)
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 30000;
            myPane.XAxis.Scale.MinorStep = 1000;
            myPane.XAxis.Scale.MajorStep = 5000;

            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = 260;
            myPane.YAxis.Scale.MinorStep = 2;
            myPane.YAxis.Scale.MajorStep = 10;

            myPane2.XAxis.Scale.Min = 0;
            myPane2.XAxis.Scale.Max = 30000;
            myPane2.XAxis.Scale.MinorStep = 1000;
            myPane2.XAxis.Scale.MajorStep = 5000;
            
            myPane2.YAxis.Scale.Min = 0;
            myPane2.YAxis.Scale.Max = 260;
            myPane2.YAxis.Scale.MinorStep = 2;
            myPane2.YAxis.Scale.MajorStep = 10;

            
            //Scale axis
            zgc.AxisChange();
            zgc2.AxisChange();

            //Save begging item for reference (?)
            tickStart = Environment.TickCount;
            Console.WriteLine("Create Graph");
           
            

        }

        //l is clock divider index so 10 bytes can be built up before being read and plotted. Simulates bytes building up in UART buffer
        public void timer1_Tick(object sender, EventArgs e)
        {
            UART_Buffer = FPGA.ReadBytes();
            Console.WriteLine("Length of Buffer = {0}", UART_Buffer.Length);
            int t = (Environment.TickCount - tickStart);
           
            if (Stop) //Ensures points only plotted if stop button NOT pressed on form
            {
                
            }
            else
            {
                //l++;
                //SimulatedBytes[l % 10] = Convert.ToByte(slider);

                //if (l % 10 == 9) //Only reads and plots once every 10 ticks
                {

                   

                    
                    //Calculates average of buffer load
                    double UART_Average = SumBytes(UART_Buffer) / UART_Buffer.Length;

                    
                    // Ensures there's at least one curve in GraphPane
                    if (zgc.GraphPane.CurveList.Count <= 0 || zgc2.GraphPane.CurveList.Count <= 0)
                        return;

                    LineItem curve = zgc.GraphPane.CurveList[0] as LineItem;
                    LineItem curve2 = zgc2.GraphPane.CurveList[0] as LineItem;
                    if (curve == null || curve2 == null)
                        return;


                    // If list is null then reference at curve.Points doesn't
                    // support IPointPairList and can't be modified
                    if (list2 == null || list1 == null)
                        return;

                    // Think this gives time in milliseconds
                    double time = (Environment.TickCount - tickStart) / 1000.0;


                    //For loop to add data points to the list
                    for (int i = 0; i < UART_Buffer.Length; i++)
                    {
                        double y = UART_Buffer[i]; //Plots buffer value
        
                        list1.Add(time1, y);
                        time1++;
                        
                        //Plots running average of each buffer load at central x value
                        double k = UART_Buffer.Length;
                        double j = i;
                        if (k > 1)
                        {
                            if (j - 1 < k / 2 && k / 2 <= j) //Inequalities necessary as k/2 may fall between 2 values of i
                            {
                                time2 += k / 2;
                                list2.Add(time2, UART_Average);
                                time2 += k / 2;
                            }
                        }
                        else
                        {
                            time2 += k / 2;
                            list2.Add(time2, UART_Average);
                            time2 += k / 2;
                        }
                    }



                    // Used to control max and min values of the x-axis so that 
                    // axis moves along as 
                    Scale xScale = zgc.GraphPane.XAxis.Scale;
                    Scale xScale2 = zgc2.GraphPane.XAxis.Scale;
                    if (time1 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' (5) of the max x value
                    {
                        xScale.Max = time1 + xScale.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                        xScale.Min = xScale.Max - 30000.0;
                        xScale2.Max = time2 + xScale2.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                        xScale2.Min = xScale2.Max - 30000.0; //Increase min values of x axis acordingly
                    }

                    /*Scale xScale2 = zgc2.GraphPane.XAxis.Scale;
                    if (time2 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' (5) of the max x value
                    {
                        xScale2.Max = time2 + xScale2.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                        xScale2.Min = xScale2.Max - 300.0;    //Increase min values of x axis acordingly
                    }*/

                    //Rescale axis so that y axis changes also
                    zgc.AxisChange();
                    zgc2.AxisChange();

                    //Redraw
                    zgc.Invalidate();
                    zgc2.Invalidate();

                    //Remaining spaces in ScreenBuffer
                    int Diff = ScreenBuffer.Length - COPY_POS;
                    //Remaining spaces fewer than next buffer load
                    bool Condition = UART_Buffer.Length > Diff;


                    // Copies buffer array into a larger ScreenBuffer array. When ScreenBuffer is filled overwrites
                    // least recent with most recent. 
                    if (!Condition)
                    {

                        Array.Copy(UART_Buffer, 0, ScreenBuffer, COPY_POS, UART_Buffer.Length);
                        COPY_POS = COPY_POS + UART_Buffer.Length; //COPY_POS shifted by UART buffer size

                    }

                    else //Not enough room at end of ScreenBuffer for whole UART bufferload
                    {
                        //Fills remaining ScreenBuffer spaces with portion of UART buffer load
                        Array.Copy(UART_Buffer, 0, ScreenBuffer, COPY_POS, ScreenBuffer.Length - COPY_POS);

                        if (AutoSaveBool) //Saves if AutoSave function selected on form
                        {
                            SaveBytesToFile(@FileLocation, ScreenBuffer);
                        }


                        //Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length); ScreenBuffer not cleared to ensures complete array is written to file

                        //Remaining of UART bufferload copied to first spaces in ScreenBuffer and COPY_POS shifted accordingly
                        Array.Copy(UART_Buffer, ScreenBuffer.Length - COPY_POS, ScreenBuffer, 0, UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS));
                        COPY_POS = UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS);



                    }
                }
            }
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

        
//Below are functions which have been outsourced for clarity in main code (above)

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
        

        public void WriteBytesToScreen(byte[] DataToWrite)
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


        public void SaveBytesToFile(string FileLocation, byte[] DataToSave)
        {
            foreach (int j in DataToSave)
            {
                string Reading = j.ToString() + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }
            return;
        }


        private void SaveBytesButton(object sender, EventArgs e)
        {
            //Button SaveButton = sender as Button;
            if (sender == null) return;
            if (sender != null)
            {
                for (int j = COPY_POS; j<ScreenBuffer.Length; j++)
                {
                    string Reading = ScreenBuffer[j].ToString() + "\r\n";
                    File.AppendAllText(@FileLocation, Reading);
                }

                for (int j = 0; j < COPY_POS; j++)
                {
                    string Reading = ScreenBuffer[j].ToString() + "\r\n";
                    File.AppendAllText(@FileLocation, Reading);
                }
                return;
            }
        }

        /*Functions below use buttons/sliders on the form itself*/
        //Wipes both average and running pointpairlists and resets x axis to 0 for new screen
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
                CreateGraph(zgc, zgc2);
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


        private void PauseButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            else if (sender != null)
            {
                Stop = true;
                return;
            }

        }

        private void ContinueButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            else if (sender != null)
            {
                Stop = false;
                return;
            }
        }



        private void EnterButton(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.Text = textBox2.Text;
            FileLocation = this.Text;
        }


        //Extracts value of slider to "slider" integer in main code
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            slider = trackBar1.Value;
        }

        private void zgc_Load(object sender, EventArgs e)
        {

        }

           
             
    }
}

       



