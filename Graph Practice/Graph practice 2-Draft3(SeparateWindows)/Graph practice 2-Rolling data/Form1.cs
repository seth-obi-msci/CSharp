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
        public int tickStart = 0;
        public int time1 = 0; //Integer to use as time coord for plotting arrays
        public int time2 = 0; //double to allow division for plotting average
        public int AdditionalTime1 = 0; //AdditionalTime variables to retain total time elapsed during cyclical plotting
        int AdditionalTime2 = 0;

        //Variables for calculating averages from DataBuffer
        public double Average1; //y axis values on screen 1
        int SumProcessedBytes1 = 0; //Sum of remaining values at end of DataBuffer if insufficient values to calculate a full average
        int AdjustIndex1 = 0;
        int AverageIndex1 = 1;
        int AverageChunkSize1; //AverageChunkSize*FPGA timebin = plotting timebin
        
        double Average2;
        int SumProcessedBytes2 = 0;
        int AdjustIndex2 = 0;
        int AverageIndex2 = 1;
        int AverageChunkSize2;

        //X Axis scaling/range
        double XMin1 = 0;
        double XMax1;
        double XScaleValue1;
        double XMin2 = 0;
        double XMax2;
        double XScaleValue2;

        //Y Axis scaling/range if autorescale not used
        int YMin1;
        int YMax1 = 3000;
        int YMin2;
        int YMax2 = 3000;

        //Used-defined values for min count rate below which ion(s) assumed lost and program pauses
        double ThresholdLineValue1;
        double ThresholdLineValue2;

        int COPY_POS = 0;  //Pointer to element in DataBuffer where UART_Buffer copied
        int l = 0; //Index for clock divider "for" loop used for byte simulation and PMT compare

        //Booleans used in the form for various functions
        bool Pause = false;
        bool IsScrolling1;
        bool IsScrolling2;
        bool PastOneScreen1 = false;
        bool PastOneScreen2 = false;
        bool TrueTime1 = false; //Option for plotting real time values on x axis
        bool TrueTime2 = false;
        int TimebinFactor = 1;//Input of FPGA timebin for program
        
        //Bools for choosing which PMT on which panes
        bool PMT1_pane1 = true;
        bool PMT2_pane1 = true;
        bool PMT1_pane2 = true;
        bool PMT2_pane2 = true;
        bool StopWarning = false; //Warns used if counts measured by PMTs varies by specified fraction

        // Various lists containing data to be plotted or saved
        PointPairList savelist1 = new PointPairList();
        PointPairList savelist2 = new PointPairList();
        PointPairList list1 = new PointPairList();  //For plotted data
        PointPairList list2 = new PointPairList();
        PointPairList RecentPoint1 = new PointPairList(); //Most recent counts, plotted with big red bar
        PointPairList RecentPoint2 = new PointPairList();
        PointPairList ThresholdList1 = new PointPairList(); //Values for threshold line
        PointPairList ThresholdList2 = new PointPairList();

        //Makes new instance of NewWindow, this is window to display second graph
        NewWindow Window2 = new NewWindow();

        //byte arrays
        byte[] SimulatedBytes = new byte[10];
        byte[] DataBuffer = new byte[30000];
        byte[] UART_Buffer;
        byte[] Plotting_1 = new byte[1];

        string FileLocation;

        public RollingGraph()
        {
            InitializeComponent();
        }

        // Creats a graph in each of the windows
        public void RollingGraph_Load(object sender, EventArgs e)
        {
            this.Text = "Main Window";
            Window2.Text = "Secondary Window";
            CreateGraph(zgc);
            CreateGraph(Window2.zgc2);
            Window2.Show();
            Console.WriteLine("Load");
        }

        //Program triggered by internal timer
        public void timer1_Tick(object sender, EventArgs e)
        {
            //Retrieves panes for access within timer1_Tick function
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            GraphPane myPane2 = Window2.zgc2.MasterPane.PaneList[0];

            SetSize(zgc);
            SetSize(Window2.zgc2);

            //Set X axis each timer.tick, required to maintain 1:1 pixel:axis relationship
            SetXAxis(zgc);
            SetXAxis(Window2.zgc2);

            int t = (Environment.TickCount - tickStart);

            if (Pause) //Ensures points only plotted if pause button NOT pressed on form
            {

            }
            else
            {
                //l is clock divider index so 10 bytes can be built up before being read and plotted. Simulates bytes building up in UART buffer
                l++;
                if (l % 100 == 99 && !StopWarning) //Compares counts measured by PMTs every 100 timerticks
                {
                    PMTCompare();
                }
                //Bytes simulation if FPGA not available
                /* if (l % 2 == 0)
                {
                    SimulatedBytes[l % 10] = Convert.ToByte(trackBar1.Value);
                }
                else if (l % 2 == 1)
                {
                    SimulatedBytes[l % 10] = Convert.ToByte(0);
                }
                if (l % 10 == 9) //Only reads and plots once every 10 ticks*/

                {
                    UART_Buffer = FPGA.ReadBytes();//Counts on extarnal UART chip assigned to internal array

                    // Ensures there's at least one curve in GraphPane
                    if (zgc.GraphPane.CurveList.Count <= 0)
                        return;

                    // If list is null then do nothing
                    if (list2 == null || list1 == null)
                    {
                        return;
                    }

                    // Gives milliseconds elapsed since starting program
                    double time = (Environment.TickCount - tickStart) / 1000.0;

                    //Scrolls Axes if nearing edge
                    XAxisScroll1();
                    XAxisScroll2();

                    //Automatically rescales Y-axis if each timer.tick if Autoscale selected
                    if (AutoScale1.Checked == true)
                    {
                        zgc.AxisChange();
                        ThresholdScrollBar1.Visible = false;
                        ThresholdLineValue1 = 0;
                    }
                    if (Window2.AutoScale2.Checked == true)
                    {
                        Window2.zgc2.AxisChange();
                        Window2.ThresholdScrollBar2.Visible = false;
                        ThresholdLineValue2 = 0;
                    }
                    zgc.Invalidate(); //Invalidate redraws graph
                    Window2.zgc2.Invalidate();

                   
                    Console.WriteLine("Bytes in buffer = {0}", UART_Buffer.Length);
                    
                    //Remaining spaces in DataBuffer
                    int Diff = DataBuffer.Length - COPY_POS;

                    // Condition satisfied when there's enough room for a UART_Buffer array to be
                    // copied into the Screen array.
                    if (Diff > UART_Buffer.Length)
                    {
                        // Copies UART_Buffer=FPGA.ReadBytes into Screen array, overwriting oldest values
                        Array.Copy(UART_Buffer, 0, DataBuffer, COPY_POS, UART_Buffer.Length);

                        //COPY_POS shifted by UART buffer size, so new values are copied into correct position.
                        COPY_POS = COPY_POS + Convert.ToInt32(UART_Buffer.Length);


                        //Calculates averages for most recent UART bufferload if DataBuffer doesnt reach capacity
                        //Sets total carried over from AverageScreenFilled to zero once first time called
                        AverageDataUnfilled1();
                        AverageDataUnfilled2();

                        //Checks fluorescence above threshold value
                        CheckIonTrapped();
                    }

                    //Not enough room at end of DataBuffer for whole UART bufferload
                    else
                    {
                        // Fills remaining DataBuffer spaces with portion of UART buffer load.
                        // UART_Buffer values that don't get copied in due to not enough space are placed
                        // at the begining of the DataBuffer once final values averaged.
                        Array.Copy(UART_Buffer, 0, DataBuffer, COPY_POS, DataBuffer.Length - COPY_POS);

                        // AverageScreenFilled used when those at end of DataBuffer that don't make up an entire AverageChunkSize
                        AverageDataFilled1();
                        AverageDataFilled2();

                        // Indexes set to beginning so that next timer tick averaging begins at start of DataBuffer (most recent values)
                        AverageIndex2 = 2;
                        AverageIndex1 = 2;

                        //Remaining of UART bufferload copied to first spaces in DataBuffer and COPY_POS shifted accordingly
                        Array.Copy(UART_Buffer, DataBuffer.Length - COPY_POS, DataBuffer, 0, UART_Buffer.Length - (DataBuffer.Length - COPY_POS));
                        COPY_POS = UART_Buffer.Length - (DataBuffer.Length - COPY_POS);

                    }
                }
            }
            //Updates X range of ThresholdLine
            ThresholdList1.Add(XMin1, ThresholdLineValue1);
            ThresholdList1.Add(XMax1, ThresholdLineValue1);

            ThresholdList2.Add(XMin2, ThresholdLineValue2);
            ThresholdList2.Add(XMax2, ThresholdLineValue2);
            
            // Removes excess points once capacity reached
            if (list1.LongCount() > 10000)
            {
                list1.RemoveRange(0, Convert.ToInt32(list1.LongCount() - 10000));
            }
            if (list2.LongCount() > 2000)
            {
                list2.RemoveRange(0, Convert.ToInt32(list2.LongCount() - 2000));
            }
            if (savelist1.LongCount() > 2000)
            {
                savelist1.RemoveRange(0, Convert.ToInt32(savelist1.LongCount() - 2000));
            }
            if (savelist2.LongCount() > 2000)
            {
                savelist2.RemoveRange(0, Convert.ToInt32(savelist2.LongCount() - 2000));
            }
            if (ThresholdList1.LongCount() > 2)
            {
                ThresholdList1.RemoveRange(0, 2);
            }
            if (ThresholdList2.LongCount() > 2)
            {
                ThresholdList2.RemoveRange(0, 2);
            }

        }

        //Below are functions which have been outsourced for clarity in main code (above)
        
        //Creates graph and sets formatting
        public void CreateGraph(ZedGraphControl ZGC)
        {
            ZGC.MasterPane.PaneList.Clear();
            GraphPane myPane = new GraphPane();
            ZGC.MasterPane.Add(myPane);

            if (ZGC == zgc)
            {
                AverageChunkSize1 = Convert.ToInt32(Math.Round(Convert.ToDouble(AvChunkBox1.Value) * 10.0 / TimebinFactor));
                XScaleValue1 = Convert.ToInt16(XScale1.Value);
                LineItem ThresholdLine1 = ZGC.MasterPane.PaneList[0].AddCurve("Threshold1", ThresholdList1, Color.DarkOliveGreen, SymbolType.None);
                ThresholdLine1.Line.Width = 2.0F;
                SetBarProperties(1, Color.White, 1.0F, Color.Red, 10.0F);

                // Layout the GraphPanes using a default Pane Layout
                using (Graphics g = this.CreateGraphics())
                {
                    ZGC.MasterPane.SetLayout(g, PaneLayout.SingleRow);
                }

                myPane.Title.Text = "Pane1";
            }

            else if (ZGC == Window2.zgc2)
            {
                AverageChunkSize2 = Convert.ToInt32(Math.Round(Convert.ToDouble(Window2.AvChunkBox2.Value) * 10.0 / TimebinFactor));
                XScaleValue2 = Convert.ToInt16(Window2.XScale2.Value);
                Console.WriteLine("XScale value2 create = {0}", XScaleValue2);
                LineItem ThresholdLine2 = ZGC.MasterPane.PaneList[0].AddCurve("Threshold2", ThresholdList2, Color.DarkGoldenrod, SymbolType.None);
                ThresholdLine2.Line.Width = 2.0F;
                SetBarProperties(2, Color.White, 1.0F, Color.Red, 10.0F);

                using (Graphics g = this.CreateGraphics())
                {
                    ZGC.MasterPane.SetLayout(g, PaneLayout.SingleRow);
                }
                myPane.Title.Text = "Pane2";
            }


            ZGC.MasterPane.PaneList[0].Legend.IsVisible = false;

            myPane.Chart.Fill = new Fill(Color.Black);

            //Timer fort the X axis, defined later
            timer1.Interval = 1;
            timer1.Enabled = true;
            timer1.Start();

            SetXAxis(ZGC);


            //Sets Y-axis if auto-scale is not selected (fixed Y axes)
            if (AutoScale1.Checked == false)
            {
                SetYAxis(ZGC);
            }
            ZGC.AxisChange();

            //Save begging time for reference
            tickStart = Environment.TickCount;

            Console.WriteLine("Create Graph");

        }



        private void SetBarProperties(int Pane, Color BarsColor, float BarsWidth, Color RecentBarColor, float RecentBarsWidth)
        {
            if (Pane == 1)
            {
                BarItem RecentBar1 = zgc.MasterPane.PaneList[0].AddBar("RecentBar", RecentPoint1, Color.Red);
                RecentBar1.Bar.Border = new Border(Color.Red, 10.0F);

                BarItem Bars1 = zgc.MasterPane.PaneList[0].AddBar("Average Counts", list1, Color.White);
                Bars1.Bar.Border = new Border(Color.White, 1.0F);
            }

            if (Pane == 2)
            {
                BarItem RecentBar2 = Window2.zgc2.MasterPane.PaneList[0].AddBar("RecentBar", RecentPoint2, Color.Red);
                RecentBar2.Bar.Border = new Border(Color.Red, 10.0F);

                BarItem Bars2 = Window2.zgc2.MasterPane.PaneList[0].AddBar("Counts (Avg ten)", list2, Color.White);
                Bars2.Bar.Border = new Border(Color.White, 1.0F);
            }

        }

        // Function to move X-axis along for scrolling effect.
        private void XAxisScroll1()
        {

            Scale xScale = zgc.MasterPane.PaneList[0].XAxis.Scale;
            // if loops used to shift x axis for each graph
            if (IsScrolling1)
            {
                if (time1 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' of the max x value
                {
                    xScale.Max = time1 + xScale.MajorStep; //Keep the end of x axis MajorStep away from end of curve
                    xScale.Min = xScale.Max - XMax1;   //Increase min values of x axis acordingly
                }

            }
        }

        public void XAxisScroll2()
        {

            Scale xScale = Window2.zgc2.MasterPane.PaneList[0].XAxis.Scale;


            // if loops used to shift x axis for each graph
            if (IsScrolling2)
            {
                if (time2 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' of the max x value
                {
                    xScale.Max = time2 + xScale.MajorStep; //Keep the end of x axis MajorStep away from end of curve
                    xScale.Min = xScale.Max - XMax2;   //Increase min values of x axis acordingly
                }

            }
        }

        //Used to set size and location of the graph panes
        private void SetSize(ZedGraphControl ZGC)
        {
            if (WindowState == FormWindowState.Minimized)
            {

            }
            else
            {
                GraphPane myPane = ZGC.MasterPane.PaneList[0] as GraphPane;

                Point YMin = new Point(0, 0);
                Point YMax = new Point(0, 1);
                Rectangle formRect = new Rectangle();
                Rectangle GraphRect = new Rectangle();
                Point LocationPoint = new Point(Convert.ToInt32((myPane.GeneralTransform(YMin, CoordType.ChartFraction)).X) - 10, Convert.ToInt32((myPane.GeneralTransform(YMin, CoordType.ChartFraction)).Y) + 40);

                if (ZGC == zgc)
                {

                    formRect = this.ClientRectangle;
                    if (ButtonsVisible1.Checked)
                    {
                        Rectangle Rect = new Rectangle(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y + 55, this.ClientRectangle.Size.Width, this.ClientRectangle.Size.Height - 55);
                        GraphRect = Rect;
                    }
                    else if (!ButtonsVisible1.Checked)
                    {
                        Rectangle Rect = new Rectangle(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y, this.ClientRectangle.Size.Width, this.ClientRectangle.Size.Height - 1);
                        GraphRect = Rect;
                    }

                    ThresholdScrollBar1.Height = Convert.ToInt32((myPane.GeneralTransform(YMax, CoordType.ChartFraction)).Y);
                    ThresholdScrollBar1.Location = LocationPoint;
                }

                if (ZGC == Window2.zgc2)
                {
                    formRect = Window2.ClientRectangle;
                    if (Window2.ButtonsVisible2.Checked)
                    {
                        Rectangle Rect = new Rectangle(Window2.ClientRectangle.Location.X, Window2.ClientRectangle.Location.Y + 55, Window2.ClientRectangle.Size.Width, Window2.ClientRectangle.Size.Height - 55);
                        GraphRect = Rect;
                    }
                    else if (!Window2.ButtonsVisible2.Checked)
                    {
                        Rectangle Rect = new Rectangle(Window2.ClientRectangle.Location.X, Window2.ClientRectangle.Location.Y, Window2.ClientRectangle.Size.Width, Window2.ClientRectangle.Size.Height - 1);
                        GraphRect = Rect;
                    }

                    Window2.ThresholdScrollBar2.Height = Convert.ToInt32((myPane.GeneralTransform(YMax, CoordType.ChartFraction)).Y);
                    Window2.ThresholdScrollBar2.Location = LocationPoint;
                }
                RectangleF ZGCrect = new RectangleF(0F, 0F, (ZGC.ClientSize.Width), (ZGC.ClientSize.Height));

                myPane.XAxis.Scale.FontSpec.Size = 10.0F;
                myPane.YAxis.Scale.FontSpec.Size = 10.0F;
                myPane.Title.FontSpec.Size = 12.0F;

                if (ZGC == zgc)
                {
                    myPane.ReSize(this.CreateGraphics(), ZGCrect);
                }
                if (ZGC == Window2.zgc2)
                {
                    myPane.ReSize(Window2.CreateGraphics(), ZGCrect);
                }

                if (ZGC.Size != formRect.Size)
                {
                    ZGC.Location = GraphRect.Location;
                    ZGC.Size = GraphRect.Size;
                }
            }
        }

        // Function to set X Axis on myPane1
        // if(TrueTime, !TrueTime) sensitivity conditions control how to scale x-axis if values are to represent  the real time intervals between data points or if all time intervals are set to one.
        // if(pane checked) ensures axis isn't rescaled when this pane is not seen (when only the other pane is selected).
        private void SetXAxis(ZedGraphControl ZGC)
        {
            double Interval; //Length of X Axis in number of pixels

            GraphPane myPane = ZGC.MasterPane.PaneList[0]; //To access pane in SetXAxis function
            Point XMax_Pane = new Point(1, 0);
            Point origin = new Point(0, 0);

            if (ZGC == zgc)
            {
                if (TrueTime1)
                {
                    Interval = TimebinFactor * AverageChunkSize1 * 100* Convert.ToDouble(((myPane.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane.GeneralTransform(origin, CoordType.ChartFraction).X)) * XScaleValue1);
                    XMax1 = XMin1 + Interval;
                }
                if (!TrueTime1)
                {
                    Interval = Convert.ToDouble((myPane.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue1);
                    XMax1 = XMin1 + Interval;
                }

                //Sets calculated max/min values to X axis range
                myPane.XAxis.Scale.Min = XMin1; 
                myPane.XAxis.Scale.Max = XMax1;
                myPane.XAxis.Scale.MinorStep = XMax1 / 100.0;
                myPane.XAxis.Scale.MajorStep = XMax1 / 10.0;
            }

            if (ZGC == Window2.zgc2)
            {
                if (TrueTime2)
                {
                    Interval = TimebinFactor * AverageChunkSize2 * 100*Convert.ToDouble(((myPane.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane.GeneralTransform(origin, CoordType.ChartFraction).X)) * XScaleValue2);
                    XMax2 = XMin2 + Interval;
                }
                if (!TrueTime2)
                {
                    Interval = Convert.ToDouble((myPane.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue2);
                    XMax2 = XMin2 + Interval;
                }

                myPane.XAxis.Scale.Min = XMin2;
                myPane.XAxis.Scale.Max = XMax2;
                myPane.XAxis.Scale.MinorStep = XMax2 / 100.0;
                myPane.XAxis.Scale.MajorStep = XMax2 / 10.0;
            }
        }

      
        // Function to set Y Axis (if not using AutoScale)
        public void SetYAxis(ZedGraphControl ZGC)
        {
            GraphPane myPane = ZGC.MasterPane.PaneList[0];

            if (ZGC == zgc)
            {
                //Resets range of ThresholdScrollBar as Y Axis changed
                ThresholdScrollBar1.Maximum = YMax1 + 9;
                ThresholdScrollBar1.Minimum = YMin1;

                //Sets values input into form to Y axis range
                myPane.YAxis.Scale.Min = YMin1;
                myPane.YAxis.Scale.Max = YMax1;
                myPane.YAxis.Scale.MinorStep = YMax1 / 100;
                myPane.YAxis.Scale.MajorStep = YMax1 / 10;
            }
            else if (ZGC == Window2.zgc2)
            {
                Console.WriteLine("In set y axis");
                Window2.ThresholdScrollBar2.Maximum = YMax2 + 9;
                Window2.ThresholdScrollBar2.Minimum = YMin2;

                myPane.YAxis.Scale.Min = YMin2;
                myPane.YAxis.Scale.Max = YMax2;
                myPane.YAxis.Scale.MinorStep = YMax2 / 100;
                myPane.YAxis.Scale.MajorStep = YMax2 / 10;
            }
        }

        //Clears plotting lists but retains savelists. "Reset" clears both.
        public void FreshScreen(ZedGraphControl ZGC)
        {
            if (ZGC == zgc)
            {
                list1.Clear();

                RecentPoint1.Clear();
                RecentPoint2.Clear();
                PastOneScreen1 = false;
                PastOneScreen2 = false;
            }
            else if (ZGC == Window2.zgc2)
            {
                list2.Clear();
                RecentPoint2.Clear();
                PastOneScreen2 = false;
            }
            CreateGraph(ZGC);
        }

        public void WriteBytesToScreen(byte[] DataToWrite)
        {
            for (int j = 0; j < DataToWrite.Length; j++)
            {
                Console.WriteLine("{0}th element is {1}", j, DataToWrite[j]);
            }
            return;
        }


        //Saves raw 100microsecond data that is recieved from FPGA
        public void SaveBytesToFile(string FileLocation)
        {

            string BytesFileLocation = FileLocation + "RawData.txt";
            
            //Add some MetaData at beginning of file
            File.AppendAllText(BytesFileLocation, Convert.ToString(DateTime.Now) + "\r\n");
            File.AppendAllText(BytesFileLocation, Convert.ToString("Total time span of data (s) = " + TimebinFactor * DataBuffer.Length / 10000.0) + "\r\n");
            File.AppendAllText(BytesFileLocation, this.filedescription.Text + "\r\n");
            File.AppendAllText(BytesFileLocation, "Time" + "\t" + "PMT1" + "\t" + "PMT2" + "\r\n");
            
            //Start saving from COPY_POS element as this is least recent value in DataBuffer
            //Even (PMT1) and odd (PMT2) values presented in separate columns in txt file
            for (int i = COPY_POS; i < DataBuffer.Length; i++)
            {
                if (i % 2 == 1)
                {
                    File.AppendAllText(BytesFileLocation, (TimebinFactor * 0.1 * (i - COPY_POS)) / 2 + "\t" + Convert.ToString(DataBuffer[i]) + "\t");
                }
                else if (i % 2 == 0)
                {
                    File.AppendAllText(BytesFileLocation, Convert.ToString(DataBuffer[i]) + "\r\n");
                }

            }
            for (int i = 0; i < COPY_POS; i++)
            {
                if (i % 2 == 1)
                {
                    File.AppendAllText(BytesFileLocation, (TimebinFactor * 0.1 * (i + DataBuffer.Length - COPY_POS)) / 2 + "\t" + Convert.ToString(DataBuffer[i]) + "\t");
                }
                else if (i % 2 == 0)
                {
                    File.AppendAllText(BytesFileLocation, Convert.ToString(DataBuffer[i]) + "\r\n");
                }

            }
        }

        // function to save a given list's values to file
        public void SaveListToFile(string FileLocation, PointPairList list, int Pane)
        {

            MetaData(1)[4] = this.filedescription.Text;

            //Add some MetaData at beginning of file
            File.AppendAllText(FileLocation, MetaData(Pane)[0] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[1] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[2] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[3] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[4] + "\r\n");

            //List converted to array for saving
            double[][] SaveArray = new double[Convert.ToInt32(list.LongCount())][];
            SaveArray[0] = list.Select(P => P.X).ToArray();
            SaveArray[1] = list.Select(P => P.Y).ToArray();

            for (int j = 0; j < SaveArray.Length; j++)
            {
                string Reading = SaveArray[0][j].ToString() + "\t" + SaveArray[1][j] + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }
        }


        // function to save both graphs' data 
        public void SaveBothListsToFile(string FileLocation, PointPairList listA, PointPairList listB)
        {
            MetaData(1)[4] = this.filedescription.Text;
            string LHSfile = FileLocation + "Pane1.txt";
            string RHSfile = FileLocation + "Pane2.txt";

            File.AppendAllText(LHSfile, "Data from Pane1 Graph" + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[0] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[1] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[2] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[3] + "\r\n");
            File.AppendAllText(LHSfile, MetaData(1)[4] + "\r\n");

            // Array of values to be saved (LHS Graph)
            double[][] SaveArray1 = new double[Convert.ToInt32(listA.LongCount())][];
            SaveArray1[0] = listA.Select(P => P.X).ToArray();
            SaveArray1[1] = listA.Select(P => P.Y).ToArray();
            for (int j = 0; j < SaveArray1.Length; j++)
            {
                string Reading = SaveArray1[0][j].ToString() + "\t" + SaveArray1[1][j] + "\r\n";
                File.AppendAllText(LHSfile, Reading);
            }

            File.AppendAllText(RHSfile, "Data from Pane2 Graph" + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[0] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[1] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[2] + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[3] + "Environment time = " + Convert.ToString(Environment.TickCount - tickStart) + "\r\n");
            File.AppendAllText(RHSfile, MetaData(2)[4] + "\r\n");

            double[][] SaveArray2 = new double[Convert.ToInt32(listB.LongCount())][];
            SaveArray2[0] = listB.Select(P => P.X).ToArray();
            SaveArray2[1] = listB.Select(P => P.Y).ToArray();
            for (int j = 0; j < SaveArray2.Length; j++)
            {
                string Reading = SaveArray2[0][j].ToString() + "\t" + SaveArray2[1][j].ToString() + "\r\n";
                File.AppendAllText(RHSfile, Reading);
            }

            Reset(zgc);
            Reset(Window2.zgc2);

        }

        //SaveButton of form triggers dialog box request filename/location
        public void SaveBytesButton_Click(object sender, EventArgs e)
        {

            Pause = true;
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = @"C:\";
            sfd1.Title = "SAVE BITCH";
            sfd1.CheckPathExists = true;
            sfd1.ShowDialog();

            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                if (Save1.Checked && Save2.Checked)
                {
                    SaveBothListsToFile(@sfd1.FileName, savelist1, savelist2);
                }

                else if (!Save2.Checked && Save1.Checked)
                {

                    SaveListToFile(@sfd1.FileName + "Pane1.txt", savelist1, 1);
                    Reset(zgc);

                }
                else if (Save2.Checked && !Save1.Checked)
                {
                    SaveListToFile(@sfd1.FileName + "Pane2.txt", savelist2, 2);
                    Reset(Window2.zgc2);
                }
                WriteBytesToScreen(DataBuffer); //Also writes values to screen
                if (SaveRaw.Checked)
                {
                    SaveBytesToFile(sfd1.FileName);
                }

                Array.Clear(UART_Buffer, 0, UART_Buffer.Length);
                Pause = false;
            }
            else if (sfd1.ShowDialog() == DialogResult.Cancel)
            {
                Pause = false;
            }
            sfd1.Dispose();

        }

        //Background info to be written to start of file
        private string[] MetaData(int pane)
        {
            string[] MetaData = new string[5];
            MetaData[0] = Convert.ToString(DateTime.Now);
            MetaData[1] = "FPGA timebin = " + Convert.ToString(TimebinFactor);

            if (pane == 1)
            {
                MetaData[2] = "Time bin size (ms) = " + Convert.ToString(AvChunkBox1.Value);
                if (IsScrolling1)
                {
                    double TotalTime = list1.LongCount() * AverageChunkSize1 * TimebinFactor / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
                else if (!IsScrolling1)
                {
                    double TotalTime = savelist1.LongCount() * AverageChunkSize1 * TimebinFactor / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
            }
            else if (pane == 2)
            {
                MetaData[2] = "Time bin size (ms) = " + Convert.ToString(Window2.AvChunkBox2.Value);
                if (IsScrolling2)
                {
                    double TotalTime = list2.LongCount() * AverageChunkSize2 * TimebinFactor / (10000.0);
                    double TotalTime2 = tickStart - Environment.TickCount;
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime)/*+"Environment time = " + Convert.ToString(TotalTime2)*/;
                }
                else if (!IsScrolling2)
                {
                    double TotalTime = savelist2.LongCount() * AverageChunkSize2 * TimebinFactor / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
            }

            return MetaData;

        }


        // Function to average bytes in DataBuffer while it isn't filled (still room for UART_Buffer to be coppied in)
        private void AverageDataUnfilled1()
        {
            int SumChunkEven = 0; //Running total over AverageChunkSize to calculate sum/average for PMT1
            int SumChunkOdd = 0; //Running total over AverageChunkSize to calculate sum/average for PMT2
            int NumOfValues = 0; //Number of values summed to calculate mean

            // while loop continues to average blocks of bytes until there aren't enough 'new' bytes to make up an 'AverageChunkSize',
            while (AverageIndex1 * AverageChunkSize1 - AdjustIndex1 < COPY_POS)
            {
                //Sums a chunk from middle of array
                for (int i = Math.Max(0, (AverageIndex1 - 2) * AverageChunkSize1 - AdjustIndex1); i < AverageIndex1 * AverageChunkSize1 - AdjustIndex1; i++) 
                {
                    if (i % 2 == 0)
                    {
                        SumChunkEven += Convert.ToInt16(DataBuffer[i]);
                        NumOfValues++;
                    }
                    else if (i % 2 == 1)
                    {
                        SumChunkOdd += Convert.ToInt16(DataBuffer[i]);
                        NumOfValues++;
                    }
                    //Console.WriteLine("iis{0}", i);
                }
                if (PMT1_pane1 == true && PMT2_pane1 == false)
                {
                    if (AverageBox1.Checked == true)
                    {
                        Average1 = (SumChunkEven + SumProcessedBytes1) / (AverageChunkSize1) * 10; //*10 to give counts per ms
                    }
                    else if (AverageBox1.Checked == false)
                    {
                        Average1 = (SumChunkEven + SumProcessedBytes1);
                    }
                }
                if (PMT2_pane1 == true && PMT1_pane1 == false)
                {
                    if (AverageBox1.Checked == true)
                    {
                        Average1 = (SumChunkOdd + SumProcessedBytes1) / (AverageChunkSize1) * 10;
                    }
                    else if (AverageBox1.Checked == false)
                    {
                        Average1 = (SumChunkOdd + SumProcessedBytes1);
                    }
                }
                if (PMT1_pane1 && PMT2_pane1)
                {
                    if (AverageBox1.Checked == true)
                    {
                        //Divide by twice AverageChunkSize as data from both PMTs summed. SumProcessedBytes is total carried over from AverageScreenFilled
                        Average1 = (SumChunkEven + SumChunkOdd + SumProcessedBytes1) / (2 * AverageChunkSize1) * 10; 
                    }
                    else if (AverageBox1.Checked == false)
                    {
                        Average1 = (SumChunkEven + SumChunkOdd + SumProcessedBytes1);
                    }
                }

                PlotPoint(1); //Adds calculated value to savelist and list for plotting
                zgc.Invalidate(); //Redraw


                if (TrueTime1)
                {
                    time1 += TimebinFactor * AverageChunkSize1*100;
                }
                else if (!TrueTime1)
                {
                    time1++;
                }
                AverageIndex1 += 2; // Average index holds placed of where to start averaging from. +2 to account for both PMT's data
                //Resets running totals in preparation for next time function called
                SumChunkEven = 0;
                SumChunkOdd = 0;
                SumProcessedBytes1 = 0;
            }

        }

        // Function to calculate averages once DataBuffer is full
        private void AverageDataFilled1()
        {
            int SumProcessedBytesEven = 0;
            int SumProcessedBytesOdd = 0;
            int NumProcessedBytes = 0;
            int SumEndChunkOdd = 0;
            int SumEndChunkEven = 0;

            // Ensures averages are plotted if more than AverageChunkSize of array is yet to be averaged
            while (DataBuffer.Length - ((AverageIndex1 - 2) * AverageChunkSize1 - AdjustIndex1) > 2 * AverageChunkSize1) // Same while loop as above function to average remaining bytes that can make up an 'AverageChunkSize'
            {

                for (int i = (AverageIndex1 - 2) * AverageChunkSize1 - AdjustIndex1; i < AverageChunkSize1 * AverageIndex1 - AdjustIndex1; i++)
                {
                    if (i % 2 == 0)
                    {
                        SumEndChunkEven += Convert.ToInt32(DataBuffer[i]);
                    }
                    else if (i % 2 == 1)
                    {
                        SumEndChunkOdd += Convert.ToInt32(DataBuffer[i]);
                    }
                }

                // If loops control which PMT's data is selected
                if (PMT1_pane1 == true && PMT2_pane1 == false)
                {
                    if (AverageBox1.Checked == true)
                    {
                        Average1 = SumEndChunkEven / (AverageChunkSize1) * 10;

                    }
                    else if (AverageBox1.Checked == false)
                    {
                        Average1 = SumEndChunkEven;

                    }
                }
                if (PMT2_pane1 == true && PMT1_pane1 == false)
                {
                    if (AverageBox1.Checked == true)
                    {
                        Average1 = SumEndChunkOdd / (AverageChunkSize1) * 10;

                    }
                    else if (AverageBox1.Checked == false)
                    {
                        Average1 = SumEndChunkOdd;
                    }
                }
                if (PMT1_pane1 && PMT2_pane1)
                {
                    if (AverageBox1.Checked == true)
                    {
                        Average1 = (SumEndChunkEven + SumEndChunkOdd) / (2 * AverageChunkSize1) * 10;

                    }
                    else if (AverageBox1.Checked == false)
                    {
                        Average1 = (SumEndChunkEven + SumEndChunkOdd);
                    }

                }

                PlotPoint(1); //Add to list1 (plotting) and savelist1
                zgc.Invalidate();

                if (TrueTime1)
                {
                    time1 += TimebinFactor * AverageChunkSize1*100;
                }
                else if (!TrueTime1)
                {
                    time1++;
                }
                SumEndChunkEven = 0;
                SumEndChunkOdd = 0;
                AverageIndex1 += 2;
            }

            // For loop to sum remaining bytes in DataBuffer that dont make up whole AverageChunkSize
            for (int i = (AverageIndex1) * AverageChunkSize1 - AdjustIndex1; i < DataBuffer.Length; i++)
            {
                if (i % 2 == 0)
                {
                    SumProcessedBytesEven += Convert.ToInt32(DataBuffer[i]);
                }
                if (i % 2 == 1)
                {
                    SumProcessedBytesOdd += Convert.ToInt32(DataBuffer[i]);
                }
                NumProcessedBytes += 1;
            }
            if (PMT1_pane1 == true && PMT2_pane1 == false)
            {
                SumProcessedBytes1 = SumProcessedBytesEven; //Sum to carry over for next time through AverageScreenUnfilled
            }

            if (PMT2_pane1 == true && PMT1_pane1 == false)
            {
                SumProcessedBytes1 = SumProcessedBytesOdd;
            }

            if (PMT1_pane1 && PMT2_pane1)
            {
                SumProcessedBytes1 = SumProcessedBytesEven + SumProcessedBytesOdd;
            }

            SumProcessedBytesEven = 0;
            SumProcessedBytesOdd = 0;
            AdjustIndex1 = NumProcessedBytes; //Number of bytes included in sum carried over to AverageScreenUnfilled

        }

        // Function to average bytes in DataBuffer while it isn't filled (still room for UART_Buffer to be coppied in)
        private void AverageDataUnfilled2()
        {

            int SumChunkEven = 0;
            int SumChunkOdd = 0;

            // while loop continues to average blocks of bytes untill there aren't enough 'new' bytes to make up an 'AverageChunkSize',
            while (AverageIndex2 * AverageChunkSize2 - AdjustIndex2 < COPY_POS)
            {
                for (int i = Math.Max(0, ((AverageIndex2 - 2) * AverageChunkSize2 - AdjustIndex2)); i < (AverageIndex2 * AverageChunkSize2 - AdjustIndex2); i++) //Sums a chunk from middle of array
                {
                    if (i % 2 == 0)
                    {
                        SumChunkEven += Convert.ToInt32(DataBuffer[i]); //Running total for PMT1 readings
                    }
                    else if (i % 2 == 1)
                    {
                        SumChunkOdd += Convert.ToInt32(DataBuffer[i]); //Running total for PMT2 readings
                    }

                }
                if (PMT1_pane2 && !PMT2_pane2)
                {
                    if (Window2.AverageBox2.Checked == true)
                    {
                        Average2 = (SumChunkEven + SumProcessedBytes2) / (AverageChunkSize2) * 10;//*10 gives average counts per ms
                    }
                    else if (Window2.AverageBox2.Checked == false)
                    {
                        Average2 = (SumChunkEven + SumProcessedBytes2);
                    }
                }
                else if (PMT2_pane2 && !PMT1_pane2)
                {
                    if (Window2.AverageBox2.Checked == true)
                    {
                        Average2 = (SumChunkOdd + SumProcessedBytes2) / (AverageChunkSize2) * 10;
                    }
                    else if (Window2.AverageBox2.Checked == false)
                    {
                        Average2 = (SumChunkOdd + SumProcessedBytes2);
                    }
                }
                else if (PMT1_pane2 && PMT2_pane2)
                {
                    if (Window2.AverageBox2.Checked == true)
                    {
                        Average2 = (SumChunkOdd + SumChunkEven + SumProcessedBytes2) / (2 * AverageChunkSize2) * 10;
                    }
                    else if (Window2.AverageBox2.Checked == false)
                    {
                        Average2 = (SumChunkOdd + SumChunkEven + SumProcessedBytes2);
                    }
                }

                PlotPoint(2); //Add to list2 (plotting) and savelist2
                zgc.Invalidate();

                if (TrueTime2)
                {
                    time2 += TimebinFactor * AverageChunkSize2*100;
                }
                else if (!TrueTime2)
                {
                    time2++;
                }

                AverageIndex2 += 2; // Average index holds place of where to start averaging from. +2 to account for both PMT's data
                //Resets totals to zero in preparation for future use of function
                SumChunkEven = 0;
                SumChunkOdd = 0;
                SumProcessedBytes2 = 0;
            }




        }
        private void AverageDataFilled2()
        {

            int SumProcessedBytesEven = 0;
            int SumProcessedBytesOdd = 0;
            int NumProcessedBytes = 0;
            int SumEndChunkEven = 0;
            int SumEndChunkOdd = 0;

            // Ensures averages are plotted if more than AverageChunkSize of array is yet to be averaged
            // Then sums remaining bytes to be carried over into next loop

            // Same while loop as above function to average remaining bytes that can make up an 'AverageChunkSize'
            while (DataBuffer.Length - ((AverageIndex2 - 2) * AverageChunkSize2 - AdjustIndex2) > 2 * AverageChunkSize2)
            {

                for (int i = ((AverageIndex2 - 2) * AverageChunkSize2 - AdjustIndex2); i < (AverageChunkSize2 * AverageIndex2 - AdjustIndex2); i++)
                {
                    if (i % 2 == 0)
                    {
                        SumEndChunkEven += Convert.ToInt32(DataBuffer[i]);//PMT1 running total
                    }
                    else if (i % 2 == 1)
                    {
                        SumEndChunkOdd += Convert.ToInt32(DataBuffer[i]);//PMT2 running total
                    }
                }

                // If loops control which PMT's data is displayed
                if (PMT1_pane2 && !PMT2_pane2)
                {
                    if (Window2.AverageBox2.Checked == true)
                    {
                        Average2 = SumEndChunkEven / (AverageChunkSize2) * 10;
                    }
                    else if (Window2.AverageBox2.Checked == false)
                    {
                        Average2 = SumEndChunkEven;
                    }
                }
                else if (PMT2_pane2 && !PMT1_pane2)
                {
                    if (Window2.AverageBox2.Checked == true)
                    {
                        Average2 = SumEndChunkOdd / (AverageChunkSize2) * 10;
                    }
                    else if (Window2.AverageBox2.Checked == false)
                    {
                        Average2 = SumEndChunkOdd;
                    }
                }
                else if (PMT1_pane2 && PMT2_pane2)
                {
                    if (Window2.AverageBox2.Checked == true)
                    {
                        Average2 = (SumEndChunkOdd + SumEndChunkEven) / (2 * AverageChunkSize2) * 10;

                    }
                    else if (Window2.AverageBox2.Checked == false)
                    {
                        Average2 = (SumEndChunkOdd + SumEndChunkEven);
                    }
                }

                PlotPoint(2);
                zgc.Invalidate();

                if (TrueTime2)
                {
                    time2 += TimebinFactor * AverageChunkSize2*100; //Incremented by actual time elapsed 
                }
                else if (!TrueTime2)
                {
                    time2++;
                }

                SumEndChunkEven = 0;
                SumEndChunkOdd = 0;
                AverageIndex2 += 2;
            }

            for (int i = ((AverageIndex2 - 2) * AverageChunkSize2 - AdjustIndex2); i < DataBuffer.Length; i++) // For loop to sum remaining bytes in DataBuffer
            {
                if (i % 2 == 0)
                {
                    SumProcessedBytesEven += Convert.ToInt32(DataBuffer[i]);
                }
                else if (i % 2 == 1)
                {
                    SumProcessedBytesOdd += Convert.ToInt32(DataBuffer[i]);
                }
                NumProcessedBytes++;
            }

            if (PMT1_pane2 && !PMT2_pane2)
            {
                SumProcessedBytes2 = SumProcessedBytesEven;
            }

            else if (PMT2_pane2 && !PMT1_pane2)
            {
                SumProcessedBytes2 = SumProcessedBytesOdd;
            }

            else if (PMT1_pane2 && PMT2_pane2)
            {
                SumProcessedBytes2 = SumProcessedBytesOdd + SumProcessedBytesEven;

            }

            SumProcessedBytesOdd = 0;
            SumProcessedBytesEven = 0;
            AdjustIndex2 = NumProcessedBytes;
        }


        // Deletes all data and sets time=0
        private void Reset(ZedGraphControl ZGC)
        {
            if (ZGC == zgc)
            {
                list1.Clear();
                savelist1.Clear();
                RecentPoint1.Clear();
                PastOneScreen1 = false;
                time1 = 0;
                AdditionalTime1 = 0;
            }
            else if (ZGC == Window2.zgc2)
            {
                list2.Clear();
                savelist2.Clear();
                RecentPoint2.Clear();
                PastOneScreen2 = false;
                time2 = 0;
                AdditionalTime2 = 0;
            }
            CreateGraph(ZGC);
        }

        // Takes averages calculated with averaging funtions and adds them the appropriate lists for plotting.
        private void PlotPoint(int Pane)
        {
            if (Pane == 1)
            {
                if (IsScrolling1)
                {
                    list1.Add(time1, Average1);
                    savelist1.Add(time1, Average1);
                }
                if (!IsScrolling1)
                {
                    list1.Add(time1, Average1);
                    //time1 is how long elapsed since beginning current screen's worth of plotting.
                    //AdditionalTime1 is how long elapsed prior to current screen.
                    savelist1.Add(time1 + AdditionalTime1, Average1); 
                    
                    if (time1 > Convert.ToInt32(XMax1)) //Reached end of X axis
                    {
                        AdditionalTime1 += Convert.ToInt32(XMax1);
                        time1 = 0;
                        PastOneScreen1 = true;
                    }

                    //If run through one screen or more, delete oldest value
                    if (PastOneScreen1 && time1 <= list1.ElementAt(0).X)
                    {
                        list1.RemoveAt(0);
                    }

                    //Removes all values outside visible range
                    while (list1.ElementAt(0).X >= XMax1)
                    {
                        list1.RemoveAt(0);
                    }
                }

                RecentPoint1.Add(time1, Average1);
                if (RecentPoint1.LongCount() > 1)
                {
                    RecentPoint1.RemoveAt(0);
                }
            }

            if (Pane == 2)
            {
                if (IsScrolling2)
                {
                    list2.Add(time2, Average2);
                    savelist2.Add(time2, Average2);
                }
                if (!IsScrolling2)
                {
                    list2.Add(time2, Average2);
                    //time2 is how long elapsed since beginning current screen's worth of plotting.
                    //AdditionalTime2 is how long elapsed prior to current screen.
                    savelist2.Add(time2 + AdditionalTime2, Average2);
                    
                    //Reset time to zero if reached end of x axis
                    if (time2 > XMax2) 
                    {
                        AdditionalTime2 += Convert.ToInt32(XMax2);
                        time2 = 0;
                        PastOneScreen2 = true;
                    }

                    //If passed through one screen (or more) delete oldest value from list
                    if (PastOneScreen2 && time2 <= list2.ElementAt(0).X)
                    {
                        list2.RemoveAt(0);
                    }

                    //Delete all values outide visible range
                    while (list2.ElementAt(0).X >= XMax2)
                    {
                        list2.RemoveAt(0);
                    }
                }
                RecentPoint2.Add(time2, Average2);

                //Keeps red bar for current fluorescence at one point
                if (RecentPoint2.LongCount() > 1)
                {
                    RecentPoint2.RemoveAt(0);
                }
            }
        }

        //Flags up warning if count rate difference between PMTs varies by more than half of average
        private void PMTCompare()
        {
            Console.WriteLine("PMT Compare");
            int PMT1Check = 0;
            int PMT2Check = 0;
            
            //Sums first 500 values for each PMT from DataBuffer 
            for (int i = 0; i < 1000; i++)
            {
                if (i % 2 == 0)
                {
                    PMT1Check += DataBuffer[i];
                }
                if (i % 2 == 1)
                {
                    PMT2Check += DataBuffer[i];
                }
            }

            double FracDiff = Math.Abs(PMT1Check - PMT2Check) / ((PMT1Check + PMT2Check) / 2);
            
            if (FracDiff >= 0.5)
            {
                Console.WriteLine("PMT ERROR!!!");
                Pause = true;
                
                //Brings up pop up warning if PMTs vary by more than specified fraction
                PopUpWarning popup = new PopUpWarning();
                DialogResult dialogresult = popup.ShowDialog();
                if (dialogresult == DialogResult.OK)
                {
                    if (popup.IsChecked() == true)
                    {
                        StopWarning = true;
                    }
                    else
                    {
                        StopWarning = false;
                    }
                    Console.WriteLine("OK");
                    Pause = false;
                }
                popup.Dispose();
            }
            FPGA.EmptyBuffer(); //Clears huge number of bytes that will have built up in UART buffer
        }

        //Compares most recent value in savelist to user-defined threshold. If below threshold, program paused.
        private void CheckIonTrapped()
        {
            int CheckIonSum1 = 0;
            int CheckIonSum2 = 0;
            if (savelist1.LongCount() != 0)
            {
                CheckIonSum1 += Convert.ToInt32(savelist1.ElementAt(Convert.ToInt32(savelist1.LongCount() - 1)).Y); //Should these be savelist or DataBuffer
            }
            double CheckIonAverage1 = CheckIonSum1;

            if (savelist2.LongCount() != 0)
            {
                CheckIonSum2 += Convert.ToInt32(savelist2.ElementAt(Convert.ToInt32(savelist2.LongCount() - 1)).Y);
            }
            double CheckIonAverage2 = CheckIonSum2;


            if ((CheckIonAverage2 < ThresholdLineValue2) && (CheckIonAverage1 < ThresholdLineValue1))
            {
                Pause = true;
                PauseCheck1.Checked = true;
                Console.WriteLine("CheckIonSum 1 and 2 are {0}, {1}", CheckIonSum1, CheckIonSum2);
            }
        }

        //Functions below for buttons/checkboxes in the form to access elements of the code.

        //Button to trigger FreshScreen function
        public void FreshScreenButton1_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                FreshScreen(zgc);
            }
            return;
        }
        public void FreshScreenButton2_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                this.FreshScreen(Window2.zgc2);
            }
            return;
        }

        public void PauseCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PauseCheck1.Checked)
            {
                Pause = true;
            }
            else if (!PauseCheck1.Checked)
            {
                Pause = false;
                FPGA.EmptyBuffer();//Clears UART buffer else there are too many values to process in one timertick
            }

        }


        // Can choose location and file name of file to save
        public void filename_TextChanged(object sender, EventArgs e)
        {
            this.Text = filedescription.Text;
            FileLocation = this.Text;
        }


        //Extracts value of slider to "slider" integer in main code (Used as replacement for incoming bytes)
        /* public void trackBar1_Scroll(object sender, EventArgs e)
         {
             slider = trackBar1.Value;
         }*/


        // Used to alter X Axis range myPane1, adjusts bar widths accordingly to prevent gaps between bars
        public void XScale1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ZoomIn1.Checked)
            {
                //XScale1.Maximum = 100;
                XScaleValue1 = Convert.ToDouble(1 / XScale1.Value);
                BarItem curve = zgc.MasterPane.PaneList[0].CurveList[2] as BarItem;
                curve.Bar.Border.Width = (float)(XScale1.Value);
                BarItem RecentBar1 = zgc.MasterPane.PaneList[0].CurveList[1] as BarItem;
                RecentBar1.Bar.Border.Width = (float)(XScale1.Value);
            }
            else if (!ZoomIn1.Checked)
            {
                XScaleValue1 = Convert.ToDouble(XScale1.Value);
            }

            SetXAxis(zgc);
        }

        // Used to alter X Axis range myPane2, adjusts bar widths accordingly to prevent gaps between bars
        public void XScale2_ValueChanged(object sender, System.EventArgs e)
        {
            Console.WriteLine("Into XScale2 Value Changed");
            if (Window2.ZoomIn2.Checked)
            {
                //XScale2.Maximum = 100;
                XScaleValue2 = Convert.ToDouble(1 / Window2.XScale2.Value);

                BarItem curve2 = Window2.zgc2.MasterPane.PaneList[0].CurveList[2] as BarItem;
                curve2.Bar.Border.Width = (float)(Window2.XScale2.Value);
                BarItem RecentBar2 = Window2.zgc2.MasterPane.PaneList[0].CurveList[1] as BarItem;
                RecentBar2.Bar.Border.Width = (float)(Window2.XScale2.Value);

            }
            else if (!Window2.ZoomIn2.Checked)
            {
                XScaleValue2 = Convert.ToDouble(Window2.XScale2.Value);
            }
            SetXAxis(Window2.zgc2);
        }

        // Used to change Y axis on both myPane1
        public void YMaxNum1_ValueChanged(object sender, System.EventArgs e)
        {
            YMax1 = Convert.ToInt32(YMaxNum1.Value);
            SetYAxis(zgc);
        }

        public void YMinNum1_ValueChanged(object sender, System.EventArgs e)
        {
            YMin1 = Convert.ToInt32(YMinNum1.Value);
            SetYAxis(zgc);
        }
        
        // Used to change Y axis on both myPane2
        public void YMaxNum2_ValueChanged(object sender, System.EventArgs e)
        {
            if (Window2.AutoScale2.Checked)
            {
            }
            else
            {
                YMax2 = Convert.ToInt32(Window2.YMaxNum2.Value);
                SetYAxis(Window2.zgc2);
                Console.WriteLine(YMax2);
            }
        }

        public void YMinNum2_ValueChanged(object sender, System.EventArgs e)
        {
            YMin2 = Convert.ToInt32(Window2.YMinNum2.Value);
            SetYAxis(Window2.zgc2);
        }

        // Changes no of values to average over, resets everything and starts plotting
        // from scratch with new average
        public void AvChunkBox1_ValueChanged(object sender, System.EventArgs e)
        {
            //AvChunkSize2.Value = AverageChunkSize2 * TimebinFactor / 10; // WHAT!!

            int AverageChunkSize1_Old = AverageChunkSize1;
            //AvChunkBox1.Value is ms timebin to average over. *10/TimebinFactor to convert to number of values 
            //in DataBuffer to average over.
            AverageChunkSize1 = Convert.ToInt32(Math.Round(Convert.ToDouble(AvChunkBox1.Value) * 10.0 / TimebinFactor));
            //Changes AdjustIndex to ensure averages calculated from same point in DataBuffer
            AdjustIndex1 = AverageIndex1 * (AverageChunkSize1 - AverageChunkSize1_Old) + AdjustIndex1;
            Console.WriteLine("ValueChanged1");
            Console.WriteLine("Average chunk 1 = {0}", AverageChunkSize1);
            Reset(zgc);
        }


        // Same as above but for myPane2
        public void AvChunkBox2_ValueChanged(object sender, System.EventArgs e)
        {
            int AverageChunkSize2_Old = AverageChunkSize2;
            //AvChunkBox2.Value is ms timebin to average over. *10/TimebinFactor to convert to number of values 
            //in DataBuffer to average over.
            AverageChunkSize2 = Convert.ToInt32(Math.Round(Convert.ToDouble(Window2.AvChunkBox2.Value) * 10.0 / TimebinFactor));
            //Changes AdjustIndex to ensure averages calculated from same point in DataBuffer
            AdjustIndex2 = AverageIndex2 * (AverageChunkSize2 - AverageChunkSize2_Old) + AdjustIndex2;
            Console.WriteLine("ValueChanged2");
            Console.WriteLine("Average chunk 2 = {0}", AverageChunkSize2);
            Reset(Window2.zgc2);
        }

        //Takes FPGATimebin input from user and adjusts timebinfactor accordingly
        public void FPGATimebin_ValueChanged(object sender, EventArgs e)
        {
            TimebinFactor = Convert.ToInt16(FPGATimebin.Value * 10);

            AvChunkBox1.Minimum = Convert.ToDecimal(0.1 * TimebinFactor);
            Window2.AvChunkBox2.Minimum = Convert.ToDecimal(0.1 * TimebinFactor);

            //Adjusts minimum plotting timebins so they cannot be a fraction of an FPGA timebin
            AvChunkBox1_ValueChanged(sender, e);
            AvChunkBox2_ValueChanged(sender, e);

            Reset(zgc);
            Reset(Window2.zgc2);

        }


        //Choice between scrolling/cyclic plotting
        public void ScrollingCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null) IsScrolling1 = !IsScrolling1;
            if (ScrollingCheckBox1.Checked)
            {
                time1 += AdditionalTime1;
            }
            else if (!ScrollingCheckBox1.Checked)
            {
                AdditionalTime1 = time1;
                time1 = 0;
                list1.Clear();
                PastOneScreen1 = false;
            }

            Console.WriteLine("IsScrolling is {0}", IsScrolling1);
        }
        public void ScrollingCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null) IsScrolling2 = !IsScrolling2;
            if (Window2.ScrollingCheckBox2.Checked)
            {
                time2 += AdditionalTime2;
            }
            else if (!Window2.ScrollingCheckBox2.Checked)
            {
                AdditionalTime2 = time2;
                time2 = 0;
                list2.Clear();
                PastOneScreen2 = false;
            }

            Console.WriteLine("IsScrolling2 is {0}", IsScrolling2);
        }
       
        //Dropdown boxes to select PMT
        public void PMTSelect1_TextChanged(object sender, EventArgs e)
        {
            if (PMT1.Text == "PMT1")
            {
                PMT1_pane1 = true;
                PMT2_pane1 = false;
                Console.WriteLine("PMT1p1 = {0} and PMT2p1 = {1}", PMT1_pane1, PMT2_pane1);
            }

            else if (PMT1.Text == "PMT2")
            {
                PMT1_pane1 = false;
                PMT2_pane1 = true;
                Console.WriteLine("PMT1p1 = {0} and PMT2p1 = {1}", PMT1_pane1, PMT2_pane1);
            }
            else if (PMT1.Text == "Both")
            {
                PMT1_pane1 = true;
                PMT2_pane1 = true;
                Console.WriteLine("PMT1p1 = {0} and PMT2p1 = {1}", PMT1_pane1, PMT2_pane1);
            }

            FreshScreen(zgc);
        }
        public void PMTSelect2_TextChanged(object sender, EventArgs e)
        {
            if (Window2.PMT2.Text == "PMT1")
            {
                PMT1_pane2 = true;
                PMT2_pane2 = false;
            }
            if (Window2.PMT2.Text == "PMT2")
            {
                PMT1_pane2 = false;
                PMT2_pane2 = true;
            }
            if (Window2.PMT2.Text == "Both")
            {
                PMT1_pane2 = true;
                PMT2_pane2 = true;
            }
            FreshScreen(Window2.zgc2);
        }

        //Checkbox to choose whether actual time values plotted on x axis
        public void TimeControl1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (TimeControl1.Checked)
            {
                TrueTime1 = true;
                
                //Rescales all values of list so consistent with "TrueTime" X axis values.
                for (int i = 0; i < list1.LongCount(); i++)
                {
                    list1.ElementAt(i).X = (list1.ElementAt(i).X) * AverageChunkSize1*TimebinFactor*100;
                }
                for (int i = 0; i < savelist1.LongCount(); i++)
                {
                    savelist1.ElementAt(i).X = (savelist1.ElementAt(i).X) * AverageChunkSize1*TimebinFactor*100;
                }

                //Updates current time value
                time1 = time1 * AverageChunkSize1*TimebinFactor*100;
            }
            if (!TimeControl1.Checked)
            {
                TrueTime1 = false;

                //Rescales all values of list so consistent with NOT "TrueTime" X axis values.
                for (int i = 0; i < list1.LongCount(); i++)
                {
                    list1.ElementAt(i).X = list1.ElementAt(i).X / (AverageChunkSize1*TimebinFactor*100);
                }
                for (int i = 0; i < savelist1.LongCount(); i++)
                {
                    savelist1.ElementAt(i).X = savelist1.ElementAt(i).X / (AverageChunkSize1*TimebinFactor*100);
                }

                //Updates current time value
                time1 = time1 / (AverageChunkSize1*TimebinFactor*100);
            }
            SetXAxis(zgc);
            Console.WriteLine("TrueTime1 = {0}", TrueTime1);
        }

        //Checkbox to choose whether actual time values plotted on x axis
        public void TimeControl2_CheckedChanged(object sender, EventArgs e)
        {
            if (Window2.TimeControl2.Checked)
            {
                TrueTime2 = true;

                //Rescales all values of list so consistent with "TrueTime" X axis values.
                for (int i = 0; i < list2.LongCount(); i++)
                {
                    list2.ElementAt(i).X = (list2.ElementAt(i).X) * (AverageChunkSize2*TimebinFactor*100);
                }
                for (int i = 0; i < savelist2.LongCount(); i++)
                {
                    savelist2.ElementAt(i).X = (savelist2.ElementAt(i).X) * (AverageChunkSize2*TimebinFactor*100);
                }

                //Updates current time value
                time2 = time2 * AverageChunkSize2*TimebinFactor;
            }
            if (!Window2.TimeControl2.Checked)
            {
                TrueTime2 = false;

                //Rescales all values of list so consistent with "TrueTime" X axis values.
                for (int i = 0; i < list2.LongCount(); i++)
                {
                    list2.ElementAt(i).X = (list2.ElementAt(i).X) / (AverageChunkSize2*TimebinFactor*100);
                }
                for (int i = 0; i < savelist2.LongCount(); i++)
                {
                    savelist2.ElementAt(i).X = (savelist2.ElementAt(i).X) / (AverageChunkSize2*TimebinFactor*100);
                }

                //Updates current timevalue
                time2 = time2 / (AverageChunkSize2*TimebinFactor*100);
            }

            SetXAxis(Window2.zgc2);
            Console.WriteLine("TrueTime2 = {0}", TrueTime2);
        }

        public void ResetButton1_Click(object sender, EventArgs e)
        {
            Reset(zgc);
        }

        public void ResetButton2_Click(object sender, EventArgs e)
        {
            Reset(Window2.zgc2);
        }

        //ScrollBar for user to choose threshold count rate on Pane1
        public void ThresholdScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //ThresholdScrollBar.Value is 0 at top, so have to invert input for graphical sense
            ThresholdLineValue1 = YMax1 - ThresholdScrollBar1.Value;
        }
        
        //ScrollBar for user to choose threshold count rate on Pane 2
        public void ThresholdScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            //ThresholdScrollBar.Value is 0 at top, so have to invert input for graphical sense
            ThresholdLineValue2 = YMax2 - Window2.ThresholdScrollBar2.Value;
        }

        //Choice of whether to use Threshold function
        public void ThresholdCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ThresholdCheckBox1.Checked)
            {
                ThresholdScrollBar1.Visible = true;
                zgc.MasterPane.PaneList[0].CurveList[0].IsVisible = true;
            }
            else if (!ThresholdCheckBox1.Checked)
            {
                ThresholdScrollBar1.Visible = false;
                zgc.MasterPane.PaneList[0].CurveList[0].IsVisible = false;
                ThresholdLineValue1 = 0;
            }

        }

        //Choice of whether to use Threshold function
        public void ThresholdCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (Window2.ThresholdCheckBox2.Checked)
            {
                Window2.ThresholdScrollBar2.Visible = true;
                Window2.zgc2.MasterPane.PaneList[0].CurveList[0].IsVisible = true;
            }
            else if (!Window2.ThresholdCheckBox2.Checked)
            {
                Window2.ThresholdScrollBar2.Visible = false;
                Window2.zgc2.MasterPane.PaneList[0].CurveList[0].IsVisible = false;
                ThresholdLineValue2 = 0;
            }

        }

        public void filedescription_TextChanged(object sender, EventArgs e)
        {

        }
       
        //Choice of whether to view buttons or make buttons invisible and 
        //have graph take up whole window
        public void ButtonsVisible1_CheckedChanged(object sender, EventArgs e)
        {
            if (ButtonsVisible1.Checked)
            {
                ThresholdCheckBox1.Visible = true;
                TimeControl1.Visible = true;
                ResetButton1.Visible = true;
                FreshScreenButton1.Visible = true;
                YMaxNum1.Visible = true;
                YMinNum1.Visible = true;
                PMT1.Visible = true;
                ScrollingCheckBox1.Visible = true;
                AvChunkBox1.Visible = true;
                SaveBytesButton.Visible = true;
                XScale1.Visible = true;
                ZoomIn1.Visible = true;
                PauseCheck1.Visible = true;
                filedescription.Visible = true;
                AutoScale1.Visible = true;
                SaveRaw.Visible = true;
                Save2.Visible = true;
                Save1.Visible = true;
                label2.Visible = true;
                label8.Visible = true;
                label10.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true;
                AverageBox1.Visible = true;
                FPGATimebin.Visible = true;

            }

            else if (!ButtonsVisible1.Checked)
            {
                ThresholdCheckBox1.Visible = false;
                ThresholdCheckBox1.Visible = false;
                TimeControl1.Visible = false;
                ResetButton1.Visible = false;
                FreshScreenButton1.Visible = false;
                YMaxNum1.Visible = false;
                YMinNum1.Visible = false;
                PMT1.Visible = false;
                ScrollingCheckBox1.Visible = false;
                AvChunkBox1.Visible = false;
                SaveBytesButton.Visible = false;
                XScale1.Visible = false;
                ZoomIn1.Visible = false;
                PauseCheck1.Visible = false;
                filedescription.Visible = false;
                AutoScale1.Visible = false;
                SaveRaw.Visible = false;
                Save2.Visible = false;
                Save1.Visible = false;
                label2.Visible = false;
                label8.Visible = false;
                label10.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
                AverageBox1.Visible = false;
                FPGATimebin.Visible = false;
            }
            SetSize(zgc);
        }
        public void ButtonsVisible2_CheckedChanged(object sender, EventArgs e)
        {
            if (Window2.ButtonsVisible2.Checked)
            {
                Window2.AverageBox2.Visible = true;
                Window2.ScrollingCheckBox2.Visible = true;
                Window2.AutoScale2.Visible = true;
                Window2.TimeControl2.Visible = true;
                Window2.ThresholdCheckBox2.Visible = true;
                Window2.ResetButton2.Visible = true;
                Window2.FreshScreenButton2.Visible = true;
                Window2.YMinNum2.Visible = true;
                Window2.YMaxNum2.Visible = true;
                Window2.AvChunkBox2.Visible = true;
                Window2.XScale2.Visible = true;
                Window2.ZoomIn2.Visible = true;
                Window2.PMT2.Visible = true;
                Window2.label10_2.Visible = true;
                Window2.label12_2.Visible = true;
                Window2.label13_2.Visible = true;
                Window2.label14_2.Visible = true;
                Window2.label15_2.Visible = true;
            }

            else if (!Window2.ButtonsVisible2.Checked)
            {
                Console.WriteLine("Unchecked");
                Window2.AverageBox2.Visible = false;
                Window2.ScrollingCheckBox2.Visible = false;
                Window2.AutoScale2.Visible = false;
                Window2.TimeControl2.Visible = false;
                Window2.ThresholdCheckBox2.Visible = false;
                Window2.ResetButton2.Visible = false;
                Window2.FreshScreenButton2.Visible = false;
                Window2.YMinNum2.Visible = false;
                Window2.YMaxNum2.Visible = false;
                Window2.AvChunkBox2.Visible = false;
                Window2.XScale2.Visible = false;
                Window2.ZoomIn2.Visible = false;
                Window2.PMT2.Visible = false;
                Window2.label10_2.Visible = false;
                Window2.label12_2.Visible = false;
                Window2.label13_2.Visible = false;
                Window2.label14_2.Visible = false;
                Window2.label15_2.Visible = false;
            }
            SetSize(Window2.zgc2);
        }

        //Choice between autoscaling Y axis or using range set by user.
        //Threshold function disabled if either (or both) of graphs set to autoscale
        public void AutoScale1_CheckedChanged(object sender, EventArgs e)
        {
            CreateGraph(zgc);
            if (!AutoScale1.Checked)
            {
                ThresholdScrollBar1.Visible = true;
                ThresholdLineValue1=0;
                SetYAxis(zgc);
            }          
        }
        public void AutoScale2_CheckedChanged(object sender, EventArgs e)
        {
            CreateGraph(Window2.zgc2);
            if (!Window2.AutoScale2.Checked)
            {
               Window2.ThresholdScrollBar2.Visible = true;
               SetYAxis(Window2.zgc2);
               ThresholdLineValue2 = 0;
            }
        }


    }
}