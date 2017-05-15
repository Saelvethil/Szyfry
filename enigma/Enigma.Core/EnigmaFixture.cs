//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System.Text;
using NUnit.Framework;

namespace Enigma.Core
{
	[TestFixture]
	public class EnigmaFixture
	{
		private RotorMachine myRotorMachine;
		private Rotor rEtw;
		private Rotor rI;
		private Rotor rII;
		private Rotor rIII;
		private Rotor rUkwB;
		
		[SetUp]
		public void SetUp()
		{
			myRotorMachine = new RotorMachine();

			rEtw = myRotorMachine.Etw;
			rI = myRotorMachine.Rotors["I"];
			rII = myRotorMachine.Rotors["II"];
			rIII = myRotorMachine.Rotors["III"];
			rUkwB = myRotorMachine.Ukws["B"];
			
			myRotorMachine.SetRotor(0, rEtw);
			myRotorMachine.SetRotor(1, rI);
			myRotorMachine.SetRotor(2, rII);
			myRotorMachine.SetRotor(3, rIII);
			myRotorMachine.SetRotor(4, rUkwB);
		}
		
		[Test]
		public void WithoutRingpositionAndRotorposition()
		{
			string s = "Hello world how are you";
			string result = encode(s);
			Assert.AreEqual("MFNCZ BBFZM PNNDS XPFM", result);
		}
		
		[Test]
		public void TestWithRingPosition()
		{
			rI.SetRotorPosition('B');
			rII.SetRotorPosition('F');
			rIII.SetRotorPosition('I');
			
			rI.SetRingPosition(25);
			rII.SetRingPosition(1);
			rIII.SetRingPosition(3);
			
			string s = "Hello world how are you";
			string result = encode(s);
			Assert.AreEqual("KJIZF GEEBV VWMHL BZRA", result);
		}
		
		private string encode(string s)
		{
			StringBuilder sb = new StringBuilder();
			s = s.Replace(" ", string.Empty).ToUpper();
			
			for (int i = 0; i < s.Length; i++)
			{
				sb.Append(myRotorMachine.Encode(s[i]));
				
				if ((i + 1) % 5 == 0)
				{
					sb.Append(" ");
				}
			}
			
			return sb.ToString();
		}
	}
}
