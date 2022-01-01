using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;

namespace MetaSet
{
	/// <summary>
	/// Represents <see cref="MetaSet"/> main form
	/// </summary>
    public partial class MainForm : Form
    {
		/// <summary>
		/// Represents currently opened file with metadata
		/// </summary>
		public TagLib.File MetaFile { get; private set; }

		/// <summary>
		/// Represents <see cref="TagLib.Tag"/> first picture (cover).
		/// </summary>
		public Image Cover
		{
			get => MetaSet.MainForm.pictureBox1.Image;

			set
			{
				MetaSet.MainForm.pictureBox1.Image = value;
				MetaSet.MainForm.imagesize.Text = (value != null) ? (value.Size.Width + "x" + value.Size.Height) : string.Empty;
				MetaFile.Tag.Pictures = (value != null) ? new TagLib.Picture[]
				{
					new()
					{
						MimeType = value.RawFormat.GetMimeType(),
						Type = TagLib.PictureType.Media,
						Data = value.ToArrayOfBytes()
					}
				} : Array.Empty<TagLib.IPicture>();
				MetaSet.MainForm.imagetype.Text = (value != null) ? MetaFile.Tag.Pictures[0].MimeType : string.Empty;
			}
		}

		/// <summary>
		/// Creates new instance of <see cref="MainForm"/>
		/// </summary>
		/// <param name="args">Represents program startup arguments</param>
		public MainForm(string[] args)
		{
			InitializeComponent();
			MetaSet.MainForm = this;

#region Program Arguments

            if (args.Length == 0) return;

			try
			{
				if (args[0] == "-s")
				{
					Location = new(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
					StartPosition = FormStartPosition.Manual;
				}
				else OpenFile(args[0]);
			}
			catch
			{
				MessageBox.Show("Invalid arguments!", "MetaSet", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}

#endregion
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = MetaSet.DonateLink,
				UseShellExecute = true
			});
		}

		/// <summary>
		/// Close currently opened metafile
		/// </summary>
		public void CloseFile()
		{
			if (MetaFile != null) this.MetaFile.Dispose();

			this.MetaFile = null;
			this.Text = "MetaSet";
			this.saveToolStripMenuItem.Enabled = false;
			this.closeToolStripMenuItem.Enabled = false;
			this.extractACoverToolStripMenuItem.Enabled = false;
			this.copyInformationAboutTrackToolStripMenuItem.Enabled = false;
			this.playATrackToolStripMenuItem.Enabled = false;
			this.checkAPropertiesToolStripMenuItem.Enabled = false;
			this.textBox1.Text = string.Empty;
			this.textBox2.Text = string.Empty;
			this.textBox3.Text = string.Empty;
			this.textBox4.Text = string.Empty;
			this.textBox5.Text = string.Empty;
			this.textBox6.Text = string.Empty;
			this.textBox7.Text = string.Empty;
			this.textBox8.Text = string.Empty;
			this.textBox9.Text = string.Empty;
			this.textBox10.Text = string.Empty;
			this.textBox11.Text = string.Empty;
			this.textBox13.Text = string.Empty;
			this.checkBox1.Checked = false;
			this.checkBox1.Enabled = true;
			this.pictureBox1.Image = null;
			this.textBox12.Text = string.Empty;
			this.textBox14.Text = string.Empty;
			this.textBox15.Text = string.Empty;
			this.saveAsToolStripMenuItem.Enabled = false;
			this.saveMetadataAsToolStripMenuItem.Enabled = false;
			this.loadMetadataFromAFileToolStripMenuItem.Enabled = false;
			this.textBox16.Text = string.Empty;
			this.textBox17.Text = string.Empty;
			this.textBox18.Text = string.Empty;
			this.textBox19.Text = string.Empty;
			this.textBox20.Text = string.Empty;
			this.imagetype.Text = string.Empty;
			this.imagesize.Text = string.Empty;
			this.deleteAnyTagsInFileToolStripMenuItem.Enabled = false;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			SaveFile();
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseFile();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.Title = textBox1.Text;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new About().ShowDialog();
		}

		private void createNewInstanceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start(Application.ExecutablePath, $"-s {this.Location.X + 32} {this.Location.Y + 32}");
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			OpenFile(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
		}

