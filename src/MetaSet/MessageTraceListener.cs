using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;

#nullable enable

namespace MetaSet
{
    /// <summary>
    /// This class implements message error (warning) system through <see cref="Trace"/> class.
    /// </summary>
    public class MessageTraceListener : TraceListener
    {
        /// <summary>
        /// Enumeration of possible types of message.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// This message is <b>error</b>, it can contain exception message as well.
            /// </summary>
            Error = 0,

            /// <summary>
            /// This message is <b>warning</b>, program continue execute.
            /// </summary>
            Warning = 1,

            /// <summary>
            /// This message is just <b>information</b>, it's informing user and no more.
            /// </summary>
            Information = 2,

            /// <summary>
            /// This message is <b>log</b> message, developers can use this to debug the code.
            /// </summary>
            Log = 3,

            /// <summary>
            /// This message is <b>unknown</b> type, it can contain everything.
            /// </summary>
            Unknown = 4
        }

        /// <summary>
        /// Beginning of error messages, it's using to detect error messages.
        /// </summary>
        public string ErrorMessageBeginning { get; set; } = "[Error] ";

        /// <summary>
        /// Beginning of warning messages, it's using to detect warning messages.
        /// </summary>
        public string WarningMessageBeginning { get; set; } = "[Warning] ";

        /// <summary>
        /// Beginning of info messages, it's using to detect info messages.
        /// </summary>
        public string InformationMessageBeginning { get; set; } = "[Info] ";

        /// <summary>
        /// Beginning of log messages, it's using to detect log messages.
        /// </summary>
        public string LogMessageBeginning { get; set; } = "[Log] ";

        /// <summary>
        /// Verbosity of the message system. Provide a minimum type.
        /// </summary>
        public MessageType LogVerbosity { get; set; } = MessageType.Information;

        /// <summary>
        /// Title of the messagebox, that appearing when message is provided.
        /// </summary>
        public string MessageBoxTitle { get; set; } = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        /// <summary>
        /// This method returns type of message, provided to the <see cref="Trace"/> and clears message from beginnings.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected MessageType GetMessageTypeAndClearString(ref string message)
        {
            if (string.IsNullOrEmpty(ErrorMessageBeginning)) throw new AggregateException("Beginning of error messages cannot be null or empty");

            if (string.IsNullOrEmpty(WarningMessageBeginning)) throw new AggregateException("Beginning of warning messages cannot be null or empty");

            if (string.IsNullOrEmpty(InformationMessageBeginning)) throw new AggregateException("Beginning of info messages cannot be null or empty");

            if (string.IsNullOrEmpty(LogMessageBeginning)) throw new AggregateException("Beginning of log messages cannot be null or empty");

            if (message == null) throw new ArgumentNullException(nameof(message), "Message cannot be null.");

            if (message.StartsWith(ErrorMessageBeginning, StringComparison.InvariantCulture))
            {
                message = message[ErrorMessageBeginning.Length..];
                return MessageType.Error;
            }
            else if (message.StartsWith(WarningMessageBeginning, StringComparison.InvariantCulture))
            {
                message = message[WarningMessageBeginning.Length..];
                return MessageType.Warning;
            }
            else if (message.StartsWith(InformationMessageBeginning, StringComparison.InvariantCulture))
            {
                message = message[InformationMessageBeginning.Length..];
                return MessageType.Information;
            }
            else if (message.StartsWith(LogMessageBeginning, StringComparison.InvariantCulture))
            {
                message = message[LogMessageBeginning.Length..];
                return MessageType.Log;
            }
            else return MessageType.Unknown;
        }

        /// <summary>
        /// Creates an instance of <see cref="MessageTraceListener"/> without any arguments.
        /// </summary>
        public MessageTraceListener()
        { }

        public override void Write(string? message)
        {
            this.WriteLine(message);
        }

        public override void WriteLine(string? message)
        {
            if (string.IsNullOrEmpty(message)) return;

            MessageType type = GetMessageTypeAndClearString(ref message);

            if (type > LogVerbosity) return;

            MessageBox.Show(message, MessageBoxTitle, MessageBoxButtons.OK, type switch
            {
                MessageType.Error => MessageBoxIcon.Error,
                MessageType.Warning => MessageBoxIcon.Warning,
                MessageType.Information => MessageBoxIcon.Information,
                MessageType.Log => MessageBoxIcon.None,
                MessageType.Unknown => MessageBoxIcon.None,
                _ => MessageBoxIcon.None
            });
        }
    }
}