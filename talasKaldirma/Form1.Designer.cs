namespace talasKaldirma
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            devDept.Eyeshot.Control.ProgressBar progressBar2 = new devDept.Eyeshot.Control.ProgressBar(devDept.Eyeshot.Control.ProgressBar.styleType.Speedometer, 0, "Idle", "Idle", Color.Black, Color.Transparent, Color.Green, 1D, false, false, 0.1D, 0.333D, true, true);
            devDept.Eyeshot.Control.SimulationTimeLine simulationTimeLine2 = new devDept.Eyeshot.Control.SimulationTimeLine(Color.Black, Color.Orange, Color.Green, Color.Red, Color.LightGray, 1D, true, 0.01D);
            devDept.Eyeshot.Control.BackgroundSettings backgroundSettings2 = new devDept.Eyeshot.Control.BackgroundSettings(devDept.Graphics.backgroundStyleType.LinearGradient, Color.FromArgb(46, 82, 103), Color.DodgerBlue, Color.FromArgb(18, 32, 41), 0.75D, null, devDept.Eyeshot.colorThemeType.Auto, 0.33D);
            devDept.Eyeshot.Camera camera2 = new devDept.Eyeshot.Camera(new devDept.Geometry.Point3D(64.0062895995168D, -124.79438226926727D, 110.81242132330227D), 331.10339631698054D, new devDept.Geometry.Quaternion(0.12940952255126034D, 0.22414386804201339D, 0.4829629131445341D, 0.83651630373780794D), devDept.Eyeshot.projectionType.Perspective, 50D, 49.209341901137947D, false, 0.001D);
            devDept.Eyeshot.Control.HomeToolBarButton homeToolBarButton2 = new devDept.Eyeshot.Control.HomeToolBarButton("Home", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, true);
            devDept.Eyeshot.Control.MagnifyingGlassToolBarButton magnifyingGlassToolBarButton2 = new devDept.Eyeshot.Control.MagnifyingGlassToolBarButton("Magnifying Glass", devDept.Eyeshot.Control.ToolBarButton.styleType.ToggleButton, true, true);
            devDept.Eyeshot.Control.ZoomWindowToolBarButton zoomWindowToolBarButton2 = new devDept.Eyeshot.Control.ZoomWindowToolBarButton("Zoom Window", devDept.Eyeshot.Control.ToolBarButton.styleType.ToggleButton, true, true);
            devDept.Eyeshot.Control.ZoomToolBarButton zoomToolBarButton2 = new devDept.Eyeshot.Control.ZoomToolBarButton("Zoom", devDept.Eyeshot.Control.ToolBarButton.styleType.ToggleButton, true, true);
            devDept.Eyeshot.Control.PanToolBarButton panToolBarButton2 = new devDept.Eyeshot.Control.PanToolBarButton("Pan", devDept.Eyeshot.Control.ToolBarButton.styleType.ToggleButton, true, true);
            devDept.Eyeshot.Control.RotateToolBarButton rotateToolBarButton2 = new devDept.Eyeshot.Control.RotateToolBarButton("Rotate", devDept.Eyeshot.Control.ToolBarButton.styleType.ToggleButton, true, true);
            devDept.Eyeshot.Control.ZoomFitToolBarButton zoomFitToolBarButton2 = new devDept.Eyeshot.Control.ZoomFitToolBarButton("Zoom Fit", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, true);
            devDept.Eyeshot.Control.ToolBar toolBar3 = new devDept.Eyeshot.Control.ToolBar(devDept.Eyeshot.Control.ToolBar.positionType.HorizontalTopCenter, true, new devDept.Eyeshot.Control.ToolBarButton[] { homeToolBarButton2, magnifyingGlassToolBarButton2, zoomWindowToolBarButton2, zoomToolBarButton2, panToolBarButton2, rotateToolBarButton2, zoomFitToolBarButton2 }, 5, 0, Color.FromArgb(0, 0, 0, 0), 0D, Color.FromArgb(0, 0, 0, 0), 0D);
            devDept.Eyeshot.Control.BeginningToolBarButton beginningToolBarButton2 = new devDept.Eyeshot.Control.BeginningToolBarButton("Go to beginning", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, false);
            devDept.Eyeshot.Control.PreviousToolBarButton previousToolBarButton2 = new devDept.Eyeshot.Control.PreviousToolBarButton("Go to previous (Ctrl+L)", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, false);
            devDept.Eyeshot.Control.StartToolBarButton startToolBarButton2 = new devDept.Eyeshot.Control.StartToolBarButton("Start the simulation (Ctrl+Space)", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, false);
            devDept.Eyeshot.Control.PauseToolBarButton pauseToolBarButton2 = new devDept.Eyeshot.Control.PauseToolBarButton("Pause the simulation (Ctrl+Space)", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, false, false);
            devDept.Eyeshot.Control.NextToolBarButton nextToolBarButton2 = new devDept.Eyeshot.Control.NextToolBarButton("Go to next (Ctrl+R)", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, false);
            devDept.Eyeshot.Control.EndToolBarButton endToolBarButton2 = new devDept.Eyeshot.Control.EndToolBarButton("Go to end", devDept.Eyeshot.Control.ToolBarButton.styleType.PushButton, true, false);
            devDept.Eyeshot.Control.ToolBar toolBar4 = new devDept.Eyeshot.Control.ToolBar(devDept.Eyeshot.Control.ToolBar.positionType.HorizontalBottomCenter, true, new devDept.Eyeshot.Control.ToolBarButton[] { beginningToolBarButton2, previousToolBarButton2, startToolBarButton2, pauseToolBarButton2, nextToolBarButton2, endToolBarButton2 }, 5, 0, Color.FromArgb(0, 0, 0, 0), 0D, Color.FromArgb(0, 0, 0, 0), 0D);
            devDept.Eyeshot.Control.LegendItem legendItem10 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(0, 0, 255));
            devDept.Eyeshot.Control.LegendItem legendItem11 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(0, 127, 255));
            devDept.Eyeshot.Control.LegendItem legendItem12 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(0, 255, 255));
            devDept.Eyeshot.Control.LegendItem legendItem13 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(0, 255, 127));
            devDept.Eyeshot.Control.LegendItem legendItem14 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(0, 255, 0));
            devDept.Eyeshot.Control.LegendItem legendItem15 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(127, 255, 0));
            devDept.Eyeshot.Control.LegendItem legendItem16 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(255, 255, 0));
            devDept.Eyeshot.Control.LegendItem legendItem17 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(255, 127, 0));
            devDept.Eyeshot.Control.LegendItem legendItem18 = new devDept.Eyeshot.Control.LegendItem(10, 30, Color.FromArgb(255, 0, 0));
            devDept.Eyeshot.Control.Legend legend2 = new devDept.Eyeshot.Control.Legend(0D, 100D, "Title", "Subtitle", true, false, false, "{0:+0.###;-0.###;0}", Color.Transparent, Color.Black, Color.Black, null, null, new devDept.Eyeshot.Control.LegendItem[] { legendItem10, legendItem11, legendItem12, legendItem13, legendItem14, legendItem15, legendItem16, legendItem17, legendItem18 }, true, false, false, 0, devDept.Eyeshot.Control.Legend.positionType.TopLeft);
            devDept.Eyeshot.Control.Histogram histogram2 = new devDept.Eyeshot.Control.Histogram(30, 80, "Title", Color.Blue, Color.Gray, Color.Black, Color.Red, Color.LightYellow, false, true, false, "{0:+0.###;-0.###;0}");
            devDept.Eyeshot.Control.Grid grid2 = new devDept.Eyeshot.Control.Grid(new devDept.Geometry.Point2D(-100D, -100D), new devDept.Geometry.Point2D(100D, 100D), 10D, new devDept.Geometry.Plane(new devDept.Geometry.Point3D(0D, 0D, 0D), new devDept.Geometry.Vector3D(1D, 0D, 0D), new devDept.Geometry.Vector3D(0D, 1D, 0D)), Color.FromArgb(63, 128, 128, 128), Color.FromArgb(127, 255, 0, 0), Color.FromArgb(127, 0, 128, 0), false, true, false, false, 10, 100, 10, Color.FromArgb(127, 90, 90, 90), Color.Transparent, false, Color.FromArgb(12, 0, 0, 255));
            devDept.Eyeshot.Control.OriginSymbol originSymbol2 = new devDept.Eyeshot.Control.OriginSymbol(10, devDept.Eyeshot.Control.originSymbolStyleType.Ball, new Font("Segoe UI", 9F), Color.Black, Color.Black, Color.Black, Color.Black, Color.Red, Color.Green, Color.Blue, "Origin", "X", "Y", "Z", false, null, false);
            devDept.Eyeshot.Control.RotateSettings rotateSettings2 = new devDept.Eyeshot.Control.RotateSettings(new devDept.Eyeshot.Control.MouseButton(devDept.Eyeshot.Control.mouseButtonsZPR.Middle, devDept.Eyeshot.Control.modifierKeys.None), 10D, true, 1D, devDept.Eyeshot.rotationType.Trackball, devDept.Eyeshot.rotationCenterType.CursorLocation, new devDept.Geometry.Point3D(0D, 0D, 0D), false);
            devDept.Eyeshot.Control.ZoomSettings zoomSettings2 = new devDept.Eyeshot.Control.ZoomSettings(new devDept.Eyeshot.Control.MouseButton(devDept.Eyeshot.Control.mouseButtonsZPR.Middle, devDept.Eyeshot.Control.modifierKeys.Shift), 25, true, devDept.Eyeshot.zoomStyleType.AtCursorLocation, false, 1D, Color.Empty, devDept.Eyeshot.Camera.perspectiveFitType.Accurate, false, 10, true);
            devDept.Eyeshot.Control.PanSettings panSettings2 = new devDept.Eyeshot.Control.PanSettings(new devDept.Eyeshot.Control.MouseButton(devDept.Eyeshot.Control.mouseButtonsZPR.Middle, devDept.Eyeshot.Control.modifierKeys.Ctrl), 25, true);
            devDept.Eyeshot.Control.NavigationSettings navigationSettings2 = new devDept.Eyeshot.Control.NavigationSettings(devDept.Eyeshot.Camera.navigationType.Examine, new devDept.Eyeshot.Control.MouseButton(devDept.Eyeshot.Control.mouseButtonsZPR.Left, devDept.Eyeshot.Control.modifierKeys.None), new devDept.Geometry.Point3D(-1000D, -1000D, -1000D), new devDept.Geometry.Point3D(1000D, 1000D, 1000D), 8D, 50D, 50D);
            devDept.Eyeshot.Control.CoordinateSystemIcon coordinateSystemIcon2 = new devDept.Eyeshot.Control.CoordinateSystemIcon(new Font("Segoe UI", 9F), Color.Black, Color.Black, Color.Black, Color.Black, Color.FromArgb(80, 80, 80), Color.FromArgb(80, 80, 80), Color.OrangeRed, "Origin", "X", "Y", "Z", true, devDept.Eyeshot.Control.coordinateSystemPositionType.BottomLeft, 37, null, false);
            devDept.Eyeshot.Control.ViewCubeIcon viewCubeIcon2 = new devDept.Eyeshot.Control.ViewCubeIcon(devDept.Eyeshot.Control.coordinateSystemPositionType.TopRight, true, Color.FromArgb(220, 20, 60), true, "FRONT", "BACK", "LEFT", "RIGHT", "TOP", "BOTTOM", Color.FromArgb(240, 77, 77, 77), Color.FromArgb(240, 77, 77, 77), Color.FromArgb(240, 77, 77, 77), Color.FromArgb(240, 77, 77, 77), Color.FromArgb(240, 77, 77, 77), Color.FromArgb(240, 77, 77, 77), 'S', 'N', 'W', 'E', true, null, Color.White, Color.Black, 120, true, true, null, null, null, null, null, null, false, new devDept.Geometry.Quaternion(0D, 0D, 0D, 1D), true);
            devDept.Eyeshot.Control.ScaleBar scaleBar2 = new devDept.Eyeshot.Control.ScaleBar(false, Color.Black, Color.White, Color.Black, "{0:G6}", false, devDept.Eyeshot.Control.ScaleBar.styleType.Alternate, 4, 15, 0.4D, devDept.Eyeshot.Control.ScaleBar.positionType.BottomCenter, null, null);
            devDept.Eyeshot.Control.Viewport viewport2 = new devDept.Eyeshot.Control.Viewport(new Point(0, 0), new Size(1397, 979), backgroundSettings2, camera2, new devDept.Eyeshot.Control.ToolBar[] { toolBar3, toolBar4 }, new devDept.Eyeshot.Control.Legend[] { legend2 }, histogram2, devDept.Eyeshot.displayType.Rendered, true, false, false, new devDept.Eyeshot.Control.Grid[] { grid2 }, new devDept.Eyeshot.Control.OriginSymbol[] { originSymbol2 }, false, rotateSettings2, zoomSettings2, panSettings2, navigationSettings2, coordinateSystemIcon2, viewCubeIcon2, scaleBar2);
            manufacture1 = new devDept.Eyeshot.Control.Manufacture();
            button1 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)manufacture1).BeginInit();
            SuspendLayout();
            // 
            // manufacture1
            // 
            manufacture1.Dock = DockStyle.Fill;
            manufacture1.Location = new Point(0, 0);
            manufacture1.Name = "manufacture1";
            manufacture1.ProgressBar = progressBar2;
            manufacture1.SimulationTimeLine = simulationTimeLine2;
            manufacture1.Size = new Size(1397, 979);
            manufacture1.TabIndex = 0;
            manufacture1.Text = "manufacture1";
            manufacture1.Viewports.Add(viewport2);
            // 
            // button1
            // 
            button1.Location = new Point(38, 67);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(38, 114);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1397, 979);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(manufacture1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)manufacture1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private devDept.Eyeshot.Control.Manufacture manufacture1;
        private Button button1;
        private Button button2;
    }
}
