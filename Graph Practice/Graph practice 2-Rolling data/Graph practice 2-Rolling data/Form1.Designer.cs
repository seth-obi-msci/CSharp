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
            this.SuspendLayout();
            // 
            // zgc
            // 
            this.zgc.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.zgc.IsAutoScrollRange = false;
            this.zgc.IsEnableHEdit = false;
            this.zgc.IsEnableHPan = true;
            this.zgc.IsEnableHZoom = true;
            this.zgc.IsEnableVEdit = false;
            this.zgc.IsEnableVPan = true;
            this.zgc.IsEnableVZoom = true;
            this.zgc.IsPrintFillPage = true;
            this.zgc.IsPrintKeepAspectRatio = true;
            this.zgc.IsScrollY2 = false;
            this.zgc.IsShowContextMenu = true;
            this.zgc.IsShowCopyMessage = true;
            this.zgc.IsShowCursorValues = false;
            this.zgc.IsShowHScrollBar = false;
            this.zgc.IsShowPointValues = false;
            this.zgc.IsShowVScrollBar = false;
            this.zgc.IsSynchronizeXAxes = false;
            this.zgc.IsSynchronizeYAxes = false;
            this.zgc.IsZoomOnMouseCenter = false;
            this.zgc.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.zgc.Location = new System.Drawing.Point(12, 12);
            this.zgc.Name = "zgc";
            this.zgc.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.zgc.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zgc.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.zgc.PointDateFormat = "g";
            this.zgc.PointValueFormat = "G";
            this.zgc.ScrollMaxX = 0;
            this.zgc.ScrollMaxY = 0;
            this.zgc.ScrollMaxY2 = 0;
            this.zgc.ScrollMinX = 0;
            this.zgc.ScrollMinY = 0;
            this.zgc.ScrollMinY2 = 0;
            this.zgc.Size = new System.Drawing.Size(547, 325);
            this.zgc.TabIndex = 0;
            this.zgc.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.zgc.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.zgc.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.zgc.ZoomStepFraction = 0.1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // RollingGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 349);
            this.Controls.Add(this.zgc);
            this.Name = "RollingGraph";
            this.Text = "RollingGraph";
            this.Resize += new System.EventHandler(this.RollingGraph_Resize);
            this.Load += new System.EventHandler(this.RollingGraph_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zgc;
        private System.Windows.Forms.Timer timer1;
    }
}