//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Enigma.Core;

namespace Enigma.Gui
{
	public partial class    FormMain : Form
	{
		public EnigmaMachine myEnigmaMachine;
		public RotorMachine myRotorMachine;
		public PlugBoard myPlugBoard;
		public readonly FormLog myFormLog = new FormLog();

		public FormMain()
		{
			InitializeComponent();
			Init();
			
			foreach (Rotor r in myRotorMachine.Rotors.Values)
			{
				Wirnik1.AddRotor(r);
				Wirnik2.AddRotor(r);
				Wirnik3.AddRotor(r);
			}

			foreach (Rotor r in myRotorMachine.Ukws.Values)
			{
				Reflektor.Items.Add(r);
			}

			Wirnik1.RotorViewChanged += rotorViewChanged;
			Wirnik2.RotorViewChanged += rotorViewChanged;
			Wirnik3.RotorViewChanged += rotorViewChanged;

			Lacznica.PlugAdded += ucPlugBoardPlugAdded;
			Lacznica.PlugRemoved += ucPlugBoardPlugRemoved;

			Wirnik1.SelectedRotor = myRotorMachine.Rotors["I"];
			Wirnik2.SelectedRotor = myRotorMachine.Rotors["II"];
			Wirnik3.SelectedRotor = myRotorMachine.Rotors["III"];

			Reflektor.SelectedIndex = 1;
		}

		private void rotorViewChanged(Rotor r, int ringPosition, char startPosition)
		{
            tbInput.ReadOnly = (Wirnik1.SelectedRotor == Wirnik2.SelectedRotor || Wirnik2.SelectedRotor == Wirnik3.SelectedRotor || Wirnik3.SelectedRotor == Wirnik1.SelectedRotor);

			setUp(null, EventArgs.Empty);
		}

		private void ucPlugBoardPlugRemoved(char c)
		{
			myPlugBoard.Remove(c);
			setUp(null, null);
		}

		private void ucPlugBoardPlugAdded(char c1, char c2)
		{
			myPlugBoard.AddPlug(c1, c2);
			setUp(null, null);
		}

		public void Init()
		{
			myPlugBoard = new PlugBoard();
			myRotorMachine = new RotorMachine();
			//myRotorMachine.SetRotor(0, myEtw);
			//myRotorMachine.SetRotor(1, myRotors["I"]);
			//myRotorMachine.SetRotor(2, myRotors["II"]);
			//myRotorMachine.SetRotor(3, myRotors["III"]);
			//myRotorMachine.SetRotor(4, myUkws["B"]);
			//myRotorMachine.Init();

			myPlugBoard.Log += log;
			myRotorMachine.Log += log;
			
			myEnigmaMachine = new EnigmaMachine(myPlugBoard, myRotorMachine);
		}

		private void log(string s)
		{
			if (myFormLog.Visible)
			{
				myFormLog.Invoke(new Action<string>(logAsync), s);
			}
		}
		
		private void logAsync(object o)
		{
			myFormLog.Log((string)o);
		}

		private void setUp(object sender, EventArgs e)
		{
			Wirnik1.Reset();
			Wirnik2.Reset();
			Wirnik3.Reset();

			myRotorMachine.SetRotor(1, Wirnik1.SelectedRotor);
			myRotorMachine.SetRotor(2, Wirnik2.SelectedRotor);
			myRotorMachine.SetRotor(3, Wirnik3.SelectedRotor);
			myRotorMachine.SetRotor(4, (Rotor)Reflektor.SelectedItem);
			//myRotorMachine.Init();

			encode();
		}


		private void tbInput_TextChanged(object sender, EventArgs e)
		{
			setUp(sender, e);
		}

		private void encode()
		{
			tbOutput.Text = myEnigmaMachine.Encode(tbInput.Text);
		}

		private void butShowLog_Click(object sender, EventArgs e)
		{
			myFormLog.Visible = !myFormLog.Visible;
		}
	}
}
