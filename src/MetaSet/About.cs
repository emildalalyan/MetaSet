using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetaSet
{
    public partial class About : Form
    {
        /// <summary>
        /// This are using for the easter egg :)
        /// </summary>
        private (string Name, Color KeyColor)[] SomeFavoriteSongs { get; } =
        {
            ("Come As You Are", Color.SteelBlue),
            ("Lithium", Color.SteelBlue),
            ("Lounge Act", Color.SteelBlue),
            ("Drain You", Color.SteelBlue),
            ("Dumb", Color.SaddleBrown),
            ("Heart-Shaped Box", Color.SaddleBrown),
            ("Rape Me", Color.SaddleBrown),
            ("Polly", Color.SteelBlue),
            ("D-7", Color.YellowGreen),
            ("Sliver", Color.SandyBrown),
            ("Something In The Way", Color.SteelBlue),
            ("You Know You're Right", Color.Black),
            ("The Man Who Sold The World", Color.SaddleBrown),
            ("Where Did You Sleep Last Night", Color.SaddleBrown)
        };

        /// <summary>
        /// Creates an instance of <see cref="About"/> form.
        /// </summary>
        public About()
        {
            InitializeComponent();
            this.VersionLabel.Text = MetaSet.Version;
#if NET5_0
            this.VersionLabel.Text += Properties.Resources.RunningOnNET5;
#elif NET6_0
            this.VersionLabel.Text += Properties.Resources.RunningOnNET6;
#endif
        }

        private void TitleLabel_DoubleClick(object sender, EventArgs e)
        {
            Random random = new();

            int element = random.Next(0, SomeFavoriteSongs.Length - 1);

            TitleLabel.Text = SomeFavoriteSongs[element].Name;
            TitleLabel.ForeColor = SomeFavoriteSongs[element].KeyColor;
        }
    }
}
