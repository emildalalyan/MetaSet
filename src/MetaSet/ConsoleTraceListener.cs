using System;
using System.Diagnostics;

namespace MetaSet
{
    /// <summary>
    /// This class implements console output system through <see cref="Trace"/> class.
    /// </summary>
    public class ConsoleTraceListener : TraceListener
    {
        /// <summary>
        /// Creates an instance of <see cref="ConsoleTraceListener"/> without any arguments.
        /// </summary>
        public ConsoleTraceListener()
        { }

        public override void Write(string message)
        {
            if (message == null) return;

            Console.Write($"[{DateTime.Now.ToLongTimeString()}] {message}");
        }

        public override void WriteLine(string message)
        {
            if (message == null) return;

            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {message}");
        }
    }
}
