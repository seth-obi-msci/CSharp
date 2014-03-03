using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Timers;
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
        public int time2=0; //double to allow division for plotting average



        double Average1;
        int SumProcessedBytes1 = 0;
        int PreviousScreenRemainder1 = 0;
        int AverageIndex1 = 1;
        int AverageChunkSize1 = 50;

        double Average2;
        int SumProcessedBytes2=0;
        int PreviousScreenRemainder2 = 0;
        int AverageIndex2 = 1;
        int AverageChunkSize2 = 100;

        int[] AverageArray2 = new int[3];
        int[] AverageArray1 = new int[3];
    
        double XMin1=0;
        double XMax1;
        double XScaleValue1;
        double XMin2=0;
        double XMax2;
        double XScaleValue2;
        int YMin;
        int YMax=300;
        

        //int NumValuesToScreen = 300; //Number of Values in point pair list and number of elements in ScreenBuffer array
        
        int COPY_POS = 0;  //Pointer to element in ScreenBuffer to copy UART_Buffer to

        int l = 0; //Index for clock divider "for" loop used for byte simulation 

        //Booleans used in the form for various functions 
        bool AutoSaveBool = false;
        bool Pause = false;
        double slider;
        bool LHSSaveBool;
        bool RHSSaveBool;
        bool IsScrolling;

        byte[] TimebinFactor =new byte[1]; 
        

        // The RollingPointPairList is an efficient storage class that always
        // keeps a rolling set of point data without needing to shift any data values
        // New RollingPairList with 30000 values
        PointPairList templist1 = new PointPairList();
        PointPairList templist2 = new PointPairList();
        PointPairList savelist1 = new PointPairList();
        PointPairList savelist2 = new PointPairList();
        PointPairList list1 = new PointPairList();
        PointPairList list2 = new PointPairList();
        PointPairList RecentPoint1 = new PointPairList();
        PointPairList RecentPoint2 = new PointPairList();

        //byte arrays
        byte[] SimulatedBytes = new byte [10];
        byte[] ScreenBuffer = new byte[30000];
        byte[] UART_Buffer;
        byte[] Plotting_1 = new byte[1];

        
        //Images for fill
        //Image Joe = Bitmap.FromFile(@"C:\Users\localadmin\Downloads\70583720.JPG");
        //Image graham = Bitmap.FromFile(@"C:\Users\localadmin\Downloads\graham.jpg");

        string FileLocation;
       
        public RollingGraph()
        {
            InitializeComponent();
            TimebinFactor[0] = 1;
        }


        private void RollingGraph_Load(object sender, EventArgs e)
        {
            CreateGraph(zgc);
            Console.WriteLine("Load");
        }
        

        public void CreateGraph(ZedGraph.ZedGraphControl zgc)
        {
            zgc.MasterPane.PaneList.Clear();

            time1 = 0;
            time2 = 0;

            XScaleValue1 = Convert.ToInt16(XScale1.Value);
            XScaleValue2 = Convert.ToInt16(XScale2.Value);

            GraphPane myPane1 = new GraphPane();
            GraphPane myPane2 = new GraphPane();
            zgc.MasterPane.Add(myPane1);
            zgc.MasterPane.Add(myPane2);
            zgc.MasterPane.PaneList[0].Legend.IsVisible = false;
            zgc.MasterPane.PaneList[1].Legend.IsVisible = false;

          
            myPane1.Chart.Fill = new Fill(Color.Black);
            myPane2.Chart.Fill = new Fill(Color.Black);
            myPane1.Title.Text = "myPane1";
            myPane2.Title.Text = "myPane2";

            if (IsScrolling)
            {
                //zgc.IsAntiAlias = true;
            }

            //Make a new curve

             BarItem RecentBar1 = myPane1.AddBar("RecentBar", RecentPoint1, Color.Red);
             RecentBar1.Bar.Fill = new Fill(Color.Red);
             RecentBar1.Bar.Border = new Border(Color.Red, 2.0F);

             BarItem curve = myPane1.AddBar("Average Counts", list1, Color.White/*, SymbolType.None*/);
             curve.Bar.Fill = new Fill(Color.White);
             curve.Bar.Border = new Border(Color.White, (float)(1.0));

             BarItem tempcurve1 = myPane1.AddBar("Average Counts", templist1, Color.White);
             tempcurve1.Bar.Fill = new Fill(Color.White);
             tempcurve1.Bar.Border = new Border(Color.White, (float)(1.0));



             BarItem RecentBar2 = myPane2.AddBar("RecentBar", RecentPoint2, Color.Red);
             RecentBar2.Bar.Fill = new Fill(Color.Red);
             RecentBar2.Bar.Border = new Border(Color.Red, 2.0F);
       
             BarItem curve2 = myPane2.AddBar("Counts (Avg ten)", list2, Color.White/*, SymbolType.None*/);
             curve2.Bar.Fill = new Fill(Color.White);
             curve2.Bar.Border = new Border(Color.White, 1.0F);

             BarItem tempcurve2 = myPane2.AddBar("Average Counts", templist2, Color.White);
             tempcurve2.Bar.Fill = new Fill(Color.White);
             tempcurve2.Bar.Border = new Border(Color.White, 1.0F);
 



             //zgc.MasterPane.PaneList[0].BarSettings.MinClusterGap = 0;
             

            //Timer fort the X axis, defined later
            timer1.Interval = 1; //10 - buffer size increases due to build up but levels out at about 112 bytes.
            timer1.Enabled = true;
            timer1.Start();
            

            //Function to set axes of graphpanes.
            SetXAxis1();
            SetXAxis2();
            if (AutoScale.Checked == false)
            {
                SetYAxis();
            }
            
            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = this.CreateGraphics())
            {
                zgc.MasterPane.SetLayout(g, PaneLayout.SingleRow);
            }


            //Save begging time for reference
            tickStart = Environment.TickCount;

            Console.WriteLine("Create Graph");
           
            

        }

        //l is clock divider index so 10 bytes can be built up before being read and plotted. Simulates bytes building up in UART buffer
        public void timer1_Tick(object sender, EventArgs e)
        {
            zgc.AxisChange();
                              
            //  Must create new instance of graphpanes to access them.
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            GraphPane myPane2 = zgc.MasterPane.PaneList[1];

            if (LHSPane.Checked && !RHSPane.Checked)
            {
                RectangleF zero_rect2 = new RectangleF(Convert.ToInt16(XMax2), 0F, 0F, 0F);
                myPane2.ReSize(this.CreateGraphics(), zero_rect2);
                myPane1.ReSize(this.CreateGraphics(), zgc.MasterPane.Rect);

            }
            else if (!LHSPane.Checked && RHSPane.Checked)
            {
                RectangleF zero_rect1 = new RectangleF(0F, 0F, 0F, 0F);
                myPane1.ReSize(this.CreateGraphics(), zero_rect1);
                myPane2.ReSize(this.CreateGraphics(), zgc.MasterPane.Rect);

            }
            else
            {

                using (Graphics g = this.CreateGraphics())
                {
                    zgc.MasterPane.SetLayout(g, PaneLayout.SingleRow);
                }

            }

            SetXAxis1();
            SetXAxis2();
           

            Point XMax_Pane = new Point(1, 0);
            Point origin = new Point(0, 0);
            Point endchart = new Point(1, 0);
            //Console.WriteLine("*origin point is {0}", myPane1.GeneralTransform(origin, CoordType.ChartFraction).X);
           // Console.WriteLine("*XMax point is {0}", myPane1.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X);

            int t = (Environment.TickCount - tickStart);
           
            if (Pause) //Ensures points only plotted if stop button NOT pressed on form
            {
                
            }
            else
            {
                //l++;
               // SimulatedBytes[l % 10] = Convert.ToByte(slider);


               // if (l % 10 == 9) //Only reads and plots once every 10 ticks
                {

                    UART_Buffer = FPGA.ReadBytes();

                    SetSize();


                    
                    // Ensures there's at least one curve in GraphPane
                    if (zgc.GraphPane.CurveList.Count <= 0)
                        return;

                    //BarItem curve = myPane1.CurveList[0] as LineItem;
                   // LineItem curve2 = myPane2.CurveList[0] as LineItem;
                    /*if (curve1 == null || curve2==null)
                        return;*/


                    // If list is null then reference at curve.Points doesn't
                    // support IPointPairList and can't be modified
                    if (list2 == null || list1 == null)
                        return;

                    if (templist2 == null || templist1 == null)
                    {
                        
                        return;
                    }

                    // Think this gives time in milliseconds
                    double time = (Environment.TickCount - tickStart) / 1000.0;


                   
                    Scale xScale = myPane1.XAxis.Scale;
                    Scale xScale2 = myPane2.XAxis.Scale;

                    // if loops used to rescale x axis for each graph
                    if (IsScrolling)
                    {
                        if (time1 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' (5) of the max x value
                        {
                            xScale.Max = time1 + xScale.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                            xScale.Min = xScale.Max - XMax1;   ////Increase min values of x axis acordingly

                        }

                        if (time2 > xScale2.Max - xScale.MajorStep)
                        {
                            xScale2.Max = time2 + xScale2.MajorStep;
                            xScale2.Min = xScale2.Max - XMax2;

                        }
                    }

                    //Redraw
                    if (AutoScale.Checked == true)
                    {
                        zgc.AxisChange();
                    }
                    zgc.Invalidate();
                   
                    //Remaining spaces in ScreenBuffer
                    int Diff = ScreenBuffer.Length - COPY_POS;


                    // if condition satisfied when there's enough room for a UART_Buffer array to be
                    // copied into the Screen array. 
                    if (Diff>UART_Buffer.Length)
                    { 

                        Array.Copy(UART_Buffer, 0, ScreenBuffer, COPY_POS, UART_Buffer.Length); // Copies UART_Buffer (FPGA.ReadBytes into Screen array, overwriting oldest values
                        COPY_POS = COPY_POS + UART_Buffer.Length; //COPY_POS shifted by UART buffer size


                        // if conditions controll when averaging function 'AverageScreenUnfilled' is call for weither of the graphpanes (1 & 2)
                        // should only be called when an 'AverageChunkSize' worth of new values have been copied into ScreenBuffer
                        if (COPY_POS >= AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2)
                        {
                            AverageScreenUnfilled2();              
                            
                            SumProcessedBytes2 = 0;  //Sets total carried over from previous step to zero once first time through COPY_POS >= AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2 loop 
                            
                        }

                        if (COPY_POS >= AverageIndex1 * AverageChunkSize1 - PreviousScreenRemainder1)
                        {
                            
                            AverageScreenUnfilled1();

                            SumProcessedBytes1 = 0;  //Sets total carried over from previous step to zero once first time through COPY_POS >= AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2 loop
                            
                        }

                    }

                    //Not enough room at end of ScreenBuffer for whole UART bufferload
                    else 
                    {
                        
                        // Fills remaining ScreenBuffer spaces with portion of UART buffer load
                        // UART_Buffer values that don't get copied in due to not enough space are placed 
                        // at the begining of the ScreenBuffer next 'tick'
                        Array.Copy(UART_Buffer, 0, ScreenBuffer, COPY_POS, ScreenBuffer.Length - COPY_POS);

                      

                        // Different averaging function, 'AverageScreenFilled', used when ScreenBuffer is filled so that no values 
                        // are left out of averaging. i.e. those at end of ScreenBuffer that don't make up an entire AverageChunkSize
                        AverageScreenFilled2();


                        AverageScreenFilled1();


                        // Indexes set to one so that next timer tick averaging begins at start of ScreenBuffer (most recent values)
                        AverageIndex2 = 1;
                        AverageIndex1 = 1;

                        
                        //Remaining of UART bufferload copied to first spaces in ScreenBuffer and COPY_POS shifted accordingly
                        Array.Copy(UART_Buffer, ScreenBuffer.Length - COPY_POS, ScreenBuffer, 0, UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS));
                        COPY_POS = UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS);
                       
                       
                       
                    }
                    
                }
            }
            if (list1.LongCount() > 30000)
            {
                list1.RemoveRange(0, Convert.ToInt32(list1.LongCount() - 30000));
            }
            if (list2.LongCount() > 30000)
            {
                list2.RemoveRange(0, Convert.ToInt32(list2.LongCount() - 30000));
            }
            if (savelist1.LongCount() > 30000)
            {
                savelist1.RemoveRange(0, Convert.ToInt32(savelist1.LongCount() - 30000));
            }
            if (savelist2.LongCount() > 30000)
            {
                savelist2.RemoveRange(0, Convert.ToInt32(savelist2.LongCount() - 30000));
            }

            Console.WriteLine("Size of buffer = {0}", UART_Buffer.Length);
        }



        

        
