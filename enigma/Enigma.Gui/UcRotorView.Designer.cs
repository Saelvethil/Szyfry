//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
namespace Enigma.Gui
{
	partial class UcRotorView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbRotatorPos = new System.Windows.Forms.TextBox();
			this.cbRotorPos = new System.Windows.Forms.ComboBox();
			this.numRotor = new System.Windows.Forms.NumericUpDown();
			this.cbRotor = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.numRotor)).BeginInit();
			this.SuspendLayout();
			// 
			// tbRotatorPos
			// 
			this.tbRotatorPos.Location = new System.Drawing.Point(52, 56);
			this.tbRotatorPos.Name = "tbRotatorPos";
			this.tbRotatorPos.ReadOnly = true;
			this.tbRotatorPos.Size = new System.Drawing.Size(42, 20);
			this.tbRotatorPos.TabIndex = 14;
			// 
			// cbRotorPos
			// 
			this.cbRotorPos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRotorPos.FormattingEnabled = true;
			this.cbRotorPos.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
			this.cbRotorPos.Location = new System.Drawing.Point(3, 56);
			this.cbRotorPos.Name = "cbRotorPos";
			this.cbRotorPos.Size = new System.Drawing.Size(43, 21);
			this.cbRotorPos.TabIndex = 13;
			this.cbRotorPos.SelectedIndexChanged += new System.EventHandler(this.startPositionChanged);
			// 
			// numRotor
			// 
			this.numRotor.Location = new System.Drawing.Point(3, 30);
			this.numRotor.Maximum = new decimal(new int[] {
            26,
            0,
            0,
            0});
			this.numRotor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRotor.Name = "numRotor";
			this.numRotor.Size = new System.Drawing.Size(91, 20);
			this.numRotor.TabIndex = 12;
			this.numRotor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRotor.ValueChanged += new System.EventHandler(this.ringPositionChanged);
			// 
			// cbRotor
			// 
			this.cbRotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRotor.DropDownWidth = 91;
			this.cbRotor.FormattingEnabled = true;
			this.cbRotor.Location = new System.Drawing.Point(3, 3);
			this.cbRotor.Name = "cbRotor";
			this.cbRotor.Size = new System.Drawing.Size(91, 21);
			this.cbRotor.TabIndex = 11;
			this.cbRotor.SelectedIndexChanged += new System.EventHandler(this.selectedRotorChanged);
			// 
			// UcRotorView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tbRotatorPos);
			this.Controls.Add(this.cbRotorPos);
			this.Controls.Add(this.numRotor);
			this.Controls.Add(this.cbRotor);
			this.Name = "UcRotorView";
			this.Size = new System.Drawing.Size(101, 82);
			((System.ComponentModel.ISupportInitialize)(this.numRotor)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox tbRotatorPos;
		public System.Windows.Forms.ComboBox cbRotorPos;
		public System.Windows.Forms.NumericUpDown numRotor;
		public System.Windows.Forms.ComboBox cbRotor;
	}
}
