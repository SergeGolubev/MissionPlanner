namespace MissionPlanner.Wizard
{
    partial class GPS_Check
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.radialGradientBG1 = new MissionPlanner.Controls.GradientBG();
            this.panelMap = new System.Windows.Forms.Panel();
            this.lbl_distance = new System.Windows.Forms.Label();
            this.lbl_homedist = new System.Windows.Forms.Label();
            this.lbl_prevdist = new System.Windows.Forms.Label();
            this.MainMap = new MissionPlanner.Controls.myGMAP();
            this.trackBar1 = new MissionPlanner.Controls.MyTrackBar();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radialGradientBG1.Image)).BeginInit();
            this.panelMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // radialGradientBG1
            // 
            this.radialGradientBG1.BackColor = System.Drawing.Color.Black;
            this.radialGradientBG1.CenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(164)))), ((int)(((byte)(33)))));
            // 
            // 
            // 
            this.radialGradientBG1.Image.BackColor = System.Drawing.Color.Transparent;
            this.radialGradientBG1.Image.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radialGradientBG1.Image.Location = new System.Drawing.Point(38, 10);
            this.radialGradientBG1.Image.MaximumSize = new System.Drawing.Size(288, 72);
            this.radialGradientBG1.Image.MinimumSize = new System.Drawing.Size(288, 72);
            this.radialGradientBG1.Image.Name = "_Image";
            this.radialGradientBG1.Image.Size = new System.Drawing.Size(288, 72);
            this.radialGradientBG1.Image.TabIndex = 0;
            this.radialGradientBG1.Image.TabStop = false;
            this.radialGradientBG1.Image.Visible = false;
            // 
            // 
            // 
            this.radialGradientBG1.Label.AutoSize = true;
            this.radialGradientBG1.Label.BackColor = System.Drawing.Color.Transparent;
            this.radialGradientBG1.Label.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.radialGradientBG1.Label.ForeColor = System.Drawing.Color.Black;
            this.radialGradientBG1.Label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radialGradientBG1.Label.Location = new System.Drawing.Point(30, 5);
            this.radialGradientBG1.Label.Name = "_Label";
            this.radialGradientBG1.Label.Size = new System.Drawing.Size(186, 29);
            this.radialGradientBG1.Label.TabIndex = 1;
            this.radialGradientBG1.Label.Text = "Проверка GPS";
            this.radialGradientBG1.Location = new System.Drawing.Point(0, 0);
            this.radialGradientBG1.Name = "radialGradientBG1";
            this.radialGradientBG1.OutsideColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(107)))), ((int)(((byte)(10)))));
            this.radialGradientBG1.Size = new System.Drawing.Size(800, 41);
            this.radialGradientBG1.TabIndex = 6;
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.lbl_distance);
            this.panelMap.Controls.Add(this.lbl_homedist);
            this.panelMap.Controls.Add(this.lbl_prevdist);
            this.panelMap.Controls.Add(this.MainMap);
            this.panelMap.Controls.Add(this.trackBar1);
            this.panelMap.Controls.Add(this.label11);
            this.panelMap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelMap.Location = new System.Drawing.Point(3, 63);
            this.panelMap.MinimumSize = new System.Drawing.Size(27, 27);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(800, 375);
            this.panelMap.TabIndex = 52;
            this.panelMap.Text = "panel6";
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            // 
            // lbl_distance
            // 
            this.lbl_distance.AutoSize = true;
            this.lbl_distance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_distance.Location = new System.Drawing.Point(3, 43);
            this.lbl_distance.Name = "lbl_distance";
            this.lbl_distance.Size = new System.Drawing.Size(49, 13);
            this.lbl_distance.TabIndex = 48;
            this.lbl_distance.Text = "Distance";
            // 
            // lbl_homedist
            // 
            this.lbl_homedist.AutoSize = true;
            this.lbl_homedist.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_homedist.Location = new System.Drawing.Point(3, 69);
            this.lbl_homedist.Name = "lbl_homedist";
            this.lbl_homedist.Size = new System.Drawing.Size(35, 13);
            this.lbl_homedist.TabIndex = 50;
            this.lbl_homedist.Text = "Home";
            // 
            // lbl_prevdist
            // 
            this.lbl_prevdist.AutoSize = true;
            this.lbl_prevdist.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_prevdist.Location = new System.Drawing.Point(3, 56);
            this.lbl_prevdist.Name = "lbl_prevdist";
            this.lbl_prevdist.Size = new System.Drawing.Size(29, 13);
            this.lbl_prevdist.TabIndex = 49;
            this.lbl_prevdist.Text = "Prev";
            // 
            // MainMap
            // 
            this.MainMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.MainMap.Bearing = 0F;
            this.MainMap.CanDragMap = true;
            this.MainMap.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainMap.EmptyTileColor = System.Drawing.Color.Gray;
            this.MainMap.GrayScaleMode = false;
            this.MainMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.MainMap.LevelsKeepInMemmory = 5;
            this.MainMap.Location = new System.Drawing.Point(27, 103);
            this.MainMap.MarkersEnabled = true;
            this.MainMap.MaxZoom = 19;
            this.MainMap.MinZoom = 0;
            this.MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MainMap.Name = "MainMap";
            this.MainMap.NegativeMode = false;
            this.MainMap.PolygonsEnabled = true;
            this.MainMap.RetryLoadTile = 0;
            this.MainMap.RoutesEnabled = false;
            this.MainMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional;
            this.MainMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.MainMap.ShowTileGridLines = false;
            this.MainMap.Size = new System.Drawing.Size(722, 233);
            this.MainMap.TabIndex = 45;
            this.MainMap.Zoom = 0D;
            this.MainMap.Paint += new System.Windows.Forms.PaintEventHandler(this.MainMap_Paint);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.trackBar1.LargeChange = 0.005F;
            this.trackBar1.Location = new System.Drawing.Point(755, 69);
            this.trackBar1.Maximum = 24F;
            this.trackBar1.Minimum = 1F;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 303);
            this.trackBar1.SmallChange = 0.001F;
            this.trackBar1.TabIndex = 46;
            this.trackBar1.TickFrequency = 1F;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 2F;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(766, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 47;
            this.label11.Text = "Zoom";
            // 
            // GPS_Check
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.radialGradientBG1);
            this.Name = "GPS_Check";
            this.Size = new System.Drawing.Size(800, 500);
            ((System.ComponentModel.ISupportInitialize)(this.radialGradientBG1.Image)).EndInit();
            this.panelMap.ResumeLayout(false);
            this.panelMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.GradientBG radialGradientBG1;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Label lbl_distance;
        private System.Windows.Forms.Label lbl_homedist;
        private System.Windows.Forms.Label lbl_prevdist;
        private Controls.myGMAP MainMap;
        private Controls.MyTrackBar trackBar1;
        private System.Windows.Forms.Label label11;
    }
}
