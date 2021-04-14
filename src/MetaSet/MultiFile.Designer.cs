
namespace MetaSet
{
    partial class MultiFileManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiFileManager));
            this.listfiles = new System.Windows.Forms.ListBox();
            this.listContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFilenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAbsoluteFilenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quantity = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // listfiles
            // 
            resources.ApplyResources(this.listfiles, "listfiles");
            this.listfiles.ContextMenuStrip = this.listContext;
            this.listfiles.Name = "listfiles";
            this.listfiles.SelectedIndexChanged += new System.EventHandler(this.listfiles_SelectedIndexChanged);
            // 
            // listContext
            // 
            this.listContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.listContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.copyFilenameToolStripMenuItem,
            this.copyAbsoluteFilenameToolStripMenuItem});
            this.listContext.Name = "listContext";
            resources.ApplyResources(this.listContext, "listContext");
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // copyFilenameToolStripMenuItem
            // 
            this.copyFilenameToolStripMenuItem.Name = "copyFilenameToolStripMenuItem";
            resources.ApplyResources(this.copyFilenameToolStripMenuItem, "copyFilenameToolStripMenuItem");
            this.copyFilenameToolStripMenuItem.Click += new System.EventHandler(this.copyFilenameToolStripMenuItem_Click);
            // 
            // copyAbsoluteFilenameToolStripMenuItem
            // 
            this.copyAbsoluteFilenameToolStripMenuItem.Name = "copyAbsoluteFilenameToolStripMenuItem";
            resources.ApplyResources(this.copyAbsoluteFilenameToolStripMenuItem, "copyAbsoluteFilenameToolStripMenuItem");
            this.copyAbsoluteFilenameToolStripMenuItem.Click += new System.EventHandler(this.copyAbsoluteFilenameToolStripMenuItem_Click);
            // 
            // quantity
            // 
            resources.ApplyResources(this.quantity, "quantity");
            this.quantity.Name = "quantity";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Name = "label4";
            // 
            // MultiFileManager
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.quantity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listfiles);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiFileManager";
            this.Shown += new System.EventHandler(this.MultiFile_Shown);
            this.listContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listfiles;
        private System.Windows.Forms.Label quantity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip listContext;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem copyFilenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAbsoluteFilenameToolStripMenuItem;
    }
}