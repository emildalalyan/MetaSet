using System.IO;

namespace MetaSet
{
    /// <summary>
    /// <see langword="Class"/>, intended to add some features to <see ref="TagLib"/> library
    /// </summary>
    public static class TagLibExtensions
    {
        /// <summary>
        /// Save copy of the <see cref="TagLib.File"/> to specified location
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void Save(this TagLib.File file, string fileName)
        {
            File.Copy(file.Name, fileName, true);
            using TagLib.File tf = TagLib.File.Create(fileName);
            file.Tag.CopyTo(tf.Tag, true);
            tf.Save();
        }

        /// <summary>
        /// This function is indicating that <see cref="TagLib.File"/> is supporting ID3v2 tags.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool ID3v2Supported(this TagLib.File file) => file.GetTag(TagLib.TagTypes.Id3v2) is TagLib.Tag;

        /// <summary>
        /// Load <see cref="TagLib.Tag"/> from the specified file location
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fileName"></param>
        public static void LoadFromFile(this TagLib.Tag tag, string fileName, string mimeType = "audio/mpeg", TagLib.ReadStyle style = TagLib.ReadStyle.None)
        {
            using TagLib.File tlf = TagLib.File.Create(fileName, mimeType, style);

            tlf.Tag.CopyTo(tag, true);
        }

        /// <summary>
        /// Save <see cref="TagLib.Tag"/> into specified separated file.
        /// </summary>
        /// <remarks>
        /// It is saving only tags. It won't save pictures in it.
        /// </remarks>
        /// <param name="tag"></param>
        /// <param name="fileName"></param>
        /// <param name="mimeType"></param>
        /// <param name="style"></param>
        public static void PutIntoFile(this TagLib.Tag tag, string fileName, string mimeType = "audio/mpeg", TagLib.ReadStyle style = TagLib.ReadStyle.None)
        {
            using TagLib.File tlb = TagLib.File.Create(fileName, mimeType, style);

            tag.CopyTo(tlb.Tag, true);
            tlb.Save();
        }

        /// <summary>
        /// Saves <see cref="TagLib.IPicture"/> to the disk as file
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="fileName"></param>
        /// <param name="fileMode"></param>
        public static void Save(this TagLib.IPicture picture, string fileName, FileMode fileMode = FileMode.OpenOrCreate)
        {
            using FileStream fs = new(fileName, fileMode);

            fs.Write(picture.Data.Data, 0, picture.Data.Data.Length);
            fs.Flush();
        }
    }
}