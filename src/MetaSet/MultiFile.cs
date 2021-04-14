using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MetaSet
{
    /// <summary>
    /// Multiple files (directory) manager
    /// </summary>
    public partial class MultiFileManager : Form
    {
        private readonly List<string> files = new();

        public MultiFileManager()
        {
            InitializeComponent();
        }

        private void listfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            MetaSet.MainForm.OpenFile(files[(sender as ListBox).SelectedIndex]);
        }

        /// <summary>
        /// Event called when <see cref="Form"/> is shown, it opens <see cref="FolderBrowserDialog"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiFile_Shown(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new()
            {
                UseDescriptionForTitle = true,
                Description = Properties.Resources.FolderDialogCaption
            };

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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listfiles.SelectedItem == null) return;
            Process.Start(new ProcessStartInfo()
            {
                FileName = files[listfiles.SelectedIndex],
                UseShellExecute = true
            });
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
