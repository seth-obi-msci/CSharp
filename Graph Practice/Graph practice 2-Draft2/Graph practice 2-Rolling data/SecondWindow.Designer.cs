namespace Graph_practice_2_Rolling_data
{
    partial class SecondWindow
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
            this.zgc2 = new ZedGraph.ZedGraphControl();
            this.Pause = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // zgc2
            // 
            this.zgc2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zgc2.AutoSize = true;
            this.zgc2.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc2.Location = new System.Drawing.Point(45, 110);
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
            // Pause
            // 
            this.Pause.AutoSize = true;
            this.Pause.Location = new System.Drawing.Point(333, 68);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(56, 17);
            this.Pause.TabIndex = 1;
            this.Pause.Text = "Pause";
            this.Pause.UseVisualStyleBackColor = true;
            this.Pause.CheckedChanged += new System.EventHandler(this.Pause_CheckedChanged);
            // 
            // SecondWindow
            // 
            this.ClientSize = new System.Drawing.Size(1284, 620);
            this.Controls.Add(this.Pause);
            this.Controls.Add(this.zgc2);
            this.Name = "SecondWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        public ZedGraph.ZedGraphControl zgc2;
        public System.Windows.Forms.CheckBox Pause;

   
    }
}