using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
// The Ink namespace, which contains the Tablet PC Platform API
using Microsoft.Ink;
using System.Windows.Ink;
using System.Windows.Input;

namespace ClockReader
{
	public class SerializationForm : System.Windows.Forms.Form
	{
        private System.ComponentModel.IContainer components;

        #region Standard Template Code


        private System.Windows.Forms.MainMenu MenuBar;
        private System.Windows.Forms.MenuItem FileMenu;
        private System.Windows.Forms.MenuItem OpenMenu;

        private System.Windows.Forms.Panel Signature;
        #endregion

        private Label label1;
        private MenuItem saveMenu;
        private GroupBox grpSequence;
        private ComboBox comboBox1;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label3;
        private Label label2;
        private PictureBox check1;
        private PictureBox check8;
        private PictureBox check10;
        private PictureBox check9;
        private PictureBox check7;
        private PictureBox check6;
        private PictureBox check5;
        private PictureBox check4;
        private PictureBox check3;
        private PictureBox check2;
        private Label label14;

        // The one and only ink collector
		private InkCollector ic;

        private int[] scoreboard = new int[13];
        private float[] airtime;

        private FileStream file;
        private StreamReader sr;
        private SplitContainer splitContainer1;
        private InkPicture ClockDrawing;
        private InkPicture pictureBox1;
        private double scale;
        private int DurationInSecond;
        private Point translationPt;
        private Panel panel1;
        private TrackBar trackBar1;
        private Button play_pause;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;        
        private MenuItem menuItem1;

        private string displayedFileName;
        private TabPage tabPage3;
        private System.Collections.Generic.Dictionary<string, Clock> dicClocks;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAirTime;
        private Chart chartPressure;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox check13;
        private PictureBox check12;
        private PictureBox check11;
        private Label lbSequences;
        private Label lbScore;

        // for sqlite 
        private SQLiteConnection sqlConnection;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem saveClockToImageToolStripMenuItem;
        private Label lbTrial;
        private Label label16;
        private Label lbDuration;
        private Label label15;
        private Label lbID;
        private MenuItem menuCapture;
        private Label Sec;

        // for animation
        private bool isPlaying = false;

        //load clock for scoring
        private Clock oClock = new Clock(); 


		// Constructor
		public SerializationForm()
		{
            #region Standard Template Code
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            

            #endregion
            this.mInkOverly = new InkOverlay(this.ClockDrawing);

            // Create the InkCollector and attach it to the signature GroupBox
            ic = new InkCollector(this.ClockDrawing.Handle);
            //ic.Enabled = true;
            renderInk = new Ink();
            dicClocks = new System.Collections.Generic.Dictionary<string, Clock>();

            timer.Interval = 1;
            timer.Tick += new EventHandler(Application_Idle);
		}

