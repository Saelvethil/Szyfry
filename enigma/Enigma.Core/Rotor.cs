//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;

namespace Enigma.Core
{
	/// <summary>
	/// The rotor is responsible for the encryption.
	/// </summary>
	public class Rotor
	{
		private readonly RotorType myType;
		private readonly string myCharSet;
		private readonly string myName;
		private const string BASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private int myIndex;
		private int myRotation;
		
		public event Action<Rotor> RotateNextRotor;
		public event Action<string> Log;
		public delegate void OnSelectionChanged(Rotor r, char character);
		public event OnSelectionChanged PositionChanged;

		/// <summary>
		/// Create a new Rotor.
		/// </summary>
		/// <param name="name">The name is only required for the log output.</param>
		/// <param name="type">Type of the Rotor.</param>
		/// <param name="charSet">The encryption character set.</param>
		public Rotor(string name, RotorType type, string charSet)
		{
			myName = name;
			myType = type;
			myCharSet = charSet;
		}
		
		/// <summary>
		/// If the rotor is of type Rotate, the rotor throws the
		/// <see cref="RotateNextRotor"/> event as soon the Notch is reached.
		/// </summary>
		public char[] Notch
		{
			get;
			set;
		}
		
		public string Name
		{
			get { return myName; }
		}
		
		public RotorType RotorType
		{
			get { return myType; }
		}
		
		/// <summary>
		/// To set the rotor position manually.
		/// </summary>
		/// <param name="index"></param>
		public void SetRotorPosition(int index)
		{
			if (myType.Equals(RotorType.Rotate))
			{
				myIndex = index;
			}
		}

		/// <summary>
		/// To set the rotor position manually.
		/// </summary>
		/// <param name="start"></param>
		public void SetRotorPosition(char start)
		{
			SetRotorPosition(BASE.IndexOf(start));
		}
		
		/// <summary>
		/// To set the ring position manually.
		/// </summary>
		/// <param name="index"></param>
		public void SetRingPosition(int index)
		{
			myRotation = index;
		}

		/// <summary>
		/// If the rotor is of type Rotate it rotates
		/// before every character to encode.
		/// </summary>
		public void Rotate()
		{
			if (myType.Equals(RotorType.Rotate))
			{
				debug("Rotate");
				myIndex = mod(myIndex + 1, BASE.Length);

				if (PositionChanged != null)
				{
					PositionChanged(this, BASE[myIndex]);
				}

				foreach (char c in Notch)
				{
					if (BASE[mod(myIndex - 1, BASE.Length)].Equals(c) && RotateNextRotor != null)
					{
						RotateNextRotor(this);
					}
				}
			}
		}
		
		/// <summary>
		/// Encodes the <paramref name="input"/> based on the
		/// current position and the character set.
		/// </summary>
		/// <param name="input">value to encode</param>
		/// <returns></returns>
		public char EncodeForward(char input)
		{
			debug("forward encode input {0}", input);
			int pos = mod(BASE.IndexOf(input) + myIndex - myRotation, BASE.Length);
			debug("forward encode input position {0}", pos);
			char output = myCharSet[pos];
			debug("forward encode output based on position {0}", output);
			output = BASE[mod(BASE.IndexOf(output) - myIndex + myRotation, BASE.Length)];
			debug("forward encode {0} to {1}", input, output);
			return output;
		}
		
		/// <summary>
		/// Encodes the <paramref name="input"/> based on the
		/// current position and the character set.
		/// </summary>
		/// <param name="input">value to encode</param>
		/// <returns></returns>
		public char EncodeBackward(char input)
		{
			int pos = mod(BASE.IndexOf(input) + myIndex - myRotation, BASE.Length);
			input = BASE[pos];
			pos = mod(myCharSet.IndexOf(input) - myIndex + myRotation, BASE.Length);
			char output = BASE[pos];
			debug("backward encode {0} to {1}", input, output);
			
			return output;
		}
		
		private static int mod(int i, int b)
		{
			int result = i % b;
			return (result < 0) ? b + result : result;
		}
		
		private void debug(string s, params object[] param)
		{
            //if (Log != null)
            //{
            //    string formatted = string.Format(s, param);
            //    Log(string.Format("Rotor {0}: {1}", Name, formatted));
            //}
		}

		public override string ToString()
		{
			return myName;
		}
	}
}
