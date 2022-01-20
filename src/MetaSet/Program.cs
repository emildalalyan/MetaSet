using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace MetaSet
{
    internal static class Program
    {
        /// <summary>
        /// Represents arguments, which was provided to program, when it has been started.
        /// </summary>
        internal static string[] ProvidedArguments { get; private set; }

        /// <summary>
        /// The main entry point for the MetaSet program.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            // Uncomment below to set invariant culture in MetaSet.

            // Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            ProvidedArguments = args;
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            return 0;
        }
    }
}
