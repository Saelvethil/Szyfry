//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Enigma.Gui
{
	public partial class UcPlugBoard : UserControl
	{
		private const string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public Dictionary<char, char> myPlugs = new Dictionary<char, char>();
		private char? mySelectedCharacter;
		private readonly Color[] myHighlightColors = new[] { Color.Turquoise,
			Color.SteelBlue, Color.BlueViolet, Color.HotPink, Color.GreenYellow,
			Color.OliveDrab, Color.Gold, Color.DarkOrange, Color.Red, Color.Navy,
			Color.Khaki, Color.Peru, Color.LightCoral };
		public event Action<char> PlugRemoved;
		public delegate void OnAddPlug (char c1, char c2);
		public event OnAddPlug PlugAdded;

		public UcPlugBoard()
		{
			InitializeComponent();
			createButtons();
		}

		public void createButtons()
		{
			Controls.Clear();

			for (int i = 0; i < CHARACTERS.Length; i++)
			{
				char c = CHARACTERS[i];
				Button butTop = new Button { FlatStyle = FlatStyle.Flat, Text = c.ToString(), Size = new Size(23, 23) };
				Button butBottom = new Button { FlatStyle = FlatStyle.Flat, Text = c.ToString(), Size = new Size(23, 23) };

				if (isExisting(c))
				{
					butTop.BackColor = myHighlightColors[getIndex(c)];
					butBottom.BackColor = myHighlightColors[getIndex(c)];
				}

				butTop.ForeColor = (butTop.BackColor.GetBrightness() > 0.4) ? Color.Black : Color.White;
				butBottom.ForeColor = (butBottom.BackColor.GetBrightness() > 0.4) ? Color.Black : Color.White;

				Controls.Add(butTop);
				Controls.Add(butBottom);

				butTop.Click += butClick;
				butBottom.Click += butClick;

				butTop.Location = new Point(i * 27, 10);
				butBottom.Location = new Point(i * 27, 100);
			}
		}

		private void butClick(object sender, EventArgs e)
		{
			char c = ((Button)sender).Text[0];

			if (isExisting(c))
			{
				if (PlugRemoved != null)
				{
					PlugRemoved(c);
					PlugRemoved(getPlug(c));
				}
				remove(c);
				mySelectedCharacter = null;
				createButtons();
			}
			else
			{
				if (mySelectedCharacter == null)
				{
					mySelectedCharacter = c;
					((Button)sender).BackColor = SystemColors.Highlight;
				}
				else if (c != mySelectedCharacter.Value)
				{
					if (PlugAdded != null)
					{
						PlugAdded(c, mySelectedCharacter.Value);
					}
					myPlugs.Add(c, mySelectedCharacter.Value);
					mySelectedCharacter = null;
					createButtons();
				}
				else
				{
					mySelectedCharacter = null;
					createButtons();
				}
			}

			Invalidate();
		}

        public void AddPlugByChar(char c)
        {
            if (isExisting(c))
            {
                if (PlugRemoved != null)
                {
                    PlugRemoved(c);
                    PlugRemoved(getPlug(c));
                }
                remove(c);
                mySelectedCharacter = null;
                createButtons();
            }
            else
            {
                if (mySelectedCharacter == null)
                {
                    mySelectedCharacter = c;
                }
                else if (c != mySelectedCharacter.Value)
                {
                    if (PlugAdded != null)
                    {
                        PlugAdded(c, mySelectedCharacter.Value);
                    }
                    myPlugs.Add(c, mySelectedCharacter.Value);
                    mySelectedCharacter = null;
                    createButtons();
                }
                else
                {
                    mySelectedCharacter = null;
                    createButtons();
                }
            }

            Invalidate();
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			for (int i = 0; i < CHARACTERS.Length; i++)
			{
				Point start = new Point(i * 27 + 11, 33);
				Point end = new Point(i * 27 + 11, 99);
				Pen pen = Pens.Black;

				char c = CHARACTERS[i];

				if (isExisting(c))
				{
					int index = CHARACTERS.IndexOf(getPlug(c));
					end = new Point(index * 27 + 11, 99);
					pen = new Pen(myHighlightColors[getIndex(c)]);
				}

				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				e.Graphics.DrawLine(pen, start, end);

				if (isExisting(c))
				{
					e.Graphics.DrawLine(pen, start.X + 1, start.Y, end.X + 1, end.Y);
					e.Graphics.DrawLine(pen, start.X - 1, start.Y, end.X - 1, end.Y);
				}
			}
		}

		private char getPlug(char a)
		{
			if (myPlugs.Keys.Contains(a))
			{
				return myPlugs[a];
			}
			if (myPlugs.Values.Contains(a))
			{
				foreach (char b in myPlugs.Keys)
				{
					if (myPlugs[b] == a)
					{
						return b;
					}
				}
			}
			return char.MinValue;
		}

		private bool isExisting(char a)
		{
			return (myPlugs.Keys.Contains(a) || myPlugs.Values.Contains(a));
		}

		private int getIndex(char a)
		{
			if (myPlugs.Keys.Contains(a))
			{
				return myPlugs.Keys.ToList().IndexOf(a);
			}
			if (myPlugs.Values.Contains(a))
			{
				return myPlugs.Values.ToList().IndexOf(a);
			}
			return -1;
		}

		private void remove(char a)
		{
			if (myPlugs.Keys.Contains(a))
			{
				myPlugs.Remove(a);
			}
			else if (myPlugs.Values.Contains(a))
			{
				myPlugs.Remove(getPlug(a));
			}
		}

		private void formLoad(object sender, EventArgs e)
		{
			DoubleBuffered = true;
		}
	}
}
