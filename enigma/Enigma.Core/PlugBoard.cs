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
	/// The PlugBoard is responsible for the replacement of characters.
	/// </summary>
	public class PlugBoard
	{
		public Dictionary<char, char> myPlugs = new Dictionary<char, char>();
		public event Action<string> Log;

		/// <summary>
		/// Add a new replacement of two characters.
		/// </summary>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <exception cref="InvalidOperationException">If there is already
		/// a replacement of one of the overgiven characters.</exception>
		public void AddPlug(char key1, char key2)
		{
			if (myPlugs.ContainsKey(key1) || myPlugs.ContainsKey(key2))
			{
				throw new InvalidOperationException("KeyAlreadySetted");
			}

			myPlugs.Add(key1, key2);
			myPlugs.Add(key2, key1);
			
			if (Log != null)
			{
				Log(string.Format("PlugBoard: {0} z {1} po³¹czone.", key1, key2));
			}
		}

		/// <summary>
		/// Removes a character from the replacement list.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(char key)
		{
			if (myPlugs.ContainsKey(key))
			{
				myPlugs.Remove(myPlugs[key]);
				myPlugs.Remove(key);
			}
			
			if (Log != null)
			{
				Log(string.Format("PlugBoard: Po³¹czenie z {0} roz³¹czone.", key));
			}
		}

		/// <summary>
		/// To replace the <paramref name="key"/> with its replacement character
		/// if there is one.
		/// </summary>
		/// <param name="key">Character to replace.</param>
		/// <returns></returns>
		public char Encode(char key)
		{
			char encoded = key;
			if (myPlugs.ContainsKey(key))
			{
				encoded = myPlugs[key];
			}
			
			if (Log != null)
			{
				Log(string.Format("PlugBoard: Encode '{0}' to '{1}'.", key, encoded));
			}
			
			return encoded;
		}
	}
}
