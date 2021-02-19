﻿using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace MetaSet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
            return 0;
        }
    }
}
