using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MetaSet
{
    /// <summary>
    /// Class contains some extensions
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// If <see cref="string"/> str == null, this function returns "N/A"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="append"></param>
        static public string IfNullReturnNA(this string str, string append = "")
        {
            if (str == null) return "N/A";
            return (str.Length > 0 ? str + append : "N/A");
        }

        /// <summary>
        /// If <see cref="string"/> str[<see cref="int"/> index] == null, this function returns "N/A"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <param name="append"></param>
        static public string IfNullReturnNA(this string[] str, int index, string append = "")
        {
            if (str == null) return "N/A";
            return str.Length > index && str[index] != null ? str[index] + append : "N/A";
        }

        /// <summary>
        /// Get MIME-type of the specified <see cref="ImageFormat"/>
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetMimeType(this ImageFormat format) => ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == format.Guid).MimeType;

        /// <summary>
        /// If <see cref="int"/> dec less than 1, then this function returns "N/A"
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="append"></param>
        static public string IfZeroOrLessReturnNA(this int dec, string append = "")
        {
            if (dec < 1) return "N/A";
            else return dec.ToString() + append;
        }

        /// <summary>
        /// Convert <see cref="System.Drawing.Image"/> to <see cref="byte"/>[]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        static public byte[] ToArrayOfBytes(this System.Drawing.Image img)
        {
            using MemoryStream m = new();
            img.Save(m, img.RawFormat);
            return m.ToArray();
        }
    }
}
