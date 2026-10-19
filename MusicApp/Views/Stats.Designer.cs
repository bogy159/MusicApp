namespace MusicApp
{
    partial class Stats
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
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stats));
            closeButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            statsDataGridView = new System.Windows.Forms.DataGridView();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            progressBar2 = new System.Windows.Forms.ProgressBar();
            textBox1 = new System.Windows.Forms.TextBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(statsDataGridView)).BeginInit();
            SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            closeButton.Location = new System.Drawing.Point(154, 489);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(75, 23);
            closeButton.TabIndex = 0;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += new System.EventHandler(CloseButton_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(86, 13);
            label1.TabIndex = 1;
            label1.Text = "Number of items:";
            // 
            // statsDataGridView
            // 
            statsDataGridView.AllowUserToAddRows = false;
            statsDataGridView.AllowUserToDeleteRows = false;
            statsDataGridView.AllowUserToOrderColumns = true;
            statsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            statsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            statsDataGridView.Location = new System.Drawing.Point(12, 65);
            statsDataGridView.MultiSelect = false;
            statsDataGridView.Name = "statsDataGridView";
            statsDataGridView.ReadOnly = true;
            statsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            statsDataGridView.Size = new System.Drawing.Size(353, 410);
            statsDataGridView.TabIndex = 12;
            // 
            // progressBar2
            // 
            progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            progressBar2.BackColor = System.Drawing.SystemColors.ControlLight;
            progressBar2.Cursor = System.Windows.Forms.Cursors.IBeam;
            progressBar2.Location = new System.Drawing.Point(150, 9);
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new System.Drawing.Size(215, 23);
            progressBar2.TabIndex = 13;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(105, 9);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new System.Drawing.Size(39, 20);
            textBox1.TabIndex = 14;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] {
            "Album Artists",
            "Contribution Artists",
            "Albums",
            "Genres",
            "Years",
            "Decades",
            "Average Durations",
            "Have Lyrics",
            "Have Pictures",
            "ID Numbers",
            "Locations"});
            comboBox1.Location = new System.Drawing.Point(105, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(260, 21);
            comboBox1.TabIndex = 15;
            comboBox1.SelectedIndexChanged += new System.EventHandler(ComboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(9, 41);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(93, 13);
            label2.TabIndex = 16;
            label2.Text = "Currently showing:";
            // 
            // Stats
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(377, 524);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(textBox1);
            Controls.Add(progressBar2);
            Controls.Add(statsDataGridView);
            Controls.Add(label1);
            Controls.Add(closeButton);
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            Name = "Stats";
            Text = "Stats";
            CancelButton = closeButton;
            ((System.ComponentModel.ISupportInitialize)(statsDataGridView)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView statsDataGridView;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
    }
}