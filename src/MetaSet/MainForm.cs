using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Threading;

namespace MetaSet;

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
#nullable enable
	public Image? Cover
#nullable disable
	{
		get => MetaSet.MainForm.pictureBox1.Image;

		set
		{
			Image previous = MetaSet.MainForm.pictureBox1.Image;

			MetaSet.MainForm.pictureBox1.Image = value;

			// We're disposing previous image, because it won't be used later.
			if (previous != null) previous.Dispose();

			MetaSet.MainForm.imagesize.Text = (value != null) ? (value.Size.Width + "x" + value.Size.Height) : string.Empty;
			MetaFile.Tag.Pictures = (value != null) ? new TagLib.Picture[]
			{
				new()
				{
					MimeType = value.RawFormat.GetMimeType(),
					Type = TagLib.PictureType.Media,
					Data = value.ToByteArray()
				}
			} : Array.Empty<TagLib.IPicture>();
			MetaSet.MainForm.imagetype.Text = (value != null) ? MetaFile.Tag.Pictures[0].MimeType : string.Empty;
		}
	}

	/// <summary>
	/// Creates new instance of <see cref="MainForm"/>
	/// </summary>
	public MainForm(string[] args)
	{
		InitializeComponent();

		Trace.Listeners.Add(Program.MessageTrace);

#if DEBUG
		Trace.Listeners.Add(Program.ConsoleTrace);

		Trace.WriteLine("[Log] DEBUG macro is defined, ConsoleTraceListener is added to listeners.");
#endif

		MetaSet.MainForm = this;

		Trace.WriteLine($"[Log] MetaSet version: {MetaSet.Version}");

#region Program Arguments

		if (Program.ProvidedArguments.Length == 0) return;

		for(int i = 0; i < Program.ProvidedArguments.Length; i++)
		{
			if (Program.ProvidedArguments[i] == "--location")
			{
				if (!int.TryParse(Program.ProvidedArguments[++i], out int x)
				||  !int.TryParse(Program.ProvidedArguments[++i], out int y))
				{
					continue;
				}

				Location = new(x, y);
				StartPosition = FormStartPosition.Manual;
			}
#if !DEBUG
			else if (Program.ProvidedArguments[i] == "--tracecon")
			{
				Trace.Listeners.Add(Program.ConsoleTrace);

				Trace.WriteLine("[Log] DEBUG macro is not defined, but ConsoleTraceListener is added to listeners (--tracecon).");
			}
#endif
			else if (Program.ProvidedArguments[i] == "--help")
            {
				Trace.WriteLine("[Info] " +
                    "Console arguments help:\n" +
                    "\n\t--location [x] [y] \t: Changes window location" +
                    "\n\t--tracecon \t\t: Add ConsoleTraceListener to listeners (only if DEBUG isn't defined)" +
                    "\n\t--help \t\t\t: Write this help" +
                    "\n\n\tUsage: metaset [arguments] [file name]");
			}
			else OpenFile(Program.ProvidedArguments[i]);
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
		
		Image img = this.pictureBox1.Image;
		this.pictureBox1.Image = null;
		if (img != null) img.Dispose();

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
		this.delcoverbtn.Enabled = false;
		this.loadfromcbbtn.Enabled = false;
		this.loadfromimage.Enabled = false;
		this.savetocbbtn.Enabled = false;
		this.savetofilebtn.Enabled = false;
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
		const int WindowOffset = 32;

		Process.Start(Application.ExecutablePath, $"--location {this.Location.X + WindowOffset} {this.Location.Y + WindowOffset}");
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
			Trace.WriteLine($"[Log] Image.Save() throws an exception with message: {ex.Message}... Trying another method...");

			try
			{
				MetaFile.Tag.Pictures[0].Save(a.FileName);
			}
			catch (Exception ex2)
			{
				Trace.WriteLine("[Warning] Errors with cover has occurred.\n" +
					$"Method 1 — Image.Save(): {ex.Message}\n" +
					$"Method 2 — File.Tag.Pictures[0].Save(): {ex2.Message}");
			}
		}
	}

	private void Form1_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = DragDropEffects.All;
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
			Trace.WriteLine($"[Warning] Error was appeared! Error message was: {ex.Message}");
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
			Trace.WriteLine("[Warning] System.IO.IOException was appeared. Maybe, this file used by another application.");
		}
		catch (Exception e)
		{
			Trace.WriteLine($"[Warning] {e.GetType().Name} was appeared. HRESULT was: 0x{e.HResult:X}");
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

		if (a.ShowDialog() == DialogResult.OK && File.Exists(a.FileName))
		{
            try
			{
				Cover = Image.FromFile(a.FileName);
			}
			catch (Exception ex)
            {
				Trace.WriteLine($"[Error] Selected image couldn't be loaded. Thrown exception is: {ex.Message}");
            }
		}
	}

	private void delcoverbtn_Click(object sender, EventArgs e)
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
		const int WindowOffset = 8;

		new MultiFileManager()
        {
			Location = new Point(this.Location.X + this.Width + WindowOffset, this.Location.Y),
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
			Trace.WriteLine($"[Warning] Error was appeared. Error message was: {e.Message}. HRESULT: 0x{e.HResult:X}");
		}
	}

	/// <summary>
	/// Calls <see cref="OpenFileDialog"/> and then opens specified file
	/// </summary>
	public void OpenFile()
	{
		using OpenFileDialog a = new()
		{
			Filter = $"All Supported Formats ({MetaSet.SupportedFormatsFilter})|{MetaSet.SupportedFormatsFilter}",
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

		string extension = Path.GetExtension(location);

		if (!MetaSet.FormatSupport.Contains(extension))
		{
			Trace.WriteLine($"[Warning] Sorry, but {extension} file format is not supported.");
			return;
		}

		CloseFile();

		try
		{
			MetaFile = TagLib.File.Create(location);
			string filename = Path.GetFileName(location);
			Trace.WriteLine($"[Log] Creating file through TagLib#. File name is {filename}...");
			Text = $"MetaSet — {filename}";
			ReadTags();
		}
		catch (Exception e)
		{
			Trace.WriteLine($"[Warning] {e.GetType().Name} was appeared.");
		}
	}

	/// <summary>
	/// Read metafile (<see cref="TagLib.File"/>) tags and show their values.
	/// </summary>
	public void ReadTags()
	{
		Trace.WriteLine("[Log] Start of reading tags...");

		MetaFile.Mode = TagLib.File.AccessMode.Read;
		copyInformationAboutTrackToolStripMenuItem.Enabled = true;
		closeToolStripMenuItem.Enabled = true;
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
		checkBox1.Checked = MetaFile.ID3v2Supported() && ((TagLib.Id3v2.Tag)MetaFile.GetTag(TagLib.TagTypes.Id3v2)).GetTextAsString("TCMP") == "1";
		checkBox1.Enabled = MetaFile.ID3v2Supported();
		textBox11.Text = (MetaFile.Tag.Performers != null) ? string.Join(",", MetaFile.Tag.Performers ?? Array.Empty<string>()) : string.Empty;
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

		delcoverbtn.Enabled = true;
		loadfromcbbtn.Enabled = true;
		loadfromimage.Enabled = true;
		savetocbbtn.Enabled = true;
		savetofilebtn.Enabled = true;

		if (MetaFile.Tag.Pictures != null && MetaFile.Tag.Pictures.Length > 0)
		{
            if (MetaFile.Tag.Pictures[0].Type == TagLib.PictureType.NotAPicture && MetaFile.Tag.Pictures.Length > 1)
            {
				int first_img_index = 0;

				for(int i = 0; i < MetaFile.Tag.Pictures.Length; i++)
                {
					if (MetaFile.Tag.Pictures[i].Type != TagLib.PictureType.NotAPicture)
					{
						first_img_index = i;
						break;
					}
				}

				MetaFile.Tag.Pictures = new TagLib.IPicture[]
				{
					MetaFile.Tag.Pictures[first_img_index]
				};
            }

			if (MetaFile.Tag.Pictures[0].Data.Count < 1) return;

			if (MetaFile.Tag.Pictures[0].MimeType == "image/jpg")
			{
				MetaFile.Tag.Pictures[0].MimeType = "image/jpeg";
				Trace.WriteLine("[Info] Mime-type of cover is \"image/jpg\", some players don't show cover if it's \"image/jpg\", so we're changing it to \"image/jpeg\".");
			}
			// Some players don't show cover if mime-type is "image/jpg", but everything is normal if it's "image/jpeg"

			using (MemoryStream ms = new(MetaFile.Tag.Pictures[0].Data.Data)) pictureBox1.Image = Image.FromStream(ms);

			if (MetaFile.Tag.Pictures[0].MimeType.Length < 1)
			{
				MetaFile.Tag.Pictures[0].MimeType = pictureBox1.Image.RawFormat.GetMimeType();
			}
			imagetype.Text = (MetaFile.Tag.Pictures[0].MimeType.Length > 0) ? MetaFile.Tag.Pictures[0].MimeType : string.Empty;
			imagesize.Text = (Cover != null) ? (Cover.Size.Width + "x" + Cover.Size.Height) : string.Empty;
		}
		else imagetype.Text = imagesize.Text = string.Empty;

		Trace.WriteLine("[Log] Tags are read and parsed...");
	}

    private void loadfromcbbtn_Click(object sender, EventArgs e)
    {
		using Image img = Clipboard.GetImage();

		if (img == null) return;

		using MemoryStream ms = new();
		img.Save(ms, ImageFormat.Bmp);
		// We're saving an image as BMP in memory stream
		// (it's needed for changing the format from MemoryBmp to Bmp)

		Cover = Image.FromStream(ms);
    }
}
