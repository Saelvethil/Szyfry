//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Enigma.Core
{
	/// <summary>
	/// This is the container for all rotors. It can hold five rotors
	/// where the first should be of type Entry and the last of type
	/// Reversal. The RotorMachine handles the rotation of all other
	/// rotors.
	/// Usage: Set the rotors with <see cref="SetRotor"/>.
	/// </summary>
	public class RotorMachine
	{
		private readonly Rotor[] myRotor = new Rotor[5];
		public event Action<string> Log;
		private readonly Rotor myEtw;
		private readonly Dictionary<string, Rotor> myUkws = new Dictionary<string, Rotor>();
		private readonly Dictionary<string, Rotor> myRotors = new Dictionary<string, Rotor>();
		
		public RotorMachine()
		{
			myEtw = new Rotor("ETW", RotorType.Entry, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

			myUkws.Add("A", new Rotor("A", RotorType.Reversal, "EJMZALYXVBWFCRQUONTSPIKHGD"));
			myUkws.Add("B", new Rotor("B", RotorType.Reversal, "YRUHQSLDPXNGOKMIEBFZCWVJAT"));
			myUkws.Add("C", new Rotor("C", RotorType.Reversal, "FVPJIAOYEDRZXWGCTKUQSBNMHL"));

			myRotors.Add("I", new Rotor("I", RotorType.Rotate, "EKMFLGDQVZNTOWYHXUSPAIBRCJ") { Notch = new[] { 'Q' } });
			myRotors.Add("II", new Rotor("II", RotorType.Rotate, "AJDKSIRUXBLHWTMCQGZNPYFVOE") { Notch = new[] { 'E' } });
			myRotors.Add("III", new Rotor("III", RotorType.Rotate, "BDFHJLCPRTXVZNYEIWGAKMUSQO") { Notch = new[] { 'V' } });
			myRotors.Add("IV", new Rotor("IV", RotorType.Rotate, "ESOVPZJAYQUIRHXLNFTGKDCMWB") { Notch = new[] { 'J' } });
			myRotors.Add("V", new Rotor("V", RotorType.Rotate, "VZBRGITYUPSDNHLXAWMJQOFECK") { Notch = new[] { 'Z', 'M' } });
			myRotors.Add("VI", new Rotor("VI", RotorType.Rotate, "JPGVOUMFYQBENHZRDKASXLICTW") { Notch = new[] { 'Z', 'M' } });
			myRotors.Add("VII", new Rotor("VII", RotorType.Rotate, "NZJHGRCXMYSWBOUFAIVLPEKQDT") { Notch = new[] { 'Z', 'M' } });
			myRotors.Add("VIII", new Rotor("VIII", RotorType.Rotate, "FKQHTLXOCBJSPDZRAMEWNIUYGV") { Notch = new[] { 'Z', 'M' } });

			myEtw.Log += log;
			foreach (Rotor rotor in myUkws.Values)
			{
				rotor.Log += log;
			}

			foreach (Rotor rotor in myRotors.Values)
			{
				rotor.Log += log;
				rotor.RotateNextRotor += rotateNextRotor;
			}
			
			SetRotor(0, myEtw);
		}
		
		public Rotor Etw
		{
			get { return myEtw; }
		}
		
		public Dictionary<string, Rotor> Ukws
		{
			get { return myUkws; }
		}
		
		public Dictionary<string, Rotor> Rotors
		{
			get { return myRotors; }
		}
		
		/// <summary>
		/// Set a new rotor to a specific position.
		/// </summary>
		/// <param name="index">Position of the rotor within the RotorMachine.</param>
		/// <param name="r">New Rotor instance.</param>
		public void SetRotor(int index, Rotor r)
		{
			myRotor[index] = r;
		}
		
		/// <summary>
		/// Encodes a specific <paramref name="input"/> based on all
		/// containing rotors.
		/// </summary>
		/// <param name="input">Character to encode.</param>
		/// <returns></returns>
		public char Encode(char input)
		{
			myRotor[1].Rotate();
			for (int i = 0; i < myRotor.Length; i++)
			{
				input = myRotor[i].EncodeForward(input);
			}
			
			for (int i = myRotor.Length - 2; i >= 0; i--)
			{
				input = myRotor[i].EncodeBackward(input);
			}
			
			return input;
		}

		private void rotateNextRotor(Rotor sender)
		{
			for (int i = 0; i < myRotor.Length; i++)
			{
				if (myRotor[i] == sender && myRotor.Length > i + 1)
				{
					myRotor[i + 1].Rotate();
				}
			}
		}

		private void log(string s)
		{
			if (Log != null)
			{
				Log(s);
			}
		}
	}
}
