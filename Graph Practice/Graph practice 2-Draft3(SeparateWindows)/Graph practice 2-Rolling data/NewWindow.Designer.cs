namespace Graph_practice_2_Rolling_data
{
    partial class NewWindow
    {

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
            this.zgc2 = new ZedGraph.ZedGraphControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.FreshScreenButton2 = new System.Windows.Forms.Button();
            this.YMaxNum2 = new System.Windows.Forms.NumericUpDown();
            this.YMinNum2 = new System.Windows.Forms.NumericUpDown();
            this.AvChunkBox2 = new System.Windows.Forms.NumericUpDown();
            this.XScale2 = new System.Windows.Forms.NumericUpDown();
            this.AverageBox2 = new System.Windows.Forms.CheckBox();
            this.AutoScale2 = new System.Windows.Forms.CheckBox();
            this.ScrollingCheckBox2 = new System.Windows.Forms.CheckBox();
            this.ZoomIn2 = new System.Windows.Forms.CheckBox();
            this.label10_2 = new System.Windows.Forms.Label();
            this.label12_2 = new System.Windows.Forms.Label();
            this.PMT2 = new System.Windows.Forms.ComboBox();
            this.TimeControl2 = new System.Windows.Forms.CheckBox();
            this.ResetButton2 = new System.Windows.Forms.Button();
            this.label13_2 = new System.Windows.Forms.Label();
            this.label14_2 = new System.Windows.Forms.Label();
            this.label15_2 = new System.Windows.Forms.Label();
            this.ThresholdScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.ThresholdCheckBox2 = new System.Windows.Forms.CheckBox();
            this.ButtonsVisible2 = new System.Windows.Forms.CheckBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            //
            // zgc2
            //
            this.zgc2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zgc2.AutoSize = true;
            this.zgc2.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc2.Location = new System.Drawing.Point(9, 112);
            this.zgc2.Margin = new System.Windows.Forms.Padding(0);
            this.zgc2.Name = "zgc2";
            this.zgc2.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zgc2.ScrollGrace = 0D;
            this.zgc2.ScrollMaxX = 0D;
            this.zgc2.ScrollMaxY = 0D;
            this.zgc2.ScrollMaxY2 = 0D;
            this.zgc2.ScrollMinX = 0D;
            this.zgc2.ScrollMinY = 0D;
            this.zgc2.ScrollMinY2 = 0D;
            this.zgc2.Size = new System.Drawing.Size(1204, 373);
            this.zgc2.TabIndex = 0;
            //
            // FreshScreenButton2
            //
            this.FreshScreenButton2.Location = new System.Drawing.Point(252, 1);
            this.FreshScreenButton2.Name = "FreshScreenButton2";
            this.FreshScreenButton2.Size = new System.Drawing.Size(75, 21);
            this.FreshScreenButton2.TabIndex = 3;
            this.FreshScreenButton2.Text = "FreshScreen";
            this.FreshScreenButton2.UseVisualStyleBackColor = true;
            //
            // YMaxNum2
            //
            this.YMaxNum2.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMaxNum2.Location = new System.Drawing.Point(441, 27);
            this.YMaxNum2.Maximum = new decimal(new int[] {
            30000000,
            0,
            0,
            0});
            this.YMaxNum2.Name = "YMaxNum2";
            this.YMaxNum2.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum2.TabIndex = 20;
            this.YMaxNum2.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            //
            // YMinNum2
            //
            this.YMinNum2.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.YMinNum2.Location = new System.Drawing.Point(352, 27);
            this.YMinNum2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.YMinNum2.Name = "YMinNum2";
            this.YMinNum2.Size = new System.Drawing.Size(47, 20);
            this.YMinNum2.TabIndex = 22;
            //
            // AvChunkBox2
            //
            this.AvChunkBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AvChunkBox2.DecimalPlaces = 1;
            this.AvChunkBox2.Location = new System.Drawing.Point(527, 28);
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
            this.AvChunkBox2.TabIndex = 24;
            this.AvChunkBox2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            //
            // XScale2
            //
            this.XScale2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.XScale2.Location = new System.Drawing.Point(613, 28);
            this.XScale2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XScale2.Name = "XScale2";
            this.XScale2.Size = new System.Drawing.Size(57, 20);
            this.XScale2.TabIndex = 26;
            this.XScale2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // AverageBox2
            //
            this.AverageBox2.AutoSize = true;
            this.AverageBox2.Location = new System.Drawing.Point(10, 2);
            this.AverageBox2.Name = "AverageBox2";
            this.AverageBox2.Size = new System.Drawing.Size(87, 17);
            this.AverageBox2.TabIndex = 35;
            this.AverageBox2.Text = "Average /ms";
            this.AverageBox2.UseVisualStyleBackColor = true;
            //
            // AutoScale2
            //
            this.AutoScale2.AutoSize = true;
            this.AutoScale2.Checked = true;
            this.AutoScale2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoScale2.Location = new System.Drawing.Point(103, 28);
            this.AutoScale2.Name = "AutoScale2";
            this.AutoScale2.Size = new System.Drawing.Size(85, 17);
            this.AutoScale2.TabIndex = 36;
            this.AutoScale2.Text = "Auto-rescale";
            this.AutoScale2.UseVisualStyleBackColor = true;
            //
            // ScrollingCheckBox2
            //
            this.ScrollingCheckBox2.AutoSize = true;
            this.ScrollingCheckBox2.Location = new System.Drawing.Point(103, 4);
            this.ScrollingCheckBox2.Name = "ScrollingCheckBox2";
            this.ScrollingCheckBox2.Size = new System.Drawing.Size(66, 17);
            this.ScrollingCheckBox2.TabIndex = 40;
            this.ScrollingCheckBox2.Text = "Scrolling";
            this.ScrollingCheckBox2.UseVisualStyleBackColor = true;
            //
            // ZoomIn2
            //
            this.ZoomIn2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ZoomIn2.AutoSize = true;
            this.ZoomIn2.Location = new System.Drawing.Point(719, 29);
            this.ZoomIn2.Name = "ZoomIn2";
            this.ZoomIn2.Size = new System.Drawing.Size(65, 17);
            this.ZoomIn2.TabIndex = 41;
            this.ZoomIn2.Text = "Zoom In";
            this.ZoomIn2.UseVisualStyleBackColor = true;
            //
            // label10_2
            //
            this.label10_2.AutoSize = true;
            this.label10_2.Location = new System.Drawing.Point(447, 10);
            this.label10_2.Name = "label10_2";
            this.label10_2.Size = new System.Drawing.Size(34, 13);
            this.label10_2.TabIndex = 47;
            this.label10_2.Text = "YMax";
            //
            // label12_2
            //
            this.label12_2.AutoSize = true;
            this.label12_2.Location = new System.Drawing.Point(359, 6);
            this.label12_2.Name = "label12_2";
            this.label12_2.Size = new System.Drawing.Size(31, 13);
            this.label12_2.TabIndex = 49;
            this.label12_2.Text = "YMin";
            //
            // PMT2
            //
            this.PMT2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PMT2.FormattingEnabled = true;
            this.PMT2.Items.AddRange(new object[] {
            "PMT1",
            "PMT2",
            "Both"});
            this.PMT2.Location = new System.Drawing.Point(835, 24);
            this.PMT2.Name = "PMT2";
            this.PMT2.Size = new System.Drawing.Size(57, 21);
            this.PMT2.TabIndex = 60;
            this.PMT2.Text = "Both";
            //
            // TimeControl2
            //
            this.TimeControl2.AutoSize = true;
            this.TimeControl2.Location = new System.Drawing.Point(215, 29);
            this.TimeControl2.Name = "TimeControl2";
            this.TimeControl2.Size = new System.Drawing.Size(94, 17);
            this.TimeControl2.TabIndex = 64;
            this.TimeControl2.Text = "True time LHS";
            this.TimeControl2.UseVisualStyleBackColor = true;
            //
            // ResetButton2
            //
            this.ResetButton2.Location = new System.Drawing.Point(171, 1);
            this.ResetButton2.Name = "ResetButton2";
            this.ResetButton2.Size = new System.Drawing.Size(75, 23);
            this.ResetButton2.TabIndex = 68;
            this.ResetButton2.Text = "Reset";
            this.ResetButton2.UseVisualStyleBackColor = true;
            //
            // label13_2
            //
            this.label13_2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13_2.AutoSize = true;
            this.label13_2.Location = new System.Drawing.Point(524, 10);
            this.label13_2.Name = "label13_2";
            this.label13_2.Size = new System.Drawing.Size(64, 13);
            this.label13_2.TabIndex = 69;
            this.label13_2.Text = "ms per point";
            //
            // label14_2
            //
            this.label14_2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14_2.AutoSize = true;
            this.label14_2.Location = new System.Drawing.Point(610, 11);
            this.label14_2.Name = "label14_2";
            this.label14_2.Size = new System.Drawing.Size(77, 13);
            this.label14_2.TabIndex = 70;
            this.label14_2.Text = "X Scale Factor";
            //
            // label15_2
            //
            this.label15_2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15_2.AutoSize = true;
            this.label15_2.Location = new System.Drawing.Point(833, 7);
            this.label15_2.Name = "label15_2";
            this.label15_2.Size = new System.Drawing.Size(63, 13);
            this.label15_2.TabIndex = 71;
            this.label15_2.Text = "PMT Select";
            //
            // ThresholdScrollBar2
            //
            this.ThresholdScrollBar2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ThresholdScrollBar2.Location = new System.Drawing.Point(72, 152);
            this.ThresholdScrollBar2.Name = "ThresholdScrollBar2";
            this.ThresholdScrollBar2.Size = new System.Drawing.Size(10, 277);
            this.ThresholdScrollBar2.TabIndex = 79;
            this.ThresholdScrollBar2.Value = 100;
            //
            // ThresholdCheckBox2
            //
            this.ThresholdCheckBox2.AutoSize = true;
            this.ThresholdCheckBox2.Checked = true;
            this.ThresholdCheckBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThresholdCheckBox2.Location = new System.Drawing.Point(7, 21);
            this.ThresholdCheckBox2.Name = "ThresholdCheckBox2";
            this.ThresholdCheckBox2.Size = new System.Drawing.Size(90, 30);
            this.ThresholdCheckBox2.TabIndex = 84;
            this.ThresholdCheckBox2.Text = "Threshold \r\nFluorescence";
            this.ThresholdCheckBox2.UseVisualStyleBackColor = true;
            this.ThresholdCheckBox2.CheckedChanged += new System.EventHandler(this.ThresholdCheckBox2_CheckedChanged);
            //
            // ButtonsVisible2
            //
            this.ButtonsVisible2.AutoSize = true;
            this.ButtonsVisible2.Checked = true;
            this.ButtonsVisible2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonsVisible2.Location = new System.Drawing.Point(719, 4);
            this.ButtonsVisible2.Name = "ButtonsVisible2";
            this.ButtonsVisible2.Size = new System.Drawing.Size(98, 17);
            this.ButtonsVisible2.TabIndex = 85;
            this.ButtonsVisible2.Text = "Set Parameters";
            this.ButtonsVisible2.UseVisualStyleBackColor = true;
            this.ButtonsVisible2.CheckedChanged += new System.EventHandler(this.ButtonsVisible2_CheckedChanged);
            //
            // trackBar1
            //
            this.trackBar1.Location = new System.Drawing.Point(295, 440);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 86;
            //
            // NewWindow
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 494);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.ButtonsVisible2);
            this.Controls.Add(this.ThresholdCheckBox2);
            this.Controls.Add(this.ThresholdScrollBar2);
            this.Controls.Add(this.label15_2);
            this.Controls.Add(this.label14_2);
            this.Controls.Add(this.label13_2);
            this.Controls.Add(this.ResetButton2);
            this.Controls.Add(this.TimeControl2);
            this.Controls.Add(this.PMT2);
            this.Controls.Add(this.label12_2);
            this.Controls.Add(this.label10_2);
            this.Controls.Add(this.ZoomIn2);
            this.Controls.Add(this.ScrollingCheckBox2);
            this.Controls.Add(this.AutoScale2);
            this.Controls.Add(this.AverageBox2);
            this.Controls.Add(this.XScale2);
            this.Controls.Add(this.AvChunkBox2);
            this.Controls.Add(this.YMinNum2);
            this.Controls.Add(this.YMaxNum2);
            this.Controls.Add(this.FreshScreenButton2);
            this.Controls.Add(this.zgc2);
            this.Name = "NewWindow";
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XScale2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }










        #endregion

        public ZedGraph.ZedGraphControl zgc2;
        public System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.Button FreshScreenButton2;
        public System.Windows.Forms.NumericUpDown YMaxNum2;
        public System.Windows.Forms.NumericUpDown YMinNum2;
        public System.Windows.Forms.NumericUpDown AvChunkBox2;
        public System.Windows.Forms.NumericUpDown XScale2;
        public System.Windows.Forms.CheckBox AverageBox2;
        public System.Windows.Forms.CheckBox AutoScale2;
        public System.Windows.Forms.CheckBox ScrollingCheckBox2;
        public System.Windows.Forms.CheckBox ZoomIn2;
        public System.Windows.Forms.Label label10_2;
        public System.Windows.Forms.Label label12_2;
        public System.Windows.Forms.ComboBox PMT2;
        public System.Windows.Forms.CheckBox TimeControl2;
        public System.Windows.Forms.Button ResetButton2;
        public System.Windows.Forms.Label label13_2;
        public System.Windows.Forms.Label label14_2;
        public System.Windows.Forms.Label label15_2;
        public System.Windows.Forms.VScrollBar ThresholdScrollBar2;
        public System.Windows.Forms.CheckBox ThresholdCheckBox2;
        public System.Windows.Forms.CheckBox ButtonsVisible2;
        public System.Windows.Forms.TrackBar trackBar1;
        private System.ComponentModel.IContainer components;
    }
}