using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace MetaSet
{
    public partial class TrackProperties : Form
    {
        /// <summary>
        /// Creates an instance of <see cref="TrackProperties"/> form.
        /// </summary>
        public TrackProperties()
        {
            const int MaxSymbolsInName = 30;

            InitializeComponent();

            this.label1.Text = (MetaSet.MainForm.MetaFile.Tag.Title ?? Path.GetFileName(MetaSet.MainForm.MetaFile.Name));
            
            if(this.label1.Text.Length > MaxSymbolsInName) this.label1.Text = this.label1.Text.Substring(0, MaxSymbolsInName) + "...";

            this.label3.Text = MetaSet.MainForm.MetaFile.Properties.AudioBitrate.IfZeroOrLessReturnNA(" KB/s");
            this.label4.Text = MetaSet.MainForm.MetaFile.Properties.AudioChannels.IfZeroOrLessReturnNA();
            this.label6.Text = MetaSet.MainForm.MetaFile.Properties.AudioSampleRate.IfZeroOrLessReturnNA(" Hz");

            string codecs = string.Empty;

            if(MetaSet.MainForm.MetaFile.Properties.Codecs != null)
            {
                foreach (TagLib.ICodec one in MetaSet.MainForm.MetaFile.Properties.Codecs)
                {
                    if (one == null) continue;

                    codecs += ((codecs.Length > 0) ? ", " : "") + one.Description;
                }
            }

            this.label8.Text = codecs.IfNullReturnNA();
            this.label10.Text = MetaSet.MainForm.MetaFile.Properties.Duration.ToString("mm\\:ss").IfNullReturnNA();
            this.pictureBox1.Image = MetaSet.MainForm.pictureBox1.Image;
            this.label12.Text = MetaSet.MainForm.MetaFile.Properties.BitsPerSample.IfZeroOrLessReturnNA(" Bit(s)");
        }
    }
}
