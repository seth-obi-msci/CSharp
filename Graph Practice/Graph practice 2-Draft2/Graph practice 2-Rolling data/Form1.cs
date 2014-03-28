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
        int AdditionalTime1 = 0;
        int AdditionalTime2 = 0;


        double Average1;
        int SumProcessedBytes1 = 0;
        int AdjustIndex1 = 0;
        int AverageIndex1 = 1;
        int AverageChunkSize1;


        double Average2;
        int SumProcessedBytes2=0;
        int AdjustIndex2 = 0;
        int AverageIndex2 = 1;
        int AverageChunkSize2;


        int[] AverageArray2 = new int[3];
        int[] AverageArray1 = new int[3];
    
        double XMin1=0;
        double XMax1;
        double XScaleValue1;
        double XMin2=0;
        double XMax2;
        double XScaleValue2;


        int YMin1;
        int YMax1=3000;
        int YMin2;
        int YMax2 = 3000;
        double ThresholdLineValue1=0;
        double ThresholdLineValue2=0;

        //int NumValuesToScreen = 300; //Number of Values in point pair list and number of elements in DataBuffer array
        
        int COPY_POS = 0;  //Pointer to element in DataBuffer to copy UART_Buffer to

        int l = 0; //Index for clock divider "for" loop used for byte simulation 

        //Booleans used in the form for various functions 
        bool AutoSaveBool = false;
        bool Pause = false;
        bool LHSSaveBool;
        bool RHSSaveBool;
        bool IsScrolling;
        bool PastOneScreen1 = false;
        bool PastOneScreen2 = false;
        bool PMT1_pane1=true;
        bool PMT2_pane1=true;
        bool PMT1_pane2=true;
        bool PMT2_pane2=true;
        bool ThresholdBool = true;
        bool StopWarning = false;

        bool TrueTime1 = false;
        bool TrueTime2 = false;

        int TimebinFactor=1; 
        

        // Various lists containing data to be plotted or saved
        PointPairList templist1 = new PointPairList();
        PointPairList templist2 = new PointPairList();
        PointPairList savelist1 = new PointPairList();
        PointPairList savelist2 = new PointPairList();
        PointPairList list1 = new PointPairList();
        PointPairList list2 = new PointPairList();
        PointPairList RecentPoint1 = new PointPairList();
        PointPairList RecentPoint2 = new PointPairList();

        PointPairList ThresholdList1 = new PointPairList();
        PointPairList ThresholdList2 = new PointPairList();


        //byte arrays
        byte[] SimulatedBytes = new byte [10];
        byte[] DataBuffer = new byte[30000];
        byte[] UART_Buffer;
        byte[] Plotting_1 = new byte[1];

       
        public RollingGraph()
        {
            InitializeComponent();
            
        }


        private void RollingGraph_Load(object sender, EventArgs e)
        {
            CreateGraph(zgc);

            //Timer to trigger timer1_Tick function
            timer1.Interval = 1;
            timer1.Enabled = true;
            timer1.Start();
            //Save begging time for reference
            tickStart = Environment.TickCount;
            Console.WriteLine("Load");
        }
        
        //l is clock divider index so 10 bytes can be built up before being read and plotted. Simulates bytes building up in UART buffer
        public void timer1_Tick(object sender, EventArgs e)
        {
           
           GraphPane myPane1 = zgc.MasterPane.PaneList[0];
           GraphPane myPane2 = zgc.MasterPane.PaneList[1];

           
           //zgc.AxisChange();
           SetSize();

           
            //Set X axis each timer.tick 
            SetXAxis1();
            SetXAxis2();

            int t = (Environment.TickCount - tickStart);
           
            if (Pause) //Ensures points only plotted if stop button NOT pressed on form
            {
                
            }
            else
            {
                l++;
                // Simulated bytes used when working away from FPGA
                if (l % 2 == 0)
                {
                    SimulatedBytes[l%10] = Convert.ToByte(trackBar1.Value);
                }
                else if (l % 2 == 1)
                {
                    SimulatedBytes[l%10] = Convert.ToByte(0);
                }
                if (l % 10 == 9) //Only reads and plots once every 10 ticks*/
                {   
               if (l % 100 == 99 && !StopWarning)
               {
                   
                   PMTCompare();
               }
                

                    UART_Buffer = SimulatedBytes;
                    Console.WriteLine("Bytes in buffer = {0}", UART_Buffer.Length);

                    // Ensures there's at least one curve in GraphPane
                    if (zgc.GraphPane.CurveList.Count <= 0)
                        return;

                    // If list is null then do nothing
                    if (list2 == null || list1 == null)
                    {
                        return;
                    }

                    if (templist2 == null || templist1 == null)
                    {
                        return;
                    }

                    // Gives milliseconds elapsed since starting program
                    double time = (Environment.TickCount - tickStart) / 1000.0;

                    XAxisScroll();

                    //Automatically rescales Y-axis if each timer.tick if Autoscale selected
                    if (AutoScale.Checked == true)
                    {
                        zgc.AxisChange();
                    }
                    if (AutoScale.Checked == false)
                    {
                        ThresholdScrollBar1.Maximum = YMax1;
                        ThresholdScrollBar2.Maximum = YMax2;
                        ThresholdScrollBar1.Minimum = YMin1;
                        ThresholdScrollBar2.Minimum = YMin2;
                    }

                    zgc.Invalidate();
                   
                    //Remaining spaces in DataBuffer
                    int Diff = DataBuffer.Length - COPY_POS;


                    /// Condition satisfied when there's enough room for a UART_Buffer array to be
                    ///  copied into the Screen array. 
                    if (Diff>UART_Buffer.Length)
                    {
                        // Copies UART_Buffer=FPGA.ReadBytes into Screen array, overwriting oldest values
                        Array.Copy(UART_Buffer, 0, DataBuffer, COPY_POS, UART_Buffer.Length); 

                        //COPY_POS shifted by UART buffer size, so new values are copied into correct position.
                        COPY_POS = COPY_POS + Convert.ToInt32(UART_Buffer.Length);


                        AverageDataUnfilled1();
                        AverageDataUnfilled2(); 

                        CheckIonTrapped();
                    }

                    //Not enough room at end of DataBuffer for whole UART bufferload
                    else 
                    {
                       
                        /// Fills remaining DataBuffer spaces with portion of UART buffer load. 
                        /// Remaining UART_Buffer bytes that dont fit copied in at start of DataBuffer
                        Array.Copy(UART_Buffer, 0, DataBuffer, COPY_POS, DataBuffer.Length - COPY_POS);

                        /// AverageDataFilled used to find averages of end values in DataBuffer
                        /// Must be able to make up average by looping round from end to beggining values of DataBuffer
                        AverageDataFilled2();
                        AverageDataFilled1();

                        // Indexes set to beginning so that next timer tick averaging begins at start of DataBuffer (most recent values)
                        AverageIndex2 = 2;
                        AverageIndex1 = 2;

                        //Remaining of UART bufferload copied to first spaces in DataBuffer and COPY_POS shifted accordingly
                        Array.Copy(UART_Buffer, DataBuffer.Length - COPY_POS, DataBuffer, 0, UART_Buffer.Length - (DataBuffer.Length - COPY_POS));
                        COPY_POS = UART_Buffer.Length - (DataBuffer.Length - COPY_POS);
                        
                    }
                }
            }

            // Lists for escaped ion detection thresholds
            ThresholdList1.Add(XMin1, ThresholdLineValue1);
            ThresholdList1.Add(XMax1, ThresholdLineValue1);

            ThresholdList2.Add(XMin2, ThresholdLineValue2);
            ThresholdList2.Add(XMax2, ThresholdLineValue2);

            // Controlling length of lists
            if (list1.LongCount() > 10000)
            {
                list1.RemoveRange(0, Convert.ToInt32(list1.LongCount() - 10000));
            }
            if (list2.LongCount() > 10000)
            {
                list2.RemoveRange(0, Convert.ToInt32(list2.LongCount() - 10000));
            }
            if (savelist1.LongCount() > 10000)
            {
                savelist1.RemoveRange(0, Convert.ToInt32(savelist1.LongCount() - 10000));
            }
            if (savelist2.LongCount() > 10000)
            {
                savelist2.RemoveRange(0, Convert.ToInt32(savelist2.LongCount() - 10000));
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


        //
        //Below are functions which have been outsourced for clarity in main code (above)
        //


        public void CreateGraph(ZedGraph.ZedGraphControl zgc)
        {
            zgc.MasterPane.PaneList.Clear();

            AverageChunkSize1 = Convert.ToInt32(Math.Round(Convert.ToDouble(AvChunkBox1.Value) * 10.0  / TimebinFactor));
            AverageChunkSize2 = Convert.ToInt32(Math.Round(Convert.ToDouble(AvChunkBox2.Value) * 10.0  / TimebinFactor));
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


            LineItem ThresholdLine1 = zgc.MasterPane.PaneList[0].AddCurve("Threshold1", ThresholdList1, Color.DarkOliveGreen, SymbolType.None);
            ThresholdLine1.Line.Width = 2.0F;
            LineItem ThresholdLine2 = zgc.MasterPane.PaneList[1].AddCurve("Threshold2", ThresholdList2, Color.DarkGoldenrod, SymbolType.None);
            ThresholdLine2.Line.Width = 2.0F;


            SetBarProperties(1, Color.White, 1.0F, Color.Red, 10.0F);
            SetBarProperties(2, Color.White, 1.0F, Color.Red, 10.0F);

            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = this.CreateGraphics())
            {
                zgc.MasterPane.SetLayout(g, PaneLayout.SingleRow);
            }


            //Function to set axes of graph panes.
            zgc.AxisChange();
            SetXAxis1();
            SetXAxis2();
            if (AutoScale.Checked == false)
            {
                SetYAxis1();
                SetYAxis2();
                ThresholdScrollBar1.Maximum = YMax1;
                ThresholdScrollBar2.Maximum = YMax2;
                ThresholdScrollBar1.Minimum = YMin1;
                ThresholdScrollBar2.Minimum = YMin2;
            }
            ThresholdScrollBar1.Value = ThresholdScrollBar1.Maximum;
            ThresholdScrollBar2.Value = ThresholdScrollBar2.Maximum;
            zgc.AxisChange();

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
                BarItem RecentBar2 = zgc.MasterPane.PaneList[1].AddBar("RecentBar", RecentPoint2, Color.Red);
                RecentBar2.Bar.Border = new Border(Color.Red, 10.0F);

                BarItem Bars2 = zgc.MasterPane.PaneList[1].AddBar("Counts (Avg ten)", list2, Color.White);
                Bars2.Bar.Border = new Border(Color.White, 1.0F);
            }

        }

        // Function to move X-axis along for scrolling effect.
        private void XAxisScroll()
        {

            Scale xScale1 = zgc.MasterPane.PaneList[0].XAxis.Scale;
            Scale xScale2 = zgc.MasterPane.PaneList[1].XAxis.Scale;
            

            // if loops used to shift x axis for each graph
            if (IsScrolling)
            {
                if (time1 > xScale1.Max - xScale1.MajorStep) // When the time values are within one 'MajorStep' of the max x value
                {
                    xScale1.Max = time1 + xScale1.MajorStep; //Keep the end of x axis MajorStep away from end of curve
                    xScale1.Min = xScale1.Max - XMax1;   //Increase min values of x axis acordingly
                }

                if (time2 > xScale2.Max - xScale2.MajorStep)
                {
                    xScale2.Max = time2 + xScale2.MajorStep;
                    xScale2.Min = xScale2.Max - XMax2;

                }
            }
        }

        //Used to set size and location of the graph panes
        private void SetSize()
        {
            if (WindowState == FormWindowState.Minimized)
            {
               
            }
            else
            {
                GraphPane myPane1 = zgc.MasterPane.PaneList[0] as GraphPane;
                GraphPane myPane2 = zgc.MasterPane.PaneList[1] as GraphPane;

                Point YMin = new Point(0, 0);
                Point YMax = new Point(0, 1);

                Rectangle formRect = this.ClientRectangle;
                Rectangle GraphRect = new Rectangle(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y + 37, this.ClientRectangle.Size.Width - 1, this.ClientRectangle.Size.Height - 1);

                /// if/else statements controll sizes of graph panes depening on which graph is selected to be viewed,
                /// and whether buttons are displayed or not. Size of pane set by defining rectangles
                if (ButtonsVisible.Checked)
                {
                    GraphRect.Inflate(0, -40);
                    if (LHSPane.Checked && !RHSPane.Checked)
                    {
                        RectangleF zero_rect2 = new RectangleF(Convert.ToInt32(XMax2), 0F, 0F, 0F);
                        myPane2.ReSize(this.CreateGraphics(), zero_rect2);
                        myPane1.ReSize(this.CreateGraphics(), zgc.MasterPane.Rect);
                        myPane1.XAxis.Scale.FontSpec.Size = 9.0F;
                        myPane1.YAxis.Scale.FontSpec.Size = 9.0F;
                        myPane1.Title.FontSpec.Size = 9.0F;
                        ThresholdScrollBar2.Height = 0;

                        Point LocationPoint1 = new Point(Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).X) - 10, Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).Y) + 70);
                        ThresholdScrollBar1.Height = Convert.ToInt32((myPane1.GeneralTransform(YMax, CoordType.ChartFraction)).Y);
                        ThresholdScrollBar1.Location = LocationPoint1;
                    }
                    else if (!LHSPane.Checked && RHSPane.Checked)
                    {
                        RectangleF zero_rect1 = new RectangleF(0F, 0F, 0F, 0F);
                        myPane1.ReSize(this.CreateGraphics(), zero_rect1);
                        myPane2.ReSize(this.CreateGraphics(), zgc.MasterPane.Rect);
                        myPane2.XAxis.Scale.FontSpec.Size = 9.0F;
                        myPane2.YAxis.Scale.FontSpec.Size = 9.0F;
                        myPane2.Title.FontSpec.Size = 9.0F;
                        ThresholdScrollBar1.Height = 0;

                        Point LocationPoint2 = new Point(Convert.ToInt32((myPane2.GeneralTransform(YMin, CoordType.ChartFraction).X)) - 10, Convert.ToInt32(myPane1.GeneralTransform(YMin, CoordType.ChartFraction).Y) + 70);
                        ThresholdScrollBar2.Height = Convert.ToInt32(myPane2.GeneralTransform(YMax, CoordType.ChartFraction).Y);
                        ThresholdScrollBar2.Location = LocationPoint2;


                    }
                    else
                    {
                        RectangleF panerect1 = new RectangleF(0F, 0F, (zgc.ClientSize.Width / 2), (zgc.ClientSize.Height));
                        RectangleF panerect2 = new RectangleF((zgc.ClientSize.Width / 2), 0F, (zgc.ClientSize.Width / 2), (zgc.ClientSize.Height));
                        myPane2.XAxis.Scale.FontSpec.Size = 10.0F;
                        myPane2.YAxis.Scale.FontSpec.Size = 10.0F;
                        myPane2.Title.FontSpec.Size = 12.0F;
                        myPane1.XAxis.Scale.FontSpec.Size = 10.0F;
                        myPane1.YAxis.Scale.FontSpec.Size = 10.0F;
                        myPane1.Title.FontSpec.Size = 12.0F;

                        myPane1.ReSize(this.CreateGraphics(), panerect1);
                        myPane2.ReSize(this.CreateGraphics(), panerect2);


                        Point LocationPoint1 = new Point(Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).X) - 10, Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).Y) + 70);
                        ThresholdScrollBar1.Height = Convert.ToInt32((myPane1.GeneralTransform(YMax, CoordType.ChartFraction)).Y);
                        ThresholdScrollBar1.Location = LocationPoint1;

                        Point LocationPoint2 = new Point(Convert.ToInt32((myPane2.GeneralTransform(YMin, CoordType.ChartFraction).X)) - 10, Convert.ToInt32(myPane1.GeneralTransform(YMin, CoordType.ChartFraction).Y) + 70);
                        ThresholdScrollBar2.Height = Convert.ToInt32(myPane2.GeneralTransform(YMax, CoordType.ChartFraction).Y);
                        ThresholdScrollBar2.Location = LocationPoint2;

                    }
                }

                else if (!ButtonsVisible.Checked)
                {
                    GraphRect.Inflate(0, 0);
                    GraphRect = new Rectangle(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y, this.ClientRectangle.Size.Width - 1, this.ClientRectangle.Size.Height - 1);
                    if (LHSPane.Checked && !RHSPane.Checked)
                    {
                        RectangleF zero_rect2 = new RectangleF(Convert.ToInt32(XMax2), 0F, 0F, 0F);
                        myPane2.ReSize(this.CreateGraphics(), zero_rect2);
                        myPane1.ReSize(this.CreateGraphics(), zgc.MasterPane.Rect);
                        myPane1.XAxis.Scale.FontSpec.Size = 9.0F;
                        myPane1.YAxis.Scale.FontSpec.Size = 9.0F;
                        myPane1.Title.FontSpec.Size = 9.0F;
                        ThresholdScrollBar2.Height = 0;

                        Point LocationPoint1 = new Point(Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).X) - 10, Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).Y)+7 );
                        ThresholdScrollBar1.Height = Convert.ToInt32((myPane1.GeneralTransform(YMax, CoordType.ChartFraction)).Y);
                        ThresholdScrollBar1.Location = LocationPoint1;
                    }
                    else if (!LHSPane.Checked && RHSPane.Checked)
                    {
                        RectangleF zero_rect1 = new RectangleF(0F, 0F, 0F, 0F);
                        myPane1.ReSize(this.CreateGraphics(), zero_rect1);
                        myPane2.ReSize(this.CreateGraphics(), zgc.MasterPane.Rect);
                        myPane2.XAxis.Scale.FontSpec.Size = 9.0F;
                        myPane2.YAxis.Scale.FontSpec.Size = 9.0F;
                        myPane2.Title.FontSpec.Size = 9.0F;
                        ThresholdScrollBar1.Height = 0;

                        Point LocationPoint2 = new Point(Convert.ToInt32((myPane2.GeneralTransform(YMin, CoordType.ChartFraction).X)) - 10, Convert.ToInt32(myPane1.GeneralTransform(YMin, CoordType.ChartFraction).Y)+7 );
                        ThresholdScrollBar2.Height = Convert.ToInt32(myPane2.GeneralTransform(YMax, CoordType.ChartFraction).Y);
                        ThresholdScrollBar2.Location = LocationPoint2;


                    }
                    else
                    {
                        RectangleF panerect1 = new RectangleF(0F, 0F, (zgc.ClientSize.Width / 2), (zgc.ClientSize.Height));
                        RectangleF panerect2 = new RectangleF((zgc.ClientSize.Width / 2), 0F, (zgc.ClientSize.Width / 2), (zgc.ClientSize.Height));
                        myPane2.XAxis.Scale.FontSpec.Size = 10.0F;
                        myPane2.YAxis.Scale.FontSpec.Size = 10.0F;
                        myPane2.Title.FontSpec.Size = 12.0F;
                        myPane1.XAxis.Scale.FontSpec.Size = 10.0F;
                        myPane1.YAxis.Scale.FontSpec.Size = 10.0F;
                        myPane1.Title.FontSpec.Size = 12.0F;

                        myPane1.ReSize(this.CreateGraphics(), panerect1);
                        myPane2.ReSize(this.CreateGraphics(), panerect2);


                        Point LocationPoint1 = new Point(Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).X) - 10, Convert.ToInt32((myPane1.GeneralTransform(YMin, CoordType.ChartFraction)).Y));
                        ThresholdScrollBar1.Height = Convert.ToInt32((myPane1.GeneralTransform(YMax, CoordType.ChartFraction)).Y-20);
                        ThresholdScrollBar1.Location = LocationPoint1;

                        Point LocationPoint2 = new Point(Convert.ToInt32((myPane2.GeneralTransform(YMin, CoordType.ChartFraction).X)) - 10, Convert.ToInt32(myPane1.GeneralTransform(YMin, CoordType.ChartFraction).Y));
                        ThresholdScrollBar2.Height = Convert.ToInt32(myPane2.GeneralTransform(YMax, CoordType.ChartFraction).Y-20);
                        ThresholdScrollBar2.Location = LocationPoint2;
                    }
                }
                if (zgc.Size != formRect.Size)
                {
                    zgc.Location = GraphRect.Location;
                    zgc.Size = GraphRect.Size;
                }
            }
        }

        /// Function to set X Axis on myPane1. if(TrueTime, !TrueTime) sensitivity conditions control how to scale x-axis, 
        /// if values are to represent  the real time intervals between data points or if all time intervals are set to one.
        /// if(pane checked) ensures axis isn't rescaled when this pane is not seen (when only the other pane is selected).
        private void SetXAxis1()
        {
            double Interval;

            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            Point XMax_Pane = new Point(1, 0);
            Point origin = new Point(0, 0);


            if (LHSPane.Checked||(!LHSPane.Checked && !RHSPane.Checked)) 
            {
                if (TrueTime1)
                {  
                    Interval = TimebinFactor*AverageChunkSize1 * 100*Convert.ToDouble((myPane1.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane1.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue1);
                    XMax1 = XMin1 + Interval;
                }
                if (!TrueTime1)
                {
                    Interval = Convert.ToDouble((myPane1.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane1.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue1);
                    XMax1 = XMin1 + Interval;
                }
            }

            myPane1.XAxis.Scale.Min = XMin1;
            myPane1.XAxis.Scale.Max = XMax1;
            myPane1.XAxis.Scale.MinorStep = XMax1 / 100.0;
            myPane1.XAxis.Scale.MajorStep = XMax1 / 10.0;
        }
        
        // Function to set X Axis on myPane2
        private void SetXAxis2()
        {
                double Interval;
                GraphPane myPane2 = zgc.MasterPane.PaneList[1];
                Point XMax_Pane = new Point(1, 0);
                Point origin = new Point(0, 0);
                if (RHSPane.Checked||(!LHSPane.Checked && !RHSPane.Checked))
                {
                    if (TrueTime2)
                    {
                        Interval = TimebinFactor*AverageChunkSize2 *100* Convert.ToDouble((myPane2.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane2.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue2);
                        XMax2 = XMin2 + Interval;
                    }
                    if (!TrueTime2)
                    {
                        Interval = Convert.ToDouble((myPane2.GeneralTransform(XMax_Pane, CoordType.ChartFraction).X - myPane2.GeneralTransform(origin, CoordType.ChartFraction).X) * XScaleValue2);
                        XMax2 = XMin2 + Interval;
                    }
                }
                myPane2.XAxis.Scale.Min = XMin2;
                myPane2.XAxis.Scale.Max = XMax2;
                myPane2.XAxis.Scale.MinorStep = XMax2 / 100.0;
                myPane2.XAxis.Scale.MajorStep = XMax2 / 10.0;
        }

        // Function to set Y Axis 
        public void SetYAxis1()
        {
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];

                ThresholdScrollBar1.Maximum = YMax1+9;
                ThresholdScrollBar1.Minimum = YMin1;

                myPane1.YAxis.Scale.Min = YMin1;
                myPane1.YAxis.Scale.Max = YMax1;
                myPane1.YAxis.Scale.MinorStep = YMax1 / 100;
                myPane1.YAxis.Scale.MajorStep = YMax1 / 10;
        }

        public void SetYAxis2()
        {
            GraphPane myPane2 = zgc.MasterPane.PaneList[1];
            ThresholdScrollBar2.Maximum = YMax2+9;
            ThresholdScrollBar2.Minimum = YMin2;

            myPane2.YAxis.Scale.Min = YMin2;
            myPane2.YAxis.Scale.Max = YMax2;
            myPane2.YAxis.Scale.MinorStep = YMax2 / 100;
            myPane2.YAxis.Scale.MajorStep = YMax2 / 10;
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


        //Saves raw 100microsecond data that is recieved from FPGA
        private void SaveBytesToFile(string FileLocation)
        {
            
            string BytesFileLocation = FileLocation + "RawData.txt";

            //Add some MetaData
            File.AppendAllText(BytesFileLocation, Convert.ToString(DateTime.Now) + "\r\n");
            File.AppendAllText(BytesFileLocation, Convert.ToString("Total time span of data (s) = " + TimebinFactor * DataBuffer.Length / 10000.0) + "\r\n");
            File.AppendAllText(BytesFileLocation, this.filedescription.Text + "\r\n");
            File.AppendAllText(BytesFileLocation, "Time" + "\t" + "PMT1" + "\t" + "PMT2"+"\r\n");

            // Start saving from COPY_POS element as this is least recent value in DataBuffer
            for(int i =COPY_POS;i<DataBuffer.Length; i++)
            {
                if (i % 2 == 1)
                {
                    File.AppendAllText(BytesFileLocation, (TimebinFactor * 0.1 * (i - COPY_POS))/2 + "\t" + Convert.ToString(DataBuffer[i]) + "\t");
                }
                else if (i % 2 == 0)
                {
                    File.AppendAllText(BytesFileLocation, Convert.ToString(DataBuffer[i]) + "\r\n");
                }
            }
            for(int i=0;i<COPY_POS;i++)
            {
                 if (i % 2 == 1)
                {
                    File.AppendAllText(BytesFileLocation, (TimebinFactor * 0.1 * (i + DataBuffer.Length-COPY_POS))/2  + "\t" + Convert.ToString(DataBuffer[i]) + "\t");
                }
                else if (i % 2 == 0)
                {
                    File.AppendAllText(BytesFileLocation,  Convert.ToString(DataBuffer[i]) + "\r\n");
                }
              
            }
        }

        // function to save a given list's values to file
        public void SaveListToFile(string FileLocation, PointPairList list, int Pane)
        {

            MetaData(1)[4] = this.filedescription.Text;

            File.AppendAllText(FileLocation, MetaData(Pane)[0] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[1] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[2] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[3] + "\r\n");
            File.AppendAllText(FileLocation, MetaData(Pane)[4] + "\r\n");

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
            string LHSfile = FileLocation+"LHS.txt";
            string RHSfile = FileLocation + "RHS.txt";

            File.AppendAllText(LHSfile, "Data from LHS Graph" + "\r\n");
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

            File.AppendAllText(RHSfile, "Data from RHS Graph" + "\r\n");
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

            Reset();

        }

        // Executed when saved button clicked, opens dialog box used for saving
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
                        SaveBothListsToFile(@sfd1.FileName, savelist1, savelist2);
                }

                else if (!RHSSaveBool && LHSSaveBool)
                {

                        SaveListToFile(@sfd1.FileName + "LHS.txt", savelist1, 1);


                }
                else if (RHSSaveBool && !LHSSaveBool)
                {
                        SaveListToFile(@sfd1.FileName + "RHS.txt", savelist2, 2);
                }
                else
                {
                    Console.WriteLine("Must select graph to save");
                    return;
                }

                WriteBytesToScreen(DataBuffer);
                if (SaveRaw.Checked)
                {
                    SaveBytesToFile(sfd1.FileName);
                }

                Array.Clear(UART_Buffer, 0, UART_Buffer.Length);
                Reset();
                Pause = false;
            }
            else if (sfd1.ShowDialog() == DialogResult.Cancel)
            {
                Pause = false;
            }
            sfd1.Dispose();
        }

        // Gets metadata for graphs
        private string[] MetaData(int pane)
        {
            string [] MetaData=new string [5];
            MetaData[0] = Convert.ToString(DateTime.Now);
            MetaData[1] = "FPGA timebin = " + Convert.ToString(TimebinFactor);
            
            if (pane == 1)
            {
                MetaData[2] = "Time bin size (ms) = " + Convert.ToString(AvChunkBox1.Value);
                if (IsScrolling)
                {
                    double TotalTime = list1.LongCount() * AverageChunkSize1 * TimebinFactor / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
                else if (!IsScrolling)
                {
                    double TotalTime = savelist1.LongCount() * AverageChunkSize1 * TimebinFactor / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime);
                }
            }
            else if (pane == 2)
            {
                MetaData[2] ="Time bin size (ms) = " + Convert.ToString(AvChunkBox2.Value);
                if (IsScrolling)
                {
                    double TotalTime = list2.LongCount() * AverageChunkSize2 * TimebinFactor / (10000.0);
                    double TotalTime2 = tickStart - Environment.TickCount;
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime)/*+"Environment time = " + Convert.ToString(TotalTime2)*/;
                }
                else if (!IsScrolling)
                {
                    double TotalTime = savelist2.LongCount() * AverageChunkSize2 * TimebinFactor / (10000.0);
                    MetaData[3] = "Total seconds recorded = " + Convert.ToString(TotalTime) ;
                }
            }

            return MetaData;

        }

        // Function to average bytes in DataBuffer while it isn't filled (still room for UART_Buffer to be coppied in)
        private void AverageDataUnfilled1()
        {
            int SumChunkEven = 0;
            int SumChunkOdd = 0;
            int NumOfValues = 0;

            // while loop continues to average blocks of bytes untill there aren't enough 'new' bytes to make up an 'AverageChunkSize', 
            while (AverageIndex1 * AverageChunkSize1 - AdjustIndex1 < COPY_POS) 
            {
                for (int i = Math.Max(0, (AverageIndex1 - 2) * AverageChunkSize1 - AdjustIndex1); i < AverageIndex1 * AverageChunkSize1 - AdjustIndex1; i++)
                {
                    // Separates bytes from each PMT
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
                }

                // If loops control which PMT's data is displayed
                if (PMT1_pane1 == true && PMT2_pane1 == false)
                {
                    if (Average.Checked == true)
                    {
                        Average1 = (SumChunkEven + SumProcessedBytes1) / (AverageChunkSize1) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average1 = (SumChunkEven + SumProcessedBytes1);
                    }
                }
                if (PMT2_pane1 == true && PMT1_pane1 == false)
                {
                    if (Average.Checked == true)
                    {
                        Average1 = (SumChunkOdd + SumProcessedBytes1) / (AverageChunkSize1) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average1 = (SumChunkOdd + SumProcessedBytes1);
                    }
                }
                if (PMT1_pane1 && PMT2_pane1)
                {
                    if (Average.Checked == true)
                    {
                        Average1 = (SumChunkEven + SumChunkOdd + SumProcessedBytes1) / (2 * AverageChunkSize1) * 10;
                        
                    }
                    else if (Average.Checked == false)
                    {
                        Average1 = (SumChunkEven + SumChunkOdd + SumProcessedBytes1);
                    }
                }
               
                PlotPoint(1);
                zgc.Invalidate();

                // if Truetime selected x axis values set to be time in microseconds
                if (TrueTime1)
                {
                    time1 += TimebinFactor * AverageChunkSize1*TimebinFactor*100;
                }
                else if (!TrueTime1)
                {
                    time1++;
                }
                AverageIndex1+=2; // Average index holds place index to average up to (start at averageindex-2)
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
            // Then sums remaining bytes to be carried over into next loop

            // Same while loop as AverageDataUnfilled function to average remaining bytes that make up an 'AverageChunkSize'
            while (DataBuffer.Length - ((AverageIndex1 - 2) * AverageChunkSize1 - AdjustIndex1) > 2*AverageChunkSize1) 
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

                // If loops control which PMT's data is displayed
                if (PMT1_pane1 == true && PMT2_pane1 == false)
                {
                    if (Average.Checked == true)
                    {
                        Average1 = SumEndChunkEven / (AverageChunkSize1) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average1 = SumEndChunkEven;
                    }
                }
                if (PMT2_pane1 == true && PMT1_pane1 == false)
                {
                    if (Average.Checked == true)
                    {
                        Average1 = SumEndChunkOdd / (AverageChunkSize1) * 10;

                    }
                    else if (Average.Checked == false)
                    {
                        Average1 = SumEndChunkOdd;
                    }
                }
                if (PMT1_pane1 && PMT2_pane1)
                {
                    if (Average.Checked == true)
                    {
                        Average1 = (SumEndChunkEven + SumEndChunkOdd) / (2 * AverageChunkSize1) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average1 = (SumEndChunkEven + SumEndChunkOdd);
                    }
                   
                }
                
                PlotPoint(1);
                zgc.Invalidate();

                // if Truetime selected x axis values set to be time in microseconds
                if (TrueTime1)
                {
                    time1 += TimebinFactor * AverageChunkSize1*TimebinFactor*100;
                }
                else if (!TrueTime1)
                {
                    time1++;
                }
                SumEndChunkEven = 0;
                SumEndChunkOdd = 0;
                AverageIndex1+=2;
            }

            // For loop to sum remaining bytes in DataBuffer that dont make up whole average chunk size
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
                SumProcessedBytes1 = SumProcessedBytesEven;
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
            AdjustIndex1 = NumProcessedBytes;

        }

        // Identical to AverageDataUnfilled1/ AverageDataFilled1, but for different average size
        private void AverageDataUnfilled2()
        {

            int SumChunkEven = 0;
            int SumChunkOdd = 0;

            // while loop continues to average blocks of bytes untill there aren't enough 'new' bytes to make up an 'AverageChunkSize', 
            while (AverageIndex2 * AverageChunkSize2 - AdjustIndex2 < COPY_POS) 
            {
                for (int i = Math.Max(0, ((AverageIndex2 - 2) * AverageChunkSize2 - AdjustIndex2)); i < (AverageIndex2 * AverageChunkSize2 - AdjustIndex2); i++)
                {
                    if (i % 2 == 0)
                    {
                        SumChunkEven += Convert.ToInt32(DataBuffer[i]);
                    }
                    else if (i % 2 == 1)
                    {
                        SumChunkOdd += Convert.ToInt32(DataBuffer[i]);
                    }
                }

                // If loops control which PMT's data is displayed
                if (PMT1_pane2 && !PMT2_pane2)
                {
                    if (Average.Checked == true)
                    {
                        Average2 = (SumChunkEven + SumProcessedBytes2) / (AverageChunkSize2) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average2 = (SumChunkEven + SumProcessedBytes2);
                    }
                }
                else if (PMT2_pane2 && !PMT1_pane2)
                {
                    if (Average.Checked == true)
                    {
                        Average2 = (SumChunkOdd + SumProcessedBytes2) / (AverageChunkSize2) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average2 = (SumChunkOdd + SumProcessedBytes2) ;
                    }
                }
                else if (PMT1_pane2 && PMT2_pane2)
                {
                    if (Average.Checked == true)
                    {
                        Average2 = (SumChunkOdd + SumChunkEven + SumProcessedBytes2) / (2 * AverageChunkSize2) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average2 = (SumChunkOdd + SumChunkEven + SumProcessedBytes2);
                    }
                }

                PlotPoint(2);

                zgc.Invalidate();

                // if Truetime selected x axis values set to be time in microseconds
                if (TrueTime2)
                {
                    time2 += TimebinFactor * AverageChunkSize2*TimebinFactor*100;
                }
                else if (!TrueTime2)
                {
                    time2++;
                }

                AverageIndex2+=2; 
                SumChunkEven = 0;
                SumChunkOdd = 0;
                SumProcessedBytes2 = 0;
            }




        }
        private void AverageDataFilled2()
        {

            int[] FilledProperties = new int[3];

            int SumProcessedBytesEven = 0;
            int SumProcessedBytesOdd = 0;
            int NumProcessedBytes = 0;
            int SumEndChunkEven = 0;
            int SumEndChunkOdd = 0;

            // Ensures averages are plotted if more than AverageChunkSize of array is yet to be averaged
            // Then sums remaining bytes to be carried over into next loop

            // Same while loop as above function to average remaining bytes that can make up an 'AverageChunkSize'
            while (DataBuffer.Length - ((AverageIndex2 - 2) * AverageChunkSize2 - AdjustIndex2) > 2*AverageChunkSize2) 
            {

                for (int i = ((AverageIndex2 -2) * AverageChunkSize2 - AdjustIndex2); i < (AverageChunkSize2 * AverageIndex2 - AdjustIndex2); i++)
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

                // If loops control which PMT's data is displayed
                if (PMT1_pane2 && !PMT2_pane2)
                {
                    if (Average.Checked == true)
                    {
                        Average2 = SumEndChunkEven / (AverageChunkSize2) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average2 =  SumEndChunkEven;
                    }
                }
                else if (PMT2_pane2 && !PMT1_pane2)
                {
                    if (Average.Checked == true)
                    {
                        Average2 = SumEndChunkOdd / (AverageChunkSize2) * 10;
                    }
                    else if (Average.Checked == false)
                    {
                        Average2 =  SumEndChunkOdd;
                    }
                }
                else if (PMT1_pane2 && PMT2_pane2)
                {
                    if (Average.Checked == true)
                    {
                        Average2 = (SumEndChunkOdd + SumEndChunkEven) / (2 * AverageChunkSize2) * 10;

                    }
                    else if (Average.Checked == false)
                    {
                        Average2 = (SumEndChunkOdd + SumEndChunkEven) ;
                    }
                }
                PlotPoint(2);

                zgc.Invalidate();

                // if Truetime selected x axis values set to be time in microseconds
                if (TrueTime2)
                {
                    time2 += TimebinFactor * AverageChunkSize2*TimebinFactor*100;
                }
                else if (!TrueTime2)
                {
                    time2++;
                }

                SumEndChunkEven = 0;
                SumEndChunkOdd = 0;
                AverageIndex2+=2;
            }

            for (int i = ((AverageIndex2-2) * AverageChunkSize2 - AdjustIndex2); i < DataBuffer.Length; i++) // For loop to sum remaining bytes in DataBuffer
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

        //Deletes points that are plotted but keeps data in savelists, the raw data, and time coordinate.
        private void FreshScreen()
        {
            list1.Clear();
            list2.Clear();
            RecentPoint1.Clear();
            RecentPoint2.Clear();
            PastOneScreen1 = false;
            PastOneScreen2 = false;
            CreateGraph(zgc);
        }

        // Overload of above to freshscreen only one of the graphs
        private void FreshScreen(int pane)
        {
            if (pane == 1)
            {
                AdditionalTime1 += time1;
                time1 = 0;
                list1.Clear();
                RecentPoint1.Clear();
                PastOneScreen1 = false;

                CreateGraph(zgc);
            }
            if (pane == 2)
            {
                AdditionalTime2 += time2;
                time2 = 0;
                list2.Clear();
                RecentPoint2.Clear();
                PastOneScreen2 = false;
                CreateGraph(zgc);
            }
        }

        // Deletes all data and sets time=0
        private void Reset()
        {
            list1.Clear();
            list2.Clear();
            savelist1.Clear();
            savelist2.Clear();
            RecentPoint1.Clear();
            RecentPoint2.Clear();
            PastOneScreen1 = false;
            PastOneScreen2 = false;
            time1 = 0;
            time2 = 0;
            AdditionalTime1 = 0;
            AdditionalTime2 = 0;
            CreateGraph(zgc);
        }

        // Overload of above to reset only one, used when changing average sizes and PMT select
        private void Reset(int pane)
        {
            if (pane == 1)
            {
                list1.Clear();
                savelist1.Clear();
                RecentPoint1.Clear();
                PastOneScreen1 = false;
                time1 = 0;
                AdditionalTime1 = 0;
            }
            else if (pane == 2)
            {
                list2.Clear();
                savelist2.Clear();
                RecentPoint2.Clear();
                PastOneScreen2 = false;
                time2 = 0;
                AdditionalTime2 = 0;
            }

            CreateGraph(zgc);
        }

        // Takes averages calculated with averaging funtions and adds them the appropriate lists for plotting. 
        private void PlotPoint(int Pane)
        {
            if (Pane == 1)
            {
                // Different time coordinates ensures that saved values match up if Scrolling/NotScrolling has been switched while displaying
                if (IsScrolling)
                {
                    list1.Add(time1, Average1);
                    savelist1.Add(time1, Average1);
                }
                if (!IsScrolling)
                {
                    list1.Add(time1, Average1);
                    savelist1.Add(time1 + AdditionalTime1, Average1); 
                    if (time1 > Convert.ToInt32(XMax1))
                    {
                        AdditionalTime1 += Convert.ToInt32(XMax1);
                        time1 = 0;
                        PastOneScreen1 = true;
                    }

                    // Removal of least recent points as new data come in
                    if (PastOneScreen1 && time1 <= list1.ElementAt(0).X)
                    {

                        list1.RemoveAt(0);
                    }
                    // Removal of points not within X range
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
                if (IsScrolling)
                {
                    list2.Add(time2, Average2);
                    savelist2.Add(time2, Average2);
                }
                if (!IsScrolling)
                {
                    list2.Add(time2, Average2);
                    savelist2.Add(time2 + AdditionalTime2, Average2);
                    if (time2 > XMax2)
                    {
                        AdditionalTime2 += Convert.ToInt32(XMax2);
                        time2 = 0;
                        PastOneScreen2 = true;
                    }
                    if (PastOneScreen2 && time2 <= list2.ElementAt(0).X)
                    {
                        list2.RemoveAt(0);
                    }
                    while (list2.ElementAt(0).X >= XMax2)
                    {
                        list2.RemoveAt(0);
                    }
                }
                RecentPoint2.Add(time2, Average2);
                if (RecentPoint2.LongCount() > 1)
                {
                    RecentPoint2.RemoveAt(0);
                }
            }
        }

        private void PMTCompare()
        {
            int PMT1Check = 0;
            int PMT2Check = 0;
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
            // Current level of PMT difference set to 50% before triggereing warning
            if (FracDiff >= 0.5)
            {
                Console.WriteLine("PMT ERROR!!!");
                Pause = true;
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
            FPGA.EmptyBuffer();
        }

        // Function to check if fluorescence drops below threshold level, suggesting ions have escaped
        private void CheckIonTrapped()
        {
            int CheckIonSum1 = 0;
            int CheckIonSum2 = 0;

            // Only most recent value compared to threshold, could be improved by checking many values
            if (savelist1.LongCount() != 0)
            {
                CheckIonSum1 += Convert.ToInt32(savelist1.ElementAt(Convert.ToInt32(savelist1.LongCount() - 1)).Y); 
            }
            double CheckIonAverage1 = CheckIonSum1;

            if (savelist2.LongCount() != 0)
            {
                CheckIonSum2 += Convert.ToInt32(savelist2.ElementAt(Convert.ToInt32(savelist2.LongCount() - 1)).Y);

            }
            double CheckIonAverage2 = CheckIonSum2;
            if (RHSPane.Checked && LHSPane.Checked)
            {
                if ((CheckIonAverage2 < ThresholdLineValue2) && (CheckIonAverage1 < ThresholdLineValue1))
                {
                    Pause = true;
                    PauseCheck.Checked = true;
                    Console.WriteLine("CheckIonSum 1 and 2 are {0}, {1}", CheckIonSum1, CheckIonSum2);
                }
            }

            else if (!LHSPane.Checked && RHSPane.Checked)
            {
                if (CheckIonAverage2 < ThresholdLineValue2)
                {
                    Pause = true;
                    PauseCheck.Checked = true;
                }
            }

            else if (LHSPane.Checked && !RHSPane.Checked)
            {
                if (CheckIonAverage1 < ThresholdLineValue1)
                {
                    Pause = true;
                    PauseCheck.Checked = true;
                }
            }

        }


        //
        //Functions below use buttons/sliders on the form itself
        //


        // Calls FreshScreen when button clicked
        private void FreshScreenButton_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                FreshScreen();
            }
            return;
        }

        // Triggers autosave function, which saves DataBuffer everytime all values are replaced
        public void AutoSave(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                AutoSaveBool = true;
            }
        }

        private void PauseCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PauseCheck.Checked)
            {
                Pause = true;
            }
            else if (!PauseCheck.Checked)
            {
                Pause = false;
                FPGA.EmptyBuffer();
            }

        }

        // Used to alter X Axis range myPane1, can induce aliasing
        void XScale1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ZoomIn.Checked)
            {
                XScaleValue1 =Convert.ToDouble( 1 / XScale1.Value);
                BarItem curve = zgc.MasterPane.PaneList[0].CurveList[2] as BarItem;
                curve.Bar.Border.Width = (float)(XScale1.Value);
            }
            else if (!ZoomIn.Checked)
            {
                XScaleValue1 =Convert.ToDouble(XScale1.Value);
            }

            SetXAxis1(); 
        }

        // Used to alter X Axis range myPane2
        void XScale2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ZoomIn.Checked)
            {
                XScaleValue2 = Convert.ToDouble(1 / XScale2.Value);
                BarItem curve2 = zgc.MasterPane.PaneList[1].CurveList[2] as BarItem;
                curve2.Bar.Border.Width = (float)(XScale2.Value);
            }
            else if (!ZoomIn.Checked)
            {
                XScaleValue2 = Convert.ToDouble(XScale2.Value);
            }
            SetXAxis2();
        }

        // Used to change Y axis on both Panes
        public void YMaxNum1_ValueChanged(object sender, System.EventArgs e)
        {
            YMax1 = Convert.ToInt32(YMaxNum1.Value);
            SetYAxis1();
        }

        public void YMinNum1_ValueChanged(object sender, System.EventArgs e)
        {
            YMin1 = Convert.ToInt32(YMinNum1.Value);
            SetYAxis1();
        }

        public void YMaxNum2_ValueChanged(object sender, System.EventArgs e)
        {
            YMax2 = Convert.ToInt32(YMaxNum2.Value);
            SetYAxis2();
        }

        public void YMinNum2_ValueChanged(object sender, System.EventArgs e)
        {
            YMin2 = Convert.ToInt32(YMinNum2.Value);
            SetYAxis2();
        }

        // Changes no of values to average over, resets everything and starts plotting
        // from scratch with new average
        void AvChunkBox1_ValueChanged(object sender, System.EventArgs e)
        {
            int AverageChunkSize1_Old = AverageChunkSize1;
            AverageChunkSize1 = Convert.ToInt32(Math.Round(Convert.ToDouble(AvChunkBox1.Value)*10.0/TimebinFactor));
            AdjustIndex1 = AverageIndex1 * (AverageChunkSize1 - AverageChunkSize1_Old) + AdjustIndex1;
            Console.WriteLine("ValueChanged1");
            Console.WriteLine("Average chunk 1 = {0}", AverageChunkSize1);

            // Reset used so no mixing of values averaged over different times in saved lists
            Reset(1);
        }
       

        // Same as above but for myPane2
        void AvChunkBox2_ValueChanged(object sender, System.EventArgs e)
        {
            int AverageChunkSize2_Old = AverageChunkSize2;
            AverageChunkSize2 = Convert.ToInt32(Math.Round(Convert.ToDouble(AvChunkBox2.Value) * 10.0 / TimebinFactor));
            AdjustIndex2 = AverageIndex2 * (AverageChunkSize2 - AverageChunkSize2_Old) + AdjustIndex2;
            Console.WriteLine("ValueChanged2");
            Console.WriteLine("Average chunk 2 = {0}", AverageChunkSize2);

            // Reset used so no mixing of values averaged over different times in saved lists
            Reset(2);
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

        private void FPGATimebin_ValueChanged(object sender, EventArgs e)
        {
            TimebinFactor = Convert.ToInt16(FPGATimebin.Value*10);

            AvChunkBox1.Minimum = Convert.ToDecimal(0.1 * TimebinFactor);
            AvChunkBox2.Minimum = Convert.ToDecimal(0.1 * TimebinFactor);

            AvChunkBox1_ValueChanged(sender, e);
            AvChunkBox2_ValueChanged(sender, e);

            Reset();
        }

        private void ScrollingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null) IsScrolling = !IsScrolling;
            if (ScrollingCheckBox.Checked)
            {
                time1 += AdditionalTime1;
                time2 += AdditionalTime2;
            }
            else if (!ScrollingCheckBox.Checked)
            {
                AdditionalTime1 = time1;
                AdditionalTime2 = time2;
                time1 = 0;
                time2 = 0;
                list1.Clear();
                list2.Clear();
                PastOneScreen1 = false;
                PastOneScreen2 = false;
            }
            //FreshScreen();
            Console.WriteLine("IsScrolling is {0}", IsScrolling);
        }

        private void PMTSelectLHS_TextChanged(object sender, EventArgs e)
        {
            if (PMTLHS.Text=="PMT1")
            {
                PMT1_pane1 = true;
                PMT2_pane1 = false;
                Console.WriteLine("PMT1p1 = {0} and PMT2p1 = {1}", PMT1_pane1, PMT2_pane1);
            }

            else if (PMTLHS.Text == "PMT2")
            {
                PMT1_pane1 = false;
                PMT2_pane1 = true;
                Console.WriteLine("PMT1p1 = {0} and PMT2p1 = {1}", PMT1_pane1, PMT2_pane1);
            }
            else if (PMTLHS.Text == "Both")
            {
                PMT1_pane1 = true;
                PMT2_pane1 = true;
                Console.WriteLine("PMT1p1 = {0} and PMT2p1 = {1}", PMT1_pane1, PMT2_pane1);
            }

            // Reset used so no mixing of values from different PMTs in saved data
            Reset(1);
        }
        private void PMTSelectRHS_TextChanged(object sender, EventArgs e)
        {
            if (PMTRHS.Text=="PMT1")
            {
                PMT1_pane2 = true;
                PMT2_pane2 = false;
            }
            if (PMTRHS.Text=="PMT2")
            {
                PMT1_pane2 = false;
                PMT2_pane2 = true;
            }
            if (PMTRHS.Text == "Both")
            {
                PMT1_pane2 = true;
                PMT2_pane2 = true;
            }
            // Reset used so no mixing of values from different PMTs in saved data
            Reset(2);
        }


        // Function to change units of X-axis, time or no. of points (ie increases one for each point regardless of timebin it represents)
        private void TimeControl1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (TimeControl1.Checked)
            {
                TrueTime1 = true;

                // Existing elements in list must have X-coord changed into time in microseconds
                for (int i = 0; i < list1.LongCount(); i++)
                {
                    list1.ElementAt(i).X = (list1.ElementAt(i).X) * AverageChunkSize1 * TimebinFactor * 100;
                }
                for (int i = 0; i < savelist1.LongCount(); i++)
                {
                    savelist1.ElementAt(i).X = (savelist1.ElementAt(i).X) * AverageChunkSize1*TimebinFactor*100;
                }
                time1 = time1 * AverageChunkSize1*TimebinFactor*100;
            }
            // X-Coordinates changed back to number of points plotted, not time
            if (!TimeControl1.Checked)
            {
                TrueTime1 = false;
                for (int i = 0; i < list1.LongCount(); i++)
                {
                    list1.ElementAt(i).X = list1.ElementAt(i).X / (AverageChunkSize1*TimebinFactor*100);
                }
                for (int i = 0; i < savelist1.LongCount(); i++)
                {
                    savelist1.ElementAt(i).X = savelist1.ElementAt(i).X / (AverageChunkSize1*TimebinFactor*100);
                }
                time1 = time1 / (AverageChunkSize1*TimebinFactor*100);
            }
            SetXAxis1();
            Console.WriteLine("TrueTime1 = {0}", TrueTime1);
        }

        // Same as above but for other graph
        private void TimeControl2_CheckedChanged(object sender, EventArgs e)
        {
            if (TimeControl2.Checked)
            {
                TrueTime2 = true;

                // Existing elements in list must have X-coord changed
                for (int i = 0; i < list2.LongCount(); i++)
                {
                    list2.ElementAt(i).X = (list2.ElementAt(i).X) * AverageChunkSize2*TimebinFactor*100;
                }
                for (int i = 0; i < savelist2.LongCount(); i++)
                {
                    savelist2.ElementAt(i).X = (savelist2.ElementAt(i).X) * AverageChunkSize2*TimebinFactor*100;
                }
                time2 = time2 * AverageChunkSize2*TimebinFactor*100;
            }

            // X-Coordinates changed back to number of points plotted, not time
            if (!TimeControl2.Checked)
            {
                TrueTime2 = false;
                for (int i = 0; i < list2.LongCount(); i++)
                {
                    list2.ElementAt(i).X = (list2.ElementAt(i).X) /(AverageChunkSize2*TimebinFactor*100);
                }
                for (int i = 0; i < savelist2.LongCount(); i++)
                {
                    savelist2.ElementAt(i).X = (savelist2.ElementAt(i).X) / (AverageChunkSize2*TimebinFactor*100);
                }
                time2 = time2 /(AverageChunkSize2*TimebinFactor*100);
            }
            SetXAxis2();
            Console.WriteLine("TrueTime2 = {0}", TrueTime2);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }


        private void ThresholdScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            ThresholdLineValue1=YMax1-ThresholdScrollBar1.Value;
        }

        private void ThresholdScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            ThresholdLineValue2 = YMax2 - ThresholdScrollBar2.Value;
        }

        // Sets whether scrol bars to set ion escaped thresholds are visible or not (to set thresholds or leave them set)
        private void ThresholdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ThresholdCheckBox.Checked)
            {
                ThresholdBool = true;
                ThresholdScrollBar1.Visible = true;
                ThresholdScrollBar2.Visible = true;
                zgc.MasterPane.PaneList[0].CurveList[0].IsVisible = true;
                zgc.MasterPane.PaneList[1].CurveList[0].IsVisible = true;
            }
            else if (!ThresholdCheckBox.Checked)
            {
                ThresholdBool = false;
                ThresholdScrollBar1.Visible = false;
                ThresholdScrollBar2.Visible = false;
                zgc.MasterPane.PaneList[0].CurveList[0].IsVisible = false;
                zgc.MasterPane.PaneList[1].CurveList[0].IsVisible = false;
                ThresholdLineValue1 = 0;
                ThresholdLineValue2 = 0;
            }
        
        }

        // Function to set all buttons visible or not, also results in change of size of graph pane to make use of space
        private void ButtonsVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (ButtonsVisible.Checked)
            {
                ThresholdCheckBox.Visible = true;
                TimeControl1.Visible = true;
                TimeControl2.Visible = true;
                ResetButton.Visible = true;
                FreshScreenButton.Visible = true;
                YMaxNum1.Visible = true;
                YMaxNum2.Visible = true;
                YMinNum1.Visible = true;
                YMinNum2.Visible = true;
                PMTLHS.Visible = true;
                PMTRHS.Visible = true;
                ScrollingCheckBox.Visible = true;
                AvChunkBox1.Visible = true;
                AvChunkBox2.Visible = true;
                SaveBytesButton.Visible = true;
                XScale1.Visible = true;
                XScale2.Visible = true;
                ZoomIn.Visible = true;
                PauseCheck.Visible = true;
                filedescription.Visible = true;
                AutoScale.Visible = true;
                SaveRaw.Visible = true;
                RHSSave.Visible = true;
                LHSSave.Visible = true;
                RHSPane.Visible = true;
                LHSPane.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true; 
                label16.Visible = true;
                label17.Visible = true;
                Average.Visible = true;
                FPGATimebin.Visible = true;

            }

            else if (!ButtonsVisible.Checked)
            {
                ThresholdCheckBox.Visible = false;
                ThresholdCheckBox.Visible = false;
                TimeControl1.Visible = false;
                TimeControl2.Visible = false;
                ResetButton.Visible = false;
                FreshScreenButton.Visible = false;
                YMaxNum1.Visible = false;
                YMaxNum2.Visible = false;
                YMinNum1.Visible = false;
                YMinNum2.Visible = false;
                PMTLHS.Visible = false;
                PMTRHS.Visible = false;
                ScrollingCheckBox.Visible = false;
                AvChunkBox1.Visible = false;
                AvChunkBox2.Visible = false;
                SaveBytesButton.Visible = false;
                XScale1.Visible = false;
                XScale2.Visible = false;
                ZoomIn.Visible = false;
                PauseCheck.Visible = false;
                filedescription.Visible = false;
                AutoScale.Visible = false;
                SaveRaw.Visible = false;
                RHSSave.Visible = false;
                LHSSave.Visible = false;
                RHSPane.Visible = false;
                LHSPane.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                label17.Visible = false;
                Average.Visible = false;
                FPGATimebin.Visible = false;
            }

        }

        // Creats graph again to autoscaling resumes even if Y axis values previously set
        private void AutoScale_CheckedChanged(object sender, EventArgs e)
        {
            CreateGraph(zgc);
        }

    }       
}
