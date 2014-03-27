using ZedGraph;

namespace Graph_practice_2_Rolling_data
{
    partial class RollingGraph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zgc = new ZedGraph.ZedGraphControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SaveBytesButton = new System.Windows.Forms.Button();
            this.FreshScreenButton1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.YMaxNum1 = new System.Windows.Forms.NumericUpDown();
            this.YMinNum1 = new System.Windows.Forms.NumericUpDown();
            this.AvChunkBox1 = new System.Windows.Forms.NumericUpDown();
            this.XScale1 = new System.Windows.Forms.NumericUpDown();
            this.Save1 = new System.Windows.Forms.CheckBox();
            this.AverageBox1 = new System.Windows.Forms.CheckBox();
            this.AutoScale1 = new System.Windows.Forms.CheckBox();
            this.FPGATimebin = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ScrollingCheckBox1 = new System.Windows.Forms.CheckBox();
            this.ZoomIn1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.PMT1 = new System.Windows.Forms.ComboBox();
            this.TimeControl1 = new System.Windows.Forms.CheckBox();
            this.SaveRaw = new System.Windows.Forms.CheckBox();
            this.PauseCheck1 = new System.Windows.Forms.CheckBox();
            this.ResetButton1 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.filedescription = new System.Windows.Forms.RichTextBox();
            this.ThresholdScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.ThresholdCheckBox1 = new System.Windows.Forms.CheckBox();
            this.ButtonsVisible1 = new System.Windows.Forms.CheckBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.Save2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPGATimebin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            //
            // zgc
            //
            this.zgc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zgc.AutoSize = true;
            this.zgc.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.Location = new System.Drawing.Point(9, 112);
            this.zgc.Margin = new System.Windows.Forms.Padding(0);
            this.zgc.Name = "zgc";
            this.zgc.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zgc.ScrollGrace = 0D;
            this.zgc.ScrollMaxX = 0D;
            this.zgc.ScrollMaxY = 0D;
            this.zgc.ScrollMaxY2 = 0D;
            this.zgc.ScrollMinX = 0D;
            this.zgc.ScrollMinY = 0D;
            this.zgc.ScrollMinY2 = 0D;
            this.zgc.Size = new System.Drawing.Size(1204, 373);
            this.zgc.TabIndex = 0;
            //
            // timer1
            //
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //
            // SaveBytesButton
            //
            this.SaveBytesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBytesButton.AutoSize = true;
            this.SaveBytesButton.Location = new System.Drawing.Point(1132, 7);
            this.SaveBytesButton.Name = "SaveBytesButton";
            this.SaveBytesButton.Size = new System.Drawing.Size(69, 23);
            this.SaveBytesButton.TabIndex = 1;
            this.SaveBytesButton.Text = "SaveBytes";
            this.SaveBytesButton.UseVisualStyleBackColor = true;
            this.SaveBytesButton.Click += new System.EventHandler(this.SaveBytesButton_Click);
            //
            // FreshScreenButton1
            //
            this.FreshScreenButton1.Location = new System.Drawing.Point(252, 1);
            this.FreshScreenButton1.Name = "FreshScreenButton1";
            this.FreshScreenButton1.Size = new System.Drawing.Size(75, 21);
            this.FreshScreenButton1.TabIndex = 3;
            this.FreshScreenButton1.Text = "FreshScreen";
            this.FreshScreenButton1.UseVisualStyleBackColor = true;
            this.FreshScreenButton1.Click += new System.EventHandler(this.FreshScreenButton1_Click);
            Window2.FreshScreenButton2.Click += new System.EventHandler(this.FreshScreenButton2_Click);
            //
            // label2
            //
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(906, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Notes";
            //
            // YMaxNum1
            //
            this.YMaxNum1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMaxNum1.Location = new System.Drawing.Point(467, 7);
            this.YMaxNum1.Maximum = new decimal(new int[] {
            30000000,
            0,
            0,
            0});
            this.YMaxNum1.Name = "YMaxNum1";
            this.YMaxNum1.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum1.TabIndex = 20;
            this.YMaxNum1.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.YMaxNum1.ValueChanged += new System.EventHandler(this.YMaxNum1_ValueChanged);
            Window2.YMaxNum2.ValueChanged += new System.EventHandler(this.YMaxNum2_ValueChanged);
            //
            // YMinNum1
            //
            this.YMinNum1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMinNum1.Location = new System.Drawing.Point(370, 4);
            this.YMinNum1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.YMinNum1.Name = "YMinNum1";
            this.YMinNum1.Size = new System.Drawing.Size(47, 20);
            this.YMinNum1.TabIndex = 22;
            this.YMinNum1.ValueChanged += new System.EventHandler(this.YMinNum1_ValueChanged);
            Window2.YMinNum2.ValueChanged += new System.EventHandler(this.YMinNum2_ValueChanged);
            //
            // AvChunkBox1
            //
            this.AvChunkBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AvChunkBox1.DecimalPlaces = 1;
            this.AvChunkBox1.Location = new System.Drawing.Point(589, 6);
            this.AvChunkBox1.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.AvChunkBox1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.AvChunkBox1.Name = "AvChunkBox1";
            this.AvChunkBox1.Size = new System.Drawing.Size(46, 20);
            this.AvChunkBox1.TabIndex = 24;
            this.AvChunkBox1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.AvChunkBox1.ValueChanged += new System.EventHandler(this.AvChunkBox1_ValueChanged);
            Window2.AvChunkBox2.ValueChanged += new System.EventHandler(this.AvChunkBox2_ValueChanged);
            //
            // XScale1
            //
            this.XScale1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.XScale1.Location = new System.Drawing.Point(691, 7);
            this.XScale1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XScale1.Name = "XScale1";
            this.XScale1.Size = new System.Drawing.Size(57, 20);
            this.XScale1.TabIndex = 26;
            this.XScale1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XScale1.ValueChanged += new System.EventHandler(this.XScale1_ValueChanged);
            Window2.XScale2.ValueChanged += new System.EventHandler(this.XScale2_ValueChanged);
            //
            // Save1
            //
            this.Save1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Save1.AutoSize = true;
            this.Save1.Location = new System.Drawing.Point(1053, 36);
            this.Save1.Name = "Save1";
            this.Save1.Size = new System.Drawing.Size(74, 17);
            this.Save1.TabIndex = 34;
            this.Save1.Text = "Save This";
            this.Save1.UseVisualStyleBackColor = true;
            //
            // AverageBox1
            //
            this.AverageBox1.AutoSize = true;
            this.AverageBox1.Location = new System.Drawing.Point(10, 2);
            this.AverageBox1.Name = "AverageBox1";
            this.AverageBox1.Size = new System.Drawing.Size(87, 17);
            this.AverageBox1.TabIndex = 35;
            this.AverageBox1.Text = "Average /ms";
            this.AverageBox1.UseVisualStyleBackColor = true;
            //
            // AutoScale1
            //
            this.AutoScale1.AutoSize = true;
            this.AutoScale1.Checked = true;
            this.AutoScale1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoScale1.Location = new System.Drawing.Point(10, 26);
            this.AutoScale1.Name = "AutoScale1";
            this.AutoScale1.Size = new System.Drawing.Size(85, 17);
            this.AutoScale1.TabIndex = 36;
            this.AutoScale1.Text = "Auto-rescale";
            this.AutoScale1.UseVisualStyleBackColor = true;
            this.AutoScale1.CheckedChanged += new System.EventHandler(this.AutoScale1_CheckedChanged);
            Window2.AutoScale2.CheckedChanged += new System.EventHandler(this.AutoScale2_CheckedChanged);
            //
            // FPGATimebin
            //
            this.FPGATimebin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FPGATimebin.DecimalPlaces = 1;
            this.FPGATimebin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FPGATimebin.Location = new System.Drawing.Point(589, 28);
            this.FPGATimebin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.FPGATimebin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FPGATimebin.Name = "FPGATimebin";
            this.FPGATimebin.Size = new System.Drawing.Size(47, 20);
            this.FPGATimebin.TabIndex = 38;
            this.FPGATimebin.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FPGATimebin.ValueChanged += new System.EventHandler(this.FPGATimebin_ValueChanged);
            //
            // label8
            //
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(508, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "FPGA Timebin";
            //
            // ScrollingCheckBox1
            //
            this.ScrollingCheckBox1.AutoSize = true;
            this.ScrollingCheckBox1.Location = new System.Drawing.Point(104, 26);
            this.ScrollingCheckBox1.Name = "ScrollingCheckBox1";
            this.ScrollingCheckBox1.Size = new System.Drawing.Size(66, 17);
            this.ScrollingCheckBox1.TabIndex = 40;
            this.ScrollingCheckBox1.Text = "Scrolling";
            this.ScrollingCheckBox1.UseVisualStyleBackColor = true;
            this.ScrollingCheckBox1.CheckedChanged += new System.EventHandler(this.ScrollingCheckBox1_CheckedChanged);
            Window2.ScrollingCheckBox2.CheckedChanged += new System.EventHandler(this.ScrollingCheckBox1_CheckedChanged);
            //
            // ZoomIn1
            //
            this.ZoomIn1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ZoomIn1.AutoSize = true;
            this.ZoomIn1.Location = new System.Drawing.Point(762, 6);
            this.ZoomIn1.Name = "ZoomIn1";
            this.ZoomIn1.Size = new System.Drawing.Size(65, 17);
            this.ZoomIn1.TabIndex = 41;
            this.ZoomIn1.Text = "Zoom In";
            this.ZoomIn1.UseVisualStyleBackColor = true;
            //
            // label10
            //
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(427, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 47;
            this.label10.Text = "YMax";
            //
            // label12
            //
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(333, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 49;
            this.label12.Text = "YMin";
            //
            // PMT1
            //
            this.PMT1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PMT1.FormattingEnabled = true;
            this.PMT1.Items.AddRange(new object[] {
            "PMT1",
            "PMT2",
            "Both"});
            this.PMT1.Location = new System.Drawing.Point(835, 24);
            this.PMT1.Name = "PMT1";
            this.PMT1.Size = new System.Drawing.Size(57, 21);
            this.PMT1.TabIndex = 60;
            this.PMT1.Text = "Both";
            this.PMT1.TextChanged += new System.EventHandler(this.PMTSelect1_TextChanged);
            Window2.PMT2.TextChanged += new System.EventHandler(this.PMTSelect2_TextChanged);
            //
            // TimeControl1
            //
            this.TimeControl1.AutoSize = true;
            this.TimeControl1.Location = new System.Drawing.Point(215, 29);
            this.TimeControl1.Name = "TimeControl1";
            this.TimeControl1.Size = new System.Drawing.Size(94, 17);
            this.TimeControl1.TabIndex = 64;
            this.TimeControl1.Text = "True time LHS";
            this.TimeControl1.UseVisualStyleBackColor = true;
            this.TimeControl1.Click += new System.EventHandler(this.TimeControl1_CheckedChanged);
            Window2.TimeControl2.Click += new System.EventHandler(this.TimeControl2_CheckedChanged);
            //
            // SaveRaw
            //
            this.SaveRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveRaw.AutoSize = true;
            this.SaveRaw.Location = new System.Drawing.Point(1134, 36);
            this.SaveRaw.Name = "SaveRaw";
            this.SaveRaw.Size = new System.Drawing.Size(73, 17);
            this.SaveRaw.TabIndex = 66;
            this.SaveRaw.Text = "SaveRaw";
            this.SaveRaw.UseVisualStyleBackColor = true;
            //
            // PauseCheck1
            //
            this.PauseCheck1.AutoSize = true;
            this.PauseCheck1.Location = new System.Drawing.Point(104, 3);
            this.PauseCheck1.Name = "PauseCheck1";
            this.PauseCheck1.Size = new System.Drawing.Size(56, 17);
            this.PauseCheck1.TabIndex = 67;
            this.PauseCheck1.Text = "Pause";
            this.PauseCheck1.UseVisualStyleBackColor = true;
            this.PauseCheck1.CheckedChanged += new System.EventHandler(this.PauseCheck_CheckedChanged);
            //
            // ResetButton1
            //
            this.ResetButton1.Location = new System.Drawing.Point(171, 1);
            this.ResetButton1.Name = "ResetButton1";
            this.ResetButton1.Size = new System.Drawing.Size(75, 23);
            this.ResetButton1.TabIndex = 68;
            this.ResetButton1.Text = "Reset";
            this.ResetButton1.UseVisualStyleBackColor = true;
            this.ResetButton1.Click += new System.EventHandler(this.ResetButton1_Click);
            Window2.ResetButton2.Click += new System.EventHandler(this.ResetButton2_Click);
            //
            // label13
            //
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(518, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 13);
            this.label13.TabIndex = 69;
            this.label13.Text = "Timebin /ms";
            //
            // label14
            //
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(641, 5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 26);
            this.label14.TabIndex = 70;
            this.label14.Text = "X Scale\r\nFactor";
            //
            // label15
            //
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(833, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 13);
            this.label15.TabIndex = 71;
            this.label15.Text = "PMT Select";
            //
            // filedescription
            //
            this.filedescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filedescription.Location = new System.Drawing.Point(947, 5);
            this.filedescription.Name = "filedescription";
            this.filedescription.Size = new System.Drawing.Size(100, 43);
            this.filedescription.TabIndex = 74;
            this.filedescription.Text = "";
            this.filedescription.TextChanged += new System.EventHandler(this.filedescription_TextChanged);
            //
            // ThresholdScrollBar1
            //
            this.ThresholdScrollBar1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ThresholdScrollBar1.Location = new System.Drawing.Point(72, 152);
            this.ThresholdScrollBar1.Name = "ThresholdScrollBar1";
            this.ThresholdScrollBar1.Size = new System.Drawing.Size(10, 277);
            this.ThresholdScrollBar1.TabIndex = 79;
            this.ThresholdScrollBar1.Value = 100;
            this.ThresholdScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ThresholdScrollBar1_Scroll);
            Window2.ThresholdScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ThresholdScrollBar2_Scroll);
            //
            // ThresholdCheckBox1
            //
            this.ThresholdCheckBox1.AutoSize = true;
            this.ThresholdCheckBox1.Checked = true;
            this.ThresholdCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThresholdCheckBox1.Location = new System.Drawing.Point(321, 31);
            this.ThresholdCheckBox1.Name = "ThresholdCheckBox1";
            this.ThresholdCheckBox1.Size = new System.Drawing.Size(140, 17);
            this.ThresholdCheckBox1.TabIndex = 84;
            this.ThresholdCheckBox1.Text = "Threshold Fluorescence";
            this.ThresholdCheckBox1.UseVisualStyleBackColor = true;
            this.ThresholdCheckBox1.CheckedChanged += new System.EventHandler(this.ThresholdCheckBox1_CheckedChanged);
            Window2.ThresholdCheckBox2.CheckedChanged += new System.EventHandler(this.ThresholdCheckBox2_CheckedChanged);
            //
            // ButtonsVisible1
            //
            this.ButtonsVisible1.AutoSize = true;
            this.ButtonsVisible1.Checked = true;
            this.ButtonsVisible1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonsVisible1.Location = new System.Drawing.Point(691, 31);
            this.ButtonsVisible1.Name = "ButtonsVisible1";
            this.ButtonsVisible1.Size = new System.Drawing.Size(98, 17);
            this.ButtonsVisible1.TabIndex = 85;
            this.ButtonsVisible1.Text = "Set Parameters";
            this.ButtonsVisible1.UseVisualStyleBackColor = true;
            this.ButtonsVisible1.CheckedChanged += new System.EventHandler(this.ButtonsVisible1_CheckedChanged);
            Window2.ButtonsVisible2.CheckedChanged += new System.EventHandler(this.ButtonsVisible2_CheckedChanged);
            //
            // trackBar1
            //
            this.trackBar1.Location = new System.Drawing.Point(295, 440);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 86;
            //
            // Save2
            //
            this.Save2.AutoSize = true;
            this.Save2.Location = new System.Drawing.Point(1053, 13);
            this.Save2.Name = "Save2";
            this.Save2.Size = new System.Drawing.Size(80, 17);
            this.Save2.TabIndex = 87;
            this.Save2.Text = "Save Other";
            this.Save2.UseVisualStyleBackColor = true;
            //
            // RollingGraph
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 494);
            this.Controls.Add(this.Save2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.ButtonsVisible1);
            this.Controls.Add(this.ThresholdCheckBox1);
            this.Controls.Add(this.ThresholdScrollBar1);
            this.Controls.Add(this.filedescription);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.ResetButton1);
            this.Controls.Add(this.PauseCheck1);
            this.Controls.Add(this.SaveRaw);
            this.Controls.Add(this.TimeControl1);
            this.Controls.Add(this.PMT1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ZoomIn1);
            this.Controls.Add(this.ScrollingCheckBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FPGATimebin);
            this.Controls.Add(this.AutoScale1);
            this.Controls.Add(this.AverageBox1);
            this.Controls.Add(this.Save1);
            this.Controls.Add(this.XScale1);
            this.Controls.Add(this.AvChunkBox1);
            this.Controls.Add(this.YMinNum1);
            this.Controls.Add(this.YMaxNum1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FreshScreenButton1);
            this.Controls.Add(this.SaveBytesButton);
            this.Controls.Add(this.zgc);
            this.Name = "RollingGraph";
            this.Load += new System.EventHandler(this.RollingGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPGATimebin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private ZedGraph.ZedGraphControl zgc;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button SaveBytesButton;
        private System.Windows.Forms.Button FreshScreenButton1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown YMaxNum1;
        private System.Windows.Forms.NumericUpDown YMinNum1;
        private System.Windows.Forms.NumericUpDown AvChunkBox1;
        private System.Windows.Forms.NumericUpDown XScale1;
        private System.Windows.Forms.CheckBox Save1;
        private System.Windows.Forms.CheckBox AverageBox1;
        private System.Windows.Forms.CheckBox AutoScale1;
        private System.Windows.Forms.NumericUpDown FPGATimebin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ScrollingCheckBox1;
        private System.Windows.Forms.CheckBox ZoomIn1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox PMT1;
        private System.Windows.Forms.CheckBox TimeControl1;
        private System.Windows.Forms.CheckBox SaveRaw;
        private System.Windows.Forms.CheckBox PauseCheck1;
        private System.Windows.Forms.Button ResetButton1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RichTextBox filedescription;
        private System.Windows.Forms.VScrollBar ThresholdScrollBar1;
        private System.Windows.Forms.CheckBox ThresholdCheckBox1;
        private System.Windows.Forms.CheckBox ButtonsVisible1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox Save2;
    }
}