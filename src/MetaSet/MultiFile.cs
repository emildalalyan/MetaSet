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
        /// <summary>
        /// List of files that you can see in this instance of <see cref="MultiFileManager"/>.
        /// </summary>
        private List<string> FilesList { get; } = new();

        /// <summary>
        /// Creates an instance of <see cref="MultiFileManager"/>
        /// </summary>
        public MultiFileManager()
        {
            InitializeComponent();
        }

        private void listfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            MetaSet.MainForm.OpenFile(FilesList[(sender as ListBox).SelectedIndex]);
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
                    FilesList.Add(file);
                }
                if (FilesList.Count < 1)
                {
                    this.Close();
                    return;
                }
                quantity.Text = FilesList.Count.IfZeroOrLessReturnNA();
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
                FileName = FilesList[listfiles.SelectedIndex],
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
            Clipboard.SetText(FilesList[listfiles.SelectedIndex]);
        }
    }
}
