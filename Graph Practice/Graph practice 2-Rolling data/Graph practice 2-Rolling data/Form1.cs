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
        public int time2=0; //double to allow division for plotting average
        double Average;



        int SumProcessedBytes1 = 0;
        int PreviousScreenRemainder1 = 0;
        int AverageIndex1 = 1;
        int AverageChunkSize1 = 50;

        int SumProcessedBytes2=0;
        int PreviousScreenRemainder2 = 0;
        int AverageIndex2 = 1;
        int AverageChunkSize2 = 2000;

        int[] AverageArray2 = new int[3];
        int[] AverageArray1 = new int[3];
    
        int XMin1=0;
        int XMax1=30000;
        int XMin2 = 0;
        int XMax2 = 30000;
        int YMin=0;
        int YMax=300;

        //int NumValuesToScreen = 300; //Number of Values in point pair list and number of elements in ScreenBuffer array
        
        int COPY_POS = 0;  //Pointer to element in ScreenBuffer to copy UART_Buffer to

        int l = 0; //Index for clock divider "for" loop used for byte simulation 

        //Booleans used in the form for various functions 
        bool AutoSaveBool = false;
        bool Pause = false;
        double slider;
        
        // The RollingPointPairList is an efficient storage class that always
        // keeps a rolling set of point data without needing to shift any data values
        // New RollingPairList with 30000 values
        RollingPointPairList list1 = new RollingPointPairList(30000);
        RollingPointPairList list2 = new RollingPointPairList(30000);
    
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
            GraphPane myPane1 = new GraphPane();
            GraphPane myPane2 = new GraphPane();
            zgc.MasterPane.Add(myPane1);
            zgc.MasterPane.Add(myPane2);
          

            myPane1.Title.Text = "myPane1" ;
            myPane2.Title.Text = "myPane2";

          

            



            //Make a new curve
             BarItem curve = myPane1.AddBar("Average Counts" ,list1, Color.Black);
            // TextureBrush joebrush = new TextureBrush(Joe);
             curve.Bar.Fill = new Fill(Color.Black);
            // curve.Line.Width = 1.0F;
             

             LineItem curve2 = myPane2.AddCurve("Counts (Avg ten)", list2, Color.Black, SymbolType.None);
             //TextureBrush grahambrush = new TextureBrush(graham);
             curve2.Line.Fill = new Fill(Color.Black);
             curve2.Line.Width = 3.0F;
            
            
            //Timer fort the X axis, defined later
           // timer1.Interval = 15; //10 - buffer size increases due to build up but levels out at about 112 bytes.
            timer1.Enabled = true;
            timer1.Start();

            //Function to set axes of graphpanes.
            SetXAxis1();
            SetXAxis2();
            SetYAxis();


            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = this.CreateGraphics())
            {
                zgc.MasterPane.SetLayout(g, PaneLayout.SingleRow);
            }


            /*Save begging time for reference
            tickStart = Environment.TickCount;*/

            Console.WriteLine("Create Graph");
           
            

        }

        //l is clock divider index so 10 bytes can be built up before being read and plotted. Simulates bytes building up in UART buffer
        public void timer1_Tick(object sender, EventArgs e)
        {
            //  Must create new instance of graphpanes to access them.
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            GraphPane myPane2 = zgc.MasterPane.PaneList[1];
             
            
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
                    byte[] TestBuff = FPGA.ReadBytes();
                    Console.WriteLine("Length of Buffer = {0}", UART_Buffer.Length);
                    Console.WriteLine("Length of TestBuffer = {0}", TestBuff.Length);
                    SetSize();


                    
                    // Ensures there's at least one curve in GraphPane
                    if (zgc.GraphPane.CurveList.Count <= 0)
                        return;

                  /*  BarItem curve = myPane1.CurveList[0] as LineItem;
                    LineItem curve2 = myPane2.CurveList[0] as LineItem;
                    if (curve == null || curve2==null)
                        return;*/


                    // If list is null then reference at curve.Points doesn't
                    // support IPointPairList and can't be modified
                    if (list2 == null || list1 == null)
                        return;

                    // Think this gives time in milliseconds
                    double time = (Environment.TickCount - tickStart) / 1000.0;


                   
                    Scale xScale = myPane1.XAxis.Scale;
                    Scale xScale2 = myPane2.XAxis.Scale;

                    // if loops used to rescale x axis for each graph
                    if (time1 > xScale.Max - xScale.MajorStep) // When the time values are within one 'MajorStep' (5) of the max x value
                    {
                        xScale.Max = time1 + xScale.MajorStep; //Keep the end of x axis MajorStep (5) away from end of curve
                        xScale.Min = xScale.Max - XMax1;    ////Increase min values of x axis acordingly
                        
                        
                    }

                    if (time2 > xScale2.Max - xScale2.MajorStep)
                    {
                        xScale2.Max = time2 + xScale2.MajorStep;
                        xScale2.Min = xScale2.Max - XMax2;    
                    }
               

                    //Redraw
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
                            
                            AverageArray2 = AverageScreenUnfilled(AverageIndex2, AverageChunkSize2, PreviousScreenRemainder2, SumProcessedBytes2, list2, time2);
                            time2 = AverageArray2[1];
                            AverageIndex2 = AverageArray2[0];
                            SumProcessedBytes2 = 0;  //Sets total carried over from previous step to zero once first time through COPY_POS >= AverageIndex2 * AverageChunkSize2 - PreviousScreenRemainder2 loop                       
                        }

                        if (COPY_POS >= AverageIndex1 * AverageChunkSize1 - PreviousScreenRemainder1)
                        {
                            AverageArray1 = AverageScreenUnfilled(AverageIndex1, AverageChunkSize1, PreviousScreenRemainder1, SumProcessedBytes1, list1, time1);
                            time1 = AverageArray1[1];
                            AverageIndex1 = AverageArray1[0];
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
                        AverageArray2 = AverageScreenFilled(AverageIndex2, AverageChunkSize2, PreviousScreenRemainder2, SumProcessedBytes2, list2, time2);
                        time2 = AverageArray2[0];
                        SumProcessedBytes2 = AverageArray2[1];
                        PreviousScreenRemainder2 = AverageArray2[2];

                        Console.WriteLine("Average is {0}", Average);
                        Console.WriteLine("Previous Screen Remainder = {0}", PreviousScreenRemainder1);
                        Console.WriteLine("Length of UART_Buffer {0}", UART_Buffer.Length);
                        Console.WriteLine("TickCount is {0}", Environment.TickCount);

                        AverageArray1 = AverageScreenFilled(AverageIndex1, AverageChunkSize1, PreviousScreenRemainder1, SumProcessedBytes1, list1, time1);
                        time1 = AverageArray1[0];
                        SumProcessedBytes1 = AverageArray1[1];
                        PreviousScreenRemainder1 = AverageArray1[2];

                        // Indexes set to one so that next timer tick averaging begins at start of ScreenBuffer (most recent values)
                        AverageIndex2 = 1;
                        AverageIndex1 = 1;

                        
                        //Remaining of UART bufferload copied to first spaces in ScreenBuffer and COPY_POS shifted accordingly
                        Array.Copy(UART_Buffer, ScreenBuffer.Length - COPY_POS, ScreenBuffer, 0, UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS));
                        COPY_POS = UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS);
                       

                       
                    }


                }
            }
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
            GraphPane myPane1 = zgc.MasterPane.PaneList[0];
            Scale xScale = myPane1.XAxis.Scale;
           

            myPane1.XAxis.Scale.Min = XMin1;
            myPane1.XAxis.Scale.Max = XMax1;
            myPane1.XAxis.Scale.MinorStep = XMax1/100;
            myPane1.XAxis.Scale.MajorStep = XMax1/10;

            zgc.AxisChange();
        }

        // Function to set X Axis on myPane2
        private void SetXAxis2()
        {
            GraphPane myPane2 = zgc.MasterPane.PaneList[1];
            Scale xScale2 = myPane2.XAxis.Scale;
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
            myPane1.YAxis.Scale.MinorStep = YMax/100;
            myPane1.YAxis.Scale.MajorStep = YMax/10;

            myPane2.YAxis.Scale.Min = YMin;
            myPane2.YAxis.Scale.Max = YMax;
            myPane2.YAxis.Scale.MinorStep = YMin/100;
            myPane2.YAxis.Scale.MajorStep = YMax/10;

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


        // function to save a given arry's bytes to file
        public void SaveBytesToFile(string FileLocation, byte[] DataToSave)
        {
            File.AppendAllText(FileLocation, Convert.ToString(DateTime.Now) + "\r\n");
            File.AppendAllText(FileLocation, this.filename.Text + "\r\n");
            //  Stop = true;
            for (int j = COPY_POS; j < ScreenBuffer.Length; j++)
            {
                string Reading = ScreenBuffer[j].ToString() + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }

            for (int j = 0; j < COPY_POS; j++)
            {
                string Reading = ScreenBuffer[j].ToString() + "\r\n";
                File.AppendAllText(FileLocation, Reading);
            }
           

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


        private void SaveBytesButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = @"C:\";
            sfd1.Title = "SAVE BITCH";
            //sfd1.CheckFileExists = true;
            sfd1.CheckPathExists = true;
            sfd1.DefaultExt = "txt";
            sfd1.ShowDialog();
            //sfd1.FileOk += new CancelEventHandler(sfd1_FileOk);
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                Pause = true;
                SaveBytesToFile(@sfd1.FileName, ScreenBuffer);
                FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                UART_Buffer = FPGA.ReadBytes();
                Array.Clear(UART_Buffer, 0, UART_Buffer.Length);
                Console.WriteLine("Length of UART_Buffer {0}", UART_Buffer.Length);
                Pause = false;
            }



        }




        /*Functions below use buttons/sliders on the form itself*/

        //Wipes both average and running pointpairlists and resets x axis to 0 for new screen, effectively restarts program
        private void FreshScreenButton(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender != null)
            {
                
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
            return;
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
        void XRange1_ValueChanged(object sender, System.EventArgs e)
        {
            XMax1 = Convert.ToInt32(XRange1.Value);
            SetXAxis1();
        }

        // Used to alter X Axis range myPane2
        void XRange2_ValueChanged(object sender, System.EventArgs e)
        {
            XMax2 = Convert.ToInt32(XRange2.Value);
            SetXAxis2();
        }

        // Used to change Y axis on both Panes
        public void YMaxNum_Enter(object sender, System.EventArgs e)
        {
            YMax = Convert.ToInt32(YMaxNum.Value);
            SetYAxis();
        }

        public void YMinNum_Enter(object sender, System.EventArgs e)
        {
            YMin = Convert.ToInt32(YMinNum.Value);
            SetYAxis();
        }

        // Changes no of values to average over, resets everything and starts plotting
        // from scratch with new average
        void AvChunkSize1_ValueChanged(object sender, System.EventArgs e)
        {
            AverageChunkSize1 = Convert.ToInt32(AvChunkSize1.Value);
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
       

        // Same as above but for myPane2
        void AvChunkSize2_ValueChanged(object sender, System.EventArgs e)
        {
            AverageChunkSize2 = Convert.ToInt32(AvChunkSize2.Value);
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



        // Function to average bytes in ScreenBuffer while it isn't filled (still room for UART_Buffer to be coppied in)
        private int[] AverageScreenUnfilled(int AverageIndex, int AverageChunkSize, int PreviousScreenRemainder, int SumProcessedBytes, RollingPointPairList list, int time)
        {
            // Two values must be return so function returns an array
            int[] Properties = new int[2];
            int SumChunk = SumProcessedBytes;//Running total carried over from end of previous ScreenBuffer
            if (AverageIndex == 1) //Ensures first value of i in for loop is not negative as it would be if set to (AverageIndex2 - 1) * AverageChunkSize2 - PreviousScreenRemainder2 and AverageIndex2=1 (as it is in if loop below
            {
                for (int i = 0; i < AverageIndex * AverageChunkSize - PreviousScreenRemainder; i++) //Sums first values of new buffer
                {
                    SumChunk += Convert.ToInt32(ScreenBuffer[i]);
                }
                Average = SumChunk / AverageChunkSize;
                list.Add(time, Average);
                time += 10;
                AverageIndex++;
                SumChunk = 0;
            }
            while (AverageIndex * AverageChunkSize - PreviousScreenRemainder < COPY_POS) // while loop continues to average blocks of bytes untill there aren't enough 'new' bytes to make up an 'AverageChunkSize', 
                                                                                         // at this point requique new UART_Buffer to be coppied into ScreenBuffer
            {
                for (int i = (AverageIndex - 1) * AverageChunkSize - PreviousScreenRemainder; i < AverageIndex * AverageChunkSize - PreviousScreenRemainder; i++) //Sums a chunk from middle of array
                {
                    SumChunk += Convert.ToInt32(ScreenBuffer[i]);
                }
                Average = SumChunk / AverageChunkSize;
                list.Add(time, Average);    // Once computed average is added to PoinPairList along with time coordinate. Points are added to curve next time 'tick', each 'tick' many point may be added to curve
                time += 10;
                AverageIndex++; // Average index hold placed of where to start averaging from. 
                SumChunk = 0;
            }

            Properties[0] = AverageIndex;  // Setting required variables to elemant in array to be returned.
            Properties[1] = time;
            return Properties;
        }

        private int[] AverageScreenFilled(int AverageIndex, int AverageChunkSize, int PreviousScreenRemainder, int SumProcessedBytes, RollingPointPairList list, int time)
        {
            int[] FilledProperties = new int[3];

            int SumScreenRemainder=0;
            int SumEndChunk = 0;

            // Ensures averages are plotted if more than AverageChunkSize of array is yet to be averaged
            // Then sums remaining bytes to be carried over into next loop

            while(ScreenBuffer.Length-((AverageIndex-1) * AverageChunkSize - PreviousScreenRemainder) > AverageChunkSize) // Same while loop as above function to average remaining bytes that can make up an 'AverageChunkSize'
            {
                for (int i = (AverageIndex - 1) * AverageChunkSize - PreviousScreenRemainder; i < AverageChunkSize * AverageIndex - PreviousScreenRemainder; i++)
                {
                    SumEndChunk += Convert.ToInt32(ScreenBuffer[i]);
                }

                Average = SumEndChunk / AverageChunkSize;
                list.Add(time, Average);
                time += 10;
                SumEndChunk = 0;
                AverageIndex++;
            }

                for (int i = (AverageIndex-1) * AverageChunkSize - PreviousScreenRemainder; i < ScreenBuffer.Length; i++) // For loop to sum remaining bytes in ScreenBuffer
                {
                    SumScreenRemainder += Convert.ToInt32(ScreenBuffer[i]);
                }
                PreviousScreenRemainder = ScreenBuffer.Length - ((AverageIndex-1) * AverageChunkSize - PreviousScreenRemainder); // Index to acount for how many 'new' bytes need to be added to the sum of the remainder to make up 'AverageChunkSize'
               
            

            SumProcessedBytes = SumScreenRemainder;  // Carries over remainder bytes' sum 
            SumScreenRemainder = 0;
            
            FilledProperties[0] = time;
            FilledProperties[1] = SumProcessedBytes;
            FilledProperties[2] = PreviousScreenRemainder;

            return FilledProperties;
        }

    }       
}

       



