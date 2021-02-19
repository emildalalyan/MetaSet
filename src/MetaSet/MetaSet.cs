using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Media;
using System.Threading.Tasks;

namespace MetaSet
{
    /// <summary>
    /// <see cref="MetaSet"/> common class
    /// </summary>
    static class MetaSet
    {
        /// <summary>
        /// MetaSet Version
        /// </summary>
        public const string Version = "1.4-stable";

        /// <summary>
        /// Loaded File
        /// </summary>
        static public TagLib.File File;

        /// <summary>
        /// Main Form Instance
        /// </summary>
        static public Form1 MainForm;

        /// <summary>
        /// Formats, supported by MetaSet
        /// </summary>
        static public readonly string[] FormatSupport = new string[]
        {
            ".mp3", ".flac", ".ogg", ".wav", ".wma", ".m4a"
        };
        
        /// <summary>
        /// Show About Dialog
        /// </summary>
        static public void AboutProgram()
        {
            using (About about = new About()) about.ShowDialog();
        }

        /// <summary>
        /// Create New MetaSet Instance
        /// </summary>
        static public void CreateInstance()
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath, $"-s {MainForm.Location.X + 32} {MainForm.Location.Y + 32}");
        }
    }

    /// <summary>
    /// Class making possible to convert some types
    /// </summary>
    static class Converting
    {
        /// <summary>
        /// Convert <see cref="System.Drawing.Image"/> to <see cref="byte"/>[]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        static public byte[] ToArrayOfBytes(this System.Drawing.Image img)
        {
            using(MemoryStream m = new MemoryStream())
            {
                img.Save(m, img.RawFormat);
                return m.ToArray();
            }
        }
    }

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
        /// <returns></returns>
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
        /// <returns></returns>
        static public string IfNullReturnNA(this string[] str, int index, string append = "")
        {
            if (str == null) return "N/A";
            return (str.Length > index ? str[index] + append : "N/A");
        }

        /// <summary>
        /// If <see cref="int"/> dec == null, this function returns "N/A"
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        static public string IfNullReturnNA(this int dec, string append = "")
        {
            if (dec < 1) return "N/A";
            else return dec.ToString() + append;
        }
    }

    /// <summary>
    /// Common <see cref="MetaSet"/> functions
    /// </summary>
    static class Functions
    {
        static public void SaveAsFunction()
        {
            using(SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = $"File (*{Path.GetExtension(MetaSet.File.Name)})|*{Path.GetExtension(MetaSet.File.Name)}"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
                {
                    File.Copy(MetaSet.File.Name, sfd.FileName, true);
                    using (TagLib.File tf = TagLib.File.Create(sfd.FileName))
                    {
                        MetaSet.File.Tag.CopyTo(tf.Tag, true);
                        tf.Save();
                    }
                    OpenFile(sfd.FileName);
                }
            }
        }
        static public void TakeScreenshot()
        {
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(MetaSet.MainForm.Width, MetaSet.MainForm.Height))
            {
                MetaSet.MainForm.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0,0, MetaSet.MainForm.Width, MetaSet.MainForm.Height));
                Clipboard.SetImage(bitmap);
            }
        }
        static public void DeleteACover()
        {
            MetaSet.File.Tag.Pictures = null;
            MetaSet.MainForm.pictureBox1.Image = null;
            MetaSet.MainForm.imagetype.Text = "";
            MetaSet.MainForm.imagesize.Text = "";
        }
        static public bool WeCanGetTag()
        {
            if (!IsOpened()) return false;
            return (MetaSet.File.GetTag(TagLib.TagTypes.Id3v2) is TagLib.Tag);
        }
        static public void ChangeACover()
        {
            using (OpenFileDialog a = new OpenFileDialog
            {
                Filter = "All Supported Formats (*.jpg;*.png)|*.jpg;*.png|Joint Photographic Experts Group (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png"
            })
            {
                if (a.ShowDialog() == DialogResult.OK)
                {
                    MetaSet.MainForm.pictureBox1.Image = System.Drawing.Image.FromFile(a.FileName);
                    MetaSet.File.Tag.Pictures = new TagLib.Picture[1] {
                        new TagLib.Picture
                        {
                            MimeType = (Path.GetExtension(a.FileName) == ".png" ? "image/png" : "image/jpeg"),
                            Type = TagLib.PictureType.Media,
                            Data = MetaSet.MainForm.pictureBox1.Image.ToArrayOfBytes()
                        }
                    };

                    MetaSet.MainForm.imagetype.Text = MetaSet.File.Tag.Pictures[0].MimeType;
                    MetaSet.MainForm.imagesize.Text = ((MetaSet.MainForm.pictureBox1.Image is System.Drawing.Image) ? MetaSet.MainForm.pictureBox1.Image.Size.Width + "x" + MetaSet.MainForm.pictureBox1.Image.Size.Height : "");
                }
            }
        }
        static public void ChangeACover(string filename)
        {
            if (Path.GetExtension(filename) != ".png" && Path.GetExtension(filename) != ".jpg") return;
            MetaSet.MainForm.pictureBox1.Image = System.Drawing.Image.FromFile(filename);

            MetaSet.File.Tag.Pictures = new TagLib.Picture[1] {
                new TagLib.Picture
                {
                    MimeType = (Path.GetExtension(filename) == ".png" ? "image/png" : "image/jpeg"),
                    Type = TagLib.PictureType.Media,
                    Data = MetaSet.MainForm.pictureBox1.Image.ToArrayOfBytes()
                }
            };

            MetaSet.MainForm.imagetype.Text = MetaSet.File.Tag.Pictures[0].MimeType;
            MetaSet.MainForm.imagesize.Text = ((MetaSet.MainForm.pictureBox1.Image is System.Drawing.Image) ? MetaSet.MainForm.pictureBox1.Image.Size.Width + "x" + MetaSet.MainForm.pictureBox1.Image.Size.Height : "");
        }
        static public void SaveTagAs()
        {
            if (!IsOpened()) return;
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Metadata File (*.met)|*.met",
                    InitialDirectory = (IsOpened()) ? Path.GetDirectoryName(MetaSet.File.Name) : ""
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
                    {
                        if (!File.Exists(sfd.FileName)) File.Create(sfd.FileName).Close();
                        using (TagLib.File tlb = TagLib.File.Create(sfd.FileName, "audio/mpeg", TagLib.ReadStyle.None))
                        {
                            MetaSet.File.Tag.CopyTo(tlb.Tag, true);
                            tlb.Save();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error was appeared! Error message was: " + e.Message);
            }
        }
        static public void ExtractCover()
        {
            if (MetaSet.MainForm.pictureBox1.Image == null) return;

            using (SaveFileDialog a = new SaveFileDialog
            {
                Filter = $"{MetaSet.File.Tag.Pictures[0].MimeType} (*.{MetaSet.File.Tag.Pictures[0].MimeType.Split('/')[1]})|*.{MetaSet.File.Tag.Pictures[0].MimeType.Split('/')[1]}"
            })
            {
                if (a.ShowDialog() == DialogResult.OK && a.FileName.Length > 0)
                {
                    try
                    {
                        MetaSet.MainForm.pictureBox1.Image.Save(a.FileName);
                    }
                    catch
                    {
                        try
                        {
                            using (FileStream b = new FileStream(a.FileName, FileMode.OpenOrCreate))
                            {
                                b.Write(MetaSet.File.Tag.Pictures[0].Data.Data, 0, MetaSet.File.Tag.Pictures[0].Data.Data.Length);
                                b.Flush();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unknown error with cover.", "Unexpected error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
                
        }
        static public int CloseFile()
        {
            if (IsOpened())
            {
                MetaSet.File.Dispose();
            }

            MetaSet.File =              null;
            MetaSet.MainForm.Text =     "MetaSet";
            MetaSet.MainForm.saveToolStripMenuItem.Enabled =                        false;
            MetaSet.MainForm.closeToolStripMenuItem.Enabled =                       false;
            MetaSet.MainForm.extractACoverToolStripMenuItem.Enabled =               false;
            MetaSet.MainForm.copyInformationAboutTrackToolStripMenuItem.Enabled =   false;
            MetaSet.MainForm.playATrackToolStripMenuItem.Enabled =                  false;
            MetaSet.MainForm.checkAPropertiesToolStripMenuItem.Enabled =            false;
            MetaSet.MainForm.textBox1.Text =        "";
            MetaSet.MainForm.textBox2.Text =        "";
            MetaSet.MainForm.textBox3.Text =        "";
            MetaSet.MainForm.textBox4.Text =        "";
            MetaSet.MainForm.textBox5.Text =        "";
            MetaSet.MainForm.textBox6.Text =        "";
            MetaSet.MainForm.textBox7.Text =        "";
            MetaSet.MainForm.textBox8.Text =        "";
            MetaSet.MainForm.textBox9.Text =        "";
            MetaSet.MainForm.textBox10.Text =       "";
            MetaSet.MainForm.textBox11.Text =       "";
            MetaSet.MainForm.textBox13.Text =       "";
            MetaSet.MainForm.checkBox1.Checked =    false;
            MetaSet.MainForm.checkBox1.Enabled =    true;
            MetaSet.MainForm.pictureBox1.Image =    null;
            MetaSet.MainForm.textBox12.Text =       "";
            MetaSet.MainForm.textBox14.Text =       "";
            MetaSet.MainForm.textBox15.Text =       "";
            MetaSet.MainForm.saveAsToolStripMenuItem.Enabled = false;
            MetaSet.MainForm.saveMetadataAsToolStripMenuItem.Enabled = false;
            MetaSet.MainForm.loadMetadataFromAFileToolStripMenuItem.Enabled = false;
            MetaSet.MainForm.textBox16.Text =       "";
            MetaSet.MainForm.textBox17.Text =       "";
            MetaSet.MainForm.textBox18.Text =       "";
            MetaSet.MainForm.textBox19.Text =       "";
            MetaSet.MainForm.textBox20.Text =       "";
            MetaSet.MainForm.imagetype.Text =       "";
            MetaSet.MainForm.imagesize.Text =       "";
            MetaSet.MainForm.deleteAnyTagsInFileToolStripMenuItem.Enabled = false;

            return 0;
        }
        static public void DeleteTags()
        {
            Functions.DeleteACover();
            MetaSet.File.RemoveTags(TagLib.TagTypes.AllTags);   

            Functions.ReadIt();
            //MessageBox.Show("For switching " + '"' + "It is part of compilation" + '"' + " checkbox, please save file and reopen it.", "MetaSet", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        static public void LoadMetaFromFile()
        {
            if (!IsOpened()) return;
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Metadata File (*.met)|*.met",
                    InitialDirectory = (IsOpened()) ? Path.GetDirectoryName(MetaSet.File.Name) : ""
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
                    {
                        using (TagLib.File tlf = TagLib.File.Create(ofd.FileName, "audio/mpeg", TagLib.ReadStyle.None)) 
                        {
                            tlf.Tag.CopyTo(MetaSet.File.Tag, true);
                        }
                        ReadIt();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error was appeared. Error message was: " + e.Message + ". HRESULT: 0x" + e.HResult.ToString("X"), "Error in MetaSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static public bool IsOpened()
        {
            return (MetaSet.File is TagLib.File);
        }
        static public void CopyTheCover()
        {
            if (!(MetaSet.MainForm.pictureBox1.Image is System.Drawing.Image)) return;
            Clipboard.SetImage(MetaSet.MainForm.pictureBox1.Image);
        }
        static public int OpenFile()
        {
            using (OpenFileDialog a = new OpenFileDialog
            {
                Filter = "All Supported Formats (*.mp3;*.flac;*.ogg;*.wav;*.wma;*.m4a)|*.mp3;*.flac;*.ogg;*.wav;*.wma;*.m4a",
                InitialDirectory = (IsOpened()) ? Path.GetDirectoryName(MetaSet.File.Name) : ""
            })
            {
                a.ShowDialog();
                if (a.FileName.Length > 1)
                {
                    CloseFile();

                    try
                    {
                        MetaSet.File = TagLib.File.Create(a.FileName);
                        MetaSet.MainForm.Text = "MetaSet — " + Path.GetFileName(MetaSet.File.Name);
                        
                        ReadIt();
                        return 0;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.GetType().Name + " was appeared." + " Message: " + e.Message);
                        return 2;
                    }
                }
            }
                
            return 1;
        }
        static public int OpenFile(string str)
        {
            if (str.Length > 1 && File.Exists(str))
            {
                CloseFile();
                if (!MetaSet.FormatSupport.Contains(Path.GetExtension(str)))
                {
                    MessageBox.Show("Sorry, but this file format is not supported.", "MetaSet");
                    return 3;
                }

                try
                {
                    MetaSet.File = TagLib.File.Create(str);
                    MetaSet.MainForm.Text = $"MetaSet — {Path.GetFileName(MetaSet.File.Name)}";
                    
                    ReadIt();
                    return 0;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.GetType().Name + " was appeared.");
                    return 2;
                }
            }
            return 1;
        }
        static public void SaveFile()
        {
            try
            {
                MetaSet.File.Save();
                SystemSounds.Beep.Play();
            }
            catch(System.IO.IOException)
            {
                MessageBox.Show("System.IO.IOException was appeared. Maybe, this file used by another application.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                MessageBox.Show($"{e.GetType().Name} was appeared. HRESULT was: {"0x" + e.HResult.ToString("X")}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        static public int ReadIt()
        {
            MetaSet.File.Mode = TagLib.File.AccessMode.Read;
            MetaSet.MainForm.copyInformationAboutTrackToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.closeToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.extractACoverToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.playATrackToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.checkAPropertiesToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.textBox1.Text = MetaSet.File.Tag.Title;
            if (MetaSet.File.Tag.AlbumArtists.Length > 0) MetaSet.MainForm.textBox2.Text = MetaSet.File.Tag.AlbumArtists[0];
            else MetaSet.MainForm.textBox2.Text = "";
            MetaSet.MainForm.textBox3.Text = MetaSet.File.Tag.Album;
            MetaSet.MainForm.textBox4.Text = MetaSet.File.Tag.Year.ToString();
            MetaSet.MainForm.textBox5.Text = MetaSet.File.Tag.Track.ToString();
            if (MetaSet.File.Tag.Genres.Length > 0) MetaSet.MainForm.textBox6.Text = MetaSet.File.Tag.Genres[0];
            else MetaSet.MainForm.textBox6.Text = "";
            MetaSet.MainForm.textBox7.Text = MetaSet.File.Tag.Lyrics;
            MetaSet.MainForm.textBox9.Text = MetaSet.File.Tag.Copyright;
            if (MetaSet.File.Tag.Composers.Length > 0) MetaSet.MainForm.textBox8.Text = MetaSet.File.Tag.Composers[0];
            else MetaSet.MainForm.textBox8.Text = "";
            MetaSet.MainForm.textBox10.Text = MetaSet.File.Tag.Disc.ToString();
            if (WeCanGetTag()) MetaSet.MainForm.checkBox1.Checked = (((TagLib.Id3v2.Tag)MetaSet.File.GetTag(TagLib.TagTypes.Id3v2)).GetTextAsString("TCMP") == "1");
            else MetaSet.MainForm.checkBox1.Enabled = false;
            if (MetaSet.File.Tag.Performers is string[]) MetaSet.MainForm.textBox11.Text = String.Join(",", MetaSet.File.Tag.Performers);
            else MetaSet.MainForm.textBox11.Text = "";
            MetaSet.MainForm.textBox12.Text = MetaSet.File.Tag.BeatsPerMinute.ToString();
            MetaSet.MainForm.saveToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.textBox13.Text = MetaSet.File.Tag.ISRC;
            MetaSet.MainForm.saveAsToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.textBox14.Text = MetaSet.File.Tag.AmazonId;
            MetaSet.MainForm.textBox15.Text = MetaSet.File.Tag.RemixedBy;
            MetaSet.MainForm.saveMetadataAsToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.loadMetadataFromAFileToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.textBox16.Text = MetaSet.File.Tag.DiscCount.ToString();
            MetaSet.MainForm.textBox17.Text = MetaSet.File.Tag.TrackCount.ToString();
            MetaSet.MainForm.textBox18.Text = MetaSet.File.Tag.Conductor;
            MetaSet.MainForm.textBox19.Text = MetaSet.File.Tag.InitialKey;
            MetaSet.MainForm.textBox20.Text = MetaSet.File.Tag.Publisher;
            MetaSet.MainForm.deleteAnyTagsInFileToolStripMenuItem.Enabled = true;

            if (MetaSet.File.Tag.Pictures.Length > 0)
            {
                if (MetaSet.File.Tag.Pictures[0].Data.Count < 1) { return 0; }
                if (MetaSet.File.Tag.Pictures[0].MimeType == "image/jpg") MetaSet.File.Tag.Pictures[0].MimeType = "image/jpeg";
                using (MemoryStream ms = new MemoryStream(MetaSet.File.Tag.Pictures[0].Data.Data))
                {
                    MetaSet.MainForm.pictureBox1.Image = System.Drawing.Image.FromStream(ms);
                }
                if (MetaSet.File.Tag.Pictures[0].MimeType.Length < 1)
                {
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    MetaSet.File.Tag.Pictures[0].MimeType = codecs.First(codec => codec.FormatID == MetaSet.MainForm.pictureBox1.Image.RawFormat.Guid).MimeType;
                }
                MetaSet.MainForm.imagetype.Text = ((MetaSet.File.Tag.Pictures[0].MimeType.Length > 0) ? MetaSet.File.Tag.Pictures[0].MimeType : "");
                MetaSet.MainForm.imagesize.Text = ((MetaSet.MainForm.pictureBox1.Image is System.Drawing.Image) ? MetaSet.MainForm.pictureBox1.Image.Size.Width + "x" + MetaSet.MainForm.pictureBox1.Image.Size.Height : "");
                //Thanks for Ben Allred from stackoverflow.com
            }
            else MetaSet.MainForm.imagetype.Text = "";
            
            return 0;
        }
    }
}
