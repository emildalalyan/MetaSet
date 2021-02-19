using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace MetaSet
{
    public partial class TrackProperties : Form
    {
        public TrackProperties()
        {
            InitializeComponent();
            this.label1.Text = MetaSet.File.Tag.Title ?? Path.GetFileName(MetaSet.File.Name);
            this.label3.Text = MetaSet.File.Properties.AudioBitrate.IfNullReturnNA(" KB/s");
            this.label4.Text = MetaSet.File.Properties.AudioChannels.IfNullReturnNA();
            this.label6.Text = MetaSet.File.Properties.AudioSampleRate.IfNullReturnNA(" Hz");

            string codecs = "";
            foreach (TagLib.ICodec one in MetaSet.File.Properties.Codecs) codecs += ((codecs.Length > 0) ? ", " : "") + one.Description;

            this.label8.Text = codecs.IfNullReturnNA();
            this.label10.Text = MetaSet.File.Properties.Duration.ToString("mm\\:ss").IfNullReturnNA();
            this.pictureBox1.Image = MetaSet.MainForm.pictureBox1.Image;
            this.label12.Text = MetaSet.File.Properties.BitsPerSample.IfNullReturnNA(" Bit(s)");
        }
    }
}
