namespace AudioVisualizer
{
    partial class ConfigWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigWindow));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudRectHeight = new System.Windows.Forms.NumericUpDown();
            this.nudRectSpace = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudColSpace = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudColCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnColorL = new System.Windows.Forms.Button();
            this.btnColorH = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnColorBack = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbFftSize = new System.Windows.Forms.ComboBox();
            this.cbVisual = new System.Windows.Forms.ComboBox();
            this.tbGain = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbComPorts = new System.Windows.Forms.ComboBox();
            this.btnExtDrawerStop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbExtDrawerType = new System.Windows.Forms.ComboBox();
            this.btnExtDrawerSet = new System.Windows.Forms.Button();
            this.nudUpdateSpeed = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbGain)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(189, 12);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 21);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "Set";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.BtnStartStop_Click);
            // 
            // cbDevice
            // 
            this.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Location = new System.Drawing.Point(12, 12);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(171, 21);
            this.cbDevice.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "rect height";
            // 
            // nudRectHeight
            // 
            this.nudRectHeight.Location = new System.Drawing.Point(79, 22);
            this.nudRectHeight.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudRectHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRectHeight.Name = "nudRectHeight";
            this.nudRectHeight.Size = new System.Drawing.Size(59, 20);
            this.nudRectHeight.TabIndex = 3;
            this.nudRectHeight.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudRectSpace
            // 
            this.nudRectSpace.Location = new System.Drawing.Point(79, 48);
            this.nudRectSpace.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudRectSpace.Name = "nudRectSpace";
            this.nudRectSpace.Size = new System.Drawing.Size(59, 20);
            this.nudRectSpace.TabIndex = 5;
            this.nudRectSpace.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "rect space";
            // 
            // nudColSpace
            // 
            this.nudColSpace.Location = new System.Drawing.Point(79, 100);
            this.nudColSpace.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudColSpace.Name = "nudColSpace";
            this.nudColSpace.Size = new System.Drawing.Size(59, 20);
            this.nudColSpace.TabIndex = 9;
            this.nudColSpace.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "column space";
            // 
            // nudColCount
            // 
            this.nudColCount.Location = new System.Drawing.Point(79, 74);
            this.nudColCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudColCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColCount.Name = "nudColCount";
            this.nudColCount.Size = new System.Drawing.Size(59, 20);
            this.nudColCount.TabIndex = 7;
            this.nudColCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "column width";
            // 
            // btnColorL
            // 
            this.btnColorL.Location = new System.Drawing.Point(6, 19);
            this.btnColorL.Name = "btnColorL";
            this.btnColorL.Size = new System.Drawing.Size(75, 23);
            this.btnColorL.TabIndex = 12;
            this.btnColorL.Text = "Low";
            this.btnColorL.UseVisualStyleBackColor = true;
            this.btnColorL.Click += new System.EventHandler(this.BtnColorL_Click);
            // 
            // btnColorH
            // 
            this.btnColorH.Location = new System.Drawing.Point(87, 19);
            this.btnColorH.Name = "btnColorH";
            this.btnColorH.Size = new System.Drawing.Size(75, 23);
            this.btnColorH.TabIndex = 14;
            this.btnColorH.Text = "High";
            this.btnColorH.UseVisualStyleBackColor = true;
            this.btnColorH.Click += new System.EventHandler(this.BtnColorH_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnColorBack);
            this.groupBox1.Controls.Add(this.btnColorH);
            this.groupBox1.Controls.Add(this.btnColorL);
            this.groupBox1.Location = new System.Drawing.Point(218, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 79);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color edit";
            // 
            // btnColorBack
            // 
            this.btnColorBack.Location = new System.Drawing.Point(6, 48);
            this.btnColorBack.Name = "btnColorBack";
            this.btnColorBack.Size = new System.Drawing.Size(75, 23);
            this.btnColorBack.TabIndex = 15;
            this.btnColorBack.Text = "Background";
            this.btnColorBack.UseVisualStyleBackColor = true;
            this.btnColorBack.Click += new System.EventHandler(this.BtnColorBack_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudRectHeight);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nudColSpace);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nudRectSpace);
            this.groupBox2.Controls.Add(this.nudColCount);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 131);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "size edit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(270, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "FFT count points:";
            // 
            // cbFftSize
            // 
            this.cbFftSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFftSize.FormattingEnabled = true;
            this.cbFftSize.Location = new System.Drawing.Point(363, 12);
            this.cbFftSize.Name = "cbFftSize";
            this.cbFftSize.Size = new System.Drawing.Size(94, 21);
            this.cbFftSize.TabIndex = 18;
            // 
            // cbVisual
            // 
            this.cbVisual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVisual.FormattingEnabled = true;
            this.cbVisual.Location = new System.Drawing.Point(12, 188);
            this.cbVisual.Name = "cbVisual";
            this.cbVisual.Size = new System.Drawing.Size(121, 21);
            this.cbVisual.TabIndex = 19;
            // 
            // tbGain
            // 
            this.tbGain.AutoSize = false;
            this.tbGain.Location = new System.Drawing.Point(256, 189);
            this.tbGain.Maximum = 100;
            this.tbGain.Minimum = 10;
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(136, 20);
            this.tbGain.TabIndex = 20;
            this.tbGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbGain.Value = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(221, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Gain";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbComPorts);
            this.groupBox3.Controls.Add(this.btnExtDrawerStop);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cbExtDrawerType);
            this.groupBox3.Controls.Add(this.btnExtDrawerSet);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(398, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 131);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "External Drawer";
            // 
            // cbComPorts
            // 
            this.cbComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComPorts.FormattingEnabled = true;
            this.cbComPorts.Location = new System.Drawing.Point(39, 48);
            this.cbComPorts.Name = "cbComPorts";
            this.cbComPorts.Size = new System.Drawing.Size(100, 21);
            this.cbComPorts.TabIndex = 28;
            // 
            // btnExtDrawerStop
            // 
            this.btnExtDrawerStop.Location = new System.Drawing.Point(6, 102);
            this.btnExtDrawerStop.Name = "btnExtDrawerStop";
            this.btnExtDrawerStop.Size = new System.Drawing.Size(75, 23);
            this.btnExtDrawerStop.TabIndex = 27;
            this.btnExtDrawerStop.Text = "stop";
            this.btnExtDrawerStop.UseVisualStyleBackColor = true;
            this.btnExtDrawerStop.Click += new System.EventHandler(this.BtnExtDrawerStop_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Port";
            // 
            // cbExtDrawerType
            // 
            this.cbExtDrawerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExtDrawerType.FormattingEnabled = true;
            this.cbExtDrawerType.Location = new System.Drawing.Point(39, 19);
            this.cbExtDrawerType.Name = "cbExtDrawerType";
            this.cbExtDrawerType.Size = new System.Drawing.Size(100, 21);
            this.cbExtDrawerType.TabIndex = 22;
            // 
            // btnExtDrawerSet
            // 
            this.btnExtDrawerSet.Location = new System.Drawing.Point(93, 102);
            this.btnExtDrawerSet.Name = "btnExtDrawerSet";
            this.btnExtDrawerSet.Size = new System.Drawing.Size(75, 23);
            this.btnExtDrawerSet.TabIndex = 0;
            this.btnExtDrawerSet.Text = "Set";
            this.btnExtDrawerSet.UseVisualStyleBackColor = true;
            this.btnExtDrawerSet.Click += new System.EventHandler(this.BtnExtDrawerSet_Click);
            // 
            // nudUpdateSpeed
            // 
            this.nudUpdateSpeed.Location = new System.Drawing.Point(295, 136);
            this.nudUpdateSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudUpdateSpeed.Name = "nudUpdateSpeed";
            this.nudUpdateSpeed.Size = new System.Drawing.Size(59, 20);
            this.nudUpdateSpeed.TabIndex = 10;
            this.nudUpdateSpeed.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "update msec";
            // 
            // ConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 215);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.nudUpdateSpeed);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbGain);
            this.Controls.Add(this.cbVisual);
            this.Controls.Add(this.cbFftSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbDevice);
            this.Controls.Add(this.btnStartStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigWindow";
            this.Text = "Configs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudRectHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRectSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbGain)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudRectHeight;
        private System.Windows.Forms.NumericUpDown nudRectSpace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudColSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudColCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnColorL;
        private System.Windows.Forms.Button btnColorH;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnColorBack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbFftSize;
        private System.Windows.Forms.ComboBox cbVisual;
        private System.Windows.Forms.TrackBar tbGain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbExtDrawerType;
        private System.Windows.Forms.Button btnExtDrawerSet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExtDrawerStop;
        private System.Windows.Forms.ComboBox cbComPorts;
        private System.Windows.Forms.NumericUpDown nudUpdateSpeed;
        private System.Windows.Forms.Label label9;
    }
}