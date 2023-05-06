//-------------------------------------------------------------------------
//
//  This is part of the Microsoft Tablet PC Platform SDK
//  Copyright (C) 2002 Microsoft Corporation
//  All rights reserved.
//
//  This source code is only intended as a supplement to the
//  Microsoft Tablet PC Platform SDK Reference and related electronic 
//  documentation provided with the Software Development Kit.
//  See these sources for more detailed information. 
//
//  File: Serialization.cs
//  Ink Serialization Application
//
//  This program demonstrates how one can serialize and deserialize 
//  ink in various formats.
//
//  The application represents a form with fields for inputting first
//  name, last name and signature. It allows the user to save this
//  data as pure ISF (Ink Serialized Format) XML using base64 encoded
//  ISF or HTML which references ISF images "fortified" with ink.  It
//  is also possible to Load from the XML and ISF formats.  The ISF 
//  save/load uses extended properties to store the first name and last
//  name, whereas the XML and HTML save/load stores this information in
//  custom attributes.
//
//  This sample does not support loading from the HTML format, because
//  HTML is not a format suitable for storing structured data, such as
//  forms. Because the data is separated into multiple streams (name,
//  signature, etc.,) a format which preserves this separation such as
//  XML or another kind of database format is required.
//
//  HTML is very useful in a flow editing environment, such as a word
//  processing document. The HTML that is saved by this sample uses
//  fortified ISFs. These ISFs have ISF embedded within them, which
//  preserves the full fidelity of the ink. A word processing
//  application could save a document containing multiple types of
//  data, such as images, tables, formatted text and ink persisted
//  in an HTML format. This HTML would render without a hitch in
//  browsers which have no special abilities to understand ink.
//  However, when loaded into an application that is ink-aware, such
//  as another word processing application, the full fidelity of the
//  original ink is available and can be rendered at very high quality,
//  edited or used for recognition.
//
//	See the "Ink Interoperability" whitepaper, in this SDK, for more
//  information about persistence formats for ink.
//
//  The features used are: Load method, Save method, and extended properties
//
//--------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;

// The Ink namespace, which contains the Tablet PC Platform API
using Microsoft.Ink;

