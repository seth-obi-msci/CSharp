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
    public partial class NewWindow : Form
    {

        //RollingGraph RollingGraph = new RollingGraph();
        bool pause2;
        public NewWindow()
        {
            InitializeComponent();
        }

        public ZedGraphControl Getzgc()
        {
            return zgc2;
        }
        //Console.WriteLine



        public void ThresholdCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            //RollingGraph.ThresholdCheckBox2_CheckedChanged();
        }

        public void ButtonsVisible2_CheckedChanged(object sender, EventArgs e)
        {
            // RollingGraph.ButtonsVisible2_CheckedChanged();
        }
    }
}
