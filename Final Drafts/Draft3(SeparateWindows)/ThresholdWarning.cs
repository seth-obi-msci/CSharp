using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graph_practice_2_Rolling_data
{
    public partial class ThresholdWarning : Form
    {
        public ThresholdWarning()
        {
            InitializeComponent();
        }

        public bool IsChecked()
        {
            if (ThreshWarnCheck.Checked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
