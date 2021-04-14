using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace MetaSet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the MetaSet program.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            // Uncomment below to set invariant culture in MetaSet.

            // Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
            return 0;
        }
    }
}
