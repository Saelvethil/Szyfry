//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Enigma.Core
{
	/// <summary>
	/// This is the main encryption engine. It contains a <see cref="PlugBoard"/> and
	/// a <see cref="RotorMachine"/>. 
	/// </summary>
	public class EnigmaMachine
	{
		private readonly Regex myRegex = new Regex("[A-Z]{1}", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly Regex myRegexSmall = new Regex("[a-z]{1}", RegexOptions.Compiled | RegexOptions.Singleline);
		public PlugBoard myBoard;
		private readonly RotorMachine myMachine;
		
		/// <summary>
		/// Create a new EnigmaMachine.
		/// </summary>
		/// <param name="board">PlugBoard cannot be null.</param>
		/// <param name="rm">RotorMachine cannot be null.</param>
		public EnigmaMachine(PlugBoard board, RotorMachine rm)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}
			if (rm == null)
			{
				throw new ArgumentNullException("rm");
			}
			
			myBoard = board;
			myMachine = rm;
		}
		
		private char encode(char key)
		{
			key = myBoard.Encode(key);
			key = myMachine.Encode(key);
			return myBoard.Encode(key);
		}
		
		/// <summary>
		/// Encodes all valid characters of a whole string.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string Encode(string s)
		{
			StringBuilder sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                string curr = s[i].ToString();
                string currUpp = curr.ToUpper();
                if (myRegex.IsMatch(currUpp))
                {
                    if (curr.Equals(currUpp))
                    {
                        sb.Append(encode(currUpp.ToCharArray()[0]));
                    }
                    else sb.Append((encode(currUpp.ToCharArray()[0])).ToString().ToLower());
                }
                else sb.Append(s[i]);
            }
			return sb.ToString();
		}
	}
}
