// CDT Week 15_1
// Accomplishment: Implemented Pendown and Penup time calculation

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using System.Collections;

using System.Diagnostics;

// The Ink namespace, which contains the Tablet PC Platform API
using Microsoft.Ink;

namespace CDT_Week_3
{
    /// <summary>
    /// The PaperForm Sample Application form class
    /// </summary>
    public class PaperForm : System.Windows.Forms.Form
    {
        // Declare the ink divider object
        private Divider myInkDivider = null;

        // Collection of Bounding Boxes for words, drawings, lines and paragraphs        
        List<Rectangle> myDrawingBoundingBoxes = new List<Rectangle>();
        List<Rectangle> wordBoundingBoxes = new List<Rectangle>();

        // Retrieve the cusps of the stroke.  The cusps mark the points where
        // the stroke changes direction abruptly.  A segment is defined as the
        // points between two cusps.
        int[] cusps;

        // Declare the InkPicture control, which contains the scanned image

        // A class variable to score the drawing


        // A class varaible to enhance the reliability of recognition
        private RefineNumbers refineNumbers = new RefineNumbers();

        // Objects that represents the ability to create a recognizer context,
        // retrieve its attributes and properties, and determine which packet properties 
        // the recognizer needs to perform recognition.
        Recognizers inkRecognizers;

        // Save the final recognized number to be processed for scoring
        StringBuilder buffer = new StringBuilder();

        // Create the Pen used to draw bounding boxes.
        // First set of bounding boxes drawn here are the bounding boxes of paragraphs.
        // These boxes are drawn with Blue pen.
        Pen penBox = new Pen(Color.Blue, 2);

        // A varaible which stores every storke object made
        Strokes strokes;

        // A array which stores all the rectangular coordinates of every recognized number.
        List<Rectangle> divRects = new List<Rectangle>();
        List<Rectangle> tempDivRects = new List<Rectangle>();

        // Variables to check the airtime of each stroke
        static int time_flag = 0, prev_min = 0, next_min = 0, prev_sec = 0, next_sec = 0, 
            prev_msec = 0, next_msec = 0, air_flag = 0, air_count = 0;

        /// <summary>
        /// Store each stroke for recognizing
        /// </summary>
        List<PenStroke> penStrokes;
        int[] largestStroke = { -1, -1 };          // use for recognize a hand [ area, strokeID ]
        int[] secondStroke = { int.MinValue, -1 }; // use for recognize a hand [ area, strokeID ]
        int HANDSIZE = 2800;
        /// <summary>
        /// Store packet points in one stroke 
        /// will re-declare when a new stroke is drawn
        /// </summary>
        List<PacketPoint> pkPoints;

        // Preparing the spaces which airtime and cusps data will be stored
        //AirCuspsFormat[] acFormat = new AirCuspsFormat[200];
        List<AirCuspsFormat> acFormat;
        
        // Class varaible for file save/open
        FileManage fManage = new FileManage();

        // Class variable for recognizing process after "Recognize" button is pressed.
        RecoRect recoRect = new RecoRect();

        private static int order = 0;

        private static bool[] is_merged, temp_is_merged;

        // Storing each of the last, last moment of stroking, and starting time of clock drawing
        int first_hour = 0, last_hour = 0;
        int first_min = 0, last_min = 0;
        int first_sec = 0, last_sec = 0;
        int first_msec = 0, last_msec = 0;

        // Indicating every X and Y coordinate for each stroke,
        // # of packets in each stroke, and # of all packets
        int indexX, indexY, each_packet_cnt = 0, accu_packet_cnt = 0;

        // Flag to capture the time stamp of the first packet of each stroke.
        private bool first_packet = true;

        #region Standard Template Code

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem miInk;
        private System.Windows.Forms.MenuItem miExit;
        #endregion
        private RadioButton penSelect;
        private RadioButton eraserSelect;
        private Button recogBtn;
        private Button clear_All_Btn;
        private TextBox nameBox;
        private Label label1;
        private InkPicture inkPicture1;
        private Timer tmrRcCounter;
        private Circle clock;
        private System.ComponentModel.IContainer components;

        private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);

