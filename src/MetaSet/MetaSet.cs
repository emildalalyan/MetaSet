﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Media;

namespace MetaSet
{
    static class MetaSet
    {
        static public readonly string Version = "1.0-stable";
        static public TagLib.File File;
        static public Form1 MainForm;
        static public readonly string[] FormatSupport = new string[]
        {
            ".mp3", ".flac", ".ogg", ".wav", ".wma", ".m4a"
        };
        
        static public void AboutProgram()
        {
            new About().ShowDialog();
            //MessageBox.Show($"MetaSet v{Version}\n----------------------\nBy Emil Dalalyan\nLicense is GNU LGPL v3.\nProgram based on TagLibSharp library.", "About MetaSet...");
        }
        static public void CreateInstance()
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath, $"-s {MainForm.Location.X + 32} {MainForm.Location.Y + 32}");
        }
    }
    static class Additional
    {
        static public byte[] ImageToArrayOfBytes(System.Drawing.Image img)
        {
            using(MemoryStream m = new MemoryStream())
            {
                img.Save(m, img.RawFormat);
                return m.ToArray();
            }
        }
        static public string IfNullReturnNA(string str, string plus = "")
        {
            if (str == null) return "N/A";
            return (str.Length > 0 ? str + plus : "N/A");
        }
        static public string IfNullReturnNA(string[] str, int num, string plus = "")
        {
            if (str == null) return "N/A";
            return (str.Length > num ? str[num] + plus : "N/A");
        }
        static public string IfNullReturnNA(int dec, string plus = "")
        {
            if (dec < 1) return "N/A";
            else return dec.ToString() + plus;
        }
    }
    static class Functions
    {
        static public void CopyInfoAboutTrack()
        {
            Clipboard.SetText("Title: " + Additional.IfNullReturnNA(MetaSet.File.Tag.Title)
                + "\nArtist: " + Additional.IfNullReturnNA(MetaSet.File.Tag.AlbumArtists, 0)
                + "\nAlbum: " + Additional.IfNullReturnNA(MetaSet.File.Tag.Album)
                + "\nCopyright: " + Additional.IfNullReturnNA(MetaSet.File.Tag.Copyright)
                + "\nComposer: " + Additional.IfNullReturnNA(MetaSet.File.Tag.Composers, 0));
        }
        static public void DeleteACover()
        {
            MetaSet.File.Tag.Pictures = null;
            MetaSet.MainForm.pictureBox1.Image = null;
        }
        static public bool WeMayToGetTag()
        {
            if (!IsOpened()) return false;
            return (MetaSet.File.GetTag(TagLib.TagTypes.Id3v2) is TagLib.Tag);
        }
        static public int ChangeACover()
        {
            using (OpenFileDialog a = new OpenFileDialog
            {
                Filter = "JPEG (*.jpg)|*.jpg"
            })
            {
                if (a.ShowDialog() == DialogResult.OK)
                {
                    MetaSet.MainForm.pictureBox1.Image = System.Drawing.Image.FromFile(a.FileName);

                    MetaSet.File.Tag.Pictures = new TagLib.Picture[1] {
                        new TagLib.Picture
                        {
                            MimeType = "image/jpeg",
                            Type = TagLib.PictureType.Media,
                            Data = Additional.ImageToArrayOfBytes(MetaSet.MainForm.pictureBox1.Image)
                        }
                    };
                }
            }
            return 0;
        }
        static public int ChangeACover(string filename)
        {
            MetaSet.MainForm.pictureBox1.Image = System.Drawing.Image.FromFile(filename);

            MetaSet.File.Tag.Pictures = new TagLib.Picture[1] {
                new TagLib.Picture
                {
                    MimeType = "image/jpeg",
                    Type = TagLib.PictureType.Media,
                    Data = Additional.ImageToArrayOfBytes(MetaSet.MainForm.pictureBox1.Image)
                }
            };
            return 0;
        }
        static public void ExtractCover()
        {
            if (MetaSet.MainForm.pictureBox1.Image == null) return;

            using (SaveFileDialog a = new SaveFileDialog
            {
                Filter = "JPEG (*.jpg)|*.jpg"
            })
            {
                if (a.ShowDialog() == DialogResult.OK && a.FileName.Length > 0)
                {
                    MetaSet.MainForm.pictureBox1.Image.Save(a.FileName, ImageFormat.Jpeg);
                }
            }
                
        }
        static public int CloseFile()
        {
            if (IsOpened()) MetaSet.File.Dispose();

            MetaSet.File =              null;
            MetaSet.MainForm.Text =     "MetaSet";
            MetaSet.MainForm.saveToolStripMenuItem.Enabled = false;
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
            MetaSet.MainForm.checkBox1.Checked =    false;
            MetaSet.MainForm.checkBox1.Enabled =    true;
            MetaSet.MainForm.pictureBox1.Image =    null;
            MetaSet.MainForm.textBox12.Text =       "";
            return 0;
        }
        static public bool IsOpened()
        {
            return MetaSet.File is TagLib.File;
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
                Filter = "All Supported Formats (*.mp3;*.flac;*.ogg;*.wav;*.wma;*.m4a)|*.mp3;*.flac;*.ogg;*.wav;*.wma;*.m4a"
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
                    MetaSet.MainForm.Text = "MetaSet — " + Path.GetFileName(MetaSet.File.Name);
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
            catch(System.IO.IOException e)
            {
                MessageBox.Show(e.GetType().Name + " was appeared. Maybe, this file used by another application.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        static public int ReadIt()
        {
            MetaSet.MainForm.copyInformationAboutTrackToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.closeToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.extractACoverToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.playATrackToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.checkAPropertiesToolStripMenuItem.Enabled = true;
            MetaSet.MainForm.textBox1.Text = MetaSet.File.Tag.Title;
            if(MetaSet.File.Tag.AlbumArtists.Length > 0) MetaSet.MainForm.textBox2.Text = MetaSet.File.Tag.AlbumArtists[0];
            MetaSet.MainForm.textBox3.Text = MetaSet.File.Tag.Album;
            MetaSet.MainForm.textBox4.Text = MetaSet.File.Tag.Year.ToString();
            MetaSet.MainForm.textBox5.Text = MetaSet.File.Tag.Track.ToString();
            if (MetaSet.File.Tag.Genres.Length > 0) MetaSet.MainForm.textBox6.Text = MetaSet.File.Tag.Genres[0];
            MetaSet.MainForm.textBox7.Text = MetaSet.File.Tag.Lyrics;
            MetaSet.MainForm.textBox9.Text = MetaSet.File.Tag.Copyright;
            if (MetaSet.File.Tag.Composers.Length > 0) MetaSet.MainForm.textBox8.Text = MetaSet.File.Tag.Composers[0];
            MetaSet.MainForm.textBox10.Text = MetaSet.File.Tag.Disc.ToString();
            if (WeMayToGetTag()) MetaSet.MainForm.checkBox1.Checked = (((TagLib.Id3v2.Tag)MetaSet.File.GetTag(TagLib.TagTypes.Id3v2)).GetTextAsString("TCMP") == "1");
            else MetaSet.MainForm.checkBox1.Enabled = false;
            if(MetaSet.File.Tag.Performers is string[]) MetaSet.MainForm.textBox11.Text = String.Join(",", MetaSet.File.Tag.Performers);
            MetaSet.MainForm.textBox12.Text = MetaSet.File.Tag.BeatsPerMinute.ToString();
            MetaSet.MainForm.saveToolStripMenuItem.Enabled = true;

            if (MetaSet.File.Tag.Pictures.Length > 0)
            {
                if (MetaSet.File.Tag.Pictures[0].Data.Count < 1) { return 0; }
                using (MemoryStream ms = new MemoryStream(MetaSet.File.Tag.Pictures[0].Data.Data))
                {
                    MetaSet.MainForm.pictureBox1.Image = System.Drawing.Image.FromStream(ms);
                }
                //Thanks for Ben Allred from stackoverflow.com
            }
            
            return 0;
        }
    }
}