		private void playATrackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile != null && File.Exists(MetaFile.Name))
			{
				Process.Start(new ProcessStartInfo()
				{
					FileName = MetaFile.Name,
					UseShellExecute = true
				});
			}
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.AlbumArtists = new string[1] { textBox2.Text };
		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.Album = textBox3.Text;
		}

		private void textBox4_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			if (!uint.TryParse(textBox4.Text, out uint value)) textBox4.Text = "0";
			MetaFile.Tag.Year = value;
		}

		private void textBox5_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			if (!uint.TryParse(textBox5.Text, out uint value)) textBox5.Text = "0";
			MetaFile.Tag.Track = value;
		}

		private void textBox6_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.Genres = new string[1] { textBox6.Text };
		}

		private void textBox7_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.Lyrics = textBox7.Text;
		}

		private void textBox9_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.Copyright = textBox9.Text;
		}

		private void textBox8_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.Composers = new string[1] { textBox8.Text };
		}

		private void textBox10_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			if (!uint.TryParse(textBox10.Text, out uint value))
			{
				textBox10.Text = "0";
			}
			
			MetaFile.Tag.Disc = value;
		}

		private void extractACoverToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile == null || Cover == null) return;

			string extension = MetaFile.Tag.Pictures[0].MimeType.Split('/')[1];

			using SaveFileDialog a = new()
			{
				Filter = $"{MetaFile.Tag.Pictures[0].MimeType} (*.{extension})|*.{extension}"
			};
			
			if (a.ShowDialog() != DialogResult.OK || a.FileName.Length < 1) return;

			try
			{
				Cover.Save(a.FileName);
			}
			catch (Exception ex)
			{
				try
				{
					MetaFile.Tag.Pictures[0].Save(a.FileName);
				}
				catch (Exception ex2)
				{
					MessageBox.Show("Errors with cover has occurred.\n" +
						$"Method 1 — Image.Save(): {ex.Message}\n" +
						$"Method 2 — File.Tag.Pictures[0].Save(): {ex2.Message}", "Unexpected error.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = MetaSet.RepoLink,
				UseShellExecute = true
			});
		}

		private void checkAPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			using TrackProperties form = new();

			form.ShowDialog();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (MetaFile == null || !MetaFile.ID3v2Supported()) return;

			((TagLib.Id3v2.Tag)MetaFile.GetTag(TagLib.TagTypes.Id3v2)).SetTextFrame("TCMP", checkBox1.Checked ? "1" : "0");	
		}

		private void textBox11_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.Performers = textBox11.Text.Split(',');
		}

		private void textBox12_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			if (!uint.TryParse(textBox12.Text, out uint value))
			{
				textBox12.Text = "0";
			}
			MetaFile.Tag.BeatsPerMinute = value;
		}

		private void textBox13_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.ISRC = textBox13.Text;
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string extension = Path.GetExtension(MetaFile.Name);

			using SaveFileDialog sfd = new()
			{
				Filter = $"File (*{extension})|*{extension}"
			};

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				MetaFile.Save(sfd.FileName);
				OpenFile(sfd.FileName);
			}
		}

		private void textBox14_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.AmazonId = textBox14.Text;	
		}

		private void textBox15_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			MetaFile.Tag.RemixedBy = textBox15.Text;
		}

		private void saveMetadataAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			try
			{
				using SaveFileDialog sfd = new()
				{
					Filter = "Metadata File (*.met)|*.met",
					InitialDirectory = (MetaFile != null) ? Path.GetDirectoryName(MetaFile.Name) : string.Empty
				};

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					if (!File.Exists(sfd.FileName)) File.Create(sfd.FileName).Close();

					MetaFile.Tag.PutIntoFile(sfd.FileName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error was appeared! Error message was: {ex.Message}");
			}
		}

		private void loadMetadataFromAFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			LoadMetaFromFile();
		}

		private void textBox16_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			if (!uint.TryParse(textBox16.Text, out uint value))
			{
				textBox16.Text = "0";
			}
			MetaFile.Tag.DiscCount = value;
		}

		private void textBox17_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			if (!uint.TryParse(textBox17.Text, out uint value))
			{
				textBox17.Text = "0";
			}
			MetaFile.Tag.TrackCount = value;
		}

		private void textBox18_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.Conductor = textBox18.Text;
		}

		private void textBox19_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.InitialKey = textBox19.Text;
		}

		private void textBox20_TextChanged(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			MetaFile.Tag.Publisher = textBox20.Text;
		}

		/// <summary>
		/// Save currently opened <see cref="TagLib.File"/> (metafile) on a disk (just save changes)
		/// </summary>
		public void SaveFile()
		{
			try
			{
				MetaFile.Save();
				SystemSounds.Beep.Play();
			}
			catch (IOException)
			{
				MessageBox.Show("System.IO.IOException was appeared. Maybe, this file used by another application.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.GetType().Name + " was appeared. HRESULT was: 0x" + e.HResult.ToString("X"), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void deleteAnyTagsInFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			RemoveTags();
			ReadTags();		
		}

		private void pictureBox1_MouseEnter(object sender, EventArgs e)
		{
			imagetype.Visible = true;
			imagesize.Visible = true;
		}

		private void pictureBox1_MouseLeave(object sender, EventArgs e)
		{
			imagetype.Visible = false;
			imagesize.Visible = false;
		}

		private void changepict_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			using OpenFileDialog a = new()
			{
				Filter = "All Supported Formats (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Joint Photographic Experts Group (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png"
			};
			if (a.ShowDialog() == DialogResult.OK)
			{
				Cover = Image.FromFile(a.FileName);
			}
		}

		private void dthecov_Click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;

			Cover = null;
		}

		private void copyimg_Click(object sender, EventArgs e)
		{
			if (MetaFile == null || Cover == null) return;
			
			Clipboard.SetImage(Cover);
		}

		private void takescrmenuitem_click(object sender, EventArgs e)
		{
			if (MetaFile == null) return;
			
			using Bitmap screen = this.TakeScreen();
			Clipboard.SetImage(screen);	
		}

		private void OpenCatalogMenuItem_Click(object sender, EventArgs e)
		{
			new MultiFileManager()
            {
				Location = new Point(this.Location.X + this.Width + 8, this.Location.Y),
				Height = this.Height
			}.Show();
		}

		/// <summary>
		/// Remove all tags (including cover) in a metafile
		/// </summary>
		public void RemoveTags()
		{
			Cover = null;
			MetaFile.RemoveTags(TagLib.TagTypes.AllTags);
		}

		/// <summary>
		/// Load metadata from external file on a disk
		/// </summary>
		public void LoadMetaFromFile()
		{
			if (MetaFile == null) return;

			try
			{
				using OpenFileDialog ofd = new()
				{
					Filter = "Metadata File (*.met)|*.met|MPEG-3 Audio File (*.mp3)|*.mp3",
					InitialDirectory = (MetaFile != null) ? Path.GetDirectoryName(MetaFile.Name) : string.Empty
				};

				if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
				{
					MetaFile.Tag.LoadFromFile(ofd.FileName);
					ReadTags();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show($"Error was appeared. Error message was: {e.Message}. HRESULT: 0x{e.HResult.ToString("X")}", "Error occurred in MetaSet", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		/// <summary>
		/// Calls <see cref="OpenFileDialog"/> and then opens specified file
		/// </summary>
		public void OpenFile()
		{
			using OpenFileDialog a = new()
			{
				Filter = "All Supported Formats (*.mp3;*.flac;*.ogg;*.wav;*.wma;*.m4a)|*.mp3;*.flac;*.ogg;*.wav;*.wma;*.m4a",
				InitialDirectory = (MetaFile != null) ? Path.GetDirectoryName(MetaFile.Name) : string.Empty
			};

			if (a.ShowDialog() == DialogResult.OK)
			{
				OpenFile(a.FileName);
			}
		}

		/// <summary>
		/// Open file by the specified location
		/// </summary>
		/// <param name="location"></param>
		public void OpenFile(string location)
		{
			if (location.Length < 1 || !File.Exists(location)) return;

			CloseFile();

			if (!MetaSet.FormatSupport.Contains(Path.GetExtension(location)))
			{
				MessageBox.Show("Sorry, but this file format is not supported.", "MetaSet");
				return;
			}
			try
			{
				MetaFile = TagLib.File.Create(location);
				Text = "MetaSet — " + Path.GetFileName(MetaFile.Name);
				ReadTags();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.GetType().Name + " was appeared.");
			}
		}

		/// <summary>
		/// Read metafile (<see cref="TagLib.File"/>) tags and show their values.
		/// </summary>
		public void ReadTags()
		{
			MetaFile.Mode = TagLib.File.AccessMode.Read;
			copyInformationAboutTrackToolStripMenuItem.Enabled = true;
			closeToolStripMenuItem.Enabled = true;
			extractACoverToolStripMenuItem.Enabled = true;
			playATrackToolStripMenuItem.Enabled = true;
			checkAPropertiesToolStripMenuItem.Enabled = true;
			textBox1.Text = MetaFile.Tag.Title;
			textBox2.Text = (MetaFile.Tag.AlbumArtists.Length > 0) ? MetaFile.Tag.AlbumArtists[0] : string.Empty;
			textBox3.Text = MetaFile.Tag.Album;
			textBox4.Text = MetaFile.Tag.Year.ToString();
			textBox5.Text = MetaFile.Tag.Track.ToString();
			textBox6.Text = (MetaFile.Tag.Genres.Length > 0) ? MetaFile.Tag.Genres[0] : string.Empty;
			textBox7.Text = MetaFile.Tag.Lyrics;
			textBox9.Text = MetaFile.Tag.Copyright;
			textBox8.Text = (MetaFile.Tag.Composers.Length > 0) ? MetaFile.Tag.Composers[0] : string.Empty;
			textBox10.Text = MetaFile.Tag.Disc.ToString();
			checkBox1.Checked = MetaFile.ID3v2Supported() ? ((TagLib.Id3v2.Tag)MetaFile.GetTag(TagLib.TagTypes.Id3v2)).GetTextAsString("TCMP") == "1" : false;
			checkBox1.Enabled = MetaFile.ID3v2Supported();
			textBox11.Text = (MetaFile.Tag.Performers != null) ? string.Join(",", MetaFile.Tag.Performers) : string.Empty;
			textBox12.Text = MetaFile.Tag.BeatsPerMinute.ToString();
			saveToolStripMenuItem.Enabled = true;
			textBox13.Text = MetaFile.Tag.ISRC;
			saveAsToolStripMenuItem.Enabled = true;
			textBox14.Text = MetaFile.Tag.AmazonId;
			textBox15.Text = MetaFile.Tag.RemixedBy;
			saveMetadataAsToolStripMenuItem.Enabled = true;
			loadMetadataFromAFileToolStripMenuItem.Enabled = true;
			textBox16.Text = MetaFile.Tag.DiscCount.ToString();
			textBox17.Text = MetaFile.Tag.TrackCount.ToString();
			textBox18.Text = MetaFile.Tag.Conductor;
			textBox19.Text = MetaFile.Tag.InitialKey;
			textBox20.Text = MetaFile.Tag.Publisher;
			deleteAnyTagsInFileToolStripMenuItem.Enabled = true;
			if (MetaFile.Tag.Pictures.Length > 0)
			{
				if (MetaFile.Tag.Pictures[0].Data.Count < 1) return;

				if (MetaFile.Tag.Pictures[0].MimeType == "image/jpg") MetaFile.Tag.Pictures[0].MimeType = "image/jpeg";
				// Some players don't show cover if mime-type is "image/jpg", but all is normal if it's "image/jpeg"

				using (MemoryStream ms = new(MetaFile.Tag.Pictures[0].Data.Data)) pictureBox1.Image = Image.FromStream(ms);

				if (MetaFile.Tag.Pictures[0].MimeType.Length < 1)
				{
					ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
					MetaFile.Tag.Pictures[0].MimeType = codecs.First((ImageCodecInfo codec) => codec.FormatID == pictureBox1.Image.RawFormat.Guid).MimeType;
				}
				imagetype.Text = (MetaFile.Tag.Pictures[0].MimeType.Length > 0) ? MetaFile.Tag.Pictures[0].MimeType : string.Empty;
				imagesize.Text = (Cover != null) ? (Cover.Size.Width + "x" + Cover.Size.Height) : string.Empty;
			}
			else imagetype.Text = imagesize.Text = string.Empty;
		}
	}
}
