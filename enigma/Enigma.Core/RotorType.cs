//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
namespace Enigma.Core
{
	/// <summary>
	/// The behavior of a rotor is defined by its type.
	/// Entry is the first static rotor, Reversal the last one
	/// and all other rotors are of type Rotate.
	/// </summary>
	public enum RotorType
	{
		Entry = 1,
		Reversal = 2,
		Rotate = 3
	}
}