        //a class variable bitmap so that all graphic objects are created from the same bitmap
        private Bitmap circleBitmap;
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaperForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.miInk = new System.Windows.Forms.MenuItem();
            this.miExit = new System.Windows.Forms.MenuItem();
            this.penSelect = new System.Windows.Forms.RadioButton();
            this.eraserSelect = new System.Windows.Forms.RadioButton();
            this.recogBtn = new System.Windows.Forms.Button();
            this.clear_All_Btn = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.inkPicture1 = new Microsoft.Ink.InkPicture();
            this.tmrRcCounter = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inkPicture1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miInk});
            // 
            // miInk
            // 
            this.miInk.Index = 0;
            this.miInk.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miExit});
            this.miInk.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.miInk.Text = "&Program";
            // 
            // miExit
            // 
            this.miExit.Index = 0;
            this.miExit.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.miExit.Text = "E&xit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // penSelect
            // 
            this.penSelect.AutoSize = true;
            this.penSelect.Checked = true;
            this.penSelect.Location = new System.Drawing.Point(229, 11);
            this.penSelect.Name = "penSelect";
            this.penSelect.Size = new System.Drawing.Size(60, 17);
            this.penSelect.TabIndex = 5;
            this.penSelect.TabStop = true;
            this.penSelect.Text = "&Pen (P)";
            this.penSelect.UseVisualStyleBackColor = true;
            this.penSelect.CheckedChanged += new System.EventHandler(this.penSelect_CheckedChanged);
            // 
            // eraserSelect
            // 
            this.eraserSelect.AutoSize = true;
            this.eraserSelect.Location = new System.Drawing.Point(295, 11);
            this.eraserSelect.Name = "eraserSelect";
            this.eraserSelect.Size = new System.Drawing.Size(71, 17);
            this.eraserSelect.TabIndex = 6;
            this.eraserSelect.Text = "&Eraser (E)";
            this.eraserSelect.UseVisualStyleBackColor = true;
            this.eraserSelect.Visible = false;
            this.eraserSelect.CheckedChanged += new System.EventHandler(this.eraserSelect_CheckedChanged);
            // 
            // recogBtn
            // 
            this.recogBtn.AccessibleName = "";
            this.recogBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recogBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.recogBtn.Location = new System.Drawing.Point(876, 4);
            this.recogBtn.Name = "recogBtn";
            this.recogBtn.Size = new System.Drawing.Size(125, 31);
            this.recogBtn.TabIndex = 7;
            this.recogBtn.Text = "&Complete (C)";
            this.recogBtn.UseVisualStyleBackColor = true;
            this.recogBtn.Click += new System.EventHandler(this.recoBtn_Click);
            // 
            // clear_All_Btn
            // 
            this.clear_All_Btn.AccessibleName = "";
            this.clear_All_Btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.clear_All_Btn.Location = new System.Drawing.Point(295, 4);
            this.clear_All_Btn.Name = "clear_All_Btn";
            this.clear_All_Btn.Size = new System.Drawing.Size(125, 31);
            this.clear_All_Btn.TabIndex = 8;
            this.clear_All_Btn.Text = "Start &Over(O)";
            this.clear_All_Btn.UseVisualStyleBackColor = true;
            this.clear_All_Btn.Click += new System.EventHandler(this.ClearAllBtn_Click);
            // 
            // nameBox
            // 
            this.nameBox.Enabled = false;
            this.nameBox.Location = new System.Drawing.Point(64, 8);
            this.nameBox.Multiline = true;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(142, 23);
            this.nameBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ID Num";
            // 
            // inkPicture1
            // 
            this.inkPicture1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inkPicture1.BackColor = System.Drawing.Color.White;
            this.inkPicture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.inkPicture1.InkEnabled = false;
            this.inkPicture1.Location = new System.Drawing.Point(5, 37);
            this.inkPicture1.MarginX = -2147483648;
            this.inkPicture1.MarginY = -2147483648;
            this.inkPicture1.Name = "inkPicture1";
            this.inkPicture1.Size = new System.Drawing.Size(996, 690);
            this.inkPicture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.inkPicture1.TabIndex = 4;
            this.inkPicture1.TabStop = false;
            this.inkPicture1.Stroke += new Microsoft.Ink.InkCollectorStrokeEventHandler(this.inkPicture1_Stroke);
            this.inkPicture1.SizeChanged += new System.EventHandler(this.inkPicture1_LocationChanged);
            this.inkPicture1.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawArea_Paint);
            // 
            // tmrRcCounter
            // 
            this.tmrRcCounter.Tick += new System.EventHandler(this.tmrRcCounter_Tick);
            // 
            // PaperForm
            // 
            this.ClientSize = new System.Drawing.Size(1006, 732);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.clear_All_Btn);
            this.Controls.Add(this.recogBtn);
            this.Controls.Add(this.eraserSelect);
            this.Controls.Add(this.penSelect);
            this.Controls.Add(this.inkPicture1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "PaperForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Clock Drawing Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PaperForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inkPicture1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }        

        #endregion
        public PaperForm()
        {
            
            
            
            #region Standard Template Code
            //
            // Required for Windows Form Designer support
            //
            
            #endregion
            InitializeComponent();

            int width = this.inkPicture1.Width;
            int height = this.inkPicture1.Height;
            Console.WriteLine("Height" + width + "Width" + width);
            
        }

        #region Standard Template Code
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // dispose the divider's resources
                if (myInkDivider != null)
                {
                    // dispose the recognizer context that we associated
                    // with the divider
                    if (myInkDivider.RecognizerContext != null)
                    {
                        myInkDivider.RecognizerContext.Dispose();
                    }

                    // dispose the strokes that we associated
                    // with the divider
                    if (myInkDivider.Strokes != null)
                    {
                        myInkDivider.Strokes.Dispose();
                    }
                    myInkDivider.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        

        #region Standard Template Code
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       {
            //This right here is changing the size of the inkSpace
          Application.Run(new PaperForm());
       }
        #endregion
        
        /// <summary>
        /// Event Handler from PaperForm->Load Event
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PaperForm_Load(object sender, System.EventArgs e)
        {

            //drawClockCircle();

            // declare containers that store the required data
            penStrokes = new List<PenStroke>();
            pkPoints = new List<PacketPoint>();
            acFormat = new List<AirCuspsFormat>();

            // TODO: Not to use the external file to decide the file name
            String applicationPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";

            // Hook event handler for the Stroke event to myInkOverlay_Stroke.
            // This is necessary since the application needs to pass the strokes 
            // to the ink divider.
            inkPicture1.Stroke += new InkCollectorStrokeEventHandler(myInkOverlay_Stroke);

            // Hook the event handler for StrokeDeleting event to myInkOverlay_StrokeDeleting.
            // This is necessary as the application needs to remove the strokes from 
            // ink divider object as well.
            inkPicture1.StrokesDeleting += new InkOverlayStrokesDeletingEventHandler(myInkOverlay_StrokeDeleting);

            // Hook the event handler for StrokeDeleted event to myInkOverlay_StrokeDeleted.
            // This is necessary to update the layout analysis result when automatic layout analysis
            // option is selected.
            inkPicture1.StrokesDeleted += new InkOverlayStrokesDeletedEventHandler(myInkOverlay_StrokeDeleted);

            // Hook up to the InkOverlay's event handlers: New packet arriving event
            inkPicture1.NewPackets += new InkCollectorNewPacketsEventHandler(inkPicture1_NewPackets);
            //inkPicture1.NewInAirPackets += new InkCollectorNewInAirPacketsEventHandler(inkPicture1_NewInAirPackets);

            

            // Create the ink divider object
            myInkDivider = new Divider();

            // TODO: 

            int temp_name = int.Parse(fManage.getID()) + 1;
            nameBox.Text = temp_name.ToString();

            // Initialize the form's dataset
            try
            {
                try
                {
                    inkRecognizers = new Recognizers();
                    myInkDivider.RecognizerContext = inkRecognizers.GetDefaultRecognizer().CreateRecognizerContext();
                }
                catch (InvalidOperationException)
                {
                    //We are in the case where no default recognizers can be found
                }
            }
            catch (FileNotFoundException error)
            {
                // If the xml or the scanned form image file are not available,
                // display an error and exit
                MessageBox.Show("A required data file was not found.  Please verify " +
                                "that the file exists in the same directory as PaperForm.exe " +
                                "and try again." + Environment.NewLine + Environment.NewLine +
                                error.ToString(),
                                "PaperForm",
                                MessageBoxButtons.OK);
                Application.Exit();
            }
            // Enable ink collection for the ink picture control
            inkPicture1.InkEnabled = true;

            // The LineHeight property helps the InkDivider distinguish between 
            // drawing and handwriting. The value should be the expected height 
            // of the user's handwriting in ink space units (0.01mm).
            // Here we set the LineHeight to 840, which is about 1/3 of an inch.
            //myInkDivider.LineHeight = 100;

            // Assign ink overlay's strokes collection to the ink divider
            // This strokes collection will be updated in the event handler
            //myInkDivider.Strokes = inkPicture1.Ink.Strokes;

            // Enable ink collection
            inkPicture1.Enabled = true;

            // Construct class variables to collect cusps coordinate and airtime
            //for (int i = 0; i < 200; i++)
            //{
            //    acFormat[i] = new AirCuspsFormat();
            //}

            // Starting to check the airtime
            if (air_flag == 0)
            {
                prev_min = next_min = DateTime.Now.Minute;
                prev_sec = next_sec = DateTime.Now.Second;
                prev_msec = next_msec = DateTime.Now.Millisecond;
                air_flag = 1;
            }
        }

        private void drawClockCircle()
        {
            int radius = 365;
            clock = new Circle(this.inkPicture1.Width / 2 - radius, this.inkPicture1.Height / 2 - radius, radius * 2);
            //clock = new Circle(0, 0, this.inkPicture1.Width - 10);
            //this.inkPicture1.Width = Screen.PrimaryScreen.WorkingArea.Width;
            //this.inkPicture1.Height = Screen.PrimaryScreen.WorkingArea.Height - 50; 
            int x = this.inkPicture1.Width;
            circleBitmap = new System.Drawing.Bitmap(this.inkPicture1.Width, this.inkPicture1.Height);
            Graphics g = Graphics.FromImage(circleBitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new System.Drawing.Pen(Color.Black, 3);
            float expRadius = clock.Radius;
            int width = this.inkPicture1.Width;
            int height = this.inkPicture1.Height;
            g.DrawEllipse(pen, this.inkPicture1.Width / 2 - clock.Radius, this.inkPicture1.Height / 2 - clock.Radius, 2 * clock.Radius, 2 * clock.Radius);
            //g.DrawEllipse(pen, clock.Center.X - 20, clock.Center.Y - 20, 20, 20);
            //g.DrawEllipse(pen, 0, 0, 2, 2);
            this.inkPicture1.Image = circleBitmap;
            this.FormBorderStyle = FormBorderStyle.None;

            Point scaleHelper = new Point(1, 1);
            inkPicture1.Renderer.PixelToInkSpace(g, ref scaleHelper);
            clock.ScaleX = scaleHelper.X;
            clock.ScaleY = scaleHelper.Y;

        }

        void inkPicture1_LocationChanged(object sender, EventArgs e)
        {
            drawClockCircle();
        }


        /// <summary>
        /// This function is called when the tablet generating a new drawing point.
        /// In this function, we are going to store pressure, point location, and timestamp for each point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inkPicture1_NewPackets(object sender, InkCollectorNewPacketsEventArgs e)
        {

            try
            {
                PacketPoint pkPoint = new PacketPoint();
                pkPoint.StrokeID = e.Stroke.Id;
                pkPoint.Size = e.Stroke.PacketSize;

                //Graphics g = inkPicture1.CreateGraphics();
                Graphics g = Graphics.FromImage(circleBitmap);
                //the indexes are wrong because inkSpace is different than inkPicture space
                Point temp_pk = new Point((int)e.PacketData.GetValue(indexX), (int)e.PacketData.GetValue(indexY));

                // Convert the X, Y position to Window based pixel coordinates
                inkPicture1.Renderer.InkSpaceToPixel(g, ref temp_pk);

                pkPoint.PkPt = temp_pk;

                #region pressure data
                // trying to get the pressure data from ink collector
                Guid[] PacketDesc = e.Stroke.PacketDescription;

                int NormalPressure = -1;
                for (int k = 0; k < PacketDesc.Length; k++)
                {
                    if (PacketDesc[k] == PacketProperty.NormalPressure)
                    {
                        // for simplicity, in case of multiple packets, use the first packet only
                        NormalPressure = e.PacketData[k];
                    }
                }

                // If we have the NormalPressure information
                // change DrawingAttributes according to the NormalPressure
                // Note that the change does not take effect until the next stroke
                if (NormalPressure != -1)
                {
                    // display the pressure on a status label
                    pkPoint.Pressure = NormalPressure;
                }
                #endregion

                /// trying to capture time stamp
                pkPoint.TimeStamp = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
                pkPoints.Add(pkPoint);
            }
            catch (Exception exception)
            {
            }
#if TRACE
            // Add Ink and Stroke objects use the HIMETRIC coordinate system
            // A HIMETRIC unit represents 0.01mm          
            String str = String.Format("    - Newpacket PacketCount={0} (X,Y)=({1},{2}) Time = {3}:{4}:{5}:{6}, Cnt = {7}, Pressure = {8}",
                e.PacketCount, e.PacketData.GetValue(0), e.PacketData.GetValue(1),
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, pkPoints.Count, NormalPressure);
            System.Console.WriteLine(str);
#endif
                                    
        }

        /// <summary>
        /// Event Handler from Ink Overlay's Stroke event
        /// This event is fired when a new stroke is drawn. 
        /// In this case, it is necessary to update the ink divider's 
        /// strokes collection. The event is fired even when the eraser stroke is created.
        /// The event handler must filter out the eraser strokes.
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void myInkOverlay_Stroke(object sender, InkCollectorStrokeEventArgs e)
        {
            // Filter out the eraser stroke.
            if (InkOverlayEditingMode.Ink == inkPicture1.EditingMode)
            {
                // Add the new stroke to the ink divider's strokes collection
                //myInkDivider.Strokes.Add(e.Stroke);


                Graphics g = Graphics.FromImage(circleBitmap);// CreateGraphics();
                //Graphics g = inkPicture1.CreateGraphics(); //try a new kind of graphics creation process
                //is this graphics object the same?
                //the coordinates around of the BoundingBox are wrong before they are convertd to inkSpace
                //the g's probably don't line up
                //########################################################################################################Problem area
                PenStroke penStk = new PenStroke(e.Stroke, g, inkPicture1);
                //this.inkPicture1.Size = new System.Drawing.Size(905, 706);//experiment to play with changing size of window
                penStk.TimeStamp = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
                int tempX = penStk.CenterPoint.X;
                int tempY = penStk.CenterPoint.Y; 
                //DateTime.Now.ToUniversalTime().Millisecond;//DateTime.Now.Ticks;
                penStk.PacketPoints = pkPoints;

                // to recognize if the stroke are hands or not
                if (penStk.BoundingArea > secondStroke[0])
                {
                    secondStroke[0] = penStk.BoundingArea;
                    secondStroke[1] = penStk.Id;

                    if (penStk.BoundingArea > largestStroke[0])
                    {
                        secondStroke[0] = largestStroke[0];
                        secondStroke[1] = largestStroke[1];

                        largestStroke[0] = penStk.BoundingArea;
                        largestStroke[1] = penStk.Id;                        
                    }
                }

                penStrokes.Add(penStk);
            }

            pkPoints = new List<PacketPoint>();
            // Repaint the screen to reflect the change
            //inkPicture1.Refresh();
        }

        /// <summary>
        /// Event Handler for Ink Overlay's StrokeDeleting event. 
        /// This event is fired when a set of stroke is about to be deleted.
        /// The stroke should also be removed from the ink divider's
        /// stroke collection as well
        /// </summary>
        /// <param name="sender">The control that raised the event</param>
        /// <param name="e">The event arguments</param>
        void myInkOverlay_StrokeDeleting(object sender, InkOverlayStrokesDeletingEventArgs e)
        {
            // Remove the strokes to be deleted from the ink divider's stroke collection
            //myInkDivider.Strokes.Remove(e.StrokesToDelete);
            //Divider inkdivider = new Divider(this.inkPicture1.Ink.Strokes);
            //inkdivider.Strokes.Remove(e.StrokesToDelete);
            //this.inkPicture1.Ink.Strokes.Remove(e.StrokesToDelete);
        }

        /// <summary>
        /// Event handler for Ink Overlay's StrokeDeleted event.
        /// This event is fired when the set of strokes were actually deleted.
        /// DivideInk method is called to analyze the current layout.
        /// </summary>
        /// <param name="sender">The control that raised the event</param>
        /// <param name="e">The event argument</param>
        void myInkOverlay_StrokeDeleted(object sender, System.EventArgs e)
        {
            // Repaint the screen to reflect the change
            inkPicture1.Refresh();
        }

        /// <summary>
        /// Event Handler from Exit Menu Item Click Event
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void miExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// UpdateEditMode is a helper method to switch the InkPicture's
        /// editing mode and update the form accordingly.
        /// </summary>
        /// <param name="mode">The new editing mode</param>
        private void UpdateEditMode(InkOverlayEditingMode mode)
        {
            if (!inkPicture1.CollectingInk)
            {
                // Ink collection must be disabled before modifying the edit mode
                inkPicture1.InkEnabled = false;
                inkPicture1.EditingMode = mode;
                inkPicture1.InkEnabled = true;
            }
            else
            {
                // If user is actively inking, we cannot switch modes.
                MessageBox.Show("Cannot switch the editing mode while collecting ink.");
            }
        }

        /// <summary>
        /// Iterate through each row of the form data and 
        /// display the recognition results
        /// </summary>
        private void Recognize()
        {
            int count = 0;

            bool still = true;

            if (penStrokes != null)
            {
                //if (penStrokes.Count == 1)
                //{
                //    // Todo: temporarily fix. I need to 
                //    dispRecognition(tempDivRects.ToArray());
                //    return;
                //}

                for (int x = 0; x < penStrokes.Count - 1; x++)
                {
                    for (int y = x + 1; y < penStrokes.Count; y++)
                    {
                        if(recoRect.checkOverlap(penStrokes[x].PixelBoundingBox, penStrokes[y].PixelBoundingBox))
                        {
                            Rectangle updatedRect = recoRect.updatedRect(penStrokes[x].PixelBoundingBox,
                                penStrokes[y].PixelBoundingBox);
                            penStrokes[x].MergeTo = y;
                            penStrokes[x].MergingRectangle = updatedRect;
                            penStrokes[y].MergeTo = x;
                            penStrokes[y].MergingRectangle = updatedRect;                                                    
                        }
                    }
                }

                //go through the list of recognized numbers and combineTo the double digits
                //check to see if number that is recognized as 1 is within a certain distance of a 0,1,2
                //also have to increase the size of the bounding box for each of the merged values to a bounding box that includesthe combined width and height of both boxes 

                

                

                #region old codes for merging the bounding box
                /*
                 * This is problematic. The way to iterate through all the rectangles is just not right.
                */
                //
                /*
                while (still)
                {
                    still = false;
                    for (int z = 0; z < penStrokes.Count - 1; z++)
                    {
                        if (recoRect.checkOverlap(penStrokes[z].PixelBoundingBox, penStrokes[z+1].PixelBoundingBox))
                        {
                            divRects[count] = recoRect.updatedRect(penStrokes[z].PixelBoundingBox,
                                penStrokes[z + 1].PixelBoundingBox);
                            is_merged[count] = true;

                            penStrokes[z].MergeTo = z + 1;
                            penStrokes[z].MergingRectangle = divRects[count];

                            penStrokes[z+1].MergeTo = z;
                            penStrokes[z+1].MergingRectangle = divRects[count];

                            count++;
                            z++;
                            still = true;
                        }
                        else if (z == tempDivRects.Length - 2)
                        {
                            divRects[count] = tempDivRects[z];
                            count++;
                            divRects[count] = tempDivRects[z + 1];
                            count++;
                        }
                        else
                        {
                            divRects[count] = tempDivRects[z];
                            count++;
                        }
                    }

                    if (still)
                    {
                        tempDivRects = new Rectangle[count];
                        tempDivRects = recoRect.copyRect(divRects, count);

                        temp_is_merged = new bool[count];
                        temp_is_merged = is_merged;

                        divRects = new Rectangle[tempDivRects.Length];
                        is_merged = new bool[tempDivRects.Length];

                        count = 0;
                    }
                }*/
                #endregion
            }
            dispRecognitionCPH();
            
            //dispRecognition(tempDivRects);
            
        }

        /// <summary>
        /// Event Handler from Pen Menu Item Click Event
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void penSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEditMode(InkOverlayEditingMode.Ink);
        }

        /// <summary>
        /// Event Handler from Eraser Menu Item Click Event
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void eraserSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEditMode(InkOverlayEditingMode.Delete);
        }

        /// <summary>
        /// Event Handler from Recognize Menu Item Click Event
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void recoBtn_Click(object sender, EventArgs e)
        {
            //DivisionResult divResult = myInkDivider.Divide();

            // Call helper function to get the bounding boxes for Drawings
            // The rectangles are inflated by 1 pixel in all directions
            //myDrawingBoundingBoxes = GetUnitBBoxes(divResult, InkDivisionType.Drawing, 3);
            this.inkPicture1.Invalidate();

            // Get the handwriting recognition results
            Recognize();
            
        }

        /// <summary>
        /// Event Handler from Clear Menu Item Click Event
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ClearAllBtn_Click(object sender, EventArgs e)
        {
            // Delete the ink strokes and redraw
            //myInkDivider.Strokes.Remove(myInkDivider.Strokes);
            inkPicture1.Ink.DeleteStrokes();
            //inkPicture1.Refresh();
            this.acFormat = new List<AirCuspsFormat>();
            this.penStrokes = new List<PenStroke>();
            this.buffer = new StringBuilder();
            tempDivRects = null;
            myDrawingBoundingBoxes = null;

            air_count = 0;
            inkPicture1.Invalidate();
          
            //inkPicture1.Stroke -= new InkCollectorStrokeEventHandler(myInkOverlay_Stroke);            
            //inkPicture1.StrokesDeleting -= new InkOverlayStrokesDeletingEventHandler(myInkOverlay_StrokeDeleting);            
            //inkPicture1.StrokesDeleted -= new InkOverlayStrokesDeletedEventHandler(myInkOverlay_StrokeDeleted);            
            //inkPicture1.NewPackets -= new InkCollectorNewPacketsEventHandler(inkPicture1_NewPackets);

            //Todo: this is not a good way to reload the paperform. I should find out why we need this
            //and use a better way to do it.
            //PaperForm_Load(sender, e);

            this.clock.Times++;
        }

        /// <summary>
        /// Paint method gets called everytime when the window is refreshed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

#if DEBUG
            // Then, draw the bounding boxes for Words
            if (null != tempDivRects)
            {
                // Color is Red
                penBox.Color = Color.Red;
                // Draw bounding boxes for misrecognized words
                foreach(PenStroke tmpPenStroke in penStrokes)
                {
                    e.Graphics.DrawRectangle(penBox, tmpPenStroke.PixelBoundingBox);
                }
                //e.Graphics.DrawRectangles(penBox, tempDivRects);
            }


            // Finally, draw the boxes for Drawings
            if (null != myDrawingBoundingBoxes)
            {

                // Color is Green pen
                penBox.Color = Color.Green;
                // Draw bounding boxes for recognized words
                //e.Graphics.DrawRectangles(penBox, myDrawingBoundingBoxes);
              
            }

            // Represent every endpoint of each stroke as a red circle.
            foreach (AirCuspsFormat acf in acFormat)
            {
                Point[] p_cusps = acf.getCusps();
                if (acf.getCusps() != null)
                {
                    foreach (Point cusp in p_cusps)
                    {
                        if (cusp != null)
                        {
                            if ((inkPicture1.EditingMode == InkOverlayEditingMode.Ink) && (cusp.X != 0))
                            {
                                penBox.Color = Color.Red;
                                e.Graphics.DrawEllipse(penBox, cusp.X - 3, cusp.Y - 3, 6, 6);
                            }
                        }
                    }
                }
            }
#endif
            first_packet = true;            
        }                

        /// <summary>
        /// Helper function to obtain array of rectangles from the 
        /// division result of the division type of interest. Each rectangle
        /// is inflated by the amount specified in the third parameter. This
        /// is done to ensure the visibility of all rectangles.
        /// </summary>
        /// <param name="divResult">Ink Divider division result</param>
        /// <param name="divType">Division type</param>
        /// <param name="inflate">Number of Pixels by which the rectangles are inflated</param>
        /// <returns> Array of rectangles containing bounding boxes of 
        /// division type specified by divType. The rectangles are in pixel unit.</returns>
        /// 

        /*
        private List<Rectangle> GetUnitBBoxes(DivisionResult divResult, InkDivisionType divType, int inflate)
        {
            // We need to convert rectangles from ink units to
            // pixel units. For that, we need Graphics object
            // to pass to InkRenderer.InkSpaceToPixel method
            //Graphics g = inkPicture1.CreateGraphics();
            Graphics g = Graphics.FromImage(this.inkPicture1.Image); 
         
            // Get the strokes to paint from the ink
            strokes = inkPicture1.Ink.Strokes;

            /// Get the Intersection point between two strokes here
#if DEBUG
            int j = 0;
            // draw the intersections of each stroke as little red circles
            foreach (Stroke currentStroke in strokes)
            {                                                                                                                      
                // Get the intersections of the stroke
                float[] intersections = currentStroke.FindIntersections(strokes);

                // Pointing intersecation process starts from here
                // Draw each intersection in the stroke
                foreach (float fi in intersections)
                {
                    // Get the point before the FINDEX
                    Point ptIntersect = currentStroke.GetPoint((int)fi);

                    // Find the fractional part of the FINDEX
                    float fiFraction = fi - (int)fi;

                    // if the fi does not have a fractional part, we have already
                    // found the intersection point.  Otherwise, use the FINDEX to 
                    // calculate the interpolated intersection point on the stroke
                    if (fiFraction > 0.0f)
                    {
                        Point ptNextIntersect = currentStroke.GetPoint((int)fi + 1);
                        ptIntersect.X += (int)((ptNextIntersect.X - ptIntersect.X) * fiFraction);
                        ptIntersect.Y += (int)((ptNextIntersect.Y - ptIntersect.Y) * fiFraction);
                    }

                    // Convert the X, Y position to Window based pixel coordinates
                    inkPicture1.Renderer.InkSpaceToPixel(g, ref ptIntersect);

                    penBox.Color = Color.Blue;

                    // Draw a red circle as the intersection position
                    g.DrawEllipse(penBox, ptIntersect.X - 3, ptIntersect.Y - 3, 6, 6);
                }
                j++;                        
            }
#endif
            // Get the division units from the division result of division type
            DivisionUnits units = divResult.ResultByType(divType);

            // chipin: once we have List, we can dynamicly add object in the array
            // so we don't need to construct it anymore.
            // Construct the rectangles
            //divRects = new Rectangle[units.Count];
            divRects = new List<Rectangle>();
            tempDivRects = new List<Rectangle>(); // new Rectangle[units.Count];
            is_merged = new bool[units.Count];
            temp_is_merged = new bool[units.Count];

            // If there is at least one unit, we construct the rectangles
            if ((null != units) && (0 < units.Count))
            {
                // InkRenderer.InkSpaceToPixel takes Point as parameter. 
                // Create two Point objects to point to (Top, Left) and
                // (Width, Height) properties of ractangle. (Width, Height)
                // is used instead of (Right, Bottom) because (Right, Bottom)
                // are read-only properties on Rectangle
                Point ptLocation = new Point();
                Point ptSize = new Point();

                buffer = new StringBuilder();

                int i = 0;

                // Iterate through the collection of division units to obtain the bounding boxes
                foreach (DivisionUnit unit in units)
                {
                    
                    // Get the bounding box of the strokes of the division unit
                    tempDivRects.Add(unit.Strokes.GetBoundingBox());

                    // The bounding box is in ink space unit. Convert them into pixel unit. 
                    ptLocation = tempDivRects[i].Location;
                    ptSize.X = tempDivRects[i].Width;
                    ptSize.Y = tempDivRects[i].Height;

                    // Convert the Location from Ink Space to Pixel Space
                    inkPicture1.Renderer.InkSpaceToPixel(g, ref ptLocation);

                    // Convert the Size from Ink Space to Pixel Space
                    inkPicture1.Renderer.InkSpaceToPixel(g, ref ptSize);

                    // Assign the result back to the corresponding properties
                    tempDivRects[i] = new Rectangle(ptLocation.X, ptLocation.Y, ptSize.X, ptSize.Y);
                    //test the location after conversion
                    int x = tempDivRects[i].X;
                    int y = tempDivRects[i].Y;
                    int z = tempDivRects[i].Location.X;
                    // Inflate the rectangle by inflate pixels in both directions
                    tempDivRects[i].Inflate(inflate, inflate);
                    //test the location after conversion
                    x = tempDivRects[i].X;
                    y = tempDivRects[i].Y; 
                    // Increment the index
                    ++i;
                }
            }
            else
            {
                // Otherwise we return null
                tempDivRects = null;
            }

            // Return the Rectangle[] object
            return tempDivRects;
        }
         */

        // Calculate Pendown time
        private float calcPendown(AirCuspsFormat acf)
        {
            float t_pendown = 0;

            int hour = acf.getLastHour();
            int min = acf.getLastMin();
            int sec = acf.getLastSec();
            int msec = acf.getLastMsec();

            msec = msec - acf.getFirstMsec();
            sec = sec - acf.getFirstSec();
            min = min - acf.getFirstMin();
            hour = hour - acf.getFirstHour();

            if (msec < 0)
            {
                sec--;
                msec += 1000;
            }
            
            if (sec < 0)
            {
                min--;
                sec += 60;
            }
            
            if (min < 0)
            {
                hour--;
                min += 60;
            }
            
            if (hour < 0)
            {
                hour += 24;
            }

            t_pendown = ((float)msec / (float)1000) + sec + (min * 60) + (hour * 3600);

            return t_pendown;
        }

        private void dispRecognitionCPH()
        {
            //ProgressBar pbar = new ProgressBar();
            bool isSaved = false;
            if (penStrokes == null || penStrokes.Count == 0)
                return;
            buffer = new StringBuilder();

            // We need to convert rectangles from ink units to
            // pixel units. For that, we need Graphics object
            // to pass to InkRenderer.InkSpaceToPixel method
            ////Graphics g = inkPicture1.CreateGraphics()

            using (Graphics g = Graphics.FromImage(circleBitmap))
            {
                int tempWidth = this.inkPicture1.Width;
                int tempHeight = this.inkPicture1.Height;
                int circleBitmapWidth = this.circleBitmap.Width;
                int circleBitmapHeight = this.circleBitmap.Height;
                for (int i = 0; i < penStrokes.Count; i++)
                {
                    // the default bounding box for a penStroke
                    Point pt1 = new Point((int)penStrokes[i].PixelBoundingBox.Left, (int)penStrokes[i].PixelBoundingBox.Top);
                    Point pt2 = new Point((int)penStrokes[i].PixelBoundingBox.Right, (int)penStrokes[i].PixelBoundingBox.Bottom);
                    // if it's merging to another penstroke, then the point would use the next one
                    if (penStrokes[i].MergeTo < penStrokes.Count)
                    {
                        pt1 = new Point((int)penStrokes[i].MergingRectangle.Left, (int)penStrokes[i].MergingRectangle.Top);
                        pt2 = new Point((int)penStrokes[i].MergingRectangle.Right, (int)penStrokes[i].MergingRectangle.Bottom);
                    }

                    // Convert to ink space units
                    inkPicture1.Renderer.PixelToInkSpace(g, ref pt1);
                    inkPicture1.Renderer.PixelToInkSpace(g, ref pt2);

                    

                    // the rectangle for the region
                    Rectangle rc = new Rectangle(pt1.X, pt1.Y, pt2.X - pt1.X, pt2.Y - pt1.Y);

                    try
                    {
                        // find the strokes that intersect and lie inside of the rectangle
                        strokes = inkPicture1.Ink.HitTest(rc, 100);
                    }
                    catch (Exception e)
                    {
                    }

                    if (penStrokes[i].BoundingArea > this.HANDSIZE)
                    {
                        if (this.largestStroke[1] == penStrokes[i].Id)
                        {
                            penStrokes[i].isHand = 1; // long hand       
                        }
                        else if (this.secondStroke[1] == penStrokes[i].Id)
                        {
                            penStrokes[i].isHand = 2; // short hand
                        }
                    }

                    // recognize the handwriting
                    if (strokes.Count > 0)
                    {
                        //exclude the repeated merging rectangles
                        if (penStrokes[i].MergeTo > i)
                        {
                            String rawStroke = strokes.ToString();
                            //dbFormat[i].setRawStroke(rawStroke);

                            String strokeString = refineNumbers.runRefineNumbers(strokes.ToString());

                            if (strokeString.Equals("C") || strokeString.Equals("K") || strokeString.Equals("P") ||
                                strokeString.Equals("S") || strokeString.Equals("U") || strokeString.Equals("V") ||
                                strokeString.Equals("W") || strokeString.Equals("X") || strokeString.Equals("Z"))
                            {
                                strokeString.ToLower();
                            }

                            buffer.Append("Stroke No." + (i + 1).ToString() + " = " + strokeString + Environment.NewLine);

                            if (dbFormatScreening(strokeString))
                            {
                                int strokeNumber = int.Parse(strokeString);                                

                                penStrokes[i].RecoStrokes = strokeNumber.ToString();
                            }
                        }
                        else
                        {
                            penStrokes[i].RecoStrokes = penStrokes[penStrokes[i].MergeTo].RecoStrokes;
                        }

                        
                    }
                }

                for (int x = 0; x <= penStrokes.Count - 1; x++)
                {
                    if (penStrokes[x].RecoStrokes == "1")
                    {
                        for (int y = 0; y <= penStrokes.Count - 1; y++)
                        {
                            PointF pt1 = penStrokes[x].CenterPoint;
                            PointF pt2 = penStrokes[y].CenterPoint;

                            double distance = Math.Sqrt(Math.Pow((pt1.X - pt2.X), 2.0) + Math.Pow((pt1.Y - pt2.Y), 2.0));
                            if (distance < 100 && distance != 0)
                            {
                                if (penStrokes[x].RecoStrokes != "H1" || penStrokes[x].RecoStrokes != "H2" ||
                                    penStrokes[x].RecoStrokes != "?" && Math.Abs(penStrokes[x].CombineTo) < 50
                                    && Math.Abs(penStrokes[y].CombineTo) < 50)
                                {
                                    penStrokes[x].CombineTo = int.Parse(penStrokes[y].RecoStrokes);
                                    penStrokes[y].CombineTo = int.Parse(penStrokes[x].RecoStrokes);
                                    penStrokes[y].OrderID = 1;
                                    penStrokes[x].OrderID = 0;


                                    Rectangle combinedRec = new Rectangle();
                                    //calculate the new width for the combinedRec by subtracting the x coord from the far side of each of their pixelBoundingBoxes
                                    int width1 = Math.Abs(penStrokes[y].PixelBoundingBox.Left - penStrokes[x].PixelBoundingBox.Right);
                                    int width2 = Math.Abs(penStrokes[y].PixelBoundingBox.Right - penStrokes[x].PixelBoundingBox.Left);
                                    int finalWidth = Math.Max(width1, width2);
                                    int finalHeight = Math.Max(penStrokes[x].PixelBoundingBox.Height, penStrokes[y].PixelBoundingBox.Height);
                                    int xVal = Math.Min(penStrokes[y].PixelBoundingBox.Left, penStrokes[x].PixelBoundingBox.Left);
                                    int yVal = Math.Min(penStrokes[y].PixelBoundingBox.Top, penStrokes[x].PixelBoundingBox.Top);
                                    Point newLoc = new Point(xVal, yVal);
                                    combinedRec.Width = finalWidth;
                                    combinedRec.Height = finalHeight;
                                    combinedRec.Location = newLoc;
                                    penStrokes[x].CombinedRectangle = combinedRec;
                                    penStrokes[y].CombinedRectangle = combinedRec;
                                }

                            }

                        }
                    }
                }
                //scoringNumbers = new ScoringNumbers(penStrokes, this.clock, circleBitmap);
                // ToDo: need to implement it again!!
                //Scoring is no longer done in this phase of the program, now implement in ClockReader


                Graphics tempG = Graphics.FromImage(circleBitmap); 
                Pen pen = new System.Drawing.Pen(Color.Black, 3);
                int width = this.inkPicture1.Width;
                int height = this.inkPicture1.Height;
                for (int i = 0; i < penStrokes.Count; i++)
                {
                    tempG.DrawEllipse(pen, penStrokes[i].CenterPoint.X - 20 , penStrokes[i].CenterPoint.Y - 20, 20, 20);
                }
                //tempG.DrawEllipse(pen, clock.Center.X, clock.Center.Y, 20, 20); 
                this.inkPicture1.Image = circleBitmap;
                this.inkPicture1.Invalidate();

                //bool[] scoreboard = scoringNumbers.scoringCPH();//new bool[13];// 

                // Check to ensure that the user has at least one recognizer installed
                if (0 == inkRecognizers.Count)
                {
                    MessageBox.Show(this, "There are no handwriting recognizers installed.  You need to have at least one in order to perform the recognition.");
                }
                else
                {
#if DEBUG
                    // Display the results
                    if (buffer.Length > 0)
                    {
                        MessageBox.Show(this, buffer.ToString());
                    }
                    else
                    {
                        MessageBox.Show(this, "There aren't any recognition results.");
                    }
#endif
                }

                //foreach (DBFormat df in dbFormat)
                //{
                //    df.setSection();
                //}

                // The variable for storing patients name
                String sub_name = nameBox.Text;

                // Save data into .isf file
                
                isSaved = fManage.saveFile(inkPicture1, sub_name, penStrokes, /*scoreboard,*/ clock);
                //this.UseWaitCursor = false;
            }

            if (isSaved)
            {
                MessageBox.Show("Successfully Save the file and Exit!!");
                Application.Exit();
            }
            else 
            {
                MessageBox.Show("No file has been saved");

            }
        }

        private bool dbFormatScreening(String strokes)
        {
            if (strokes.Equals("0") || strokes.Equals("1") || strokes.Equals("2") || strokes.Equals("3") ||
                strokes.Equals("4") || strokes.Equals("5") || strokes.Equals("6") || strokes.Equals("7") ||
                strokes.Equals("8") || strokes.Equals("9"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void inkPicture1_Stroke(object sender, InkCollectorStrokeEventArgs e)
        {

        }

        private void tmrRcCounter_Tick(object sender, EventArgs e)
        {

        }
    }
}
