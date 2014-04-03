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
    public partial class SecondWindow : Form
    {
        bool pause2;
        public SecondWindow()
        {
            InitializeComponent();
        }

        public ZedGraphControl Getzgc()
        {
            return zgc2;
        }
        //Console.WriteLine

        public void Pause_CheckedChanged(object sender, EventArgs e)
        {
        if (Pause.Checked)
        {
            pause2=true;
        }

        }
        
    }
}
