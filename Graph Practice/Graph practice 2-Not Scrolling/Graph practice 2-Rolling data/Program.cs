using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Timers;


namespace Graph_practice_2_Rolling_data
{

	static class Program
	{
        private static System.Timers.Timer aTimer;
        
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            
            /*Timer msTimer = new Timer();
            msTimer.Interval = 1;
            msTimer.Enabled = true;*/
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
            Console.WriteLine("No. of devices is {0}",FPGA.CountDevices());
            FPGA.OpenDevice();
            Application.Run(new RollingGraph());

        }

            
	}

}


