//  --------------------------------
//  Copyright (c) 2009 Michael Schuler, Sascha Burger. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://enigma.codeplex.com/license
//  ---------------------------------
using System;
using System.Windows.Forms;

namespace Enigma.Gui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
