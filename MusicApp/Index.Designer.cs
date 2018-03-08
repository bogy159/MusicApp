namespace MusicApp
{
    partial class Index
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeArtistsInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateMusixmatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lyricsUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getHeadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.beginningButton = new System.Windows.Forms.Button();
            this.pageTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.previousButton = new System.Windows.Forms.Button();
            this.lastButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.itemsNumTextBox = new System.Windows.Forms.TextBox();
            this.changePgSizeButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.statsButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(716, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeArtistsInfoToolStripMenuItem,
            this.changeTitleToolStripMenuItem,
            this.updateMusixmatchToolStripMenuItem,
            this.lyricsUpdateToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // changeArtistsInfoToolStripMenuItem
            // 
            this.changeArtistsInfoToolStripMenuItem.Enabled = false;
            this.changeArtistsInfoToolStripMenuItem.Name = "changeArtistsInfoToolStripMenuItem";
            this.changeArtistsInfoToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.changeArtistsInfoToolStripMenuItem.Text = " Contributing Artists";
            this.changeArtistsInfoToolStripMenuItem.Click += new System.EventHandler(this.ChangeArtistsInfoToolStripMenuItem_Click);
            // 
            // changeTitleToolStripMenuItem
            // 
            this.changeTitleToolStripMenuItem.Enabled = false;
            this.changeTitleToolStripMenuItem.Name = "changeTitleToolStripMenuItem";
            this.changeTitleToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.changeTitleToolStripMenuItem.Text = "Title";
            this.changeTitleToolStripMenuItem.Click += new System.EventHandler(this.ChangeTitleToolStripMenuItem_Click);
            // 
            // updateMusixmatchToolStripMenuItem
            // 
            this.updateMusixmatchToolStripMenuItem.Enabled = false;
            this.updateMusixmatchToolStripMenuItem.Name = "updateMusixmatchToolStripMenuItem";
            this.updateMusixmatchToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.updateMusixmatchToolStripMenuItem.Text = "Musixmatch Update";
            this.updateMusixmatchToolStripMenuItem.Click += new System.EventHandler(this.MusixmatchUpdateToolStripMenuItem_Click);
            // 
            // lyricsUpdateToolStripMenuItem
            // 
            this.lyricsUpdateToolStripMenuItem.Enabled = false;
            this.lyricsUpdateToolStripMenuItem.Name = "lyricsUpdateToolStripMenuItem";
            this.lyricsUpdateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.lyricsUpdateToolStripMenuItem.Text = "Lyrics Update";
            this.lyricsUpdateToolStripMenuItem.Click += new System.EventHandler(this.LyricsUpdateToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getHeadersToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.otherToolStripMenuItem.Text = "Other";
            // 
            // getHeadersToolStripMenuItem
            // 
            this.getHeadersToolStripMenuItem.Name = "getHeadersToolStripMenuItem";
            this.getHeadersToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.getHeadersToolStripMenuItem.Text = "Get Headers";
            this.getHeadersToolStripMenuItem.Click += new System.EventHandler(this.GetHeadersToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 56);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(692, 339);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellDoubleClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.progressBar1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.progressBar1.Location = new System.Drawing.Point(491, 27);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(213, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Progress";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton2.Enabled = false;
            this.CancelButton2.Location = new System.Drawing.Point(410, 27);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 4;
            this.CancelButton2.Text = "Stop";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // beginningButton
            // 
            this.beginningButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.beginningButton.AutoSize = true;
            this.beginningButton.Enabled = false;
            this.beginningButton.Location = new System.Drawing.Point(12, 401);
            this.beginningButton.Name = "beginningButton";
            this.beginningButton.Size = new System.Drawing.Size(35, 23);
            this.beginningButton.TabIndex = 5;
            this.beginningButton.Text = "<<";
            this.beginningButton.UseVisualStyleBackColor = true;
            this.beginningButton.Click += new System.EventHandler(this.BeginningButton_Click);
            // 
            // pageTextBox
            // 
            this.pageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageTextBox.Location = new System.Drawing.Point(94, 403);
            this.pageTextBox.Name = "pageTextBox";
            this.pageTextBox.ReadOnly = true;
            this.pageTextBox.Size = new System.Drawing.Size(45, 20);
            this.pageTextBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 406);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Songs per page:";
            // 
            // previousButton
            // 
            this.previousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.previousButton.Enabled = false;
            this.previousButton.Location = new System.Drawing.Point(53, 401);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(35, 23);
            this.previousButton.TabIndex = 11;
            this.previousButton.Text = "<";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // lastButton
            // 
            this.lastButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastButton.Enabled = false;
            this.lastButton.Location = new System.Drawing.Point(186, 401);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(35, 23);
            this.lastButton.TabIndex = 12;
            this.lastButton.Text = ">>";
            this.lastButton.UseVisualStyleBackColor = true;
            this.lastButton.Click += new System.EventHandler(this.LastButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextButton.Enabled = false;
            this.nextButton.Location = new System.Drawing.Point(145, 401);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(35, 23);
            this.nextButton.TabIndex = 13;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // itemsNumTextBox
            // 
            this.itemsNumTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.itemsNumTextBox.Location = new System.Drawing.Point(318, 403);
            this.itemsNumTextBox.Name = "itemsNumTextBox";
            this.itemsNumTextBox.Size = new System.Drawing.Size(45, 20);
            this.itemsNumTextBox.TabIndex = 14;
            this.itemsNumTextBox.Text = "10";
            this.itemsNumTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ItemsNumTextBox_KeyPress);
            // 
            // changePgSizeButton
            // 
            this.changePgSizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.changePgSizeButton.Enabled = false;
            this.changePgSizeButton.Location = new System.Drawing.Point(369, 401);
            this.changePgSizeButton.Name = "changePgSizeButton";
            this.changePgSizeButton.Size = new System.Drawing.Size(35, 23);
            this.changePgSizeButton.TabIndex = 15;
            this.changePgSizeButton.Text = "Go to page";
            this.changePgSizeButton.UseVisualStyleBackColor = true;
            this.changePgSizeButton.Click += new System.EventHandler(this.ChangePgSizeButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshButton.Enabled = false;
            this.refreshButton.Location = new System.Drawing.Point(410, 401);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 16;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // statsButton
            // 
            this.statsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.statsButton.Enabled = false;
            this.statsButton.Location = new System.Drawing.Point(629, 401);
            this.statsButton.Name = "statsButton";
            this.statsButton.Size = new System.Drawing.Size(75, 23);
            this.statsButton.TabIndex = 17;
            this.statsButton.Text = "Stats";
            this.statsButton.UseVisualStyleBackColor = true;
            this.statsButton.Click += new System.EventHandler(this.StatsButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Enabled = false;
            this.searchButton.Location = new System.Drawing.Point(105, 27);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 18;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(186, 30);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(218, 20);
            this.searchTextBox.TabIndex = 19;
            this.searchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 436);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.statsButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.changePgSizeButton);
            this.Controls.Add(this.itemsNumTextBox);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.lastButton);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pageTextBox);
            this.Controls.Add(this.beginningButton);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Index";
            this.Text = "Main Window";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeArtistsInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeTitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getHeadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateMusixmatchToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelButton2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button beginningButton;
        private System.Windows.Forms.TextBox pageTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button lastButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.TextBox itemsNumTextBox;
        private System.Windows.Forms.Button changePgSizeButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button statsButton;
        private System.Windows.Forms.ToolStripMenuItem lyricsUpdateToolStripMenuItem;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox searchTextBox;
    }
}

