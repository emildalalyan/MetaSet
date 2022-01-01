namespace MetaSet
{
    /// <summary>
    /// Common <see cref="MetaSet"/> class. It contains currently opened file, <see cref="global::MetaSet.MainForm"/> instance, e.t.c
    /// </summary>
    public static class MetaSet
    {
        /// <summary>
        /// <see cref="MetaSet"/> version string constant
        /// </summary>
        public const string Version = "1.5.1-stable";

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
        /// If you want to help me, then donate me there.
        /// </summary>
        public const string DonateLink = "https://donationalerts.com/r/emildalalyan";

        /// <summary>
        /// Link to the repo of the MetaSet program
        /// </summary>
        public const string RepoLink = "https://github.com/emildalalyan/MetaSet";
    }
}
