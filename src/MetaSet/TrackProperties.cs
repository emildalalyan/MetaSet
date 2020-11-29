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
            this.label3.Text = Additional.IfNullReturnNA(MetaSet.File.Properties.AudioBitrate, " KB/s");
            this.label4.Text = Additional.IfNullReturnNA(MetaSet.File.Properties.AudioChannels);
            this.label6.Text = Additional.IfNullReturnNA(MetaSet.File.Properties.AudioSampleRate, " Hz");

            string codecs = "";
            foreach(var one in MetaSet.File.Properties.Codecs.ToArray())
            {
                if (codecs.Length < 1) codecs = one.Description;
                else codecs += ", " + one.Description;
            }

            this.label8.Text = Additional.IfNullReturnNA(codecs);
            this.label10.Text = Additional.IfNullReturnNA(MetaSet.File.Properties.Duration.ToString("mm\\:ss"));
            this.pictureBox1.Image = MetaSet.MainForm.pictureBox1.Image;
            this.label12.Text = Additional.IfNullReturnNA(MetaSet.File.Properties.BitsPerSample, " Bit(s)");
        }
    }
}
