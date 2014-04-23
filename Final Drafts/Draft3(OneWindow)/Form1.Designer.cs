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
            this.FreshScreenButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.YMaxNum1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.YMinNum1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.AvChunkBox1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.XScale1 = new System.Windows.Forms.NumericUpDown();
            this.XScale2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AvChunkBox2 = new System.Windows.Forms.NumericUpDown();
            this.RHSSave = new System.Windows.Forms.CheckBox();
            this.LHSSave = new System.Windows.Forms.CheckBox();
            this.Average = new System.Windows.Forms.CheckBox();
            this.AutoScale = new System.Windows.Forms.CheckBox();
            this.FPGATimebin = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ScrollingCheckBox = new System.Windows.Forms.CheckBox();
            this.ZoomIn = new System.Windows.Forms.CheckBox();
            this.LHSPane = new System.Windows.Forms.CheckBox();
            this.RHSPane = new System.Windows.Forms.CheckBox();
            this.YMinNum2 = new System.Windows.Forms.NumericUpDown();
            this.YMaxNum2 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.PMTLHS = new System.Windows.Forms.ComboBox();
            this.PMTRHS = new System.Windows.Forms.ComboBox();
            this.TimeControl1 = new System.Windows.Forms.CheckBox();
            this.TimeControl2 = new System.Windows.Forms.CheckBox();
            this.SaveRaw = new System.Windows.Forms.CheckBox();
            this.PauseCheck = new System.Windows.Forms.CheckBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.filedescription = new System.Windows.Forms.RichTextBox();
            this.ThresholdScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.ThresholdScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.ThresholdCheckBox = new System.Windows.Forms.CheckBox();
            this.ButtonsVisible = new System.Windows.Forms.CheckBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPGATimebin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum2)).BeginInit();
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
            // FreshScreenButton
            // 
            this.FreshScreenButton.Location = new System.Drawing.Point(252, 1);
            this.FreshScreenButton.Name = "FreshScreenButton";
            this.FreshScreenButton.Size = new System.Drawing.Size(75, 21);
            this.FreshScreenButton.TabIndex = 3;
            this.FreshScreenButton.Text = "FreshScreen";
            this.FreshScreenButton.UseVisualStyleBackColor = true;
            this.FreshScreenButton.Click += new System.EventHandler(this.FreshScreenButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(898, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Notes";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(610, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "LHS";
            // 
            // YMaxNum1
            // 
            this.YMaxNum1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMaxNum1.Location = new System.Drawing.Point(442, 29);
            this.YMaxNum1.Maximum = new decimal(new int[] {
            30000000,
            0,
            0,
            0});
            this.YMaxNum1.Name = "YMaxNum1";
            this.YMaxNum1.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum1.TabIndex = 20;
            this.YMaxNum1.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.YMaxNum1.ValueChanged += new System.EventHandler(this.YMaxNum1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(406, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "LHS";
            // 
            // YMinNum1
            // 
            this.YMinNum1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMinNum1.Location = new System.Drawing.Point(352, 28);
            this.YMinNum1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.YMinNum1.Name = "YMinNum1";
            this.YMinNum1.Size = new System.Drawing.Size(47, 20);
            this.YMinNum1.TabIndex = 22;
            this.YMinNum1.ValueChanged += new System.EventHandler(this.YMinNum1_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(320, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "LHS";
            // 
            // AvChunkBox1
            // 
            this.AvChunkBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AvChunkBox1.DecimalPlaces = 1;
            this.AvChunkBox1.Location = new System.Drawing.Point(551, 29);
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
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(515, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "LHS";
            // 
            // XScale1
            // 
            this.XScale1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.XScale1.Location = new System.Drawing.Point(645, 29);
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
            // 
            // XScale2
            // 
            this.XScale2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.XScale2.Location = new System.Drawing.Point(646, 51);
            this.XScale2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XScale2.Name = "XScale2";
            this.XScale2.Size = new System.Drawing.Size(56, 20);
            this.XScale2.TabIndex = 27;
            this.XScale2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XScale2.ValueChanged += new System.EventHandler(this.XScale2_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(610, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "RHS";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(515, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "RHS";
            // 
            // AvChunkBox2
            // 
            this.AvChunkBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AvChunkBox2.DecimalPlaces = 1;
            this.AvChunkBox2.Location = new System.Drawing.Point(551, 51);
            this.AvChunkBox2.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.AvChunkBox2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.AvChunkBox2.Name = "AvChunkBox2";
            this.AvChunkBox2.Size = new System.Drawing.Size(46, 20);
            this.AvChunkBox2.TabIndex = 30;
            this.AvChunkBox2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.AvChunkBox2.ValueChanged += new System.EventHandler(this.AvChunkBox2_ValueChanged);
            // 
            // RHSSave
            // 
            this.RHSSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RHSSave.AutoSize = true;
            this.RHSSave.Location = new System.Drawing.Point(1051, 36);
            this.RHSSave.Name = "RHSSave";
            this.RHSSave.Size = new System.Drawing.Size(77, 17);
            this.RHSSave.TabIndex = 33;
            this.RHSSave.Text = "Save RHS";
            this.RHSSave.UseVisualStyleBackColor = true;
            this.RHSSave.CheckedChanged += new System.EventHandler(this.RHSSave_CheckedChanged);
            // 
            // LHSSave
            // 
            this.LHSSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LHSSave.AutoSize = true;
            this.LHSSave.Location = new System.Drawing.Point(1051, 11);
            this.LHSSave.Name = "LHSSave";
            this.LHSSave.Size = new System.Drawing.Size(75, 17);
            this.LHSSave.TabIndex = 34;
            this.LHSSave.Text = "Save LHS";
            this.LHSSave.UseVisualStyleBackColor = true;
            this.LHSSave.CheckedChanged += new System.EventHandler(this.LHSSave_CheckedChanged);
            // 
            // Average
            // 
            this.Average.AutoSize = true;
            this.Average.Location = new System.Drawing.Point(10, 2);
            this.Average.Name = "Average";
            this.Average.Size = new System.Drawing.Size(97, 30);
            this.Average.TabIndex = 35;
            this.Average.Text = "Average (over \r\nFPGA Timebin)\r\n";
            this.Average.UseVisualStyleBackColor = true;
            // 
            // AutoScale
            // 
            this.AutoScale.AutoSize = true;
            this.AutoScale.Checked = true;
            this.AutoScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoScale.Location = new System.Drawing.Point(104, 48);
            this.AutoScale.Name = "AutoScale";
            this.AutoScale.Size = new System.Drawing.Size(85, 17);
            this.AutoScale.TabIndex = 36;
            this.AutoScale.Text = "Auto-rescale";
            this.AutoScale.UseVisualStyleBackColor = true;
            this.AutoScale.CheckedChanged += new System.EventHandler(this.AutoScale_CheckedChanged);
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
            this.FPGATimebin.Location = new System.Drawing.Point(1145, 56);
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
            this.label8.Location = new System.Drawing.Point(1065, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "FPGA Timebin";
            // 
            // ScrollingCheckBox
            // 
            this.ScrollingCheckBox.AutoSize = true;
            this.ScrollingCheckBox.Location = new System.Drawing.Point(104, 26);
            this.ScrollingCheckBox.Name = "ScrollingCheckBox";
            this.ScrollingCheckBox.Size = new System.Drawing.Size(66, 17);
            this.ScrollingCheckBox.TabIndex = 40;
            this.ScrollingCheckBox.Text = "Scrolling";
            this.ScrollingCheckBox.UseVisualStyleBackColor = true;
            this.ScrollingCheckBox.CheckedChanged += new System.EventHandler(this.ScrollingCheckBox_CheckedChanged);
            // 
            // ZoomIn
            // 
            this.ZoomIn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ZoomIn.AutoSize = true;
            this.ZoomIn.Location = new System.Drawing.Point(719, 36);
            this.ZoomIn.Name = "ZoomIn";
            this.ZoomIn.Size = new System.Drawing.Size(65, 17);
            this.ZoomIn.TabIndex = 41;
            this.ZoomIn.Text = "Zoom In";
            this.ZoomIn.UseVisualStyleBackColor = true;
            // 
            // LHSPane
            // 
            this.LHSPane.AutoSize = true;
            this.LHSPane.Checked = true;
            this.LHSPane.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LHSPane.Location = new System.Drawing.Point(10, 32);
            this.LHSPane.Name = "LHSPane";
            this.LHSPane.Size = new System.Drawing.Size(72, 17);
            this.LHSPane.TabIndex = 42;
            this.LHSPane.Text = "LHSPane";
            this.LHSPane.UseVisualStyleBackColor = true;
            // 
            // RHSPane
            // 
            this.RHSPane.AutoSize = true;
            this.RHSPane.Checked = true;
            this.RHSPane.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RHSPane.Location = new System.Drawing.Point(10, 48);
            this.RHSPane.Name = "RHSPane";
            this.RHSPane.Size = new System.Drawing.Size(74, 17);
            this.RHSPane.TabIndex = 43;
            this.RHSPane.Text = "RHSPane";
            this.RHSPane.UseVisualStyleBackColor = true;
            // 
            // YMinNum2
            // 
            this.YMinNum2.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMinNum2.Location = new System.Drawing.Point(352, 50);
            this.YMinNum2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.YMinNum2.Name = "YMinNum2";
            this.YMinNum2.Size = new System.Drawing.Size(47, 20);
            this.YMinNum2.TabIndex = 44;
            this.YMinNum2.ValueChanged += new System.EventHandler(this.YMinNum2_ValueChanged);
            // 
            // YMaxNum2
            // 
            this.YMaxNum2.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMaxNum2.Location = new System.Drawing.Point(442, 51);
            this.YMaxNum2.Maximum = new decimal(new int[] {
            30000000,
            0,
            0,
            0});
            this.YMaxNum2.Name = "YMaxNum2";
            this.YMaxNum2.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum2.TabIndex = 45;
            this.YMaxNum2.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.YMaxNum2.ValueChanged += new System.EventHandler(this.YMaxNum2_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(406, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "RHS";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(426, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 47;
            this.label10.Text = "YMax";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(318, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 13);
            this.label11.TabIndex = 48;
            this.label11.Text = "RHS";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(336, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 49;
            this.label12.Text = "YMin";
            // 
            // PMTLHS
            // 
            this.PMTLHS.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PMTLHS.FormattingEnabled = true;
            this.PMTLHS.Items.AddRange(new object[] {
            "PMT1",
            "PMT2",
            "Both"});
            this.PMTLHS.Location = new System.Drawing.Point(835, 24);
            this.PMTLHS.Name = "PMTLHS";
            this.PMTLHS.Size = new System.Drawing.Size(57, 21);
            this.PMTLHS.TabIndex = 60;
            this.PMTLHS.Text = "Both";
            this.PMTLHS.TextChanged += new System.EventHandler(this.PMTSelectLHS_TextChanged);
            // 
            // PMTRHS
            // 
            this.PMTRHS.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PMTRHS.FormattingEnabled = true;
            this.PMTRHS.Items.AddRange(new object[] {
            "PMT1",
            "PMT2",
            "Both"});
            this.PMTRHS.Location = new System.Drawing.Point(835, 48);
            this.PMTRHS.Name = "PMTRHS";
            this.PMTRHS.Size = new System.Drawing.Size(57, 21);
            this.PMTRHS.TabIndex = 63;
            this.PMTRHS.Text = "Both";
            this.PMTRHS.TextChanged += new System.EventHandler(this.PMTSelectRHS_TextChanged);
            // 
            // TimeControl1
            // 
            this.TimeControl1.AutoSize = true;
            this.TimeControl1.Location = new System.Drawing.Point(218, 48);
            this.TimeControl1.Name = "TimeControl1";
            this.TimeControl1.Size = new System.Drawing.Size(94, 17);
            this.TimeControl1.TabIndex = 64;
            this.TimeControl1.Text = "True time LHS";
            this.TimeControl1.UseVisualStyleBackColor = true;
            this.TimeControl1.Click += new System.EventHandler(this.TimeControl1_CheckedChanged);
            // 
            // TimeControl2
            // 
            this.TimeControl2.AutoSize = true;
            this.TimeControl2.Location = new System.Drawing.Point(218, 26);
            this.TimeControl2.Name = "TimeControl2";
            this.TimeControl2.Size = new System.Drawing.Size(96, 17);
            this.TimeControl2.TabIndex = 65;
            this.TimeControl2.Text = "True time RHS\r\n";
            this.TimeControl2.UseVisualStyleBackColor = true;
            this.TimeControl2.CheckedChanged += new System.EventHandler(this.TimeControl2_CheckedChanged);
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
            // PauseCheck
            // 
            this.PauseCheck.AutoSize = true;
            this.PauseCheck.Location = new System.Drawing.Point(104, 3);
            this.PauseCheck.Name = "PauseCheck";
            this.PauseCheck.Size = new System.Drawing.Size(56, 17);
            this.PauseCheck.TabIndex = 67;
            this.PauseCheck.Text = "Pause";
            this.PauseCheck.UseVisualStyleBackColor = true;
            this.PauseCheck.CheckedChanged += new System.EventHandler(this.PauseCheck_CheckedChanged);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(171, 1);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 68;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(524, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 13);
            this.label13.TabIndex = 69;
            this.label13.Text = "ms per point";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(610, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 70;
            this.label14.Text = "X Scale Factor";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(807, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 13);
            this.label15.TabIndex = 71;
            this.label15.Text = "PMT Select";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(801, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(28, 13);
            this.label16.TabIndex = 72;
            this.label16.Text = "LHS";
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(801, 51);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 73;
            this.label17.Text = "RHS";
            // 
            // filedescription
            // 
            this.filedescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filedescription.Location = new System.Drawing.Point(939, 30);
            this.filedescription.Name = "filedescription";
            this.filedescription.Size = new System.Drawing.Size(100, 43);
            this.filedescription.TabIndex = 74;
            this.filedescription.Text = "";
            // 
            // ThresholdScrollBar2
            // 
            this.ThresholdScrollBar2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ThresholdScrollBar2.Location = new System.Drawing.Point(630, 152);
            this.ThresholdScrollBar2.Name = "ThresholdScrollBar2";
            this.ThresholdScrollBar2.Size = new System.Drawing.Size(10, 277);
            this.ThresholdScrollBar2.TabIndex = 83;
            this.ThresholdScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ThresholdScrollBar2_Scroll);
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
            // 
            // ThresholdCheckBox
            // 
            this.ThresholdCheckBox.AutoSize = true;
            this.ThresholdCheckBox.Checked = true;
            this.ThresholdCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThresholdCheckBox.Location = new System.Drawing.Point(9, 71);
            this.ThresholdCheckBox.Name = "ThresholdCheckBox";
            this.ThresholdCheckBox.Size = new System.Drawing.Size(140, 17);
            this.ThresholdCheckBox.TabIndex = 84;
            this.ThresholdCheckBox.Text = "Threshold Fluorescence";
            this.ThresholdCheckBox.UseVisualStyleBackColor = true;
            this.ThresholdCheckBox.CheckedChanged += new System.EventHandler(this.ThresholdCheckBox_CheckedChanged);
            // 
            // ButtonsVisible
            // 
            this.ButtonsVisible.AutoSize = true;
            this.ButtonsVisible.Checked = true;
            this.ButtonsVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonsVisible.Location = new System.Drawing.Point(693, 4);
            this.ButtonsVisible.Name = "ButtonsVisible";
            this.ButtonsVisible.Size = new System.Drawing.Size(98, 17);
            this.ButtonsVisible.TabIndex = 85;
            this.ButtonsVisible.Text = "Set Parameters";
            this.ButtonsVisible.UseVisualStyleBackColor = true;
            this.ButtonsVisible.CheckedChanged += new System.EventHandler(this.ButtonsVisible_CheckedChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(153, 437);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 86;
            this.trackBar1.Value = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1213, 24);
            this.menuStrip1.TabIndex = 87;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // RollingGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 494);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.ButtonsVisible);
            this.Controls.Add(this.ThresholdCheckBox);
            this.Controls.Add(this.ThresholdScrollBar2);
            this.Controls.Add(this.ThresholdScrollBar1);
            this.Controls.Add(this.filedescription);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.PauseCheck);
            this.Controls.Add(this.SaveRaw);
            this.Controls.Add(this.TimeControl2);
            this.Controls.Add(this.TimeControl1);
            this.Controls.Add(this.PMTRHS);
            this.Controls.Add(this.PMTLHS);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.YMaxNum2);
            this.Controls.Add(this.YMinNum2);
            this.Controls.Add(this.RHSPane);
            this.Controls.Add(this.LHSPane);
            this.Controls.Add(this.ZoomIn);
            this.Controls.Add(this.ScrollingCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FPGATimebin);
            this.Controls.Add(this.AutoScale);
            this.Controls.Add(this.Average);
            this.Controls.Add(this.LHSSave);
            this.Controls.Add(this.RHSSave);
            this.Controls.Add(this.AvChunkBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.XScale2);
            this.Controls.Add(this.XScale1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AvChunkBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.YMinNum1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.YMaxNum1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FreshScreenButton);
            this.Controls.Add(this.SaveBytesButton);
            this.Controls.Add(this.zgc);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RollingGraph";
            this.Load += new System.EventHandler(this.RollingGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPGATimebin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



      
        

        

        

        #endregion

        private ZedGraph.ZedGraphControl zgc;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button SaveBytesButton;
        private System.Windows.Forms.Button FreshScreenButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown YMaxNum1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown YMinNum1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown AvChunkBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown XScale1;
        private System.Windows.Forms.NumericUpDown XScale2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown AvChunkBox2;
        private System.Windows.Forms.CheckBox RHSSave;
        private System.Windows.Forms.CheckBox LHSSave;
        private System.Windows.Forms.CheckBox Average;
        private System.Windows.Forms.CheckBox AutoScale;
        private System.Windows.Forms.NumericUpDown FPGATimebin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ScrollingCheckBox;
        private System.Windows.Forms.CheckBox ZoomIn;
        private System.Windows.Forms.CheckBox LHSPane;
        private System.Windows.Forms.CheckBox RHSPane;
        private System.Windows.Forms.NumericUpDown YMinNum2;
        private System.Windows.Forms.NumericUpDown YMaxNum2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox PMTLHS;
        private System.Windows.Forms.ComboBox PMTRHS;
        private System.Windows.Forms.CheckBox TimeControl1;
        private System.Windows.Forms.CheckBox TimeControl2;
        private System.Windows.Forms.CheckBox SaveRaw;
        private System.Windows.Forms.CheckBox PauseCheck;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RichTextBox filedescription;
        private System.Windows.Forms.VScrollBar ThresholdScrollBar2;
        private System.Windows.Forms.VScrollBar ThresholdScrollBar1;
        private System.Windows.Forms.CheckBox ThresholdCheckBox;
        private System.Windows.Forms.CheckBox ButtonsVisible;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}