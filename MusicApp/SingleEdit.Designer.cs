namespace MusicApp
{
    partial class SingleEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleEdit));
            OK = new Button();
            CancelButton2 = new Button();
            locationBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            nameTextBox = new TextBox();
            aArtistTextBox = new TextBox();
            titleTextBox = new TextBox();
            yearTextBox = new TextBox();
            albumTextBox = new TextBox();
            genreBox = new TextBox();
            genreLabel = new Label();
            contributingTextBox = new TextBox();
            label7 = new Label();
            lyricsTextBox = new RichTextBox();
            APIbutton = new Button();
            LyricsButton = new Button();
            label8 = new Label();
            quickGetButton = new Button();
            SuspendLayout();
            // 
            // OK
            // 
            OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OK.Location = new Point(341, 808);
            OK.Margin = new Padding(4, 5, 4, 5);
            OK.Name = "OK";
            OK.Size = new Size(100, 35);
            OK.TabIndex = 0;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            OK.Click += OK_Click;
            // 
            // CancelButton2
            // 
            CancelButton2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CancelButton2.Location = new Point(449, 808);
            CancelButton2.Margin = new Padding(4, 5, 4, 5);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(100, 35);
            CancelButton2.TabIndex = 1;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton_Click;
            // 
            // locationBox
            // 
            locationBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            locationBox.Location = new Point(107, 18);
            locationBox.Margin = new Padding(4, 5, 4, 5);
            locationBox.Name = "locationBox";
            locationBox.ReadOnly = true;
            locationBox.Size = new Size(441, 27);
            locationBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 23);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(83, 20);
            label1.TabIndex = 3;
            label1.Text = "Path to file:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 66);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 4;
            label2.Text = "File Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 111);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(95, 20);
            label3.TabIndex = 5;
            label3.Text = "Album Artist:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 191);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(79, 20);
            label4.TabIndex = 6;
            label4.Text = "Song Title:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 271);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(40, 20);
            label5.TabIndex = 7;
            label5.Text = "Year:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 231);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(56, 20);
            label6.TabIndex = 13;
            label6.Text = "Album:";
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameTextBox.Location = new Point(107, 66);
            nameTextBox.Margin = new Padding(4, 5, 4, 5);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(441, 27);
            nameTextBox.TabIndex = 14;
            // 
            // aArtistTextBox
            // 
            aArtistTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            aArtistTextBox.Location = new Point(107, 106);
            aArtistTextBox.Margin = new Padding(4, 5, 4, 5);
            aArtistTextBox.Name = "aArtistTextBox";
            aArtistTextBox.Size = new Size(441, 27);
            aArtistTextBox.TabIndex = 15;
            // 
            // titleTextBox
            // 
            titleTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            titleTextBox.Location = new Point(107, 186);
            titleTextBox.Margin = new Padding(4, 5, 4, 5);
            titleTextBox.Name = "titleTextBox";
            titleTextBox.Size = new Size(441, 27);
            titleTextBox.TabIndex = 16;
            // 
            // yearTextBox
            // 
            yearTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            yearTextBox.Location = new Point(107, 266);
            yearTextBox.Margin = new Padding(4, 5, 4, 5);
            yearTextBox.MaxLength = 4;
            yearTextBox.Name = "yearTextBox";
            yearTextBox.Size = new Size(441, 27);
            yearTextBox.TabIndex = 18;
            yearTextBox.KeyPress += YearTextBox_KeyPress;
            // 
            // albumTextBox
            // 
            albumTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            albumTextBox.Location = new Point(107, 226);
            albumTextBox.Margin = new Padding(4, 5, 4, 5);
            albumTextBox.Name = "albumTextBox";
            albumTextBox.Size = new Size(441, 27);
            albumTextBox.TabIndex = 19;
            // 
            // genreBox
            // 
            genreBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            genreBox.Location = new Point(107, 306);
            genreBox.Margin = new Padding(4, 5, 4, 5);
            genreBox.Name = "genreBox";
            genreBox.Size = new Size(441, 27);
            genreBox.TabIndex = 20;
            // 
            // genreLabel
            // 
            genreLabel.AutoSize = true;
            genreLabel.Location = new Point(16, 311);
            genreLabel.Margin = new Padding(4, 0, 4, 0);
            genreLabel.Name = "genreLabel";
            genreLabel.Size = new Size(51, 20);
            genreLabel.TabIndex = 21;
            genreLabel.Text = "Genre:";
            // 
            // contributingTextBox
            // 
            contributingTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            contributingTextBox.Location = new Point(107, 146);
            contributingTextBox.Margin = new Padding(4, 5, 4, 5);
            contributingTextBox.Name = "contributingTextBox";
            contributingTextBox.Size = new Size(441, 27);
            contributingTextBox.TabIndex = 22;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 151);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(95, 20);
            label7.TabIndex = 23;
            label7.Text = "Contributing:";
            // 
            // lyricsTextBox
            // 
            lyricsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lyricsTextBox.Location = new Point(107, 346);
            lyricsTextBox.Margin = new Padding(4, 5, 4, 5);
            lyricsTextBox.Name = "lyricsTextBox";
            lyricsTextBox.Size = new Size(441, 450);
            lyricsTextBox.TabIndex = 24;
            lyricsTextBox.Text = "";
            // 
            // APIbutton
            // 
            APIbutton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            APIbutton.Location = new Point(124, 808);
            APIbutton.Margin = new Padding(4, 5, 4, 5);
            APIbutton.Name = "APIbutton";
            APIbutton.Size = new Size(100, 35);
            APIbutton.TabIndex = 25;
            APIbutton.Text = "MusixMatch";
            APIbutton.UseVisualStyleBackColor = true;
            APIbutton.Click += APIbutton_Click;
            // 
            // LyricsButton
            // 
            LyricsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LyricsButton.Location = new Point(232, 808);
            LyricsButton.Margin = new Padding(4, 5, 4, 5);
            LyricsButton.Name = "LyricsButton";
            LyricsButton.Size = new Size(100, 35);
            LyricsButton.TabIndex = 27;
            LyricsButton.Text = "Get Lyrics";
            LyricsButton.UseVisualStyleBackColor = true;
            LyricsButton.Click += LyricsButton_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(16, 351);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(47, 20);
            label8.TabIndex = 28;
            label8.Text = "Lyrics:";
            // 
            // quickGetButton
            // 
            quickGetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            quickGetButton.Location = new Point(16, 808);
            quickGetButton.Margin = new Padding(4, 5, 4, 5);
            quickGetButton.Name = "quickGetButton";
            quickGetButton.Size = new Size(100, 35);
            quickGetButton.TabIndex = 29;
            quickGetButton.Text = "Get Data";
            quickGetButton.UseVisualStyleBackColor = true;
            quickGetButton.Click += QuickGetButton_Click;
            // 
            // SingleEdit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(565, 862);
            Controls.Add(contributingTextBox);
            Controls.Add(quickGetButton);
            Controls.Add(label8);
            Controls.Add(LyricsButton);
            Controls.Add(APIbutton);
            Controls.Add(lyricsTextBox);
            Controls.Add(label7);
            Controls.Add(genreLabel);
            Controls.Add(genreBox);
            Controls.Add(albumTextBox);
            Controls.Add(yearTextBox);
            Controls.Add(titleTextBox);
            Controls.Add(aArtistTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(locationBox);
            Controls.Add(CancelButton2);
            Controls.Add(OK);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            Name = "SingleEdit";
            Text = "SingleEdit";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.TextBox locationBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox aArtistTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.TextBox yearTextBox;
        private System.Windows.Forms.TextBox albumTextBox;
        private System.Windows.Forms.TextBox genreBox;
        private System.Windows.Forms.Label genreLabel;
        private System.Windows.Forms.TextBox contributingTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox lyricsTextBox;
        private System.Windows.Forms.Button APIbutton;
        private System.Windows.Forms.Button LyricsButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button quickGetButton;
    }
}