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
        RollingPointPairList list1 = new RollingPointPairList(300);
        RollingPointPairList list2 = new RollingPointPairList(300);

        //byte arrays
        byte[] SimulatedBytes = new byte [10];
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
            timer2.Interval = 10;
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


        //l is clock divider index so 10 bytes can be built up before being read and plotted. Simulates bytes building up in UART buffer
        public void timer1_Tick(object sender, EventArgs e)
        {
            //l++;
            //SimulatedBytes[l % 10] = Convert.ToByte(slider);

            //if (l % 10 == 9) //Only reads and plots once every 10 ticks
            {
                
                int t = (Environment.TickCount - tickStart);
                UART_Buffer = FPGA.ReadBytes();

                //Calculates average of buffer load
                double UART_Average = SumBytes(UART_Buffer) / UART_Buffer.Length;

                Console.WriteLine(UART_Buffer.Length);
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
                
                if (!Stop) //Ensures points only plotted if stop button NOT pressed on form
                {
                    //For loop to add data points to the list
                    for (int i = 0; i < UART_Buffer.Length; i++)
                    {

                        double y = UART_Buffer[i]; //Plots buffer value
                        list.Add(time1, y);
                        time1++;

                        //Plots running average of each buffer load at central x value
                        double k = UART_Buffer.Length;
                        if (i - 1 < k / 2 && k / 2 <= i) //Inequalities necessary as k/2 may fall between 2 values of i
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
                        SaveBytesToFile(@"C:\Users\localadmin\Desktop\Data_file1.txt", ScreenBuffer);
                    }

                    /* OLD AVERAGING METHOD
                    for (int i = 0; i < Data.Length / 10; i++)
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


                    //Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length); ScreenBuffer not cleared to ensures complete array is written to file

                    //Remaining of UART bufferload copied to first spaces in ScreenBuffer and COPY_POS shifted accordingly
                    Array.Copy(UART_Buffer, ScreenBuffer.Length - COPY_POS, ScreenBuffer, 0, UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS));
                    COPY_POS = UART_Buffer.Length - (ScreenBuffer.Length - COPY_POS);

                }


               
                /*  Ensures there's at least one curve in GraphPane
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
            List<int> Data_Readings = new List<int>(DataToSave.Length);

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
                SaveBytesToFile(@"C:\Users\localadmin\Desktop\Data_file1.txt", ScreenBuffer);
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


        private void StopGraph(object sender, EventArgs e)
        {
            if (sender == null) Stop = false;
            if (sender != null)
            {
                Stop = true;
            }
            return;

        }
          
        //Extracts value of slider to "slider" integer in main code
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            slider = trackBar1.Value;
        }


        //Everything below for unnecessary random number generation
    /*
        private void Lambda(object sender, EventArgs e)
        {
            lambd = trackBar1.Value;
            //Console.WriteLine(lambd);
            return;
        }

        public static int GetPoisson(double lambda)
        {
            return (lambda < 30.0) ? PoissonSmall(lambda) : PoissonLarge(lambda);
        }

        private static int PoissonSmall(double lambda)
        {
            // Algorithm due to Donald Knuth, 1969.
            double p = 1.0, L = Math.Exp(-lambda);
            int k = 0;
            do
            {
                k++;
                p *= GetUniform();
            }
            while (p > L);
            return k - 1;
        }

        private static int PoissonLarge(double lambda)
        {
            // "Rejection method PA" from "The Computer Generation of 
            // Poisson Random Variables" by A. C. Atkinson,
            // Journal of the Royal Statistical Society Series C 
            // (Applied Statistics) Vol. 28, No. 1. (1979)
            // The article is on pages 29-35. 
            // The algorithm given here is on page 32.

            double c = 0.767 - 3.36 / lambda;
            double beta = Math.PI / Math.Sqrt(3.0 * lambda);
            double alpha = beta * lambda;
            double k = Math.Log(c) - lambda - Math.Log(beta);

            for (; ; )
            {
                double u = GetUniform();
                double x = (alpha - Math.Log((1.0 - u) / u)) / beta;
                int n = (int)Math.Floor(x + 0.5);
                if (n < 0)
                    continue;
                double v = GetUniform();
                double y = alpha - beta * x;
                double temp = 1.0 + Math.Exp(y);
                double lhs = y + Math.Log(v / (temp * temp));
                double rhs = k + n * Math.Log(lambda) - LogFactorial(n);
                if (lhs <= rhs)
                    return n;
            }
        }

        public static double GetUniform()
        {
            // 0 <= u < 2^32
            uint u = GetUint();
            // The magic number below is 1/(2^32 + 2).
            // The result is strictly between 0 and 1.
            return (u + 1.0) * 2.328306435454494e-10;
        }

        private static uint GetUint()
        {
            uint m_z = 0;
            uint m_w = 0;
            m_z = 36969 * (m_z & 65535) + (m_z >> 16);
            m_w = 18000 * (m_w & 65535) + (m_w >> 16);
            return (m_z << 16) + m_w;
        }

        static double LogFactorial(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (n > 254)
            {
                double x = n + 1;
                return (x - 0.5) * Math.Log(x) - x + 0.5 * Math.Log(2 * Math.PI) + 1.0 / (12.0 * x);
            }
            else
            {
                double[] lf = 
            {
                0.000000000000000,
                0.000000000000000,
                0.693147180559945,
                1.791759469228055,
                3.178053830347946,
                4.787491742782046,
                6.579251212010101,
                8.525161361065415,
                10.604602902745251,
                12.801827480081469,
                15.104412573075516,
                17.502307845873887,
                19.987214495661885,
                22.552163853123421,
                25.191221182738683,
                27.899271383840894,
                30.671860106080675,
                33.505073450136891,
                36.395445208033053,
                39.339884187199495,
                42.335616460753485,
                45.380138898476908,
                48.471181351835227,
                51.606675567764377,
                54.784729398112319,
                58.003605222980518,
                61.261701761002001,
                64.557538627006323,
                67.889743137181526,
                71.257038967168000,
                74.658236348830158,
                78.092223553315307,
                81.557959456115029,
                85.054467017581516,
                88.580827542197682,
                92.136175603687079,
                95.719694542143202,
                99.330612454787428,
                102.968198614513810,
                106.631760260643450,
                110.320639714757390,
                114.034211781461690,
                117.771881399745060,
                121.533081515438640,
                125.317271149356880,
                129.123933639127240,
                132.952575035616290,
                136.802722637326350,
                140.673923648234250,
                144.565743946344900,
                148.477766951773020,
                152.409592584497350,
                156.360836303078800,
                160.331128216630930,
                164.320112263195170,
                168.327445448427650,
                172.352797139162820,
                176.395848406997370,
                180.456291417543780,
                184.533828861449510,
                188.628173423671600,
                192.739047287844900,
                196.866181672889980,
                201.009316399281570,
                205.168199482641200,
                209.342586752536820,
                213.532241494563270,
                217.736934113954250,
                221.956441819130360,
                226.190548323727570,
                230.439043565776930,
                234.701723442818260,
                238.978389561834350,
                243.268849002982730,
                247.572914096186910,
                251.890402209723190,
                256.221135550009480,
                260.564940971863220,
                264.921649798552780,
                269.291097651019810,
                273.673124285693690,
                278.067573440366120,
                282.474292687630400,
                286.893133295426990,
                291.323950094270290,
                295.766601350760600,
                300.220948647014100,
                304.686856765668720,
                309.164193580146900,
                313.652829949878990,
                318.152639620209300,
                322.663499126726210,
                327.185287703775200,
                331.717887196928470,
                336.261181979198450,
                340.815058870798960,
                345.379407062266860,
                349.954118040770250,
                354.539085519440790,
                359.134205369575340,
                363.739375555563470,
                368.354496072404690,
                372.979468885689020,
                377.614197873918670,
                382.258588773060010,
                386.912549123217560,
                391.575988217329610,
                396.248817051791490,
                400.930948278915760,
                405.622296161144900,
                410.322776526937280,
                415.032306728249580,
                419.750805599544780,
                424.478193418257090,
                429.214391866651570,
                433.959323995014870,
                438.712914186121170,
                443.475088120918940,
                448.245772745384610,
                453.024896238496130,
                457.812387981278110,
                462.608178526874890,
                467.412199571608080,
                472.224383926980520,
                477.044665492585580,
                481.872979229887900,
                486.709261136839360,
                491.553448223298010,
                496.405478487217580,
                501.265290891579240,
                506.132825342034830,
                511.008022665236070,
                515.890824587822520,
                520.781173716044240,
                525.679013515995050,
                530.584288294433580,
                535.496943180169520,
                540.416924105997740,
                545.344177791154950,
                550.278651724285620,
                555.220294146894960,
                560.169054037273100,
                565.124881094874350,
                570.087725725134190,
                575.057539024710200,
                580.034272767130800,
                585.017879388839220,
                590.008311975617860,
                595.005524249382010,
                600.009470555327430,
                605.020105849423770,
                610.037385686238740,
                615.061266207084940,
                620.091704128477430,
                625.128656730891070,
                630.172081847810200,
                635.221937855059760,
                640.278183660408100,
                645.340778693435030,
                650.409682895655240,
                655.484856710889060,
                660.566261075873510,
                665.653857411105950,
                670.747607611912710,
                675.847474039736880,
                680.953419513637530,
                686.065407301994010,
                691.183401114410800,
                696.307365093814040,
                701.437263808737160,
                706.573062245787470,
                711.714725802289990,
                716.862220279103440,
                722.015511873601330,
                727.174567172815840,
                732.339353146739310,
                737.509837141777440,
                742.685986874351220,
                747.867770424643370,
                753.055156230484160,
                758.248113081374300,
                763.446610112640200,
                768.650616799717000,
                773.860102952558460,
                779.075038710167410,
                784.295394535245690,
                789.521141208958970,
                794.752249825813460,
                799.988691788643450,
                805.230438803703120,
                810.477462875863580,
                815.729736303910160,
                820.987231675937890,
                826.249921864842800,
                831.517780023906310,
                836.790779582469900,
                842.068894241700490,
                847.352097970438420,
                852.640365001133090,
                857.933669825857460,
                863.231987192405430,
                868.535292100464630,
                873.843559797865740,
                879.156765776907600,
                884.474885770751830,
                889.797895749890240,
                895.125771918679900,
                900.458490711945270,
                905.796028791646340,
                911.138363043611210,
                916.485470574328820,
                921.837328707804890,
                927.193914982476710,
                932.555207148186240,
                937.921183163208070,
                943.291821191335660,
                948.667099599019820,
                954.046996952560450,
                959.431492015349480,
                964.820563745165940,
                970.214191291518320,
                975.612353993036210,
                981.015031374908400,
                986.422203146368590,
                991.833849198223450,
                997.249949600427840,
                1002.670484599700300,
                1008.095434617181700,
                1013.524780246136200,
                1018.958502249690200,
                1024.396581558613400,
                1029.838999269135500,
                1035.285736640801600,
                1040.736775094367400,
                1046.192096209724900,
                1051.651681723869200,
                1057.115513528895000,
                1062.583573670030100,
                1068.055844343701400,
                1073.532307895632800,
                1079.012946818975000,
                1084.497743752465600,
                1089.986681478622400,
                1095.479742921962700,
                1100.976911147256000,
                1106.478169357800900,
                1111.983500893733000,
                1117.492889230361000,
                1123.006317976526100,
                1128.523770872990800,
                1134.045231790853000,
                1139.570684729984800,
                1145.100113817496100,
                1150.633503306223700,
                1156.170837573242400,
            };
                return lf[n];
            }
        }*/

 
    }
}

       



