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
        /// Trace listener, that showing messagebox, when it's getting message string
        /// </summary>
        public static MessageTraceListener MessageTrace { get; } = new();

        /// <summary>
        /// Trace listener, that outputs to console, when it's getting message string
        /// </summary>
        public static ConsoleTraceListener ConsoleTrace { get; } = new();

        /// <summary>
        /// The main entry point of the <b>MetaSet</b> program.
        /// </summary>
        [STAThread]
        internal static int Main(string[] args)
        {
            // Uncomment below to set invariant culture in MetaSet.

            // Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            ProvidedArguments = args;
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
            return 0;
        }
    }
}