        #region Standard Template Code
		/// Clean up any resources being used.
		/// Windows Forms Designer template code
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
        #endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerializationForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.MenuBar = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenu = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.OpenMenu = new System.Windows.Forms.MenuItem();
            this.saveMenu = new System.Windows.Forms.MenuItem();
            this.menuCapture = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSequence = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.check8 = new System.Windows.Forms.PictureBox();
            this.check10 = new System.Windows.Forms.PictureBox();
            this.check9 = new System.Windows.Forms.PictureBox();
            this.check7 = new System.Windows.Forms.PictureBox();
            this.check6 = new System.Windows.Forms.PictureBox();
            this.check5 = new System.Windows.Forms.PictureBox();
            this.check4 = new System.Windows.Forms.PictureBox();
            this.check3 = new System.Windows.Forms.PictureBox();
            this.check2 = new System.Windows.Forms.PictureBox();
            this.check1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Sec = new System.Windows.Forms.Label();
            this.lbTrial = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbDuration = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbID = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.play_pause = new System.Windows.Forms.Button();
            this.ClockDrawing = new Microsoft.Ink.InkPicture();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveClockToImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbSequences = new System.Windows.Forms.Label();
            this.lbScore = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.check13 = new System.Windows.Forms.PictureBox();
            this.check12 = new System.Windows.Forms.PictureBox();
            this.check11 = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chartPressure = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartAirTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new Microsoft.Ink.InkPicture();
            ((System.ComponentModel.ISupportInitialize)(this.check8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClockDrawing)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.check13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check11)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAirTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            this.MenuBar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenu});
            // 
            // FileMenu
            // 
            this.FileMenu.Index = 0;
            this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.OpenMenu,
            this.saveMenu,
            this.menuCapture});
            this.FileMenu.Text = "&File";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Open A Folder";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // OpenMenu
            // 
            this.OpenMenu.Index = 1;
            this.OpenMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.OpenMenu.Text = "&Open A File";
            this.OpenMenu.Click += new System.EventHandler(this.OpenMenu_Click);
            // 
            // saveMenu
            // 
            this.saveMenu.Index = 2;
            this.saveMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.saveMenu.Text = "&Save";
            this.saveMenu.Click += new System.EventHandler(this.saveMenu_Click);
            // 
            // menuCapture
            // 
            this.menuCapture.Index = 3;
            this.menuCapture.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.menuCapture.Text = "Capture An &Image";
            this.menuCapture.Click += new System.EventHandler(this.menuCapture_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Participant ID:";
            // 
            // grpSequence
            // 
            this.grpSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSequence.AutoSize = true;
            this.grpSequence.Location = new System.Drawing.Point(6, 57);
            this.grpSequence.Name = "grpSequence";
            this.grpSequence.Size = new System.Drawing.Size(23, 56);
            this.grpSequence.TabIndex = 4;
            this.grpSequence.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Scoring Set 1",
            "Scoring Set 2",
            "Scoring Set 3"});
            this.comboBox1.Location = new System.Drawing.Point(6, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(279, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "3. Numbers are in correct order.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(222, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "4. Numbers are drawn w/o rotating the paper.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "5. Numbers are in the correct postion.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "6. Numbers are all inside circle.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "7. Two hands are present.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(189, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "8. The hour target number is indicated.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 248);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(199, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "9. The minute target number is indicated.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 279);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(197, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "10. The hands  are in correct proportion.";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 310);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(191, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "11. There are no superfluous markings.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 372);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "13. A center is present.";
            // 
            // check8
            // 
            this.check8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check8.Enabled = false;
            this.check8.Image = ((System.Drawing.Image)(resources.GetObject("check8.Image")));
            this.check8.Location = new System.Drawing.Point(459, 220);
            this.check8.Name = "check8";
            this.check8.Size = new System.Drawing.Size(23, 22);
            this.check8.TabIndex = 32;
            this.check8.TabStop = false;
            this.check8.Visible = false;
            // 
            // check10
            // 
            this.check10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check10.Image = ((System.Drawing.Image)(resources.GetObject("check10.Image")));
            this.check10.Location = new System.Drawing.Point(459, 282);
            this.check10.Name = "check10";
            this.check10.Size = new System.Drawing.Size(23, 23);
            this.check10.TabIndex = 29;
            this.check10.TabStop = false;
            this.check10.Visible = false;
            // 
            // check9
            // 
            this.check9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check9.Image = ((System.Drawing.Image)(resources.GetObject("check9.Image")));
            this.check9.Location = new System.Drawing.Point(459, 251);
            this.check9.Name = "check9";
            this.check9.Size = new System.Drawing.Size(23, 23);
            this.check9.TabIndex = 28;
            this.check9.TabStop = false;
            this.check9.Visible = false;
            // 
            // check7
            // 
            this.check7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check7.Enabled = false;
            this.check7.Image = ((System.Drawing.Image)(resources.GetObject("check7.Image")));
            this.check7.Location = new System.Drawing.Point(459, 189);
            this.check7.Name = "check7";
            this.check7.Size = new System.Drawing.Size(23, 23);
            this.check7.TabIndex = 27;
            this.check7.TabStop = false;
            this.check7.Visible = false;
            // 
            // check6
            // 
            this.check6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check6.Image = ((System.Drawing.Image)(resources.GetObject("check6.Image")));
            this.check6.Location = new System.Drawing.Point(459, 158);
            this.check6.Name = "check6";
            this.check6.Size = new System.Drawing.Size(23, 23);
            this.check6.TabIndex = 26;
            this.check6.TabStop = false;
            this.check6.Visible = false;
            // 
            // check5
            // 
            this.check5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check5.Image = ((System.Drawing.Image)(resources.GetObject("check5.Image")));
            this.check5.Location = new System.Drawing.Point(459, 127);
            this.check5.Name = "check5";
            this.check5.Size = new System.Drawing.Size(23, 23);
            this.check5.TabIndex = 25;
            this.check5.TabStop = false;
            this.check5.Visible = false;
            // 
            // check4
            // 
            this.check4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check4.Image = ((System.Drawing.Image)(resources.GetObject("check4.Image")));
            this.check4.Location = new System.Drawing.Point(459, 96);
            this.check4.Name = "check4";
            this.check4.Size = new System.Drawing.Size(23, 23);
            this.check4.TabIndex = 24;
            this.check4.TabStop = false;
            this.check4.Visible = false;
            // 
            // check3
            // 
            this.check3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check3.Image = ((System.Drawing.Image)(resources.GetObject("check3.Image")));
            this.check3.Location = new System.Drawing.Point(459, 65);
            this.check3.Name = "check3";
            this.check3.Size = new System.Drawing.Size(23, 23);
            this.check3.TabIndex = 23;
            this.check3.TabStop = false;
            this.check3.Visible = false;
            // 
            // check2
            // 
            this.check2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check2.Image = ((System.Drawing.Image)(resources.GetObject("check2.Image")));
            this.check2.Location = new System.Drawing.Point(459, 34);
            this.check2.Name = "check2";
            this.check2.Size = new System.Drawing.Size(23, 23);
            this.check2.TabIndex = 22;
            this.check2.TabStop = false;
            this.check2.Visible = false;
            // 
            // check1
            // 
            this.check1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check1.Image = ((System.Drawing.Image)(resources.GetObject("check1.Image")));
            this.check1.Location = new System.Drawing.Point(459, 3);
            this.check1.Name = "check1";
            this.check1.Size = new System.Drawing.Size(23, 23);
            this.check1.TabIndex = 21;
            this.check1.TabStop = false;
            this.check1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "1. Only numbers 1-12 are present.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 341);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(172, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "12. The hands are relatively joined.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "2. Only Arabic numbers are used.";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(3, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Sec);
            this.splitContainer1.Panel1.Controls.Add(this.lbTrial);
            this.splitContainer1.Panel1.Controls.Add(this.label16);
            this.splitContainer1.Panel1.Controls.Add(this.lbDuration);
            this.splitContainer1.Panel1.Controls.Add(this.label15);
            this.splitContainer1.Panel1.Controls.Add(this.lbID);
            this.splitContainer1.Panel1.Controls.Add(this.trackBar1);
            this.splitContainer1.Panel1.Controls.Add(this.play_pause);
            this.splitContainer1.Panel1.Controls.Add(this.ClockDrawing);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(931, 589);
            this.splitContainer1.SplitterDistance = 394;
            this.splitContainer1.TabIndex = 0;
            // 
            // Sec
            // 
            this.Sec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Sec.AutoSize = true;
            this.Sec.Location = new System.Drawing.Point(359, 561);
            this.Sec.Name = "Sec";
            this.Sec.Size = new System.Drawing.Size(13, 13);
            this.Sec.TabIndex = 12;
            this.Sec.Text = "0";
            // 
            // lbTrial
            // 
            this.lbTrial.AutoSize = true;
            this.lbTrial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTrial.Location = new System.Drawing.Point(127, 53);
            this.lbTrial.Name = "lbTrial";
            this.lbTrial.Size = new System.Drawing.Size(0, 17);
            this.lbTrial.TabIndex = 11;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(4, 53);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(117, 17);
            this.label16.TabIndex = 10;
            this.label16.Text = "Numbers of Trial:";
            // 
            // lbDuration
            // 
            this.lbDuration.AutoSize = true;
            this.lbDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDuration.Location = new System.Drawing.Point(106, 33);
            this.lbDuration.Name = "lbDuration";
            this.lbDuration.Size = new System.Drawing.Size(0, 17);
            this.lbDuration.TabIndex = 9;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(3, 33);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 17);
            this.label15.TabIndex = 8;
            this.label15.Text = "Test Duration:";
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbID.Location = new System.Drawing.Point(106, 13);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(0, 17);
            this.lbID.TabIndex = 7;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Enabled = false;
            this.trackBar1.Location = new System.Drawing.Point(39, 557);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(313, 45);
            this.trackBar1.TabIndex = 6;
            // 
            // play_pause
            // 
            this.play_pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.play_pause.BackColor = System.Drawing.Color.White;
            this.play_pause.Image = ((System.Drawing.Image)(resources.GetObject("play_pause.Image")));
            this.play_pause.Location = new System.Drawing.Point(6, 557);
            this.play_pause.Name = "play_pause";
            this.play_pause.Size = new System.Drawing.Size(34, 27);
            this.play_pause.TabIndex = 5;
            this.play_pause.UseVisualStyleBackColor = false;
            this.play_pause.Click += new System.EventHandler(this.play_pause_Click);
            // 
            // ClockDrawing
            // 
            this.ClockDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ClockDrawing.BackColor = System.Drawing.Color.White;
            this.ClockDrawing.ContextMenuStrip = this.contextMenuStrip1;
            this.ClockDrawing.Location = new System.Drawing.Point(6, 100);
            this.ClockDrawing.MarginX = -2147483648;
            this.ClockDrawing.MarginY = -2147483648;
            this.ClockDrawing.MaximumSize = new System.Drawing.Size(2000, 2000);
            this.ClockDrawing.MinimumSize = new System.Drawing.Size(362, 306);
            this.ClockDrawing.Name = "ClockDrawing";
            this.ClockDrawing.Padding = new System.Windows.Forms.Padding(3);
            this.ClockDrawing.Size = new System.Drawing.Size(379, 454);
            this.ClockDrawing.TabIndex = 4;
            this.ClockDrawing.TabStop = false;
            this.ClockDrawing.SizeChanged += new System.EventHandler(this.ClockDrawing_SizeChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveClockToImageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 26);
            // 
            // saveClockToImageToolStripMenuItem
            // 
            this.saveClockToImageToolStripMenuItem.Name = "saveClockToImageToolStripMenuItem";
            this.saveClockToImageToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveClockToImageToolStripMenuItem.Text = "Save Clock to Image";
            this.saveClockToImageToolStripMenuItem.Click += new System.EventHandler(this.saveClockToImageToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(529, 581);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.lbSequences);
            this.tabPage1.Controls.Add(this.lbScore);
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.grpSequence);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(521, 555);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scoring";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbSequences
            // 
            this.lbSequences.AutoSize = true;
            this.lbSequences.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbSequences.Location = new System.Drawing.Point(3, 37);
            this.lbSequences.Name = "lbSequences";
            this.lbSequences.Size = new System.Drawing.Size(79, 17);
            this.lbSequences.TabIndex = 21;
            this.lbSequences.Text = "Sequences";
            // 
            // lbScore
            // 
            this.lbScore.AutoSize = true;
            this.lbScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbScore.Location = new System.Drawing.Point(3, 140);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new System.Drawing.Size(52, 17);
            this.lbScore.TabIndex = 20;
            this.lbScore.Text = "Scores";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 257F));
            this.tableLayoutPanel1.Controls.Add(this.check13, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.check12, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.check11, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.check8, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.check9, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.check7, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.check6, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.check5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.check4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.check3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.check2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.check1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.check10, 1, 9);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 160);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.689467F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692544F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(485, 411);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // check13
            // 
            this.check13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check13.Image = ((System.Drawing.Image)(resources.GetObject("check13.Image")));
            this.check13.Location = new System.Drawing.Point(459, 375);
            this.check13.Name = "check13";
            this.check13.Size = new System.Drawing.Size(23, 22);
            this.check13.TabIndex = 36;
            this.check13.TabStop = false;
            this.check13.Visible = false;
            // 
            // check12
            // 
            this.check12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check12.Image = ((System.Drawing.Image)(resources.GetObject("check12.Image")));
            this.check12.Location = new System.Drawing.Point(459, 344);
            this.check12.Name = "check12";
            this.check12.Size = new System.Drawing.Size(23, 22);
            this.check12.TabIndex = 35;
            this.check12.TabStop = false;
            this.check12.Visible = false;
            // 
            // check11
            // 
            this.check11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check11.Image = ((System.Drawing.Image)(resources.GetObject("check11.Image")));
            this.check11.Location = new System.Drawing.Point(459, 313);
            this.check11.Name = "check11";
            this.check11.Size = new System.Drawing.Size(23, 22);
            this.check11.TabIndex = 34;
            this.check11.TabStop = false;
            this.check11.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.chartPressure);
            this.tabPage2.Controls.Add(this.chartAirTime);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(521, 555);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chartPressure
            // 
            this.chartPressure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.Interval = 30D;
            chartArea1.AxisX.Title = "Recognized Characters";
            chartArea1.AxisY.Title = "Pressure";
            chartArea1.Name = "ChartArea1";
            this.chartPressure.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPressure.Legends.Add(legend1);
            this.chartPressure.Location = new System.Drawing.Point(6, 349);
            this.chartPressure.Name = "chartPressure";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.CustomProperties = "LabelStyle=Left";
            series1.Legend = "Legend1";
            series1.Name = "Pressure";
            this.chartPressure.Series.Add(series1);
            this.chartPressure.Size = new System.Drawing.Size(0, 300);
            this.chartPressure.TabIndex = 1;
            this.chartPressure.Text = "chart1";
            // 
            // chartAirTime
            // 
            this.chartAirTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.Title = "Recognized Characters";
            chartArea2.AxisY.Title = "Milliseconds";
            chartArea2.Name = "ChartArea1";
            this.chartAirTime.ChartAreas.Add(chartArea2);
            legend2.ItemColumnSpacing = 10;
            legend2.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;
            legend2.Name = "Legend1";
            this.chartAirTime.Legends.Add(legend2);
            this.chartAirTime.Location = new System.Drawing.Point(6, 6);
            this.chartAirTime.Name = "chartAirTime";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.EmptyPointStyle.CustomProperties = "LabelStyle=Bottom";
            series2.EmptyPointStyle.MarkerSize = 3;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.Name = "Air Time ";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.chartAirTime.Series.Add(series2);
            this.chartAirTime.Size = new System.Drawing.Size(0, 337);
            this.chartAirTime.TabIndex = 0;
            this.chartAirTime.Text = "chart1";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(521, 555);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Monitoring";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(3, 624);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(931, 121);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(8, 10);
            this.pictureBox1.MarginX = -2147483648;
            this.pictureBox1.MarginY = -2147483648;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(113, 106);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // SerializationForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(939, 753);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Menu = this.MenuBar;
            this.Name = "SerializationForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Clock Reader Beta 1";
            ((System.ComponentModel.ISupportInitialize)(this.check8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClockDrawing)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.check13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check11)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAirTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

		#endregion

        #region Standard Template Code
		/// The main entry point for the application.
		[STAThread]
		static void Main() 
		{
			Application.Run(new SerializationForm());
		}
        #endregion

        #region event handler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClockDrawing_SizeChanged(object sender, EventArgs e)
        {
            if (ic != null)
            {
                Ink loadedInk = new Ink();
                loadedInk = ic.Ink.Clone();
                ic.Ink.DeleteStrokes(ic.Ink.Strokes);

                Rectangle scaleRectangle = new System.Drawing.Rectangle(0, 0, this.ClockDrawing.Height, this.ClockDrawing.Height);
                loadedInk.Strokes.ScaleToRectangle(scaleRectangle);
                loadedInk.Strokes.Scale(25, 25);
                // temporarily disable the ink collector and swap ink objects
                Rectangle boundRect = loadedInk.GetBoundingBox();

                ic.Enabled = false;
                ic.Ink = loadedInk;
                //ic.Enabled = true;

                this.ClockDrawing.Invalidate();
            }
        }

        //receive clock object from ScoringNumbers class and define myClock as clock



		/// <summary>
        /// This function only open a file instead of a series of files in a folder.
        /// This function handles the Open... command. It will determine the type of ink which is being opened and call the appropriate helper routine.
        /// The try...catch section in this function will handle all of the error handling for the functions it calls.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void OpenMenu_Click(object sender, System.EventArgs e)
		{			
			/// Create the OpenFileDialog, which presents a standard Windows
			/// dialog to the user.
			OpenFileDialog openDialog = new OpenFileDialog();

			/// Set the filter to suggest our recommended extensions
            openDialog.Filter = "Ink Serialized Format files (*.isf)|*.isf";
 
			/// If the dialog exits and the user didn't choose Cancel
			if(openDialog.ShowDialog() == DialogResult.OK)
			{
                loadClockInk(Path.GetFullPath(openDialog.FileName));

                this.displayedFileName = openDialog.FileName.Substring(openDialog.FileName.LastIndexOf('\\') + 1);
                dispStrokesOnCanvas(dicClocks[displayedFileName]);
                ////add stuff here

                //
                //
                //

                //check the local score
                //have to pass penstrokes
                //Temporary workaround for getting a circle object to the scoring numbers algorithms
                //TODO re-implement this
                Circle myCircle = new Circle(ClockDrawing.Width / 2 - 365, ClockDrawing.Height / 2 - 365, 365 * 2); 
                //send local clock to scoringNumbers algo
                ScoringNumbers currentScore = new ScoringNumbers(oClock, myCircle);
                //score values
                int[] scoreBoard = currentScore.scoringCPH();
                dispScores(oClock.ScoreBoard);


                dispStrokeOrder(dicClocks[displayedFileName].PenStrokes);
                reportAirTime(dicClocks[displayedFileName].PenStrokes);
                reportPressure(dicClocks[displayedFileName].PenStrokes);

                // display information
                this.lbID.Text = this.displayedFileName.Split('.')[0];

                long startingTime = dicClocks[displayedFileName].PenStrokes.First().PacketPoints.First().TimeStamp;
                long endingTime = dicClocks[displayedFileName].PenStrokes.Last().PacketPoints.Last().TimeStamp;
                long longDuration = endingTime - startingTime;
                this.DurationInSecond = (int)(longDuration / 1000);
                int dispSecond = DurationInSecond % 60;
                int dispMinute = (DurationInSecond - dispSecond) / 60;

                string dispDuration = dispMinute.ToString() + " minutes " + dispSecond.ToString() + " seconds ";
                this.lbDuration.Text = dispDuration;

                this.lbTrial.Text = dicClocks[displayedFileName].numOfTrial.ToString();
			}
		}

        /// <summary>
        /// This function open a folder that contains multiple records of clock drawings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();

            DialogResult result = openDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderPath = openDialog.SelectedPath;
                if (folderPath != "")
                {
                    string[] filePaths = Directory.GetFiles(folderPath, "*.isf");
                    foreach (string file in filePaths)
                    {

                    }
                }
            }
        }

        private void saveMenu_Click(object sender, EventArgs e)
        {
            /// Create a stream which will be used to save data to the output file
            Stream myStream = null;

            /// Create the SaveFileDialog, which presents a standard Windows
            /// Save dialog to the user.
            SaveFileDialog saveDialog = new SaveFileDialog();

            /// Set the filter to suggest our recommended extensions
            saveDialog.Filter = "Graphics Interchange Format files (*.ISF)|*.ISF";

            /// If the dialog exits and the user didn't choose Cancel
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    /// Attempt to Open the file with read/write permission
                    myStream = saveDialog.OpenFile();
                    if (myStream != null)
                    {
                        // Put the filename in a more canonical format
                        String filename = saveDialog.FileName.ToLower();

                        // Get a version of the filename without an extension
                        // This will be used for saving the associated image
                        String extensionlessFilename = Path.GetFileNameWithoutExtension(filename);

                        // Get the extension of the file 
                        String extension = Path.GetExtension(filename);

                        String filePath = filename.Replace(extensionlessFilename + extension, "");

                        saveISF(myStream);
                    }
                    else
                    {
                        // Throw an exception if a null pointer is returned for the stream
                        throw new IOException();
                    }
                }
                catch (IOException /*ioe*/)
                {
                    MessageBox.Show("File error");
                }
                finally
                {
                    // Close the stream in the finally clause so it
                    // is always reached, regardless of whether an 
                    // exception occurs.  SaveXML, SaveHTML, and
                    // SaveISF can throw, so this precaution is necessary.
                    if (null != myStream)
                    {
                        myStream.Close();
                    }
                }
            } // End if user chose OK from dialog 
        }

        private void saveISF(Stream myStream)
        {
            byte[] isf;

            // Perform the serialization
            isf = ic.Ink.Save(PersistenceFormat.InkSerializedFormat);

            // Write the ISF to the stream
            myStream.Write(isf, 0, isf.Length);
        }

        private void penButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void eraserButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void blackButton_CheckedChanged(object sender, EventArgs e)
        {
            ic.DefaultDrawingAttributes.Color = Color.Black;
        }

        private void redButton_CheckedChanged(object sender, EventArgs e)
        {
            ic.DefaultDrawingAttributes.Color = Color.Red;
        }

        private void greenButton_CheckedChanged(object sender, EventArgs e)
        {
            ic.DefaultDrawingAttributes.Color = Color.Green;
        }

        private void blueButton_CheckedChanged(object sender, EventArgs e)
        {
            ic.DefaultDrawingAttributes.Color = Color.Blue;
        }

        void lbReco_MouseEnter(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                Label lb = (Label)sender;
                int id = int.Parse(lb.Name);
                //PenStroke stroke = dicClocks[displayedFileName].PenStrokes[id];
                Microsoft.Ink.Stroke stroke = this.ic.Ink.Strokes[id];
                Rectangle boundingRect = stroke.GetBoundingBox();//stroke.PenStk.GetBoundingBox();
                Point upperLeft = new System.Drawing.Point(boundingRect.X, boundingRect.Y);
                Point bottomRight = new System.Drawing.Point(boundingRect.Right, boundingRect.Bottom);

                using (Graphics g = this.ClockDrawing.CreateGraphics())
                {
                    this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref upperLeft);
                    this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref bottomRight);
                    Rectangle drawingRect = new Rectangle(upperLeft, new System.Drawing.Size(bottomRight.X - upperLeft.X, bottomRight.Y - upperLeft.Y));

                    System.Drawing.Pen pen = new System.Drawing.Pen(Color.Red, 3);
                    g.DrawRectangle(pen, drawingRect);
                    //drawingRect.Inflate(5,5);
                    this.ClockDrawing.Invalidate(drawingRect);
                }
            }
        }

        void lbReco_MouseLeave(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                Label lb = (Label)sender;
                int id = int.Parse(lb.Name);
                //PenStroke stroke = dicClocks[displayedFileName].PenStrokes[id];
                Microsoft.Ink.Stroke stroke = this.ic.Ink.Strokes[id];
                Rectangle boundingRect = stroke.GetBoundingBox();//stroke.PenStk.GetBoundingBox();
                Point upperLeft = new System.Drawing.Point(boundingRect.X, boundingRect.Y);
                Point bottomRight = new System.Drawing.Point(boundingRect.Right, boundingRect.Bottom);
                using (Graphics g = this.ClockDrawing.CreateGraphics())
                {
                    this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref upperLeft);
                    this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref bottomRight);
                    Rectangle drawingRect = new Rectangle(upperLeft, new System.Drawing.Size(bottomRight.X - upperLeft.X, bottomRight.Y - upperLeft.Y));
                    drawingRect.Inflate(6, 6);
                    this.ClockDrawing.Invalidate(drawingRect);
                }
            }
        }

        #endregion
        
        #region load data


        /// <summary>
        /// load a ISF file from a path
        /// </summary>
        /// <param name="path"></param>
        private void loadClockInk(string path)
        {
            /// Create a stream which will be used to load data from the output file
            FileStream myStream = null;
            try
            {
                /// Attempt to Open the file with read only permission
                myStream = File.OpenRead(path);
                if (myStream != null)
                {
                    // Put the filename in a more canonical format
                    String filename = path.Substring(path.LastIndexOf('\\')+1);
                    String filepath = path.Remove(path.LastIndexOf('\\') + 1);

                    // Get a version of the filename without an extension
                    // This will be used for saving the associated image
                    String extensionlessFilename = Path.GetFileNameWithoutExtension(filename);

                    string connectionString = @"Data Source=" + filepath + extensionlessFilename + "_sqlite.db3";
                    this.sqlConnection = new SQLiteConnection(connectionString);
                    this.sqlConnection.Open();

                    
                    oClock.Ink = loadISF(myStream);
                    this.LoadStrokes(oClock);
                    if (dicClocks.ContainsKey(filename))
                        dicClocks[filename] = oClock;
                    else
                        dicClocks.Add(filename, oClock);

                    createButton(oClock, filename);
                }
                else
                {
                    // Throw an exception if a null pointer is returned for the stream
                    throw new IOException();
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("File error");
            }
            catch (Exception e)
            {
                // If the xml or the scanned form image file are not available,
                // display an error and exit
                MessageBox.Show("An error occured while loading ink from the specified file.\n" +
                    "Please verify that the file contains valid serialized ink and try again.",
                    "Serialization",
                    MessageBoxButtons.OK);
            }
            finally
            {
                // Close the stream in the finally clause so it
                // is always reached, regardless of whether an 
                // exception occurs.  LoadXML, LoadHTML, and
                // LoadISF can throw, so this precaution is necessary.
                if (null != myStream)
                {
                    myStream.Close();
                }
            }

        }

        // This function will load ISF into the ink object.
        // It will also pull the data stored in the object's extended properties and
        // repopulate the text boxes.
        private Ink loadISF(Stream s)
        {
            Ink loadedInk = new Ink();
            byte[] isfBytes = new byte[s.Length];

            // read in the ISF
            s.Read(isfBytes, 0, (int)s.Length);

            // load the ink into a new ink object
            // once an ink object has been "dirtied" it can never load ink again
            loadedInk.Load(isfBytes);

            return loadedInk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void LoadStrokes(Clock clock)
        {
            // read all the data into data table object for further querry
            DataTable dtPenStrokes = new DataTable();
            DataTable dtPoints = new DataTable();
            DataTable dtRects = new DataTable();
            DataTable dtPacketPoints = new DataTable();
            DataTable dtClock = new DataTable();

            string commandText = @"SELECT * FROM pen_stroke";
            SQLiteCommand command = new SQLiteCommand(commandText, sqlConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            dtPenStrokes.Load(reader);
            reader.Close();

            commandText = @"SELECT * FROM point";
            command = new SQLiteCommand(commandText, sqlConnection);
            reader = command.ExecuteReader();
            dtPoints.Load(reader);
            reader.Close();            

            commandText = @"SELECT * FROM rect";
            command = new SQLiteCommand(commandText, sqlConnection);
            reader = command.ExecuteReader();
            dtRects.Load(reader);
            reader.Close();

            commandText = @"SELECT * FROM packetpoint";
            command = new SQLiteCommand(commandText, sqlConnection);
            reader = command.ExecuteReader();
            dtPacketPoints.Load(reader);
            reader.Close();

            commandText = @"SELECT * FROM clock";
            command = new SQLiteCommand(commandText, sqlConnection);
            reader = command.ExecuteReader();
            dtClock.Load(reader);
            reader.Close();

            // create a penstroke object and fill up its required data
            List<PenStroke> strokes = new List<PenStroke>();
            int count = 0;
            try
            {
                // load the clock
                DataRow rowClock = dtClock.Rows[0];
                int clockX = int.Parse(rowClock[0].ToString());
                int clockY = int.Parse(rowClock[1].ToString());
                int clockR = int.Parse(rowClock[2].ToString());

                clock.X = clockX;
                clock.Y = clockY;
                clock.R = clockR;
                clock.numOfTrial = int.Parse(rowClock[5].ToString());

                // load pen stroke
                foreach (DataRow row in dtPenStrokes.Rows)
                {
                    PenStroke stroke = new PenStroke();
                    stroke.OrderID = count;

                    //bezier points
                    string[] bzpoints = row[1].ToString().Split(',');
                    for (int i = 0; i < bzpoints.Length - 1; i++)
                    {
                        DataRow rowPoint = dtPoints.Rows[int.Parse(bzpoints[i])-1];
                        stroke.BezierPoints.Add(new Point(int.Parse(rowPoint[2].ToString()),
                            int.Parse(rowPoint[3].ToString())));
                    }

                    //bounding box
                    int boundingbox = int.Parse(row[4].ToString())-1;
                    DataRow rowRect = dtRects.Rows[boundingbox];
                    stroke.BoundingBox = new System.Drawing.Rectangle(int.Parse(rowRect[1].ToString()),
                        int.Parse(rowRect[2].ToString()), int.Parse(rowRect[4].ToString()), int.Parse(rowRect[9].ToString()));

                    //combine to property
                    stroke.CombineTo = int.Parse(row[5].ToString());

                    //merge to property
                    stroke.MergeTo = int.Parse(row[6].ToString());

                    //merging rectangle
                    int mergingRect = int.Parse(row[7].ToString())-1;
                    rowRect = dtRects.Rows[mergingRect];
                    stroke.MergingRectangle = new System.Drawing.Rectangle(int.Parse(rowRect[1].ToString()),
                        int.Parse(rowRect[2].ToString()), int.Parse(rowRect[4].ToString()), int.Parse(rowRect[9].ToString()));

                    //packet points
                    string[] pkpoints = row[8].ToString().Split(',');
                    for (int i = 0; i < pkpoints.Length - 1; i++)
                    {
                        DataRow rowPkPoint = dtPacketPoints.Rows[int.Parse(pkpoints[i]) - 1];

                        int pointID = int.Parse(rowPkPoint[1].ToString())-1;
                        DataRow rowPoint = dtPoints.Rows[pointID];

                        PacketPoint pkPoint = new PacketPoint();
                        pkPoint.Point = new System.Drawing.Point(int.Parse(rowPoint[2].ToString()), int.Parse(rowPoint[3].ToString()));
                        pkPoint.Pressure = int.Parse(rowPkPoint[2].ToString());
                        pkPoint.TimeStamp = long.Parse(rowPkPoint[5].ToString());
                        stroke.PacketPoints.Add(pkPoint);
                    }

                    //pixel bounding box
                    int pixelBoundingBox = int.Parse(row[10].ToString())-1;
                    rowRect = dtRects.Rows[pixelBoundingBox];
                    stroke.PixelBoundingBox = new System.Drawing.Rectangle(int.Parse(rowRect[1].ToString()),
                        int.Parse(rowRect[2].ToString()), int.Parse(rowRect[4].ToString()), int.Parse(rowRect[9].ToString()));


                    //reco strokes
                    stroke.RecoStrokes = row[11].ToString();
                    stroke.isHand = int.Parse(row[12].ToString());
                    if (stroke.RecoStrokes != "")
                    {
                        if (int.Parse(stroke.RecoStrokes) < 0)
                        {
                            if (stroke.isHand <= 0) stroke.RecoStrokes = "?";
                            if (stroke.isHand == 1) stroke.RecoStrokes = "H1";
                            if (stroke.isHand == 2) stroke.RecoStrokes = "H2";
                        }
                    }
                    // timestamp
                    stroke.TimeStamp = long.Parse(row[13].ToString(), System.Globalization.NumberStyles.Float);
                    
                    strokes.Add(stroke);

                    count++;
                }
            }
            catch (Exception e)
            {

            }

            clock.PenStrokes = strokes;
        }





        // load each score
        /*
        private int[] loadScores()
        {
            DataTable dt = new DataTable();

            string commandText = @"SELECT score FROM score";
            int[] scores = new int[13];
            try
            {
                SQLiteCommand command = new SQLiteCommand(commandText, sqlConnection);

                SQLiteDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();

                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    scores[i] = int.Parse(row.ItemArray[0].ToString());
                    i++;
                }
            }
            catch (Exception e)
            {
            }
            return scores;
        }
        */

        #endregion

        #region display data on UI
        /// <summary>
        /// Todo: need a way to draw a exactly right circle and those inks
        /// </summary>
        /// <param name="loadedInk"></param>
        
        private void createButton(Clock oClock, string clockName)
        {
            Ink loadedInk = oClock.Ink.Clone();

            Pen pen = new System.Drawing.Pen(Color.Black, 1);

            // convert the ink to a bitmap and put it on pictureBox
            Bitmap curStrokesBitmap = new Bitmap(110, 110);
            Graphics g = Graphics.FromImage(curStrokesBitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Point ptRect = new System.Drawing.Point(this.pictureBox1.Width - 10, this.pictureBox1.Height - 10);
            this.pictureBox1.Renderer.PixelToInkSpace(g, ref ptRect);

            Renderer r = new Renderer();
            loadedInk.Strokes.ScaleToRectangle(new Rectangle(0, 0, ptRect.X, ptRect.Y));
            Microsoft.Ink.Stroke stroke = loadedInk.Strokes[0];


            r.Draw(g, loadedInk.Strokes);
            g.DrawEllipse(pen, 0, 0, this.pictureBox1.Width - 10, this.pictureBox1.Width - 10);

            int numOfClocks = dicClocks.Count - 1;

            // Create a Button for this clock
            Button newBt = new Button();
            newBt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            newBt.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            newBt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            newBt.Location = new System.Drawing.Point(numOfClocks * 120 + 10, 6);
            newBt.Name = clockName;
            newBt.Size = new System.Drawing.Size(110, 110);
            newBt.TabIndex = 1;
            newBt.Text = "";
            newBt.UseVisualStyleBackColor = true;
            newBt.Image = curStrokesBitmap;
            newBt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))));
            newBt.Click += new EventHandler(newBt_Click);
            this.panel1.Controls.Add(newBt);
        }

        void newBt_Click(object sender, EventArgs e)
        {
            string name = ((Button)sender).Name;

            dispStrokesOnCanvas(this.dicClocks[name]); 

        }

        private void dispStrokesOnCanvas(Clock oClock) 
		{
            Ink loadedInk = oClock.Ink.Clone();

            Pen pen = new System.Drawing.Pen(Color.Black, 1);

            /// draw the circle and strokes on main animated picture box
            
            // Define the circle
            int radius = Math.Min(this.ClockDrawing.Width, this.ClockDrawing.Height) / 2 - 10;
            int drawingX = (this.ClockDrawing.Width - (2 * radius)) / 2;
            int drawingY = 10;

            // Get the drawing elements
            Bitmap canvasBitmap = new System.Drawing.Bitmap(this.ClockDrawing.Width, this.ClockDrawing.Height);
            Graphics g2 = Graphics.FromImage(canvasBitmap);//this.ClockDrawing.CreateGraphics();
            Ink disPlayInk = oClock.Ink.Clone();

            // calculate the translation and scale factor between the drawing circle and the circle from the database
            this.scale = (double)radius / (double)oClock.R;
            this.translationPt = new System.Drawing.Point((int)(drawingX - (oClock.X * scale)), (int)(drawingY - (oClock.Y * scale)));
            this.ClockDrawing.Renderer.PixelToInkSpace(g2, ref translationPt);

            disPlayInk.Strokes.Scale((float)scale, (float)scale);
            disPlayInk.Strokes.Move(translationPt.X, translationPt.Y);

            // Draw the circle
            g2.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, this.ClockDrawing.Width, this.ClockDrawing.Height));
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new System.Drawing.Pen(Color.Black, 2);
            g2.DrawEllipse(pen, drawingX, drawingY, radius * 2, radius * 2);
            //g2.FillEllipse(Brushes.Red, drawingX + radius - 20, drawingY + radius - 2, 2, 2);
            this.ClockDrawing.Image = canvasBitmap;


            // put the strokes into the ink collector
            ic.Ink = disPlayInk;//loadedInk;

            Point tmpPt = new System.Drawing.Point(1, 1);
            this.ClockDrawing.Renderer.PixelToInkSpace(g2, ref tmpPt);

            //// Repaint the inkable region
            this.ClockDrawing.Invalidate();
		}
        
        private void dispScores(int[] scores)
        {
            for (int i = 0; i < scores.Length; i++)
            {
                int score = scores[i];
                switch (i)
                {
                    case 0:
                        if (score == 1)
                            check1.Visible = true;
                        break;
                    case 1:
                        if (score == 1)
                            check2.Visible = true;
                        break;
                    case 2:
                        if (score == 1)
                            check3.Visible = true;
                        break;
                    case 3:
                        if (score == 1)
                            check4.Visible = true;
                        break;
                    case 4:
                        if (score == 1)
                            check5.Visible = true;
                        break;
                    case 5:
                        if (score == 1)
                            check6.Visible = true;
                        break;
                    case 6:
                        if (score == 1)
                            check7.Visible = true;
                        break;
                    case 7:
                        if (score == 1)
                            check8.Visible = true;
                        break;
                    case 8:
                        if (score == 1)
                            check9.Visible = true;
                        break;
                    case 9:
                        if (score == 1)
                            check10.Visible = true;
                        break;
                    case 10:
                        if (score == 1)
                            check11.Visible = true;
                        break;
                    case 11:
                        if (score == 1)
                            check12.Visible = true;
                        break;
                    case 12:
                        if (score == 1)
                            check13.Visible = true;
                        break;
                }
            }
        }

        private void dispStrokeOrder(List<PenStroke> strokes)
        {
            int count = this.grpSequence.Controls.Count;
            for(int i=count-1;i>=0;i--)
            {
                this.grpSequence.Controls.Remove(this.grpSequence.Controls[i]);
            }
            
            int xLocation = 6;
            int id = 0;
            foreach (PenStroke stroke in strokes)
            {
                Label lbReco = new Label();
                // 
                // label15
                // 
                lbReco.AutoSize = true;
                lbReco.Cursor = System.Windows.Forms.Cursors.Hand;
                lbReco.Location = new System.Drawing.Point(xLocation, 30);
                lbReco.Name = id.ToString();
                lbReco.Size = new System.Drawing.Size(60, 30);
                lbReco.TabIndex = 0;
                lbReco.Text = stroke.RecoStrokes+",";
                lbReco.Tag = stroke.Id;
                lbReco.MouseEnter += new EventHandler(lbReco_MouseEnter);
                lbReco.MouseLeave += new EventHandler(lbReco_MouseLeave);
                lbReco.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lbReco_MouseDoubleClick);
                this.grpSequence.Controls.Add(lbReco);

                xLocation = xLocation + 20;
                id = id + 1;
            }
        }

        void lbReco_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ((Label)sender).MouseLeave -= new EventHandler(lbReco_MouseLeave);
            ((Label)sender).MouseEnter -= new EventHandler(lbReco_MouseEnter);
            ((Label)sender).Visible = false;

            TextBox txtChangeBox = new TextBox();
            txtChangeBox.Location = ((Label)sender).Location;
            txtChangeBox.Size = ((Label)sender).Size;
            txtChangeBox.Focus();
            txtChangeBox.Name = ((Label)sender).Name;
            txtChangeBox.Text = ((Label)sender).Text;
            txtChangeBox.LostFocus += new EventHandler(txtChangeBox_LostFocus);
            txtChangeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(txtChangeBox_KeyDown);
            this.grpSequence.Controls.Add(txtChangeBox);
            
        }

        void txtChangeBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch(e.KeyValue)
            {
                case 13:
                    this.txtChangeBox_LostFocus(sender, new EventArgs());
                    break;
            }
        }

        void txtChangeBox_LostFocus(object sender, EventArgs e)
        {
            foreach (Control control in this.grpSequence.Controls)
            {
                if (!control.Visible && control is Label)
                {
                    control.Visible = true;
                    ((Label)control).MouseLeave += new EventHandler(lbReco_MouseLeave);
                    ((Label)control).MouseEnter += new EventHandler(lbReco_MouseEnter);

                    ((Label)control).Text = ((TextBox)sender).Text + ",";
                    ((Label)control).Name = ((TextBox)sender).Name;
                    break;
                }
            }

            this.grpSequence.Controls.Remove((TextBox)sender);
        }

        private void reportAirTime(List<PenStroke> penStrokes)
        {
            long preAirtime = 0;
            Series seriesAirTime = this.chartAirTime.Series[0];
            int i = -1;
            foreach (PenStroke stroke in penStrokes)
            {
                i++;
                int lastIndex = stroke.PacketPoints.Count-1;
                if (preAirtime == 0)
                {                    
                    preAirtime = stroke.PacketPoints[lastIndex].TimeStamp;
                    continue;
                }

                if (stroke.MergeTo < 100 && stroke.MergeTo < i)
                {
                    preAirtime = stroke.PacketPoints[lastIndex].TimeStamp;
                    continue;
                }

                long curAirTime = stroke.PacketPoints[0].TimeStamp;
                long timeDiff = curAirTime - preAirtime;
                object[] addingY = new object[]{timeDiff};
                seriesAirTime.Points.AddXY(stroke.RecoStrokes, addingY);
                preAirtime = stroke.PacketPoints[lastIndex].TimeStamp;
            }
        }

        private void reportPressure(List<PenStroke> penStrokes)
        {
            foreach (PenStroke stroke in penStrokes)
            {
                foreach (PacketPoint pkPoint in stroke.PacketPoints)
                {
                    Series seriesPkPoint = this.chartPressure.Series[0];
                    object[] addingY = new object[]{pkPoint.Pressure};
                    seriesPkPoint.Points.AddXY(stroke.RecoStrokes, addingY);
                }
            }
        }

        #endregion
        
        // Read each score from the file
        private float[] readAirtime(char[] delimiters, String tempString)
        {
            int i = 0, j = 0;
            String[] subAir = tempString.Split(delimiters, StringSplitOptions.None);
            airtime = new float[subAir.Length / 4];
            float[] tmpAirtime = new float[subAir.Length / 4];

            foreach (String subs in subAir)
            {
                if ((i > 4) && (i % 4 == 1))
                {
                    airtime[j] = float.Parse(subs);
                    tmpAirtime[j] = float.Parse(subs);
                    j++;
                }
                i++;
            }
            return tmpAirtime;
        }        

        private void clearScoreboard()
        {
            check1.Visible = false;
            check2.Visible = false;
            check3.Visible = false;
            check4.Visible = false;
            check5.Visible = false;
            check6.Visible = false;
            check7.Visible = false;
            check8.Visible = false;
            check9.Visible = false;
            check10.Visible = false;
            check11.Visible = false;
            check12.Visible = false;
            check13.Visible = false;
        }

        // Display participant information.
        private StringBuilder genParInfo(String filename)
        {            
            StringBuilder tmpStringBuilder = new StringBuilder("");

            String tempString = "Name: " + filename + Environment.NewLine;
            tmpStringBuilder.Append(tempString);


            // Calculate total score
            int tot_score = calcTotScore();

            tempString = "Total Score: " + tot_score.ToString() + " / 13" + Environment.NewLine;
            tmpStringBuilder.Append(tempString);

            // Calculate average airtime
            float avg_airtime = calcAvgAir();

            tempString = "Average airtime: " + avg_airtime.ToString() + " sec" + Environment.NewLine;
            tmpStringBuilder.Append(tempString);
                      
            return tmpStringBuilder;
        }

        // Calculate total score
        private int calcTotScore()
        {
            int temp_score = 0;

            foreach (int score in scoreboard)
            {
                temp_score += score; 
            }

            return temp_score;
        }

        // Calculate average airtime
        private float calcAvgAir()
        {
            float temp_air = 0;

            foreach (float air in airtime)
            {
                temp_air += air;
            }

            temp_air /= airtime.Length;

            return temp_air;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenMenu_Click(sender, e);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveMenu_Click(sender, e);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        Timer timer = new Timer();
                
        private void play_pause_Click(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                this.mInkOverly.Ink.DeleteStrokes(this.mInkOverly.Ink.Strokes);
                this.ClockDrawing.Ink.DeleteStrokes(this.ClockDrawing.Ink.Strokes);

                isPlaying = true;

                long now = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
                long difference = now - pauseTime;
                startTime = startTime + difference;

                timer.Start();

                this.play_pause.Image = Properties.Resources.media_pause;
            }
            else
            {
                this.play_pause.Image = Properties.Resources.play;
                isPlaying = false;
                pauseTime = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);

                timer.Stop();
            }
        }

        int packetPtId = 0;
        int strokeID = 0;
        Microsoft.Ink.Ink renderInk;

        Point[] pkPoints;
        List<Microsoft.Ink.Stroke> existingStrokes = new List<Microsoft.Ink.Stroke>();
        Microsoft.Ink.InkOverlay mInkOverly;
        private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
        long startTime;
        long pauseTime;
        long oriStartTime;
        //void ClockDrawing_Paint(object sender, PaintEventArgs e)
        private void Animation()
        {
            try
            {
                Clock[] tmpClock = new Clock[dicClocks.Count];
                dicClocks.Values.CopyTo(tmpClock, 0);

                if (tmpClock != null && tmpClock[0] != null)
                {
                    if (isPlaying)
                    {
                        //Strokes strokes = tmpClock[0].Ink.Strokes;
                        if (tmpClock.Length > 0 && tmpClock[0] != null)
                        {
                            Strokes oriStorkes = tmpClock[0].Ink.Strokes;
                            List<PenStroke> strokes = tmpClock[0].PenStrokes;
                            using (Graphics g = this.ClockDrawing.CreateGraphics())
                            {
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                                Renderer r = new Renderer();
                                Ink loadingInk = new Ink();

                                Microsoft.Ink.DrawingAttributes da = new Microsoft.Ink.DrawingAttributes();
                                ExtendedProperties inkProperties = ic.Ink.ExtendedProperties;


                                if (packetPtId == 0)
                                {

                                    if (strokeID == 0)
                                    {
                                        ic.Ink.DeleteStrokes(ic.Ink.Strokes);
                                        this.ClockDrawing.Invalidate();
                                        startTime = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
                                        oriStartTime = strokes[strokeID].PacketPoints[packetPtId].TimeStamp;

                                    }
                                    long oriPkgPtTime = strokes[strokeID].PacketPoints[packetPtId].TimeStamp;
                                    long now = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
                                    long fromStart = now - startTime;
                                    this.BeginInvoke(new AnimationDelegate(DisplaySec), new object[] { fromStart });

                                    // create a new stroke
                                    if (now - startTime >= oriPkgPtTime - oriStartTime)
                                    {
                                        createNewStroke(g, strokes, oriStorkes, inkProperties);
                                    }
                                }
                                else
                                {
                                    long now = (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);

                                    long difference = now - startTime;
                                    Microsoft.Ink.Stroke renderingStroke = ic.Ink.Strokes[ic.Ink.Strokes.Count - 1];

                                    int tmpPckPtId = 0;
                                    int pckPtCount = strokes[strokeID].PacketPoints.Count;
                                    System.Drawing.Point finalPktPt = new System.Drawing.Point();
                                    while (true)
                                    {
                                        long oriPkgPtTime = strokes[strokeID].PacketPoints[packetPtId].TimeStamp;

                                        if (difference >= oriPkgPtTime - oriStartTime && packetPtId != pckPtCount)
                                        {
                                            System.Drawing.Point tmpOriPt = oriStorkes[strokeID].GetPoint(packetPtId);
                                            finalPktPt = new System.Drawing.Point((int)(tmpOriPt.X * scale) + translationPt.X, (int)(tmpOriPt.Y * scale) + translationPt.Y);

                                            renderingStroke.SetPoint(packetPtId, finalPktPt);

                                            packetPtId++;
                                            if (packetPtId == pckPtCount) break;
                                        }
                                        else if (packetPtId != pckPtCount && tmpPckPtId < pckPtCount && finalPktPt != new Point(0,0))
                                        // set the rest of the points in the stroke as the current end point
                                        {
                                            if (tmpPckPtId < packetPtId) tmpPckPtId = packetPtId;
                                            renderingStroke.SetPoint(tmpPckPtId, finalPktPt);

                                            tmpPckPtId++;
                                        }
                                        else
                                            break;
                                    }

                                    Microsoft.Ink.Stroke tmpStroke = ic.Ink.Strokes[ic.Ink.Strokes.Count - 1];
                                    Point upperLeft = new System.Drawing.Point(tmpStroke.GetBoundingBox().X, tmpStroke.GetBoundingBox().Y);
                                    Point bottomRight = new System.Drawing.Point(tmpStroke.GetBoundingBox().Right, tmpStroke.GetBoundingBox().Bottom);
                                    this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref upperLeft);
                                    this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref bottomRight);

                                    this.ClockDrawing.Invalidate(new Rectangle(upperLeft.X, upperLeft.Y, bottomRight.X, bottomRight.Y));

                                    if (strokes[strokeID].PacketPoints.Count == packetPtId)
                                    {
                                        packetPtId = 0;
                                        strokeID++;
                                        if (strokeID == strokes.Count)
                                        {
                                            strokeID = 0;
                                            isPlaying = false;
                                            Application.Idle -= new EventHandler(Application_Idle);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Point scaleRectangle = new System.Drawing.Point(this.ClockDrawing.Height, this.ClockDrawing.Height);
                        //Strokes staticStrokes = tmpClock[0].Ink.Strokes;
                        //staticStrokes.ScaleToRectangle(new Rectangle(0, 0, scaleRectangle.X, scaleRectangle.Y));
                        //staticStrokes.Scale(0.85f, 0.85f);
                        ////staticStrokes.Move(1750, 800);
                        //Graphics g2 = this.ClockDrawing.CreateGraphics();
                        //Renderer r = new Renderer();
                        //r.Draw(g2, staticStrokes);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void createNewStroke(Graphics g, List<PenStroke> strokes, Strokes oriStorkes, ExtendedProperties inkProperties)
        {
            pkPoints = new Point[strokes[strokeID].PacketPoints.Count];
            System.Drawing.Point tmpPt = oriStorkes[strokeID].GetPoint(packetPtId);
            System.Drawing.Point finalPktPt = new System.Drawing.Point((int)(tmpPt.X * scale) + translationPt.X, (int)(tmpPt.Y * scale) + translationPt.Y);

            Point newPt = new Point((int)(tmpPt.X * 0.8) - 500, (int)(tmpPt.Y * 0.8) - 5500);
            int[] pressureValue = new int[strokes[strokeID].PacketPoints.Count];
            for (int i = 0; i < pkPoints.Length; i++)
            {
                pkPoints[i] = finalPktPt;
            }

            Microsoft.Ink.Stroke tmpStroke = ic.Ink.CreateStroke(pkPoints);// mInkOverly.Ink.CreateStroke(pkPoints);
            if (inkProperties.DoesPropertyExist(PacketProperty.NormalPressure))
            {
                tmpStroke.SetPacketValuesByProperty(PacketProperty.NormalPressure, pressureValue);
            }

            Point upperLeft = new System.Drawing.Point(tmpStroke.GetBoundingBox().X, tmpStroke.GetBoundingBox().Y);
            Point bottomRight = new System.Drawing.Point(tmpStroke.GetBoundingBox().Right, tmpStroke.GetBoundingBox().Bottom);
            this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref upperLeft);
            this.ClockDrawing.Renderer.InkSpaceToPixel(g, ref bottomRight);

            this.ClockDrawing.Invalidate(new Rectangle(upperLeft.X, upperLeft.Y, bottomRight.X, bottomRight.Y));

            packetPtId++;
        }

        private delegate void AnimationDelegate(long sec);
        private void DisplaySec(long sec)
        {
            Sec.Text = (sec/1000).ToString();
            double percent = ((double)sec/1000) / (double)DurationInSecond;
            this.trackBar1.Value = (int)(percent * 100) > 100 ? 100 : (int)(percent * 100);

        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (this.dicClocks.Count == 0)
            {
                timer.Stop();
                MessageBox.Show("Please load a clock");
                return;
            }


            Animation();
        }

        private void saveClockToImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog savingDialog = new SaveFileDialog();
            savingDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            savingDialog.Title = "Save an Image File";
            savingDialog.ShowDialog();

            if (savingDialog.FileName != "")
            {
                // create graphics object from the bitmap
                Bitmap b2 = new Bitmap(this.ClockDrawing.Image);
                Graphics g1 = Graphics.FromImage(b2);
                g1.SmoothingMode = SmoothingMode.AntiAlias;
                // make a renderer to draw ink on the graphics surface
                Renderer r = new Renderer();
                r.Draw(g1, ic.Ink.Strokes);

                b2.Save(savingDialog.FileName);
            }
        }

        private void menuCapture_Click(object sender, EventArgs e)
        {
            this.saveClockToImageToolStripMenuItem_Click(sender, e);
        }

    }
}