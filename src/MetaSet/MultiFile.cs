using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MetaSet
{
    /// <summary>
    /// Multi File Manager
    /// </summary>
    public partial class MultiFileManager : Form
    {
        private readonly List<string> files = new List<string>();

        public MultiFileManager()
        {
            InitializeComponent();
        }

        private void listfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.OpenFile(files[(sender as ListBox).SelectedIndex]);
        }

        /// <summary>
        /// Event called when Form is shown, it opens <see cref="FolderBrowserDialog"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiFile_Shown(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                Description = Properties.Resources.FolderDialogCaption
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedPath.Length > 0)
                {
                    foreach (string file in Directory.EnumerateFiles(dialog.SelectedPath))
                    {
                        if (!MetaSet.FormatSupport.Contains(Path.GetExtension(file))) continue;

                        listfiles.Items.Add(Path.GetFileName(file));
                        files.Add(file);
                    }
                    if (files.Count < 1)
                    {
                        this.Close();
                        return;
                    }
                    quantity.Text = files.Count.IfNullReturnNA();
                    this.Text = dialog.SelectedPath.Length > 0 ? $"{dialog.SelectedPath} — MetaSet" : "MetaSet";
                    this.Show();
                }
                else this.Close();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listfiles.SelectedItem == null) return;
            System.Diagnostics.Process.Start(files[listfiles.SelectedIndex]);
        }

        private void copyFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listfiles.SelectedItem == null) return;
            Clipboard.SetText(listfiles.SelectedItem.ToString());
        }

        private void copyAbsoluteFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listfiles.SelectedItem == null) return;
            Clipboard.SetText(files[listfiles.SelectedIndex]);
        }
    }
}
