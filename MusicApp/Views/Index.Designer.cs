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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            openExtendedToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            changeArtistsInfoToolStripMenuItem = new ToolStripMenuItem();
            changeTitleToolStripMenuItem = new ToolStripMenuItem();
            updateEnrichmentToolStripMenuItem = new ToolStripMenuItem();
            lyricsUpdateToolStripMenuItem = new ToolStripMenuItem();
            picturesUpdateToolStripMenuItem = new ToolStripMenuItem();
            otherToolStripMenuItem = new ToolStripMenuItem();
            getHeadersToolStripMenuItem = new ToolStripMenuItem();
            dataGridView1 = new DataGridView();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            CancelButton2 = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            beginningButton = new Button();
            pageTextBox = new TextBox();
            label2 = new Label();
            previousButton = new Button();
            lastButton = new Button();
            nextButton = new Button();
            itemsNumTextBox = new TextBox();
            changePgSizeButton = new Button();
            refreshButton = new Button();
            statsButton = new Button();
            searchButton = new Button();
            searchTextBox = new TextBox();
            refreshExtendedButton = new Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, otherToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 3, 0, 3);
            menuStrip1.Size = new Size(955, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, openExtendedToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(194, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // openExtendedToolStripMenuItem
            // 
            openExtendedToolStripMenuItem.Name = "openExtendedToolStripMenuItem";
            openExtendedToolStripMenuItem.Size = new Size(194, 26);
            openExtendedToolStripMenuItem.Text = "Open extended";
            openExtendedToolStripMenuItem.Click += OpenExtendedToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(194, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeArtistsInfoToolStripMenuItem, changeTitleToolStripMenuItem, updateEnrichmentToolStripMenuItem, lyricsUpdateToolStripMenuItem, picturesUpdateToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(49, 24);
            editToolStripMenuItem.Text = "Edit";
            // 
            // changeArtistsInfoToolStripMenuItem
            // 
            changeArtistsInfoToolStripMenuItem.Enabled = false;
            changeArtistsInfoToolStripMenuItem.Name = "changeArtistsInfoToolStripMenuItem";
            changeArtistsInfoToolStripMenuItem.Size = new Size(224, 26);
            changeArtistsInfoToolStripMenuItem.Text = " Contributing Artists";
            changeArtistsInfoToolStripMenuItem.Click += ChangeArtistsInfoToolStripMenuItem_Click;
            // 
            // changeTitleToolStripMenuItem
            // 
            changeTitleToolStripMenuItem.Enabled = false;
            changeTitleToolStripMenuItem.Name = "changeTitleToolStripMenuItem";
            changeTitleToolStripMenuItem.Size = new Size(224, 26);
            changeTitleToolStripMenuItem.Text = "Title";
            changeTitleToolStripMenuItem.Click += ChangeTitleToolStripMenuItem_Click;
            // 
            // updateEnrichmentToolStripMenuItem
            // 
            updateEnrichmentToolStripMenuItem.Enabled = false;
            updateEnrichmentToolStripMenuItem.Name = "updateEnrichmentToolStripMenuItem";
            updateEnrichmentToolStripMenuItem.Size = new Size(224, 26);
            updateEnrichmentToolStripMenuItem.Text = "Enrichment Update";
            updateEnrichmentToolStripMenuItem.Click += EnrichmentUpdateToolStripMenuItem_Click;
            // 
            // lyricsUpdateToolStripMenuItem
            // 
            lyricsUpdateToolStripMenuItem.Enabled = false;
            lyricsUpdateToolStripMenuItem.Name = "lyricsUpdateToolStripMenuItem";
            lyricsUpdateToolStripMenuItem.Size = new Size(224, 26);
            lyricsUpdateToolStripMenuItem.Text = "Lyrics Update";
            lyricsUpdateToolStripMenuItem.Click += LyricsUpdateToolStripMenuItem_Click;
            // 
            // picturesUpdateToolStripMenuItem
            // 
            picturesUpdateToolStripMenuItem.Enabled = false;
            picturesUpdateToolStripMenuItem.Name = "picturesUpdateToolStripMenuItem";
            picturesUpdateToolStripMenuItem.Size = new Size(224, 26);
            picturesUpdateToolStripMenuItem.Text = "Pictures Update";
            picturesUpdateToolStripMenuItem.Click += PicturesUpdateToolStripMenuItem_Click;
            // 
            // otherToolStripMenuItem
            // 
            otherToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { getHeadersToolStripMenuItem });
            otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            otherToolStripMenuItem.Size = new Size(60, 24);
            otherToolStripMenuItem.Text = "Other";
            // 
            // getHeadersToolStripMenuItem
            // 
            getHeadersToolStripMenuItem.Name = "getHeadersToolStripMenuItem";
            getHeadersToolStripMenuItem.Size = new Size(174, 26);
            getHeadersToolStripMenuItem.Text = "Get Headers";
            getHeadersToolStripMenuItem.Click += GetHeadersToolStripMenuItem_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(16, 86);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(923, 522);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.ColumnDividerDoubleClick += DataGridView1_ColumnDividerDoubleClick;
            dataGridView1.MouseDown += dataGridView1_MouseDown;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBar1.BackColor = SystemColors.ControlLight;
            progressBar1.Cursor = Cursors.IBeam;
            progressBar1.Location = new Point(655, 42);
            progressBar1.Margin = new Padding(4, 5, 4, 5);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(284, 35);
            progressBar1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 57);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 3;
            label1.Text = "Progress";
            label1.Click += label1_Click;
            // 
            // CancelButton2
            // 
            CancelButton2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CancelButton2.Enabled = false;
            CancelButton2.Location = new Point(547, 42);
            CancelButton2.Margin = new Padding(4, 5, 4, 5);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(100, 35);
            CancelButton2.TabIndex = 4;
            CancelButton2.Text = "Stop";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton_Click;
            // 
            // beginningButton
            // 
            beginningButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            beginningButton.AutoSize = true;
            beginningButton.Enabled = false;
            beginningButton.Location = new Point(16, 617);
            beginningButton.Margin = new Padding(4, 5, 4, 5);
            beginningButton.Name = "beginningButton";
            beginningButton.Size = new Size(47, 35);
            beginningButton.TabIndex = 5;
            beginningButton.Text = "<<";
            beginningButton.UseVisualStyleBackColor = true;
            beginningButton.Click += BeginningButton_Click;
            // 
            // pageTextBox
            // 
            pageTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pageTextBox.Location = new Point(125, 620);
            pageTextBox.Margin = new Padding(4, 5, 4, 5);
            pageTextBox.Name = "pageTextBox";
            pageTextBox.ReadOnly = true;
            pageTextBox.Size = new Size(59, 27);
            pageTextBox.TabIndex = 9;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(303, 625);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(116, 20);
            label2.TabIndex = 10;
            label2.Text = "Songs per page:";
            // 
            // previousButton
            // 
            previousButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            previousButton.Enabled = false;
            previousButton.Location = new Point(71, 617);
            previousButton.Margin = new Padding(4, 5, 4, 5);
            previousButton.Name = "previousButton";
            previousButton.Size = new Size(47, 35);
            previousButton.TabIndex = 11;
            previousButton.Text = "<";
            previousButton.UseVisualStyleBackColor = true;
            previousButton.Click += PreviousButton_Click;
            // 
            // lastButton
            // 
            lastButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lastButton.Enabled = false;
            lastButton.Location = new Point(248, 617);
            lastButton.Margin = new Padding(4, 5, 4, 5);
            lastButton.Name = "lastButton";
            lastButton.Size = new Size(47, 35);
            lastButton.TabIndex = 12;
            lastButton.Text = ">>";
            lastButton.UseVisualStyleBackColor = true;
            lastButton.Click += LastButton_Click;
            // 
            // nextButton
            // 
            nextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            nextButton.Enabled = false;
            nextButton.Location = new Point(193, 617);
            nextButton.Margin = new Padding(4, 5, 4, 5);
            nextButton.Name = "nextButton";
            nextButton.Size = new Size(47, 35);
            nextButton.TabIndex = 13;
            nextButton.Text = ">";
            nextButton.UseVisualStyleBackColor = true;
            nextButton.Click += NextButton_Click;
            // 
            // itemsNumTextBox
            // 
            itemsNumTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            itemsNumTextBox.Location = new Point(424, 620);
            itemsNumTextBox.Margin = new Padding(4, 5, 4, 5);
            itemsNumTextBox.Name = "itemsNumTextBox";
            itemsNumTextBox.Size = new Size(59, 27);
            itemsNumTextBox.TabIndex = 14;
            itemsNumTextBox.Text = "10";
            itemsNumTextBox.KeyPress += ItemsNumTextBox_KeyPress;
            // 
            // changePgSizeButton
            // 
            changePgSizeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            changePgSizeButton.Enabled = false;
            changePgSizeButton.Location = new Point(492, 617);
            changePgSizeButton.Margin = new Padding(4, 5, 4, 5);
            changePgSizeButton.Name = "changePgSizeButton";
            changePgSizeButton.Size = new Size(47, 35);
            changePgSizeButton.TabIndex = 15;
            changePgSizeButton.Text = "Go to page";
            changePgSizeButton.UseVisualStyleBackColor = true;
            changePgSizeButton.Click += ChangePgSizeButton_Click;
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            refreshButton.Enabled = false;
            refreshButton.Location = new Point(547, 617);
            refreshButton.Margin = new Padding(4, 5, 4, 5);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(100, 35);
            refreshButton.TabIndex = 16;
            refreshButton.Text = "Refresh";
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += RefreshButton_Click;
            // 
            // statsButton
            // 
            statsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            statsButton.Enabled = false;
            statsButton.Location = new Point(839, 617);
            statsButton.Margin = new Padding(4, 5, 4, 5);
            statsButton.Name = "statsButton";
            statsButton.Size = new Size(100, 35);
            statsButton.TabIndex = 17;
            statsButton.Text = "Stats";
            statsButton.UseVisualStyleBackColor = true;
            statsButton.Click += StatsButton_Click;
            // 
            // searchButton
            // 
            searchButton.Enabled = false;
            searchButton.Location = new Point(140, 42);
            searchButton.Margin = new Padding(4, 5, 4, 5);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(100, 35);
            searchButton.TabIndex = 18;
            searchButton.Text = "Search";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += SearchButton_Click;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchTextBox.Location = new Point(248, 46);
            searchTextBox.Margin = new Padding(4, 5, 4, 5);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(289, 27);
            searchTextBox.TabIndex = 19;
            searchTextBox.KeyPress += SearchTextBox_KeyPress;
            // 
            // refreshExtendedButton
            // 
            refreshExtendedButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            refreshExtendedButton.Enabled = false;
            refreshExtendedButton.Location = new Point(655, 617);
            refreshExtendedButton.Margin = new Padding(4, 5, 4, 5);
            refreshExtendedButton.Name = "refreshExtendedButton";
            refreshExtendedButton.Size = new Size(100, 35);
            refreshExtendedButton.TabIndex = 20;
            refreshExtendedButton.Text = "Extend";
            refreshExtendedButton.UseVisualStyleBackColor = true;
            refreshExtendedButton.Click += RefreshExtended_Click;
            // 
            // Index
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(955, 671);
            Controls.Add(refreshExtendedButton);
            Controls.Add(searchTextBox);
            Controls.Add(searchButton);
            Controls.Add(statsButton);
            Controls.Add(refreshButton);
            Controls.Add(changePgSizeButton);
            Controls.Add(itemsNumTextBox);
            Controls.Add(nextButton);
            Controls.Add(lastButton);
            Controls.Add(previousButton);
            Controls.Add(label2);
            Controls.Add(pageTextBox);
            Controls.Add(beginningButton);
            Controls.Add(CancelButton2);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(dataGridView1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 5, 4, 5);
            Name = "Index";
            Text = "Main Window";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem updateEnrichmentToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem openExtendedToolStripMenuItem;
        private System.Windows.Forms.Button refreshExtendedButton;
        private ToolStripMenuItem picturesUpdateToolStripMenuItem;
    }
}

