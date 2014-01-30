﻿using ZedGraph;

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
        private void InitializeComponent()
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
            this.YMaxNum = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.YMinNum = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.AvChunkSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.XRange = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XRange)).BeginInit();
            this.SuspendLayout();
            // 
            // zgc
            // 
            this.zgc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zgc.AutoSize = true;
            this.zgc.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.Location = new System.Drawing.Point(12, 46);
            this.zgc.Margin = new System.Windows.Forms.Padding(100, 10, 10, 10);
            this.zgc.Name = "zgc";
            this.zgc.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zgc.ScrollGrace = 0D;
            this.zgc.ScrollMaxX = 0D;
            this.zgc.ScrollMaxY = 0D;
            this.zgc.ScrollMaxY2 = 0D;
            this.zgc.ScrollMinX = 0D;
            this.zgc.ScrollMinY = 0D;
            this.zgc.ScrollMinY2 = 0D;
            this.zgc.Size = new System.Drawing.Size(1164, 373);
            this.zgc.TabIndex = 0;
            this.zgc.Load += new System.EventHandler(this.zgc_Load);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(360, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "SaveBytes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveBytesButton);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(465, 20);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(73, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "AutoSave";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.AutoSave);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(279, 17);
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
            this.trackBar1.Location = new System.Drawing.Point(12, 421);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(547, 45);
            this.trackBar1.TabIndex = 5;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(131, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 21);
            this.button3.TabIndex = 6;
            this.button3.Text = "Pause";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.PauseButton);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(212, 17);
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
            this.label2.Location = new System.Drawing.Point(860, 439);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "File Location:";
            // 
            // filename
            // 
            this.filename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.filename.Location = new System.Drawing.Point(936, 432);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(247, 20);
            this.filename.TabIndex = 12;
            this.filename.TextChanged += new System.EventHandler(this.filename_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1016, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Set X Range";
            // 
            // YMaxNum
            // 
            this.YMaxNum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.YMaxNum.Location = new System.Drawing.Point(863, 19);
            this.YMaxNum.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.YMaxNum.Name = "YMaxNum";
            this.YMaxNum.Size = new System.Drawing.Size(52, 20);
            this.YMaxNum.TabIndex = 20;
            this.YMaxNum.Enter += new System.EventHandler(this.YMaxNum_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(820, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Y Max";
            // 
            // YMinNum
            // 
            this.YMinNum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.YMinNum.Location = new System.Drawing.Point(759, 17);
            this.YMinNum.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.YMinNum.Name = "YMinNum";
            this.YMinNum.Size = new System.Drawing.Size(47, 20);
            this.YMinNum.TabIndex = 22;
            this.YMinNum.Enter += new System.EventHandler(this.YMinNum_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(719, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Y Min";
            // 
            // AvChunkSize
            // 
            this.AvChunkSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.AvChunkSize.Location = new System.Drawing.Point(656, 17);
            this.AvChunkSize.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.AvChunkSize.Name = "AvChunkSize";
            this.AvChunkSize.Size = new System.Drawing.Size(46, 20);
            this.AvChunkSize.TabIndex = 24;
            this.AvChunkSize.ValueChanged += new System.EventHandler(this.AvChunkSize_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(555, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Counts to Average";
            // 
            // XRange
            // 
            this.XRange.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.XRange.Location = new System.Drawing.Point(1090, 17);
            this.XRange.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.XRange.Name = "XRange";
            this.XRange.Size = new System.Drawing.Size(57, 20);
            this.XRange.TabIndex = 26;
            this.XRange.ValueChanged += new System.EventHandler(this.XRange_ValueChanged);
            // 
            // RollingGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 461);
            this.Controls.Add(this.XRange);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AvChunkSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.YMinNum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.YMaxNum);
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
            ((System.ComponentModel.ISupportInitialize)(this.YMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YMinNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvChunkSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XRange)).EndInit();
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
        private System.Windows.Forms.NumericUpDown YMaxNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown YMinNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown AvChunkSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown XRange;
    }
}