//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
namespace Enigma.Gui
{
    partial class FormMain
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
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.butShowLog = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Reflektor = new System.Windows.Forms.ComboBox();
            this.gbPlugBoard = new System.Windows.Forms.GroupBox();
            this.gpInput = new System.Windows.Forms.GroupBox();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.Lacznica = new Enigma.Gui.UcPlugBoard();
            this.Wirnik3 = new Enigma.Gui.UcRotorView();
            this.Wirnik2 = new Enigma.Gui.UcRotorView();
            this.Wirnik1 = new Enigma.Gui.UcRotorView();
            this.gbSettings.SuspendLayout();
            this.gbPlugBoard.SuspendLayout();
            this.gpInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSettings
            // 
            this.gbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSettings.Controls.Add(this.butShowLog);
            this.gbSettings.Controls.Add(this.label7);
            this.gbSettings.Controls.Add(this.label6);
            this.gbSettings.Controls.Add(this.label5);
            this.gbSettings.Controls.Add(this.label4);
            this.gbSettings.Controls.Add(this.label3);
            this.gbSettings.Controls.Add(this.label2);
            this.gbSettings.Controls.Add(this.label1);
            this.gbSettings.Controls.Add(this.Wirnik3);
            this.gbSettings.Controls.Add(this.Wirnik2);
            this.gbSettings.Controls.Add(this.Wirnik1);
            this.gbSettings.Controls.Add(this.Reflektor);
            this.gbSettings.Location = new System.Drawing.Point(12, 12);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(715, 129);
            this.gbSettings.TabIndex = 0;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Ustawienia";
            // 
            // butShowLog
            // 
            this.butShowLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.butShowLog.Location = new System.Drawing.Point(597, 87);
            this.butShowLog.Name = "butShowLog";
            this.butShowLog.Size = new System.Drawing.Size(75, 23);
            this.butShowLog.TabIndex = 23;
            this.butShowLog.Text = "Wyświetl log";
            this.butShowLog.UseVisualStyleBackColor = true;
            this.butShowLog.Click += new System.EventHandler(this.butShowLog_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Początkowa pozycja wirników";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Ustawienie wirnika";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Wirnik";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Reflektor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(276, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Wirnik 3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(386, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Wirnik 2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(490, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Wirnik 1";
            // 
            // Reflektor
            // 
            this.Reflektor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Reflektor.FormattingEnabled = true;
            this.Reflektor.Location = new System.Drawing.Point(172, 33);
            this.Reflektor.Name = "Reflektor";
            this.Reflektor.Size = new System.Drawing.Size(91, 21);
            this.Reflektor.TabIndex = 3;
            this.Reflektor.SelectedIndexChanged += new System.EventHandler(this.setUp);
            // 
            // gbPlugBoard
            // 
            this.gbPlugBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPlugBoard.Controls.Add(this.Lacznica);
            this.gbPlugBoard.Location = new System.Drawing.Point(12, 147);
            this.gbPlugBoard.Name = "gbPlugBoard";
            this.gbPlugBoard.Size = new System.Drawing.Size(715, 145);
            this.gbPlugBoard.TabIndex = 1;
            this.gbPlugBoard.TabStop = false;
            this.gbPlugBoard.Text = "Łącznica";
            // 
            // gpInput
            // 
            this.gpInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpInput.Controls.Add(this.tbOutput);
            this.gpInput.Controls.Add(this.tbInput);
            this.gpInput.Location = new System.Drawing.Point(7, 298);
            this.gpInput.Name = "gpInput";
            this.gpInput.Size = new System.Drawing.Size(720, 19);
            this.gpInput.TabIndex = 2;
            this.gpInput.TabStop = false;
            this.gpInput.Text = "Wiadomość";
            this.gpInput.Visible = false;
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Location = new System.Drawing.Point(405, 19);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(309, 0);
            this.tbOutput.TabIndex = 1;
            this.tbOutput.Visible = false;
            // 
            // tbInput
            // 
            this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInput.Location = new System.Drawing.Point(6, 19);
            this.tbInput.Multiline = true;
            this.tbInput.Name = "tbInput";
            this.tbInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInput.Size = new System.Drawing.Size(381, 0);
            this.tbInput.TabIndex = 0;
            this.tbInput.Visible = false;
            this.tbInput.TextChanged += new System.EventHandler(this.tbInput_TextChanged);
            // 
            // Lacznica
            // 
            this.Lacznica.Location = new System.Drawing.Point(6, 19);
            this.Lacznica.Name = "Lacznica";
            this.Lacznica.Size = new System.Drawing.Size(703, 120);
            this.Lacznica.TabIndex = 0;
            // 
            // Wirnik3
            // 
            this.Wirnik3.Location = new System.Drawing.Point(276, 30);
            this.Wirnik3.Name = "Wirnik3";
            this.Wirnik3.SelectedRotor = null;
            this.Wirnik3.Size = new System.Drawing.Size(101, 82);
            this.Wirnik3.TabIndex = 15;
            // 
            // Wirnik2
            // 
            this.Wirnik2.Location = new System.Drawing.Point(386, 30);
            this.Wirnik2.Name = "Wirnik2";
            this.Wirnik2.SelectedRotor = null;
            this.Wirnik2.Size = new System.Drawing.Size(101, 82);
            this.Wirnik2.TabIndex = 14;
            // 
            // Wirnik1
            // 
            this.Wirnik1.Location = new System.Drawing.Point(490, 30);
            this.Wirnik1.Name = "Wirnik1";
            this.Wirnik1.SelectedRotor = null;
            this.Wirnik1.Size = new System.Drawing.Size(101, 82);
            this.Wirnik1.TabIndex = 13;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(805, 329);
            this.Controls.Add(this.gpInput);
            this.Controls.Add(this.gbPlugBoard);
            this.Controls.Add(this.gbSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enigma";
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gbPlugBoard.ResumeLayout(false);
            this.gpInput.ResumeLayout(false);
            this.gpInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.GroupBox gpInput;
		private System.Windows.Forms.GroupBox gbPlugBoard;
        public System.Windows.Forms.ComboBox Reflektor;
		public UcPlugBoard Lacznica;
		public UcRotorView Wirnik3;
		public UcRotorView Wirnik2;
		public UcRotorView Wirnik1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button butShowLog;
        public System.Windows.Forms.TextBox tbOutput;
        public System.Windows.Forms.TextBox tbInput;
    }
}

