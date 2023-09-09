using System.Linq;

namespace MetaSet
{
    /// <summary>
    /// Common <see cref="MetaSet"/> class. It contains <see cref="global::MetaSet.MainForm"/> instance, e.t.c
    /// </summary>
    public static class MetaSet
    {
        /// <summary>
        /// <see cref="MetaSet"/> version string constant
        /// </summary>
        public const string Version = "1.6-unstable";

        /// <summary>
        /// Instance of the <see cref="MainForm"/>
        /// </summary>
        public static MainForm MainForm { get; internal set; }

        /// <summary>
        /// Formats, supported by the MetaSet program
        /// </summary>
        public static readonly string[] FormatSupport = new string[]
        {
            ".mp3", ".flac", ".ogg", ".wav", ".wma", ".m4a", ".aac", ".aiff"
        };

        /// <summary>
        /// Filter string, generated from <see cref="MetaSet.FormatSupport"/> array.
        /// </summary>
        public static string SupportedFormatsFilter { get; } = $"{string.Join(';', MetaSet.FormatSupport.Select((string str) => "*" + str))}";

        /// <summary>
        /// If you want to help me, then donate me there.
        /// </summary>
        public const string DonateLink = "https://donationalerts.com/r/emildalalyan";

        /// <summary>
        /// Link to the repo of the MetaSet program
        /// </summary>
        public const string RepoLink = "https://github.com/emildalalyan/MetaSet";
    }
}
