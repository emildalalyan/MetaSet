using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Media;
using System.Drawing;

namespace MetaSet
{
    /// <summary>
    /// Common <see cref="MetaSet"/> class. It contains currently opened file, <see cref="global::MetaSet.MainForm"/> instance, e.t.c
    /// </summary>
    static public class MetaSet
    {
        /// <summary>
        /// <see cref="MetaSet"/> version string constant
        /// </summary>
        public const string Version = "1.5-stable";

        /// <summary>
        /// Instance of the <see cref="MainForm"/>
        /// </summary>
        static public MainForm MainForm { get; set; }

        /// <summary>
        /// Formats, supported by the MetaSet program
        /// </summary>
        static public readonly string[] FormatSupport = new string[]
        {
            ".mp3", ".flac", ".ogg", ".wav", ".wma", ".m4a"
        };
    }
}