namespace Serialization
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
        private TextBox textBox1;
        private Label label1;
        private RichTextBox partInfo;
        private MenuItem saveMenu;
        private GroupBox groupBox1;
        private RadioButton eraserButton;
        private RadioButton penButton;
        private GroupBox groupBox2;
        private RadioButton blueButton;
        private RadioButton greenButton;
        private RadioButton redButton;
        private RadioButton blackButton;
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
        private GroupBox groupBox3;
        private Label label13;
        private Label label3;
        private Label label2;
        private PictureBox check1;
        private PictureBox check8;
        private PictureBox check12;
        private PictureBox check11;
        private PictureBox check10;
        private PictureBox check9;
        private PictureBox check7;
        private PictureBox check6;
        private PictureBox check5;
        private PictureBox check4;
        private PictureBox check3;
        private PictureBox check2;
        private PictureBox check13;
        private Label label14;

        // The one and only ink collector
		private InkCollector ic;

        private int[] scoreboard = new int[13];
        private float[] airtime;

        private FileStream file;
        private StreamReader sr;
        private Button openButton;
        private Button saveButton;
        private SplitContainer splitContainer1;
        private PictureBox ClockDrawing;
        private Panel panel1;
        private TrackBar trackBar1;
        private Button play_pause;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private PictureBox pictureBox1;
        private MenuItem menuItem1;

        private string displayedFileName;
        private System.Collections.Generic.Dictionary<string, Clock> dicClocks;

		// Constructor
		public SerializationForm()
		{
            #region Standard Template Code
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            #endregion


            // Create the InkCollector and attach it to the signature GroupBox
            ic = new InkCollector(this.ClockDrawing.Handle);
            ic.Enabled = true;

            dicClocks = new System.Collections.Generic.Dictionary<string, Clock>();
            
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
            this.MenuBar = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenu = new System.Windows.Forms.MenuItem();
            this.OpenMenu = new System.Windows.Forms.MenuItem();
            this.saveMenu = new System.Windows.Forms.MenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.partInfo = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.eraserButton = new System.Windows.Forms.RadioButton();
            this.penButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.blueButton = new System.Windows.Forms.RadioButton();
            this.greenButton = new System.Windows.Forms.RadioButton();
            this.redButton = new System.Windows.Forms.RadioButton();
            this.blackButton = new System.Windows.Forms.RadioButton();
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.check13 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.check8 = new System.Windows.Forms.PictureBox();
            this.check12 = new System.Windows.Forms.PictureBox();
            this.check11 = new System.Windows.Forms.PictureBox();
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
            this.openButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.play_pause = new System.Windows.Forms.Button();
            this.ClockDrawing = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.check13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClockDrawing)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.saveMenu});
            this.FileMenu.Text = "&File";
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
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(3, -4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(890, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "ClockReader Analyzer (Beta Ver 0.9)";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Participant Info.:";
            // 
            // partInfo
            // 
            this.partInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.partInfo.BackColor = System.Drawing.SystemColors.Menu;
            this.partInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.partInfo.Location = new System.Drawing.Point(6, 20);
            this.partInfo.Name = "partInfo";
            this.partInfo.ReadOnly = true;
            this.partInfo.Size = new System.Drawing.Size(362, 74);
            this.partInfo.TabIndex = 3;
            this.partInfo.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eraserButton);
            this.groupBox1.Controls.Add(this.penButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 38);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Mode";
            // 
            // eraserButton
            // 
            this.eraserButton.AutoSize = true;
            this.eraserButton.Location = new System.Drawing.Point(56, 15);
            this.eraserButton.Name = "eraserButton";
            this.eraserButton.Size = new System.Drawing.Size(55, 17);
            this.eraserButton.TabIndex = 1;
            this.eraserButton.TabStop = true;
            this.eraserButton.Text = "&Eraser";
            this.eraserButton.UseVisualStyleBackColor = true;
            this.eraserButton.CheckedChanged += new System.EventHandler(this.eraserButton_CheckedChanged);
            // 
            // penButton
            // 
            this.penButton.AutoSize = true;
            this.penButton.Checked = true;
            this.penButton.Location = new System.Drawing.Point(6, 15);
            this.penButton.Name = "penButton";
            this.penButton.Size = new System.Drawing.Size(44, 17);
            this.penButton.TabIndex = 0;
            this.penButton.TabStop = true;
            this.penButton.Text = "&Pen";
            this.penButton.UseVisualStyleBackColor = true;
            this.penButton.CheckedChanged += new System.EventHandler(this.penButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.blueButton);
            this.groupBox2.Controls.Add(this.greenButton);
            this.groupBox2.Controls.Add(this.redButton);
            this.groupBox2.Controls.Add(this.blackButton);
            this.groupBox2.Location = new System.Drawing.Point(6, 318);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 37);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color";
            // 
            // blueButton
            // 
            this.blueButton.AutoSize = true;
            this.blueButton.Location = new System.Drawing.Point(175, 15);
            this.blueButton.Name = "blueButton";
            this.blueButton.Size = new System.Drawing.Size(46, 17);
            this.blueButton.TabIndex = 3;
            this.blueButton.TabStop = true;
            this.blueButton.Text = "Blue";
            this.blueButton.UseVisualStyleBackColor = true;
            this.blueButton.CheckedChanged += new System.EventHandler(this.blueButton_CheckedChanged);
            // 
            // greenButton
            // 
            this.greenButton.AutoSize = true;
            this.greenButton.Location = new System.Drawing.Point(115, 15);
            this.greenButton.Name = "greenButton";
            this.greenButton.Size = new System.Drawing.Size(54, 17);
            this.greenButton.TabIndex = 2;
            this.greenButton.TabStop = true;
            this.greenButton.Text = "Green";
            this.greenButton.UseVisualStyleBackColor = true;
            this.greenButton.CheckedChanged += new System.EventHandler(this.greenButton_CheckedChanged);
            // 
            // redButton
            // 
            this.redButton.AutoSize = true;
            this.redButton.Location = new System.Drawing.Point(64, 15);
            this.redButton.Name = "redButton";
            this.redButton.Size = new System.Drawing.Size(45, 17);
            this.redButton.TabIndex = 1;
            this.redButton.TabStop = true;
            this.redButton.Text = "Red";
            this.redButton.UseVisualStyleBackColor = true;
            this.redButton.CheckedChanged += new System.EventHandler(this.redButton_CheckedChanged);
            // 
            // blackButton
            // 
            this.blackButton.AutoSize = true;
            this.blackButton.Checked = true;
            this.blackButton.Location = new System.Drawing.Point(6, 15);
            this.blackButton.Name = "blackButton";
            this.blackButton.Size = new System.Drawing.Size(52, 17);
            this.blackButton.TabIndex = 0;
            this.blackButton.TabStop = true;
            this.blackButton.Text = "Black";
            this.blackButton.UseVisualStyleBackColor = true;
            this.blackButton.CheckedChanged += new System.EventHandler(this.blackButton_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(237, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "3. Numbers are in correct order.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(222, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "4. Numbers are drawn w/o rotating the paper.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "5. Numbers are in the correct postion.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "6. Numbers are all inside circle.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "7. Two hands are present.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 134);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(189, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "8. The hour target number is indicated.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(199, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "9. The minute target number is indicated.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 166);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(197, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "10. The hands  are in correct proportion.";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(191, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "11. There are no superfluous markings.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.check13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.check8);
            this.groupBox3.Controls.Add(this.check12);
            this.groupBox3.Controls.Add(this.check11);
            this.groupBox3.Controls.Add(this.check10);
            this.groupBox3.Controls.Add(this.check9);
            this.groupBox3.Controls.Add(this.check7);
            this.groupBox3.Controls.Add(this.check6);
            this.groupBox3.Controls.Add(this.check5);
            this.groupBox3.Controls.Add(this.check4);
            this.groupBox3.Controls.Add(this.check3);
            this.groupBox3.Controls.Add(this.check2);
            this.groupBox3.Controls.Add(this.check1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(6, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 237);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scoring";
            // 
            // check13
            // 
            this.check13.Image = ((System.Drawing.Image)(resources.GetObject("check13.Image")));
            this.check13.Location = new System.Drawing.Point(234, 210);
            this.check13.Name = "check13";
            this.check13.Size = new System.Drawing.Size(23, 18);
            this.check13.TabIndex = 34;
            this.check13.TabStop = false;
            this.check13.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 213);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "13. A center is present.";
            // 
            // check8
            // 
            this.check8.Enabled = false;
            this.check8.Image = ((System.Drawing.Image)(resources.GetObject("check8.Image")));
            this.check8.Location = new System.Drawing.Point(234, 129);
            this.check8.Name = "check8";
            this.check8.Size = new System.Drawing.Size(23, 18);
            this.check8.TabIndex = 32;
            this.check8.TabStop = false;
            this.check8.Visible = false;
            // 
            // check12
            // 
            this.check12.Image = ((System.Drawing.Image)(resources.GetObject("check12.Image")));
            this.check12.Location = new System.Drawing.Point(234, 194);
            this.check12.Name = "check12";
            this.check12.Size = new System.Drawing.Size(23, 18);
            this.check12.TabIndex = 31;
            this.check12.TabStop = false;
            this.check12.Visible = false;
            // 
            // check11
            // 
            this.check11.Image = ((System.Drawing.Image)(resources.GetObject("check11.Image")));
            this.check11.Location = new System.Drawing.Point(234, 178);
            this.check11.Name = "check11";
            this.check11.Size = new System.Drawing.Size(23, 18);
            this.check11.TabIndex = 30;
            this.check11.TabStop = false;
            this.check11.Visible = false;
            // 
            // check10
            // 
            this.check10.Image = ((System.Drawing.Image)(resources.GetObject("check10.Image")));
            this.check10.Location = new System.Drawing.Point(234, 162);
            this.check10.Name = "check10";
            this.check10.Size = new System.Drawing.Size(23, 18);
            this.check10.TabIndex = 29;
            this.check10.TabStop = false;
            this.check10.Visible = false;
            // 
            // check9
            // 
            this.check9.Image = ((System.Drawing.Image)(resources.GetObject("check9.Image")));
            this.check9.Location = new System.Drawing.Point(234, 145);
            this.check9.Name = "check9";
            this.check9.Size = new System.Drawing.Size(23, 18);
            this.check9.TabIndex = 28;
            this.check9.TabStop = false;
            this.check9.Visible = false;
            // 
            // check7
            // 
            this.check7.Enabled = false;
            this.check7.Image = ((System.Drawing.Image)(resources.GetObject("check7.Image")));
            this.check7.Location = new System.Drawing.Point(234, 112);
            this.check7.Name = "check7";
            this.check7.Size = new System.Drawing.Size(23, 18);
            this.check7.TabIndex = 27;
            this.check7.TabStop = false;
            this.check7.Visible = false;
            // 
            // check6
            // 
            this.check6.Image = ((System.Drawing.Image)(resources.GetObject("check6.Image")));
            this.check6.Location = new System.Drawing.Point(234, 94);
            this.check6.Name = "check6";
            this.check6.Size = new System.Drawing.Size(23, 18);
            this.check6.TabIndex = 26;
            this.check6.TabStop = false;
            this.check6.Visible = false;
            // 
            // check5
            // 
            this.check5.Image = ((System.Drawing.Image)(resources.GetObject("check5.Image")));
            this.check5.Location = new System.Drawing.Point(234, 78);
            this.check5.Name = "check5";
            this.check5.Size = new System.Drawing.Size(23, 18);
            this.check5.TabIndex = 25;
            this.check5.TabStop = false;
            this.check5.Visible = false;
            // 
            // check4
            // 
            this.check4.Image = ((System.Drawing.Image)(resources.GetObject("check4.Image")));
            this.check4.Location = new System.Drawing.Point(234, 62);
            this.check4.Name = "check4";
            this.check4.Size = new System.Drawing.Size(23, 18);
            this.check4.TabIndex = 24;
            this.check4.TabStop = false;
            this.check4.Visible = false;
            // 
            // check3
            // 
            this.check3.Image = ((System.Drawing.Image)(resources.GetObject("check3.Image")));
            this.check3.Location = new System.Drawing.Point(234, 45);
            this.check3.Name = "check3";
            this.check3.Size = new System.Drawing.Size(23, 18);
            this.check3.TabIndex = 23;
            this.check3.TabStop = false;
            this.check3.Visible = false;
            // 
            // check2
            // 
            this.check2.Image = ((System.Drawing.Image)(resources.GetObject("check2.Image")));
            this.check2.Location = new System.Drawing.Point(234, 29);
            this.check2.Name = "check2";
            this.check2.Size = new System.Drawing.Size(23, 18);
            this.check2.TabIndex = 22;
            this.check2.TabStop = false;
            this.check2.Visible = false;
            // 
            // check1
            // 
            this.check1.Image = ((System.Drawing.Image)(resources.GetObject("check1.Image")));
            this.check1.Location = new System.Drawing.Point(234, 13);
            this.check1.Name = "check1";
            this.check1.Size = new System.Drawing.Size(23, 18);
            this.check1.TabIndex = 21;
            this.check1.TabStop = false;
            this.check1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "1. Only numbers 1-12 are present.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 198);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(172, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "12. The hands are relatively joined.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "2. Only Arabic numbers are used.";
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(6, 374);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(98, 24);
            this.openButton.TabIndex = 19;
            this.openButton.Text = "&Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(110, 374);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(98, 24);
            this.saveButton.TabIndex = 20;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.trackBar1);
            this.splitContainer1.Panel1.Controls.Add(this.play_pause);
            this.splitContainer1.Panel1.Controls.Add(this.ClockDrawing);
            this.splitContainer1.Panel1.Controls.Add(this.partInfo);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(890, 472);
            this.splitContainer1.SplitterDistance = 377;
            this.splitContainer1.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(39, 440);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(333, 45);
            this.trackBar1.TabIndex = 6;
            // 
            // play_pause
            // 
            this.play_pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.play_pause.BackColor = System.Drawing.Color.White;
            this.play_pause.Image = global::Serialization.Properties.Resources.play;
            this.play_pause.Location = new System.Drawing.Point(6, 440);
            this.play_pause.Name = "play_pause";
            this.play_pause.Size = new System.Drawing.Size(34, 27);
            this.play_pause.TabIndex = 5;
            this.play_pause.UseVisualStyleBackColor = false;
            // 
            // ClockDrawing
            // 
            this.ClockDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ClockDrawing.BackColor = System.Drawing.Color.White;
            this.ClockDrawing.Location = new System.Drawing.Point(6, 100);
            this.ClockDrawing.MaximumSize = new System.Drawing.Size(2000, 2000);
            this.ClockDrawing.MinimumSize = new System.Drawing.Size(362, 306);
            this.ClockDrawing.Name = "ClockDrawing";
            this.ClockDrawing.Padding = new System.Windows.Forms.Padding(3);
            this.ClockDrawing.Size = new System.Drawing.Size(362, 337);
            this.ClockDrawing.TabIndex = 4;
            this.ClockDrawing.TabStop = false;
            this.ClockDrawing.SizeChanged += new System.EventHandler(this.ClockDrawing_SizeChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(505, 464);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.saveButton);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.openButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(497, 438);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(497, 438);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(3, 507);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(890, 121);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(8, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(113, 106);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Open A Folder";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // SerializationForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(898, 636);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.textBox1);
            this.Menu = this.MenuBar;
            this.Name = "SerializationForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Drawing Results";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.check13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check11)).EndInit();
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
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClockDrawing)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void ClockDrawing_SizeChanged(object sender, EventArgs e)
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
                Pen pen = new System.Drawing.Pen(Color.Black, 3);
                Bitmap pictureBoxBitmap = new System.Drawing.Bitmap(this.ClockDrawing.Width, this.ClockDrawing.Height);
                Graphics g2 = Graphics.FromImage(pictureBoxBitmap);
                g2.DrawEllipse(pen, 0, 0, pictureBoxBitmap.Width * 0.9f, pictureBoxBitmap.Width * 0.9f);
                this.ClockDrawing.Image = pictureBoxBitmap;                
                
                ic.Enabled = false;
                ic.Ink = loadedInk;
                ic.Enabled = true;

                this.ClockDrawing.Invalidate();
            }
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
                loadClock(Path.GetFullPath(openDialog.FileName));
                this.displayedFileName = openDialog.FileName.Substring(openDialog.FileName.LastIndexOf('\\') + 1);
                // Display Score
                dispScore(dicClocks[displayedFileName].ScoreBoard);
                
                // Display Participant information
                //genParInfo(extensionlessFilename);

                writeStrokes(dicClocks[displayedFileName].Ink);
                
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


        /// <summary>
        /// load a ISF file from a path
        /// </summary>
        /// <param name="path"></param>
        private void loadClock(string path)
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

                    // Get a version of the filename without an extension
                    // This will be used for saving the associated image
                    String extensionlessFilename = Path.GetFileNameWithoutExtension(filename);

                    // Get the extension of the file 
                    String extension = filename.Substring(filename.LastIndexOf('.'));                    

                    file = new FileStream(path + extensionlessFilename + "_score.csv", FileMode.OpenOrCreate, FileAccess.Read);
                    sr = new StreamReader(file);
                    String tempString = sr.ReadToEnd();

                    char[] delimiters = new char[] { ',', '\n' };

                    //declare a clock instace that store all the data read from the files
                    Clock tmpClock = new Clock();

                    // Read score from file
                    tmpClock.ScoreBoard = readScores(delimiters, tempString);

                    file = new FileStream(path + extensionlessFilename + "_air.csv", FileMode.OpenOrCreate, FileAccess.Read);
                    sr = new StreamReader(file);
                    tempString = sr.ReadToEnd();

                    file = new FileStream(path + extensionlessFilename + "_air.csv", FileMode.OpenOrCreate, FileAccess.Read);
                    //MediaPlayer.URL = filePath + extensionlessFilename + ".wmv";

                    // Read Airtime from file
                    tmpClock.AirTime = readAirtime(delimiters, tempString);

                    this.dicClocks.Add(filename, tmpClock);
                    
                    tmpClock.Ink = loadISF(myStream);
                }
                else
                {
                    // Throw an exception if a null pointer is returned for the stream
                    throw new IOException();
                }
            }
            catch (IOException ioe)
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


        private void writeStrokes(Ink loadedInk) 
		{
            Pen pen = new System.Drawing.Pen(Color.Black, 1);            							

            // convert the ink to a bitmap and put it on pictureBox
            Bitmap curStrokesBitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics g = Graphics.FromImage(curStrokesBitmap);
            Renderer r = new Renderer();
            loadedInk.Strokes.ScaleToRectangle(new Rectangle(-20, -10, this.pictureBox1.Width, this.pictureBox1.Height));
            loadedInk.Strokes.Scale(35, 35);
            Stroke stroke = loadedInk.Strokes[0];
            
            r.Draw(g, loadedInk.Strokes);
            g.DrawEllipse(pen, 0, 0, this.pictureBox1.Width - 8, this.pictureBox1.Width - 8);

            this.pictureBox1.Image = curStrokesBitmap;

            // draw the picture on main animated picture box
            Rectangle scaleRectangle = new System.Drawing.Rectangle(0, 0, this.ClockDrawing.Height, this.ClockDrawing.Height);
            loadedInk.Strokes.ScaleToRectangle(scaleRectangle);
            loadedInk.Strokes.Scale(25, 25);

            pen = new System.Drawing.Pen(Color.Black, 3);
            Bitmap pictureBoxBitmap = new System.Drawing.Bitmap(this.ClockDrawing.Width, this.ClockDrawing.Height);
            Graphics g2 = Graphics.FromImage(pictureBoxBitmap);
            g2.DrawEllipse(pen, 0, 0, pictureBoxBitmap.Width*0.9f, pictureBoxBitmap.Width*0.9f);
            this.ClockDrawing.Image = pictureBoxBitmap; 

            // temporarily disable the ink collector and swap ink objects
			ic.Enabled = false;
			ic.Ink = loadedInk;
			ic.Enabled = true;

			// Repaint the inkable region
            this.ClockDrawing.Invalidate();
                                    
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

        // Read each score from the file
        private int[] readScores(char[] delimiters, String tempString)
        {
            int i = 0, j = 0;
            String[] subScore = tempString.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            int[] tmpScores = new int[13];
            foreach (String subs in subScore)
            {
                if ((i > 1) && (i % 2 == 1))
                {
                    scoreboard[j] = int.Parse(subs);
                    tmpScores[j] = int.Parse(subs);
                    j++;
                }
                i++;
            }
            return tmpScores;
        }

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

        // Display each score
        private void dispScore(int[] scores)
        {
            int i = 0;

            clearScoreboard();

            foreach (int score in scoreboard)
            {
                if (/*(score == 1) &&*/ (i == 0))
                {
                    check1.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 1))
                {
                    check2.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 2))
                {
                    check3.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 3))
                {
                    check4.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 4))
                {
                    check5.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 5))
                {
                    check6.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 6))
                {
                    check7.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 7))
                {
                    check8.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 8))
                {
                    check9.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 9))
                {
                    check10.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 10))
                {
                    check11.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 11))
                {
                    check12.Visible = true;
                }
                if (/*(score == 1) &&*/ (i == 12))
                {
                    check13.Visible = true;
                }
                i++;
            }
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

    }
}
