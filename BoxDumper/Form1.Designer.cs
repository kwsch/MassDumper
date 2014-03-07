namespace BoxDumper
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.B_OS = new System.Windows.Forms.Button();
            this.B_OKEY = new System.Windows.Forms.Button();
            this.B_OEKX = new System.Windows.Forms.Button();
            this.T_SAV = new System.Windows.Forms.TextBox();
            this.T_KEY = new System.Windows.Forms.TextBox();
            this.T_EKX = new System.Windows.Forms.TextBox();
            this.L_C = new System.Windows.Forms.Label();
            this.T_C = new System.Windows.Forms.TextBox();
            this.T_Dialog = new System.Windows.Forms.RichTextBox();
            this.B_DUMP = new System.Windows.Forms.Button();
            this.L_O = new System.Windows.Forms.Label();
            this.T_O = new System.Windows.Forms.TextBox();
            this.C_Format = new System.Windows.Forms.ComboBox();
            this.B_OutPath = new System.Windows.Forms.Button();
            this.T_OutPath = new System.Windows.Forms.TextBox();
            this.CHK_ALT = new System.Windows.Forms.CheckBox();
            this.TC = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.GB1 = new System.Windows.Forms.GroupBox();
            this.CHK_PK6 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.C_DStart = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CHK_ALT2 = new System.Windows.Forms.CheckBox();
            this.L_O2 = new System.Windows.Forms.Label();
            this.L_O1 = new System.Windows.Forms.Label();
            this.B_DumpKeyRange = new System.Windows.Forms.Button();
            this.T_O2 = new System.Windows.Forms.TextBox();
            this.T_O1 = new System.Windows.Forms.TextBox();
            this.C_End = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.C_Start = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.T_EKX2 = new System.Windows.Forms.TextBox();
            this.B_OEKX2 = new System.Windows.Forms.Button();
            this.T_SAV2 = new System.Windows.Forms.TextBox();
            this.B_OS2 = new System.Windows.Forms.Button();
            this.B_About = new System.Windows.Forms.Button();
            this.TC.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.GB1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_OS
            // 
            this.B_OS.Location = new System.Drawing.Point(14, 8);
            this.B_OS.Name = "B_OS";
            this.B_OS.Size = new System.Drawing.Size(96, 23);
            this.B_OS.TabIndex = 0;
            this.B_OS.Text = "Open Save File";
            this.B_OS.UseVisualStyleBackColor = true;
            this.B_OS.Click += new System.EventHandler(this.B_OpenBoxSave_Click);
            // 
            // B_OKEY
            // 
            this.B_OKEY.Location = new System.Drawing.Point(6, 42);
            this.B_OKEY.Name = "B_OKEY";
            this.B_OKEY.Size = new System.Drawing.Size(96, 23);
            this.B_OKEY.TabIndex = 1;
            this.B_OKEY.Text = "Open ConcatKey";
            this.B_OKEY.UseVisualStyleBackColor = true;
            this.B_OKEY.Click += new System.EventHandler(this.B_OpenBoxKey_Click);
            // 
            // B_OEKX
            // 
            this.B_OEKX.Location = new System.Drawing.Point(7, 14);
            this.B_OEKX.Name = "B_OEKX";
            this.B_OEKX.Size = new System.Drawing.Size(96, 23);
            this.B_OEKX.TabIndex = 2;
            this.B_OEKX.Text = "Open Blank EKX";
            this.B_OEKX.UseVisualStyleBackColor = true;
            this.B_OEKX.Click += new System.EventHandler(this.B_OpenBlank_Click);
            // 
            // T_SAV
            // 
            this.T_SAV.Location = new System.Drawing.Point(116, 10);
            this.T_SAV.Name = "T_SAV";
            this.T_SAV.ReadOnly = true;
            this.T_SAV.Size = new System.Drawing.Size(258, 20);
            this.T_SAV.TabIndex = 3;
            this.T_SAV.TextChanged += new System.EventHandler(this.enableT1GB);
            // 
            // T_KEY
            // 
            this.T_KEY.Location = new System.Drawing.Point(108, 44);
            this.T_KEY.Name = "T_KEY";
            this.T_KEY.ReadOnly = true;
            this.T_KEY.Size = new System.Drawing.Size(258, 20);
            this.T_KEY.TabIndex = 4;
            this.T_KEY.TextChanged += new System.EventHandler(this.switchkey);
            // 
            // T_EKX
            // 
            this.T_EKX.Location = new System.Drawing.Point(108, 16);
            this.T_EKX.Name = "T_EKX";
            this.T_EKX.ReadOnly = true;
            this.T_EKX.Size = new System.Drawing.Size(258, 20);
            this.T_EKX.TabIndex = 5;
            this.T_EKX.TextChanged += new System.EventHandler(this.toggledump);
            // 
            // L_C
            // 
            this.L_C.AutoSize = true;
            this.L_C.Location = new System.Drawing.Point(3, 103);
            this.L_C.Name = "L_C";
            this.L_C.Size = new System.Drawing.Size(59, 13);
            this.L_C.TabIndex = 6;
            this.L_C.Text = "Box Count:";
            // 
            // T_C
            // 
            this.T_C.Location = new System.Drawing.Point(68, 99);
            this.T_C.Name = "T_C";
            this.T_C.ReadOnly = true;
            this.T_C.Size = new System.Drawing.Size(26, 20);
            this.T_C.TabIndex = 7;
            // 
            // T_Dialog
            // 
            this.T_Dialog.Location = new System.Drawing.Point(12, 218);
            this.T_Dialog.Name = "T_Dialog";
            this.T_Dialog.ReadOnly = true;
            this.T_Dialog.Size = new System.Drawing.Size(396, 243);
            this.T_Dialog.TabIndex = 8;
            this.T_Dialog.Text = "";
            this.T_Dialog.WordWrap = false;
            // 
            // B_DUMP
            // 
            this.B_DUMP.Enabled = false;
            this.B_DUMP.Location = new System.Drawing.Point(322, 99);
            this.B_DUMP.Name = "B_DUMP";
            this.B_DUMP.Size = new System.Drawing.Size(44, 20);
            this.B_DUMP.TabIndex = 9;
            this.B_DUMP.Text = "Dump";
            this.B_DUMP.UseVisualStyleBackColor = true;
            this.B_DUMP.Click += new System.EventHandler(this.B_DUMP_Click);
            // 
            // L_O
            // 
            this.L_O.AutoSize = true;
            this.L_O.Location = new System.Drawing.Point(120, 124);
            this.L_O.Name = "L_O";
            this.L_O.Size = new System.Drawing.Size(38, 13);
            this.L_O.TabIndex = 10;
            this.L_O.Text = "Offset:";
            this.L_O.Visible = false;
            // 
            // T_O
            // 
            this.T_O.Enabled = false;
            this.T_O.Location = new System.Drawing.Point(164, 120);
            this.T_O.Name = "T_O";
            this.T_O.Size = new System.Drawing.Size(43, 20);
            this.T_O.TabIndex = 11;
            this.T_O.TextChanged += new System.EventHandler(this.toggledump);
            // 
            // C_Format
            // 
            this.C_Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.C_Format.Enabled = false;
            this.C_Format.FormattingEnabled = true;
            this.C_Format.Items.AddRange(new object[] {
            "Default",
            "Reddit",
            "TSV",
            ".csv"});
            this.C_Format.Location = new System.Drawing.Point(251, 99);
            this.C_Format.Name = "C_Format";
            this.C_Format.Size = new System.Drawing.Size(65, 21);
            this.C_Format.TabIndex = 12;
            this.C_Format.SelectedIndexChanged += new System.EventHandler(this.switchmode);
            // 
            // B_OutPath
            // 
            this.B_OutPath.Location = new System.Drawing.Point(6, 72);
            this.B_OutPath.Name = "B_OutPath";
            this.B_OutPath.Size = new System.Drawing.Size(96, 23);
            this.B_OutPath.TabIndex = 13;
            this.B_OutPath.Text = "Output Path:";
            this.B_OutPath.UseVisualStyleBackColor = true;
            this.B_OutPath.Visible = false;
            this.B_OutPath.Click += new System.EventHandler(this.B_ChangeOutputFolder_Click);
            // 
            // T_OutPath
            // 
            this.T_OutPath.Location = new System.Drawing.Point(108, 72);
            this.T_OutPath.Name = "T_OutPath";
            this.T_OutPath.ReadOnly = true;
            this.T_OutPath.Size = new System.Drawing.Size(258, 20);
            this.T_OutPath.TabIndex = 14;
            this.T_OutPath.Visible = false;
            // 
            // CHK_ALT
            // 
            this.CHK_ALT.AutoSize = true;
            this.CHK_ALT.Checked = true;
            this.CHK_ALT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_ALT.Enabled = false;
            this.CHK_ALT.Location = new System.Drawing.Point(218, 102);
            this.CHK_ALT.Name = "CHK_ALT";
            this.CHK_ALT.Size = new System.Drawing.Size(33, 17);
            this.CHK_ALT.TabIndex = 15;
            this.CHK_ALT.Text = "A";
            this.CHK_ALT.UseVisualStyleBackColor = true;
            this.CHK_ALT.Click += new System.EventHandler(this.toggledump);
            // 
            // TC
            // 
            this.TC.Controls.Add(this.tabPage1);
            this.TC.Controls.Add(this.tabPage2);
            this.TC.Location = new System.Drawing.Point(12, 7);
            this.TC.Name = "TC";
            this.TC.SelectedIndex = 0;
            this.TC.Size = new System.Drawing.Size(396, 205);
            this.TC.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.T_SAV);
            this.tabPage1.Controls.Add(this.B_OS);
            this.tabPage1.Controls.Add(this.GB1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(388, 179);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Dump Boxes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // GB1
            // 
            this.GB1.Controls.Add(this.T_O);
            this.GB1.Controls.Add(this.CHK_PK6);
            this.GB1.Controls.Add(this.label6);
            this.GB1.Controls.Add(this.C_DStart);
            this.GB1.Controls.Add(this.B_OKEY);
            this.GB1.Controls.Add(this.CHK_ALT);
            this.GB1.Controls.Add(this.B_OEKX);
            this.GB1.Controls.Add(this.T_OutPath);
            this.GB1.Controls.Add(this.T_KEY);
            this.GB1.Controls.Add(this.B_OutPath);
            this.GB1.Controls.Add(this.T_EKX);
            this.GB1.Controls.Add(this.C_Format);
            this.GB1.Controls.Add(this.L_C);
            this.GB1.Controls.Add(this.T_C);
            this.GB1.Controls.Add(this.L_O);
            this.GB1.Controls.Add(this.B_DUMP);
            this.GB1.Enabled = false;
            this.GB1.Location = new System.Drawing.Point(7, 30);
            this.GB1.Name = "GB1";
            this.GB1.Size = new System.Drawing.Size(374, 144);
            this.GB1.TabIndex = 21;
            this.GB1.TabStop = false;
            // 
            // CHK_PK6
            // 
            this.CHK_PK6.AutoSize = true;
            this.CHK_PK6.Checked = true;
            this.CHK_PK6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_PK6.Location = new System.Drawing.Point(263, 124);
            this.CHK_PK6.Name = "CHK_PK6";
            this.CHK_PK6.Size = new System.Drawing.Size(46, 17);
            this.CHK_PK6.TabIndex = 16;
            this.CHK_PK6.Text = "PK6";
            this.CHK_PK6.UseVisualStyleBackColor = true;
            this.CHK_PK6.Visible = false;
            this.CHK_PK6.CheckedChanged += new System.EventHandler(this.changecheck);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(100, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Key Starts:";
            // 
            // C_DStart
            // 
            this.C_DStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.C_DStart.Enabled = false;
            this.C_DStart.FormattingEnabled = true;
            this.C_DStart.Items.AddRange(new object[] {
            "1"});
            this.C_DStart.Location = new System.Drawing.Point(164, 99);
            this.C_DStart.Name = "C_DStart";
            this.C_DStart.Size = new System.Drawing.Size(43, 21);
            this.C_DStart.TabIndex = 17;
            this.C_DStart.SelectedIndexChanged += new System.EventHandler(this.changestart);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.T_SAV2);
            this.tabPage2.Controls.Add(this.B_OS2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(388, 179);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Dump KeyRange";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CHK_ALT2);
            this.groupBox2.Controls.Add(this.L_O2);
            this.groupBox2.Controls.Add(this.L_O1);
            this.groupBox2.Controls.Add(this.B_DumpKeyRange);
            this.groupBox2.Controls.Add(this.T_O2);
            this.groupBox2.Controls.Add(this.T_O1);
            this.groupBox2.Controls.Add(this.C_End);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.C_Start);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.T_EKX2);
            this.groupBox2.Controls.Add(this.B_OEKX2);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(7, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(374, 144);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // CHK_ALT2
            // 
            this.CHK_ALT2.AutoSize = true;
            this.CHK_ALT2.Checked = true;
            this.CHK_ALT2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_ALT2.Enabled = false;
            this.CHK_ALT2.Location = new System.Drawing.Point(304, 121);
            this.CHK_ALT2.Name = "CHK_ALT2";
            this.CHK_ALT2.Size = new System.Drawing.Size(33, 17);
            this.CHK_ALT2.TabIndex = 17;
            this.CHK_ALT2.Text = "A";
            this.CHK_ALT2.UseVisualStyleBackColor = true;
            this.CHK_ALT2.TextChanged += new System.EventHandler(this.updaterange);
            this.CHK_ALT2.Click += new System.EventHandler(this.updaterange);
            // 
            // L_O2
            // 
            this.L_O2.AutoSize = true;
            this.L_O2.Location = new System.Drawing.Point(146, 98);
            this.L_O2.Name = "L_O2";
            this.L_O2.Size = new System.Drawing.Size(60, 13);
            this.L_O2.TabIndex = 14;
            this.L_O2.Text = "End Offset:";
            this.L_O2.Visible = false;
            // 
            // L_O1
            // 
            this.L_O1.AutoSize = true;
            this.L_O1.Location = new System.Drawing.Point(143, 65);
            this.L_O1.Name = "L_O1";
            this.L_O1.Size = new System.Drawing.Size(63, 13);
            this.L_O1.TabIndex = 13;
            this.L_O1.Text = "Start Offset:";
            this.L_O1.Visible = false;
            // 
            // B_DumpKeyRange
            // 
            this.B_DumpKeyRange.Enabled = false;
            this.B_DumpKeyRange.Location = new System.Drawing.Point(282, 62);
            this.B_DumpKeyRange.Name = "B_DumpKeyRange";
            this.B_DumpKeyRange.Size = new System.Drawing.Size(75, 54);
            this.B_DumpKeyRange.TabIndex = 12;
            this.B_DumpKeyRange.Text = "Dump Key for Box Range";
            this.B_DumpKeyRange.UseVisualStyleBackColor = true;
            this.B_DumpKeyRange.Click += new System.EventHandler(this.B_DumpKeyRange_Click);
            // 
            // T_O2
            // 
            this.T_O2.Location = new System.Drawing.Point(212, 95);
            this.T_O2.Name = "T_O2";
            this.T_O2.Size = new System.Drawing.Size(64, 20);
            this.T_O2.TabIndex = 11;
            this.T_O2.Visible = false;
            this.T_O2.TextChanged += new System.EventHandler(this.togglekeydump);
            // 
            // T_O1
            // 
            this.T_O1.Location = new System.Drawing.Point(212, 62);
            this.T_O1.Name = "T_O1";
            this.T_O1.Size = new System.Drawing.Size(64, 20);
            this.T_O1.TabIndex = 10;
            this.T_O1.Visible = false;
            this.T_O1.TextChanged += new System.EventHandler(this.togglekeydump);
            // 
            // C_End
            // 
            this.C_End.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.C_End.FormattingEnabled = true;
            this.C_End.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.C_End.Location = new System.Drawing.Point(74, 95);
            this.C_End.Name = "C_End";
            this.C_End.Size = new System.Drawing.Size(43, 21);
            this.C_End.TabIndex = 9;
            this.C_End.SelectedIndexChanged += new System.EventHandler(this.updaterange);
            this.C_End.TextChanged += new System.EventHandler(this.updaterange);
            this.C_End.Click += new System.EventHandler(this.updaterange);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "End Box:";
            // 
            // C_Start
            // 
            this.C_Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.C_Start.FormattingEnabled = true;
            this.C_Start.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.C_Start.Location = new System.Drawing.Point(74, 62);
            this.C_Start.Name = "C_Start";
            this.C_Start.Size = new System.Drawing.Size(43, 21);
            this.C_Start.TabIndex = 7;
            this.C_Start.SelectedIndexChanged += new System.EventHandler(this.updaterange);
            this.C_Start.TextChanged += new System.EventHandler(this.updaterange);
            this.C_Start.Click += new System.EventHandler(this.updaterange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Start Box:";
            // 
            // T_EKX2
            // 
            this.T_EKX2.Location = new System.Drawing.Point(108, 16);
            this.T_EKX2.Name = "T_EKX2";
            this.T_EKX2.ReadOnly = true;
            this.T_EKX2.Size = new System.Drawing.Size(258, 20);
            this.T_EKX2.TabIndex = 5;
            // 
            // B_OEKX2
            // 
            this.B_OEKX2.Location = new System.Drawing.Point(7, 14);
            this.B_OEKX2.Name = "B_OEKX2";
            this.B_OEKX2.Size = new System.Drawing.Size(96, 23);
            this.B_OEKX2.TabIndex = 3;
            this.B_OEKX2.Text = "Open Blank EKX";
            this.B_OEKX2.UseVisualStyleBackColor = true;
            this.B_OEKX2.Click += new System.EventHandler(this.B_OpenBlank_Click);
            // 
            // T_SAV2
            // 
            this.T_SAV2.Location = new System.Drawing.Point(116, 10);
            this.T_SAV2.Name = "T_SAV2";
            this.T_SAV2.ReadOnly = true;
            this.T_SAV2.Size = new System.Drawing.Size(258, 20);
            this.T_SAV2.TabIndex = 19;
            this.T_SAV2.TextChanged += new System.EventHandler(this.enableT2GB);
            // 
            // B_OS2
            // 
            this.B_OS2.Location = new System.Drawing.Point(14, 8);
            this.B_OS2.Name = "B_OS2";
            this.B_OS2.Size = new System.Drawing.Size(96, 23);
            this.B_OS2.TabIndex = 18;
            this.B_OS2.Text = "Open Save File";
            this.B_OS2.UseVisualStyleBackColor = true;
            this.B_OS2.Click += new System.EventHandler(this.B_OS2_Click);
            // 
            // B_About
            // 
            this.B_About.Location = new System.Drawing.Point(383, 6);
            this.B_About.Name = "B_About";
            this.B_About.Size = new System.Drawing.Size(23, 21);
            this.B_About.TabIndex = 19;
            this.B_About.Text = "?";
            this.B_About.UseVisualStyleBackColor = true;
            this.B_About.Click += new System.EventHandler(this.B_About_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 470);
            this.Controls.Add(this.B_About);
            this.Controls.Add(this.TC);
            this.Controls.Add(this.T_Dialog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Mass Dumper";
            this.TC.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.GB1.ResumeLayout(false);
            this.GB1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_OS;
        private System.Windows.Forms.Button B_OKEY;
        private System.Windows.Forms.Button B_OEKX;
        private System.Windows.Forms.TextBox T_SAV;
        private System.Windows.Forms.TextBox T_KEY;
        private System.Windows.Forms.TextBox T_EKX;
        private System.Windows.Forms.Label L_C;
        private System.Windows.Forms.TextBox T_C;
        private System.Windows.Forms.RichTextBox T_Dialog;
        private System.Windows.Forms.Button B_DUMP;
        private System.Windows.Forms.Label L_O;
        private System.Windows.Forms.TextBox T_O;
        private System.Windows.Forms.ComboBox C_Format;
        private System.Windows.Forms.Button B_OutPath;
        private System.Windows.Forms.TextBox T_OutPath;
        private System.Windows.Forms.CheckBox CHK_ALT;
        private System.Windows.Forms.TabControl TC;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox CHK_PK6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label L_O2;
        private System.Windows.Forms.Label L_O1;
        private System.Windows.Forms.Button B_DumpKeyRange;
        private System.Windows.Forms.TextBox T_O2;
        private System.Windows.Forms.TextBox T_O1;
        private System.Windows.Forms.ComboBox C_End;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox C_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox T_EKX2;
        private System.Windows.Forms.Button B_OEKX2;
        private System.Windows.Forms.CheckBox CHK_ALT2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox C_DStart;
        private System.Windows.Forms.Button B_About;
        private System.Windows.Forms.TextBox T_SAV2;
        private System.Windows.Forms.Button B_OS2;
        private System.Windows.Forms.GroupBox GB1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

