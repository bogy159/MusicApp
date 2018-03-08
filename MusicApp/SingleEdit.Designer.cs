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
            this.OK = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.locationBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.aArtistTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.yearTextBox = new System.Windows.Forms.TextBox();
            this.albumTextBox = new System.Windows.Forms.TextBox();
            this.genreBox = new System.Windows.Forms.TextBox();
            this.genreLabel = new System.Windows.Forms.Label();
            this.contributingTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lyricsTextBox = new System.Windows.Forms.RichTextBox();
            this.APIbutton = new System.Windows.Forms.Button();
            this.LyricsButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.quickGetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Location = new System.Drawing.Point(256, 525);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton2.Location = new System.Drawing.Point(337, 525);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 1;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // locationBox
            // 
            this.locationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.locationBox.Location = new System.Drawing.Point(78, 12);
            this.locationBox.Name = "locationBox";
            this.locationBox.ReadOnly = true;
            this.locationBox.Size = new System.Drawing.Size(334, 20);
            this.locationBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path to file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "File Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Album Artist:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Song Title:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Year:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Album:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(78, 43);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(334, 20);
            this.nameTextBox.TabIndex = 14;
            // 
            // aArtistTextBox
            // 
            this.aArtistTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aArtistTextBox.Location = new System.Drawing.Point(78, 69);
            this.aArtistTextBox.Name = "aArtistTextBox";
            this.aArtistTextBox.Size = new System.Drawing.Size(334, 20);
            this.aArtistTextBox.TabIndex = 15;
            // 
            // titleTextBox
            // 
            this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleTextBox.Location = new System.Drawing.Point(78, 121);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(334, 20);
            this.titleTextBox.TabIndex = 16;
            // 
            // yearTextBox
            // 
            this.yearTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.yearTextBox.Location = new System.Drawing.Point(78, 173);
            this.yearTextBox.MaxLength = 4;
            this.yearTextBox.Name = "yearTextBox";
            this.yearTextBox.Size = new System.Drawing.Size(334, 20);
            this.yearTextBox.TabIndex = 18;
            this.yearTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.YearTextBox_KeyPress);
            // 
            // albumTextBox
            // 
            this.albumTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.albumTextBox.Location = new System.Drawing.Point(78, 147);
            this.albumTextBox.Name = "albumTextBox";
            this.albumTextBox.Size = new System.Drawing.Size(334, 20);
            this.albumTextBox.TabIndex = 19;
            // 
            // genreBox
            // 
            this.genreBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.genreBox.Location = new System.Drawing.Point(78, 199);
            this.genreBox.Name = "genreBox";
            this.genreBox.Size = new System.Drawing.Size(334, 20);
            this.genreBox.TabIndex = 20;
            // 
            // genreLabel
            // 
            this.genreLabel.AutoSize = true;
            this.genreLabel.Location = new System.Drawing.Point(12, 202);
            this.genreLabel.Name = "genreLabel";
            this.genreLabel.Size = new System.Drawing.Size(39, 13);
            this.genreLabel.TabIndex = 21;
            this.genreLabel.Text = "Genre:";
            // 
            // contributingTextBox
            // 
            this.contributingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contributingTextBox.Location = new System.Drawing.Point(78, 95);
            this.contributingTextBox.Name = "contributingTextBox";
            this.contributingTextBox.Size = new System.Drawing.Size(334, 20);
            this.contributingTextBox.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Contributing:";
            // 
            // lyricsTextBox
            // 
            this.lyricsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lyricsTextBox.Location = new System.Drawing.Point(78, 225);
            this.lyricsTextBox.Name = "lyricsTextBox";
            this.lyricsTextBox.Size = new System.Drawing.Size(334, 294);
            this.lyricsTextBox.TabIndex = 24;
            this.lyricsTextBox.Text = "";
            // 
            // APIbutton
            // 
            this.APIbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.APIbutton.Location = new System.Drawing.Point(93, 525);
            this.APIbutton.Name = "APIbutton";
            this.APIbutton.Size = new System.Drawing.Size(75, 23);
            this.APIbutton.TabIndex = 25;
            this.APIbutton.Text = "MusixMatch";
            this.APIbutton.UseVisualStyleBackColor = true;
            this.APIbutton.Click += new System.EventHandler(this.APIbutton_Click);
            // 
            // LyricsButton
            // 
            this.LyricsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LyricsButton.Location = new System.Drawing.Point(174, 525);
            this.LyricsButton.Name = "LyricsButton";
            this.LyricsButton.Size = new System.Drawing.Size(75, 23);
            this.LyricsButton.TabIndex = 27;
            this.LyricsButton.Text = "Get Lyrics";
            this.LyricsButton.UseVisualStyleBackColor = true;
            this.LyricsButton.Click += new System.EventHandler(this.LyricsButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Lyrics:";
            // 
            // quickGetButton
            // 
            this.quickGetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quickGetButton.Location = new System.Drawing.Point(12, 525);
            this.quickGetButton.Name = "quickGetButton";
            this.quickGetButton.Size = new System.Drawing.Size(75, 23);
            this.quickGetButton.TabIndex = 29;
            this.quickGetButton.Text = "Get Data";
            this.quickGetButton.UseVisualStyleBackColor = true;
            this.quickGetButton.Click += new System.EventHandler(this.QuickGetButton_Click);
            // 
            // SingleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 560);
            this.Controls.Add(this.quickGetButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LyricsButton);
            this.Controls.Add(this.APIbutton);
            this.Controls.Add(this.lyricsTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.contributingTextBox);
            this.Controls.Add(this.genreLabel);
            this.Controls.Add(this.genreBox);
            this.Controls.Add(this.albumTextBox);
            this.Controls.Add(this.yearTextBox);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.aArtistTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.locationBox);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SingleEdit";
            this.Text = "SingleEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

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