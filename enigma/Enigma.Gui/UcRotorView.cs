//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Windows.Forms;
using Enigma.Core;

namespace Enigma.Gui
{
	public partial class UcRotorView : UserControl
	{
		public delegate void OnRotorViewChanged(Rotor r, int ringPosition, char startPosition);
		public event OnRotorViewChanged RotorViewChanged;

		public UcRotorView()
		{
			InitializeComponent();
			cbRotorPos.SelectedIndex = 0;
		}

		public void AddRotor(Rotor r)
		{
			cbRotor.Items.Add(r);
			r.PositionChanged += positionChanged;

			if (cbRotor.SelectedIndex < 0)
			{
				cbRotor.SelectedIndex = 0;
			}
		}

		public Rotor SelectedRotor
		{
			set { cbRotor.SelectedItem = value; }
			get { return (Rotor)cbRotor.SelectedItem; }
		}

		public void Reset()
		{
			if (cbRotor.SelectedItem != null)
			{
				Rotor r = (Rotor)cbRotor.SelectedItem;
				r.SetRingPosition((int)numRotor.Value - 1);
				r.SetRotorPosition(cbRotorPos.SelectedIndex);
			}
			tbRotatorPos.Text = cbRotorPos.Text;
		}

		private void positionChanged(Rotor r, char character)
		{
			if (r.Name.Equals(((Rotor)cbRotor.SelectedItem).Name))
			{
				tbRotatorPos.Text = character.ToString();
			}
		}

		private void selectedRotorChanged(object sender, EventArgs e)
		{
			Rotor r = (cbRotor.SelectedItem != null) ? (Rotor)cbRotor.SelectedItem : null;
			if (RotorViewChanged != null)
			{
				RotorViewChanged(r, (int)numRotor.Value - 1, cbRotorPos.Text[0]);
			}
		}

		private void ringPositionChanged(object sender, EventArgs e)
		{
			if (cbRotor.SelectedItem != null)
			{
				((Rotor)cbRotor.SelectedItem).SetRingPosition((int)numRotor.Value - 1);
				selectedRotorChanged(sender, e);
			}
		}

		private void startPositionChanged(object sender, EventArgs e)
		{
			if (cbRotor.SelectedItem != null)
			{
				((Rotor)cbRotor.SelectedItem).SetRotorPosition(cbRotorPos.SelectedIndex);
				tbRotatorPos.Text = cbRotorPos.Text;
				selectedRotorChanged(sender, e);
			}
		}
	}
}