//Below are functions which have been outsourced for clarity in main code (above)


        //Used to set size and location of the graph
        private void SetSize()
        {

            Rectangle formRect = this.ClientRectangle;
            formRect.Inflate(0,-43);

      
            if (zgc.Size != formRect.Size)
            {
                zgc.Location = formRect.Location;
                zgc.Size = formRect.Size;
            }
        }

         // Function to set X Axis on myPane1
        private void SetXAxis1()
        {
            zgc.AxisChange();
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            Scale xScale = myPane1.XAxis.Scale;
            Point XMax_Pane = new Point(1, 0);
            Point origin = new Point(0, 0);
            Point endchart = new Point(1, 0);
            if (LHSPane.Checked)
            {
                XMax1 = Convert.ToDouble((myPane1.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane1.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue1);
            }
           
            myPane1.XAxis.Scale.Min = XMin1;
            xScale.Max = XMax1;
            myPane1.XAxis.Scale.MinorStep = XMax1/100;
            myPane1.XAxis.Scale.MajorStep = XMax1/10;

            zgc.AxisChange();
        }

        // Function to set X Axis on myPane2
        private void SetXAxis2()
        {
         
                zgc.AxisChange();
                GraphPane myPane2 = zgc.MasterPane.PaneList[1];
                Scale xScale2 = myPane2.XAxis.Scale;
                Point XMax_Pane = new Point(1, 0);
                Point origin = new Point(0, 0);
                Point endchart = new Point(1, 0);
                // Console.WriteLine("*origin point is {0}", myPane2.GeneralTransform(origin, CoordType.ChartFraction).X);
                // Console.WriteLine("*XMax point is {0}", myPane2.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X);
                if (RHSPane.Checked)
                {
                    XMax2 = Convert.ToInt16((myPane2.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane2.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue2);
                }
                    myPane2.XAxis.Scale.Min = XMin2;
                myPane2.XAxis.Scale.Max = XMax2;
                myPane2.XAxis.Scale.MinorStep = XMax2 / 100.0;
                myPane2.XAxis.Scale.MajorStep = XMax2 / 10.0;
            
            
            zgc.AxisChange();
        }

        // Function to set Y Axis on both panes (generally similar Y values)
        public void SetYAxis()
        {
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            GraphPane myPane2 = zgc.MasterPane.PaneList[1];
           

                myPane1.YAxis.Scale.Min = YMin;
                myPane1.YAxis.Scale.Max = YMax;
                myPane1.YAxis.Scale.MinorStep = YMax / 100;
                myPane1.YAxis.Scale.MajorStep = YMax / 10;

                myPane2.YAxis.Scale.Min = YMin;
                myPane2.YAxis.Scale.Max = YMax;
                myPane2.YAxis.Scale.MinorStep = YMax / 100;
                myPane2.YAxis.Scale.MajorStep = YMax / 10;

                zgc.AxisChange();
        }

        private void RollingGraph_Load_1(object sender, EventArgs e)
        {

        }
        
        // Used for checking code
        public void WriteBytesToScreen(byte[] DataToWrite)
        {
            for (int j = 0; j < DataToWrite.Length; j++)
            {
                Console.WriteLine("{0}th element is {1}", j, DataToWrite[j]);
            }
            return;
        }

        
        // Finds sum of byte array, used in averaging
        public double SumBytes(byte[] DataToSum)
        {
            double total = 0;
            for (int j = 0; j < DataToSum.Length; j++)
            {
                total += DataToSum[j];
            }

            return total;
        }

        private void SaveBytesToFile(string FileLocation)
        {
            
            string BytesFileLocation = FileLocation + "RawData.txt";
            //Add some MetaData
            File.AppendAllText(BytesFileLocation, Convert.ToString(DateTime.Now) + "\r\n");
            File.AppendAllText(BytesFileLocation, Convert.ToString("Total time span of data = " + TimebinFactor[0] * ScreenBuffer.Length / 10000.0) + "\r\n");
            File.AppendAllText(BytesFileLocation, this.filename.Text + "\r\n");

            for(int i =COPY_POS;i<ScreenBuffer.Length; i++)
            {
            File.AppendAllText(BytesFileLocation, Convert.ToString(ScreenBuffer[i]) + "\r\n");
            }
            for(int i=0;i<COPY_POS;i++)
            {
                File.AppendAllText(BytesFileLocation, Convert.ToString(ScreenBuffer[i]) + "\r\n");
            }
        }

        // function to save a given arry's bytes to file
        public void SaveListToFile(string FileLocation, PointPairList list)
        {

            MetaData(1)[4] = this.filename.Text;

            File.AppendAllText(FileLocation, "Data from LHS Graph" + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[0] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[1] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[2] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[3] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[4] + "\r\n");

            double[] SaveArray = list.Select(P => P.Y).ToArray();
            Console.WriteLine("Size of SvarArray {0}", SaveArray.Length);

          
            //  Stop = true;
            for (int j = 0; j < SaveArray.Length; j++)
            {
                string Reading = SaveArray[j].ToString() + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }


        }

     /*   public void SaveListToFile(string FileLocation, PointPairList list, PointPairList templist)
        {

            MetaData(1)[4] = this.filename.Text;


            File.AppendAllText(FileLocation, "Data from LHS Graph" + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[0] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[1] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[2] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[3] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(1)[4] + "\r\n");




            double[] SaveArray = list.Concat(templist).Select(P => P.Y).ToArray();
            Console.WriteLine("Size of SvarArray {0}", SaveArray.Length);


            //  Stop = true;
            for (int j = 0; j < SaveArray.Length; j++)
            {
                string Reading = SaveArray[j].ToString() + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }


        }*/

        // function to save both graphs' data
        public void SaveBothListsToFile(string FileLocation)
        {
            MetaData(1)[4] = this.filename.Text;
            string LHSfile = FileLocation + "LHS.txt";
            string RHSfile = FileLocation + "RHS.txt";

            File.AppendAllText(LHSfile, "Data from LHS Graph" + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[0] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[1] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[2] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[3] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[4] + "\r\n");


            File.AppendAllText(RHSfile, "Data from LHS Graph" + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[0] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[1] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[2] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[3] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[4] + "\r\n");
            

            double[] SaveArray1 = list1.Select(P => P.Y).ToArray();
            Console.WriteLine("Size of SvarArray {0}", SaveArray1.Length);


            //  Stop = true;
            for (int j = 0; j < SaveArray1.Length; j++)
            {
                string Reading = SaveArray1[j].ToString() + "\r\n";
                File.AppendAllText(LHSfile, Reading);
            }
           
            double[] SaveArray2 = list2.Select(P => P.Y).ToArray();
            Console.WriteLine("Size of SvarArray {0}", SaveArray2.Length);

            File.AppendAllText(FileLocation, "Data from Graph1" + "\r\n"+"\r\n"+"\r\n"+"\r\n");
            //  Stop = true;
            for (int j = 0; j < SaveArray2.Length; j++)
            {
                string Reading = SaveArray2[j].ToString() + "\r\n";
                File.AppendAllText(RHSfile, Reading);
            }


        }
        // function to save both graphs' data when using non scrolling mode
        public void SaveBothListsToFile_NoScrol(string FileLocation)
        {
            MetaData(1)[4] = this.filename.Text;
            string LHSfile = FileLocation+"LHS.txt";
            string RHSfile = FileLocation + "RHS.txt";

            File.AppendAllText(LHSfile, "Data from LHS Graph" + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[0] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[1] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[2] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[3] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[4] + "\r\n");


            File.AppendAllText(RHSfile, "Data from LHS Graph" + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[0] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[1] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[2] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[3] +"Environmenttime = " + Convert.ToString(Environment.TickCount - tickStart) +"\r\n" );
            File.AppendAllText(RHSfile, MetaData(2)[4] + "\r\n");

            double[] SaveArray1 = savelist1.Select(P => P.Y).ToArray();
            Console.WriteLine("Size of SvarArray {0}", SaveArray1.Length);


            //  Stop = true;
            for (int j = 0; j < SaveArray1.Length; j++)
            {
                string Reading = SaveArray1[j].ToString() + "\r\n";
                File.AppendAllText(LHSfile, Reading);
            }

            double[] SaveArray2 = savelist2.Select(P => P.Y).ToArray();
            //Console.WriteLine("Size of SvarArray {0}", SaveArray2.Length);

            
            //  Stop = true;
            for (int j = 0; j < SaveArray2.Length; j++)
            {
                string Reading = SaveArray2[j].ToString() + "\r\n";
                File.AppendAllText(RHSfile, Reading);
            }



            FreshScreen();

        }
        private void SaveBytesButton_Click(object sender, EventArgs e)
        {

            Pause = true;
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = @"C:\";
            sfd1.Title = "SAVE BITCH";
            //sfd1.CheckFileExists = true;
            sfd1.CheckPathExists = true;
            sfd1.ShowDialog();
            //sfd1.FileOk += new CancelEventHandler(sfd1_FileOk);
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                if (LHSSaveBool && RHSSaveBool)
                {


                    if (IsScrolling)
                    {
                        SaveBothListsToFile(@sfd1.FileName);
                    }
                    else if (!IsScrolling)
                    {
                        SaveBothListsToFile_NoScrol(@sfd1.FileName);
                    }
                   
                }

                else if (!RHSSaveBool && LHSSaveBool)
                {



                    if (IsScrolling)
                    {
                        SaveListToFile(@sfd1.FileName + "LHS.txt", list1);
                    }
                    else if (!IsScrolling)
                    {
                        SaveListToFile(@sfd1.FileName + "LHS.txt", savelist1);
                    }

                    //Console.WriteLine("Length of UART_Buffer {0}", UART_Buffer.Length);
                   

                }
                else if (RHSSaveBool && !LHSSaveBool)
                {

                    if (IsScrolling)
                    {
                        SaveListToFile(@sfd1.FileName + "RHS.txt", list2);
                    }
                    else if (!IsScrolling)
                    {
                        SaveListToFile(@sfd1.FileName + "RHS.txt", savelist2);
                    }

                }
               /* else
                {
                    Console.WriteLine("Must select graph to save");

                    return;
                }*/
                WriteBytesToScreen(ScreenBuffer);
                SaveBytesToFile(sfd1.FileName);

                FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                Array.Clear(UART_Buffer, 0, UART_Buffer.Length);
                //Console.WriteLine("Length of UART_Buffer {0}", UART_Buffer.Length);
                FreshScreen();
                Pause = false;
            }
            
        }

        private string[] MetaData(int pane)
        {
            string [] MetaData=new string [5];
            MetaData[0] = Convert.ToString(DateTime.Now);
            MetaData[1] = "FPGA timebin = " + Convert.ToString(TimebinFactor[0]);
            
            if (pane == 1)
            {
                MetaData[2] = "Counts averaged over = " + Convert.ToString(AverageChunkSize1);
                if (IsScrolling)
                {
                    double TotalTime = list1.LongCount() * AverageChunkSize1 * TimebinFactor[0] / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
                else if (!IsScrolling)
                {
                    double TotalTime = savelist1.LongCount() * AverageChunkSize1 * TimebinFactor[0] / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
            }
            else if (pane == 2)
            {
                MetaData[2] ="Counts averaged over = " + Convert.ToString(AverageChunkSize2);
                if (IsScrolling)
                {
                    double TotalTime = list2.LongCount() * AverageChunkSize2 * TimebinFactor[0] / (10000.0);
                    double TotalTime2 = tickStart - Environment.TickCount;
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime)/*+"Environment time = " + Convert.ToString(TotalTime2)*/;
                }
                else if (!IsScrolling)
                {
                    double TotalTime = savelist2.LongCount() * AverageChunkSize2 * TimebinFactor[0] / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime) ;
                }
            }

            return MetaData;

        }

        /*Functions below use buttons/sliders on the form itself*/

        //Wipes both average and running pointpairlists and resets x axis to 0 for new screen, effectively restarts program
        private void FreshScreenButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                FreshScreen();
            }
            return;
        }
        private void FreshScreen()
        {
            SumProcessedBytes1 = 0;
            SumProcessedBytes2 = 0;
            PreviousScreenRemainder1 = 0;
            PreviousScreenRemainder2 = 0;
            AverageIndex1 = 1;
            AverageIndex2 = 1;
            list1.Clear();
            list2.Clear();
            templist1.Clear();
            templist2.Clear();
            Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length);
            COPY_POS = 0;
            CreateGraph(zgc);
        }
        
        // Triggers autosave function, which saves ScreenBuffer everytime all values are replaced
        public void AutoSave(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                AutoSaveBool = true;
            }

        }


        // Stops everything
        private void PauseButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            else if (sender != null)
            {
                Pause = true;
                return;
            }

        }

        // Resumes everything
        private void ContinueButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            else if (sender != null)
            {
                Pause = false;
                return;
            }
        }


        // Can choose location and file name of file to save
        private void filename_TextChanged(object sender, EventArgs e)
        {
            this.Text = filename.Text;
            FileLocation = this.Text;
        }


        //Extracts value of slider to "slider" integer in main code (Used as replacement for incoming bytes)
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            slider = trackBar1.Value;
        }
  

        // Used to alter X Axis range myPane1
        void XScale1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ZoomIn.Checked)
            {
                XScale1.Maximum = 100;
                XScaleValue1 =Convert.ToDouble( 1 / XScale1.Value);
                BarItem curve = zgc.MasterPane.PaneList[0].CurveList[1] as BarItem;
                curve.Bar.Border.Width = (float)(XScale1.Value);
                BarItem tempcurve1 = zgc.MasterPane.PaneList[0].CurveList[2] as BarItem;
                tempcurve1.Bar.Border.Width = (float)(XScale1.Value);
                BarItem RecentBar1 = zgc.MasterPane.PaneList[0].CurveList[0] as BarItem;
                RecentBar1.Bar.Border.Width = (float)(XScale1.Value);
            }
            else if (!ZoomIn.Checked && !IsScrolling)
            {
                XScale1.Maximum = 100;
                XScaleValue1 =Convert.ToDouble(XScale1.Value);
            }
            else if (!ZoomIn.Checked && IsScrolling)
            {
                XScale1.Maximum = 1;
                XScaleValue1 = Convert.ToDouble(XScale1.Value);
                BarItem curve = zgc.MasterPane.PaneList[0].CurveList[1] as BarItem;
                curve.Bar.Border.Width = (float)(XScale1.Value);
                BarItem tempcurve1 = zgc.MasterPane.PaneList[0].CurveList[2] as BarItem;
                tempcurve1.Bar.Border.Width = (float)(XScale1.Value);
                BarItem RecentBar1 = zgc.MasterPane.PaneList[0].CurveList[0] as BarItem;
                RecentBar1.Bar.Border.Width = (float)(XScale1.Value);
            }

            SetXAxis1(); 
        }

        // Used to alter X Axis range myPane2
        void XScale2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ZoomIn.Checked)
            {
                XScale2.Maximum = 100;
                XScaleValue2 = Convert.ToDouble(1 / XScale2.Value);
                BarItem curve2 = zgc.MasterPane.PaneList[1].CurveList[1] as BarItem;
                curve2.Bar.Border.Width = (float)(XScale1.Value);
                BarItem tempcurve2 = zgc.MasterPane.PaneList[1].CurveList[2] as BarItem;
                tempcurve2.Bar.Border.Width = (float)(XScale1.Value);
                BarItem RecentBar2 = zgc.MasterPane.PaneList[1].CurveList[0] as BarItem;
                RecentBar2.Bar.Border.Width = (float)(XScale1.Value);
            }
            else if (!ZoomIn.Checked && !IsScrolling)
            {
                XScale2.Maximum = 100;
                XScaleValue2 = Convert.ToDouble(XScale2.Value);
            }
            else if (!ZoomIn.Checked && IsScrolling)
            {
                XScale2.Maximum = 1;
                XScaleValue2 = Convert.ToDouble(XScale2.Value);
                BarItem curve2 = zgc.MasterPane.PaneList[1].CurveList[1] as BarItem;
                curve2.Bar.Border.Width = (float)(XScale1.Value);
                BarItem tempcurve2 = zgc.MasterPane.PaneList[1].CurveList[2] as BarItem;
                tempcurve2.Bar.Border.Width = (float)(XScale1.Value);
                BarItem RecentBar2 = zgc.MasterPane.PaneList[1].CurveList[0] as BarItem;
                RecentBar2.Bar.Border.Width = (float)(XScale1.Value);
            }
            SetXAxis2();
        }

        // Used to change Y axis on both Panes
        public void YMaxNum_ValueChanged(object sender, System.EventArgs e)
        {
            YMax = Convert.ToInt32(YMaxNum.Value);
            SetYAxis();
        }

        public void YMinNum_ValueChanged(object sender, System.EventArgs e)
        {
            YMin = Convert.ToInt32(YMinNum.Value);
            SetYAxis();
        }

        // Changes no of values to average over, resets everything and starts plotting
        // from scratch with new average
        void AvChunkSize1_ValueChanged(object sender, System.EventArgs e)
        {
            AverageChunkSize1 = Convert.ToInt32(AvChunkSize1.Value);
            FreshScreen();
        }
       

        // Same as above but for myPane2
        void AvChunkSize2_ValueChanged(object sender, System.EventArgs e)
        {
            AverageChunkSize2 = Convert.ToInt32(AvChunkSize2.Value);
            FreshScreen();
        }



        // Function to average bytes in ScreenBuffer while it isn't filled (still room for UART_Buffer to be coppied in)
        private void AverageScreenUnfilled1()
        {
            // Two values must be return so function returns an array
           
            int[] Properties = new int[2];
            int SumChunk = SumProcessedBytes1;//Running total carried over from end of previous ScreenBuffer

            if (AverageIndex1 == 1) //Ensures first value of i in for loop is not negative as it would be if set to (AverageIndex2 - 1) * AverageChunkSize2 - PreviousScreenRemainder2 and AverageIndex2=1 (as it is in if loop below
            {
                for (int i = 0; i < AverageIndex1 * AverageChunkSize1 - PreviousScreenRemainder1; i++) //Sums first values of new buffer
                {
                    SumChunk += Convert.ToInt32(ScreenBuffer[i]);
                }
                if (SumVsAv.Checked == false)
                {
                    Average1 = SumChunk / (AverageChunkSize1*TimebinFactor[0]);
                }
                else if (SumVsAv.Checked == true)
                {
                    Average1 = SumChunk / TimebinFactor[0];
                }
                if (!IsScrolling)
                {
                    savelist1.Add(time1, Average1);
                    if (time1 > XMax1)
                    {
                        templist1.Clear();
                        templist1.Add(list1);
                        list1.Clear();
                        time1 = 0;

                    }

                    if (templist1.LongCount() != 0)
                    {
                        templist1.RemoveRange(0, 1);

                    }
                }
                list1.Add(time1, Average1);
               

                RecentPoint1.Add(time1,Average1);
                 if (RecentPoint1.LongCount() > 1)
                 {
                     RecentPoint1.RemoveAt(0);
                 }

                zgc.Invalidate();

                time1 += 1;// TimebinFactor[0] * AverageChunkSize1;
    
                AverageIndex1++;
                SumChunk = 0;
       
            }
            while (AverageIndex1 * AverageChunkSize1 - PreviousScreenRemainder1 < COPY_POS) // while loop continues to average blocks of bytes untill there aren't enough 'new' bytes to make up an 'AverageChunkSize', 
                                                                                         // at this point requique new UART_Buffer to be coppied into ScreenBuffer
            {
                for (int i = (AverageIndex1 - 1) * AverageChunkSize1 - PreviousScreenRemainder1; i < AverageIndex1 * AverageChunkSize1 - PreviousScreenRemainder1; i++) //Sums a chunk from middle of array
                {
                    SumChunk += Convert.ToInt32(ScreenBuffer[i]);
                }
                if (SumVsAv.Checked == false)
                {
                    Average1 = SumChunk / (AverageChunkSize1 * TimebinFactor[0]);
                }
                else if (SumVsAv.Checked == true)
                {
                    Average1 = SumChunk / TimebinFactor[0];
                }
                if (!IsScrolling)
                {
                    savelist1.Add(time1, Average1);
                    if (time1 > XMax1)
                    {
                        templist1.Clear();
                        templist1.Add(list1);
                        list1.Clear();
                        time1 = 0;

                    }

                    if (templist1.LongCount() != 0)
                    {
                        templist1.RemoveRange(0, 1);

                    }
                }
                list1.Add(time1, Average1);

                RecentPoint1.Add(time1, Average1);
                if (RecentPoint1.LongCount() > 1)
                {
                    RecentPoint1.RemoveAt(0);
                }

                zgc.Invalidate();


                time1 += 1;// TimebinFactor[0] * AverageChunkSize1;
                AverageIndex1++; // Average index hold placed of where to start averaging from. 
                SumChunk = 0;
                //Console.WriteLine(zgc.MasterPane.PaneList[0].CurveList.Count());
  
            }


            
        }
        private void AverageScreenFilled1()
        {



            int SumScreenRemainder = 0;
            int SumEndChunk = 0;

            // Ensures averages are plotted if more than AverageChunkSize of array is yet to be averaged
            // Then sums remaining bytes to be carried over into next loop

            while (ScreenBuffer.Length - ((AverageIndex1 - 1) * AverageChunkSize1 - PreviousScreenRemainder1) > AverageChunkSize1) // Same while loop as above function to average remaining bytes that can make up an 'AverageChunkSize'
            {

                for (int i = (AverageIndex1 - 1) * AverageChunkSize1 - PreviousScreenRemainder1; i < AverageChunkSize1 * AverageIndex1 - PreviousScreenRemainder1; i++)
                {
                    SumEndChunk += Convert.ToInt32(ScreenBuffer[i]);
                }

                if (SumVsAv.Checked == false)
                {
                    Average1 = SumEndChunk / (AverageChunkSize1 * TimebinFactor[0]);
                }
                else if (SumVsAv.Checked == true)
                {
                    Average1 = SumEndChunk / TimebinFactor[0];
                }
                if (!IsScrolling)
                {
                    savelist1.Add(time1, Average1);
                    if (time1 > XMax1)
                    {
                        templist1.Clear();
                        templist1.Add(list1);
                        list1.Clear();
                        time1 = 0;
                    }

                    if (templist1.LongCount() != 0)
                    {
                        templist1.RemoveAt(0);

                    }
                }
               
                list1.Add(time1, Average1);
               
                RecentPoint1.Add(time1, Average1);

                if (RecentPoint1.LongCount() > 1)
                {
                    RecentPoint1.RemoveAt(0);
                }

                zgc.Invalidate();


                time1 += 1;// TimebinFactor[0] * AverageChunkSize1;
                SumEndChunk = 0;
                AverageIndex1++;
            }


            for (int i = (AverageIndex1 - 1) * AverageChunkSize1 - PreviousScreenRemainder1; i < ScreenBuffer.Length; i++) // For loop to sum remaining bytes in ScreenBuffer
            {
                SumScreenRemainder += Convert.ToInt32(ScreenBuffer[i]);
            }
            PreviousScreenRemainder1 = ScreenBuffer.Length - ((AverageIndex1 - 1) * AverageChunkSize1 - PreviousScreenRemainder1); // Index to acount for how many 'new' bytes need to be added to the sum of the remainder to make up 'AverageChunkSize'



            SumProcessedBytes1 = SumScreenRemainder;  // Carries over remainder bytes' sum 
            SumScreenRemainder = 0;

        }
        // Function to average bytes in ScreenBuffer while it isn't filled (still room for UART_Buffer to be coppied in)
        private void AverageScreenUnfilled2()
        {
            // Two values must be return so function returns an array

           
            int SumChunk = SumProcessedBytes2;//Running total carried over from end of previous ScreenBuffer

            if (AverageIndex2 == 1) //Ensures first value of i in for loop is not negative as it would be if set to (AverageIndex2 - 1) * AverageChunkSize2 - PreviousScreenRemainder2 and AverageIndex2=1 (as it is in if loop below
            {
                for (int i = 0; i < AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2; i++) //Sums first values of new buffer
                {
                    SumChunk += Convert.ToInt32(ScreenBuffer[i]);
                }
                if (SumVsAv.Checked == false)
                {
                    Average2 = SumChunk / (AverageChunkSize2 * TimebinFactor[0]);
                }
                else if (SumVsAv.Checked == true)
                {
                    Average2 = SumChunk / TimebinFactor[0];
                }
                if (!IsScrolling)
                {
                    savelist2.Add(time2,Average2);
                    if (time2 > XMax2)
                    {
                        
                        templist2.Clear();
                        templist2.Add(list2);
                        list2.Clear();
                        time2 = 0;
                    }

                    if (templist2.LongCount() != 0)
                    {
                        templist2.RemoveRange(0, 1);

                    }
                }
                list2.Add(time2, Average2);

                RecentPoint2.Add(time2, Average2);
                if (RecentPoint2.LongCount() > 1)
                {
                    RecentPoint2.RemoveAt(0);
                }

                zgc.Invalidate();
               
                time2 += 1;// TimebinFactor[0] * AverageChunkSize;

                AverageIndex2++;
                SumChunk = 0;
                
            }
            while (AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2 < COPY_POS) // while loop continues to average blocks of bytes untill there aren't enough 'new' bytes to make up an 'AverageChunkSize', 
            // at this point requique new UART_Buffer to be coppied into ScreenBuffer
            {
                for (int i = (AverageIndex2 - 1) * AverageChunkSize2 - PreviousScreenRemainder2; i < AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2; i++) //Sums a chunk from middle of array
                {
                    SumChunk += Convert.ToInt32(ScreenBuffer[i]);
                }
                if (SumVsAv.Checked == false)
                {
                    Average2 = SumChunk / (AverageChunkSize2 * TimebinFactor[0]);
                }
                else if (SumVsAv.Checked == true)
                {
                    Average2 = SumChunk / TimebinFactor[0];
                }
                if (!IsScrolling)
                {
                    savelist2.Add(time2, Average2);
                    if (time2 > XMax2)
                    {
                        templist2.Clear();
                        templist2.Add(list2);
                        list2.Clear();
                        time2 = 0;
                    }

                    if (templist2.LongCount() != 0)
                    {
                        templist2.RemoveRange(0, 1);

                    }
                }
                list2.Add(time2, Average2);

                RecentPoint2.Add(time2, Average2);

                if (RecentPoint2.LongCount() > 1)
                {
                    RecentPoint2.RemoveAt(0);
                }

                zgc.Invalidate();

                time2 += 1;// TimebinFactor[0] * AverageChunkSize;
                AverageIndex2++; // Average index hold placed of where to start averaging from. 
                SumChunk = 0;
               
            }

           
           

        }
        private void AverageScreenFilled2()
        {
           
            int[] FilledProperties = new int[3];

            int SumScreenRemainder=0;
            int SumEndChunk = 0;
            
            // Ensures averages are plotted if more than AverageChunkSize of array is yet to be averaged
            // Then sums remaining bytes to be carried over into next loop

            while(ScreenBuffer.Length-((AverageIndex2-1) * AverageChunkSize2 - PreviousScreenRemainder2) > AverageChunkSize2) // Same while loop as above function to average remaining bytes that can make up an 'AverageChunkSize'
            {
                
                for (int i = (AverageIndex2 - 1) * AverageChunkSize2 - PreviousScreenRemainder2; i < AverageChunkSize2 * AverageIndex2 - PreviousScreenRemainder2; i++)
                {
                    SumEndChunk += Convert.ToInt32(ScreenBuffer[i]);
                }

                if (SumVsAv.Checked == false)
                {
                    Average2 = SumEndChunk / (AverageChunkSize2 * TimebinFactor[0]);
                }
                else if (SumVsAv.Checked == true)
                {
                    Average2 = SumEndChunk / TimebinFactor[0];
                }
                if (!IsScrolling)
                {
                    savelist2.Add(time2, Average2);
                    if (time2 > XMax2)
                    {
                        templist2.Clear();
                        templist2.Add(list2);
                        list2.Clear();
                        time2 = 0;
                    }

                    if (templist2.LongCount() != 0)
                    {
                        templist2.RemoveRange(0, 1);

                    }
                }

                list2.Add(time2, Average2);

                RecentPoint2.Add(time2, Average2);
                if (RecentPoint2.LongCount() > 1)
                {
                    RecentPoint2.RemoveAt(0);
                }

                zgc.Invalidate();
                
                time2 += 1;//TimebinFactor[0] * AverageChunkSize;
                SumEndChunk = 0;
                AverageIndex2++;
            }

                for (int i = (AverageIndex2-1) * AverageChunkSize2 - PreviousScreenRemainder2; i < ScreenBuffer.Length; i++) // For loop to sum remaining bytes in ScreenBuffer
                {
                    SumScreenRemainder += Convert.ToInt32(ScreenBuffer[i]);
                }
                PreviousScreenRemainder2 = ScreenBuffer.Length - ((AverageIndex2-1) * AverageChunkSize2 - PreviousScreenRemainder2); // Index to acount for how many 'new' bytes need to be added to the sum of the remainder to make up 'AverageChunkSize'
               
            

            SumProcessedBytes2 = SumScreenRemainder;  // Carries over remainder bytes' sum 
            SumScreenRemainder = 0;

        }

        private void LHSSave_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == null) LHSSaveBool=false ;
            else if (sender != null) LHSSaveBool = true;
        }

        private void RHSSave_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == null) RHSSaveBool = false;
            else if (sender != null) RHSSaveBool = true;
        }


        private void Timebinfactorvalue_ValueChanged(object sender, EventArgs e)
        {
            TimebinFactor[0] = Convert.ToByte(Timebinfactorvalue.Value);
            SumProcessedBytes1 = 0;
            SumProcessedBytes2 = 0;
            PreviousScreenRemainder1 = 0;
            PreviousScreenRemainder2 = 0;
            AverageIndex1 = 1;
            AverageIndex2 = 1;
            list1.Clear();
            list2.Clear();
            Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length);
            COPY_POS = 0;
            CreateGraph(zgc);
        }

        private void ScrollingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null) IsScrolling = !IsScrolling;
            FreshScreen();
            Console.WriteLine("IsScrolling is {0}", IsScrolling);

        }



    }       
}
