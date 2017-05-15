//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Enigma.Gui
{
	public partial class FormLog : Form
	{
		private readonly Queue<string> myQueue = new Queue<string>();
		private readonly StringBuilder myStringBuilder = new StringBuilder();
		private readonly Timer myTimer = new Timer();
		private readonly Object myLockObject = new object();
		private bool myHasChanges;
		
		public FormLog()
		{
			InitializeComponent();
			myTimer.Enabled = true;
			myTimer.Interval = 1000;
			myTimer.Start();
			myTimer.Tick += timerTick;
		}

		public void Log(string s)
		{
			lock (myLockObject)
			{
				myQueue.Enqueue(s);
				myHasChanges = true;

				if (myQueue.Count > 100)
				{
					myQueue.Dequeue();
				}
			}
		}
		
		private void timerTick(object sender, EventArgs e)
		{
			if (myHasChanges)
			{
				tbLog.SuspendLayout();
				tbLog.Clear();
				myStringBuilder.Remove(0, myStringBuilder.Length);
				lock (myLockObject)
				{
					foreach (string log in myQueue.ToArray())
					{
						myStringBuilder.AppendLine(log);
					}
				}
				tbLog.Text = myStringBuilder.ToString();
				tbLog.ResumeLayout();
				myHasChanges = false;
			}
		}

		private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Visible = false;
		}
	}
}
