using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Read_and_Plot_FPGA_Data
{
	/*static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new Form1() );

        */  

    class square
    {
        void main()
        {
            int devices = FPGA.CountDevices();



            if (devices > 0)
            {
                Console.WriteLine("Device(s) detected (" + devices + ")");
                FPGA.SelectDevice(0);
                if (FPGA.OpenDevice()) // device successfully connected
                {
                    FPGA.ResetDevice(0);
                    Console.WriteLine("Device Opened");
                    FPGA.bUSBPortIsOpen = true;
                    //SetUSBPortText(true);

                }
                else
                {
                    Console.WriteLine("Error: Device would not open", true);
                    FPGA.bUSBPortIsOpen = false;
                    //SetUSBPortText(false);
                }
            }


            else
            {
                FPGA.bUSBPortIsOpen = false;
                //SetUSBPortText(false);
                Console.WriteLine("Error: No devices detected", true);
            }






            //Attempted method to fill array with datd from UART
            /*byte[] counts = new byte[FPGA.CheckQueue()];
            for (int i = 0; i < FPGA.CheckQueue(); i++)
            {
                counts[i] = FPGA.ReadBytes()[i];
                Console.WriteLine(counts[i]);
            }*/

            Thread.Sleep(1000);
            uint queue = FPGA.CheckQueue();
            Console.WriteLine("Bytes in the queue {0}", queue);

            //Methods used by Sarah to fill array
            byte[] Bytes = FPGA.ReadBytes();
            for (int i = 0; i < Bytes.Length; i++)
            {
                Console.WriteLine("{0}th byte is {1}", i, Bytes[i]);
            }

            if (FPGA.bUSBPortIsOpen == true)
            {
                Console.WriteLine("FPGA is really open, wide!");
            }



        }
		}
	}
