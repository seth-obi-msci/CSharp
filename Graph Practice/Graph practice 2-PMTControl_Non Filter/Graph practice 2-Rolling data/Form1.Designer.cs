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
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.filename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.YMaxNum1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.YMinNum1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.AvChunkSize1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.XScale1 = new System.Windows.Forms.NumericUpDown();
            this.XScale2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AvChunkSize2 = new System.Windows.Forms.NumericUpDown();
            this.RHSSave = new System.Windows.Forms.CheckBox();
            this.LHSSave = new System.Windows.Forms.CheckBox();
            this.SumVsAv = new System.Windows.Forms.CheckBox();
            this.AutoScale = new System.Windows.Forms.CheckBox();
            this.Timebinfactorvalue = new System.Windows.Forms.NumericUpDown();
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
            this.PMTSelectLHS = new System.Windows.Forms.ComboBox();
            this.PMTSelectRHS = new System.Windows.Forms.ComboBox();
            this.PMTLHS = new System.Windows.Forms.ComboBox();
            this.PMTRHS = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkSize1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkSize2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Timebinfactorvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum2)).BeginInit();
            this.SuspendLayout();
            // 
            // zgc
            // 
            this.zgc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zgc.AutoSize = true;
            this.zgc.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.Location = new System.Drawing.Point(0, 63);
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
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(533, 457);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "SaveBytes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveBytesButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(608, 462);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(73, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "AutoSave";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.AutoSave);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(252, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 21);
            this.button2.TabIndex = 3;
            this.button2.Text = "FreshScreen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.FreshScreenButton);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBar1.Location = new System.Drawing.Point(12, 454);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(448, 45);
            this.trackBar1.TabIndex = 5;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(104, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 21);
            this.button3.TabIndex = 6;
            this.button3.Text = "Pause";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.PauseButton);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(185, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(61, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Continue";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ContinueButton);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(871, 462);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "File description:";
            // 
            // filename
            // 
            this.filename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.filename.Location = new System.Drawing.Point(954, 459);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(231, 20);
            this.filename.TabIndex = 12;
            this.filename.TextChanged += new System.EventHandler(this.filename_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(739, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 26);
            this.label1.TabIndex = 14;
            this.label1.Text = "X Scale Factor \r\n LHS";
            // 
            // YMaxNum1
            // 
            this.YMaxNum1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMaxNum1.Location = new System.Drawing.Point(525, 18);
            this.YMaxNum1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.YMaxNum1.Name = "YMaxNum1";
            this.YMaxNum1.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum1.TabIndex = 20;
            this.YMaxNum1.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.YMaxNum1.ValueChanged += new System.EventHandler(this.YMaxNum1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(479, 22);
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
            this.YMinNum1.Location = new System.Drawing.Point(406, 20);
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
            this.label5.Location = new System.Drawing.Point(362, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "LHS";
            // 
            // AvChunkSize1
            // 
            this.AvChunkSize1.DecimalPlaces = 1;
            this.AvChunkSize1.Location = new System.Drawing.Point(687, 10);
            this.AvChunkSize1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.AvChunkSize1.Name = "AvChunkSize1";
            this.AvChunkSize1.Size = new System.Drawing.Size(46, 20);
            this.AvChunkSize1.TabIndex = 24;
            this.AvChunkSize1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.AvChunkSize1.ValueChanged += new System.EventHandler(this.AvChunkSize1_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(617, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 26);
            this.label6.TabIndex = 25;
            this.label6.Text = "ms per point\r\n LHS";
            // 
            // XScale1
            // 
            this.XScale1.Location = new System.Drawing.Point(816, 10);
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
            this.XScale2.Location = new System.Drawing.Point(1121, 8);
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
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1041, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 26);
            this.label3.TabIndex = 28;
            this.label3.Text = "X Scale Factor \r\n RHS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(920, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 26);
            this.label7.TabIndex = 29;
            this.label7.Text = "ms per point \r\n RHS";
            // 
            // AvChunkSize2
            // 
            this.AvChunkSize2.Location = new System.Drawing.Point(993, 9);
            this.AvChunkSize2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.AvChunkSize2.Name = "AvChunkSize2";
            this.AvChunkSize2.Size = new System.Drawing.Size(46, 20);
            this.AvChunkSize2.TabIndex = 30;
            this.AvChunkSize2.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.AvChunkSize2.ValueChanged += new System.EventHandler(this.AvChunkSize2_ValueChanged);
            // 
            // RHSSave
            // 
            this.RHSSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RHSSave.AutoSize = true;
            this.RHSSave.Location = new System.Drawing.Point(778, 461);
            this.RHSSave.Name = "RHSSave";
            this.RHSSave.Size = new System.Drawing.Size(77, 17);
            this.RHSSave.TabIndex = 33;
            this.RHSSave.Text = "Save RHS";
            this.RHSSave.UseVisualStyleBackColor = true;
            this.RHSSave.CheckedChanged += new System.EventHandler(this.RHSSave_CheckedChanged);
            // 
            // LHSSave
            // 
            this.LHSSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LHSSave.AutoSize = true;
            this.LHSSave.Location = new System.Drawing.Point(689, 461);
            this.LHSSave.Name = "LHSSave";
            this.LHSSave.Size = new System.Drawing.Size(75, 17);
            this.LHSSave.TabIndex = 34;
            this.LHSSave.Text = "Save LHS";
            this.LHSSave.UseVisualStyleBackColor = true;
            this.LHSSave.CheckedChanged += new System.EventHandler(this.LHSSave_CheckedChanged);
            // 
            // SumVsAv
            // 
            this.SumVsAv.AutoSize = true;
            this.SumVsAv.Location = new System.Drawing.Point(12, 4);
            this.SumVsAv.Name = "SumVsAv";
            this.SumVsAv.Size = new System.Drawing.Size(86, 17);
            this.SumVsAv.TabIndex = 35;
            this.SumVsAv.Text = "Tick for Sum";
            this.SumVsAv.UseVisualStyleBackColor = true;
            // 
            // AutoScale
            // 
            this.AutoScale.AutoSize = true;
            this.AutoScale.Location = new System.Drawing.Point(104, 43);
            this.AutoScale.Name = "AutoScale";
            this.AutoScale.Size = new System.Drawing.Size(123, 17);
            this.AutoScale.TabIndex = 36;
            this.AutoScale.Text = "Tick for auto rescale";
            this.AutoScale.UseVisualStyleBackColor = true;
            // 
            // Timebinfactorvalue
            // 
            this.Timebinfactorvalue.Location = new System.Drawing.Point(1000, 38);
            this.Timebinfactorvalue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Timebinfactorvalue.Name = "Timebinfactorvalue";
            this.Timebinfactorvalue.Size = new System.Drawing.Size(47, 20);
            this.Timebinfactorvalue.TabIndex = 38;
            this.Timebinfactorvalue.ValueChanged += new System.EventHandler(this.Timebinfactorvalue_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(920, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "TimebinFactor";
            // 
            // ScrollingCheckBox
            // 
            this.ScrollingCheckBox.AutoSize = true;
            this.ScrollingCheckBox.Location = new System.Drawing.Point(104, 26);
            this.ScrollingCheckBox.Name = "ScrollingCheckBox";
            this.ScrollingCheckBox.Size = new System.Drawing.Size(105, 17);
            this.ScrollingCheckBox.TabIndex = 40;
            this.ScrollingCheckBox.Text = "Tick for Scrolling";
            this.ScrollingCheckBox.UseVisualStyleBackColor = true;
            this.ScrollingCheckBox.CheckedChanged += new System.EventHandler(this.ScrollingCheckBox_CheckedChanged);
            // 
            // ZoomIn
            // 
            this.ZoomIn.AutoSize = true;
            this.ZoomIn.Location = new System.Drawing.Point(668, 37);
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
            this.LHSPane.Location = new System.Drawing.Point(12, 25);
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
            this.RHSPane.Location = new System.Drawing.Point(10, 43);
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
            this.YMinNum2.Location = new System.Drawing.Point(406, 42);
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
            this.YMaxNum2.Location = new System.Drawing.Point(525, 40);
            this.YMaxNum2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.YMaxNum2.Name = "YMaxNum2";
            this.YMaxNum2.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum2.TabIndex = 45;
            this.YMaxNum2.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.YMaxNum2.ValueChanged += new System.EventHandler(this.YMaxNum2_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(477, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "RHS";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(473, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 47;
            this.label10.Text = "YMax";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(362, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 13);
            this.label11.TabIndex = 48;
            this.label11.Text = "RHS";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(359, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 49;
            this.label12.Text = "YMin";
            // 
            // PMTLHS
            // 
            this.PMTLHS.FormattingEnabled = true;
            this.PMTLHS.Items.AddRange(new object[] {
            "PMT1",
            "PMT2",
            "Both"});
            this.PMTLHS.Location = new System.Drawing.Point(816, 38);
            this.PMTLHS.Name = "PMTLHS";
            this.PMTLHS.Size = new System.Drawing.Size(57, 21);
            this.PMTLHS.TabIndex = 60;
            this.PMTLHS.Text = "Both";
            this.PMTLHS.TextChanged += new System.EventHandler(this.PMTSelectLHS_TextChanged);
            // 
            // PMTRHS
            // 
            this.PMTRHS.FormattingEnabled = true;
            this.PMTRHS.Items.AddRange(new object[] {
            "PMT1",
            "PMT2",
            "Both"});
            this.PMTRHS.Location = new System.Drawing.Point(1120, 35);
            this.PMTRHS.Name = "PMTRHS";
            this.PMTRHS.Size = new System.Drawing.Size(57, 21);
            this.PMTRHS.TabIndex = 63;
            this.PMTRHS.Text = "Both";
            this.PMTRHS.TextChanged += new System.EventHandler(this.PMTSelectRHS_TextChanged);
            // 
            // RollingGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 494);
            this.Controls.Add(this.PMTRHS);
            this.Controls.Add(this.PMTLHS);
            this.Controls.Add(this.PMTSelectRHS);
            this.Controls.Add(this.PMTSelectLHS);
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
            this.Controls.Add(this.Timebinfactorvalue);
            this.Controls.Add(this.AutoScale);
            this.Controls.Add(this.SumVsAv);
            this.Controls.Add(this.LHSSave);
            this.Controls.Add(this.RHSSave);
            this.Controls.Add(this.AvChunkSize2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.XScale2);
            this.Controls.Add(this.XScale1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AvChunkSize1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.YMinNum1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.YMaxNum1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.zgc);
            this.Name = "RollingGraph";
            this.Text = "RollingGraph";
            this.Load += new System.EventHandler(this.RollingGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkSize1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkSize2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Timebinfactorvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      
        

        

        

        #endregion

        private ZedGraph.ZedGraphControl zgc;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox filename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown YMaxNum1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown YMinNum1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown AvChunkSize1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown XScale1;
        private System.Windows.Forms.NumericUpDown XScale2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown AvChunkSize2;
        private System.Windows.Forms.CheckBox RHSSave;
        private System.Windows.Forms.CheckBox LHSSave;
        private System.Windows.Forms.CheckBox SumVsAv;
        private System.Windows.Forms.CheckBox AutoScale;
        private System.Windows.Forms.NumericUpDown Timebinfactorvalue;
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
        private System.Windows.Forms.ComboBox PMTSelectLHS;
        private System.Windows.Forms.ComboBox PMTSelectRHS;
        private System.Windows.Forms.ComboBox PMTLHS;
        private System.Windows.Forms.ComboBox PMTRHS;
    }
}