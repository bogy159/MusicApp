namespace MusicApp.Views
{
    public partial class PictureSwap
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel pictureLayoutPanel;
        private PictureBox currentPictureBox;
        private PictureBox newPictureBox;
        private Button OKButton;
        private Button getPictureButton;
        private Button cancelButton;
        private Button swapButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PictureSwap));
            currentPictureBox = new PictureBox();
            newPictureBox = new PictureBox();
            OKButton = new Button();
            getPictureButton = new Button();
            cancelButton = new Button();
            swapButton = new Button();
            pictureLayoutPanel = new TableLayoutPanel();
            ImportButton = new Button();
            ((System.ComponentModel.ISupportInitialize)currentPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)newPictureBox).BeginInit();
            pictureLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // currentPictureBox
            // 
            currentPictureBox.BorderStyle = BorderStyle.FixedSingle;
            currentPictureBox.Dock = DockStyle.Fill;
            currentPictureBox.Location = new Point(0, 0);
            currentPictureBox.Margin = new Padding(0, 0, 6, 0);
            currentPictureBox.Name = "currentPictureBox";
            currentPictureBox.Size = new Size(396, 398);
            currentPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            currentPictureBox.TabIndex = 32;
            currentPictureBox.TabStop = false;
            // 
            // newPictureBox
            // 
            newPictureBox.BorderStyle = BorderStyle.FixedSingle;
            newPictureBox.Dock = DockStyle.Fill;
            newPictureBox.Location = new Point(408, 0);
            newPictureBox.Margin = new Padding(6, 0, 0, 0);
            newPictureBox.Name = "newPictureBox";
            newPictureBox.Size = new Size(397, 398);
            newPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            newPictureBox.TabIndex = 33;
            newPictureBox.TabStop = false;
            // 
            // OKButton
            // 
            OKButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OKButton.Location = new Point(610, 420);
            OKButton.Margin = new Padding(4, 5, 4, 5);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(100, 35);
            OKButton.TabIndex = 34;
            OKButton.Text = "OK";
            OKButton.UseVisualStyleBackColor = true;
            OKButton.Click += OKButton_Click;
            // 
            // getPictureButton
            // 
            getPictureButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            getPictureButton.Location = new Point(12, 420);
            getPictureButton.Margin = new Padding(4, 5, 4, 5);
            getPictureButton.Name = "getPictureButton";
            getPictureButton.Size = new Size(100, 35);
            getPictureButton.TabIndex = 35;
            getPictureButton.Text = "Get Picture";
            getPictureButton.UseVisualStyleBackColor = true;
            getPictureButton.Click += GetPictureButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(718, 420);
            cancelButton.Margin = new Padding(4, 5, 4, 5);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(100, 35);
            cancelButton.TabIndex = 36;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // swapButton
            // 
            swapButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            swapButton.Enabled = false;
            swapButton.Location = new Point(228, 420);
            swapButton.Margin = new Padding(4, 5, 4, 5);
            swapButton.Name = "swapButton";
            swapButton.Size = new Size(100, 35);
            swapButton.TabIndex = 37;
            swapButton.Text = "Swap";
            swapButton.UseVisualStyleBackColor = true;
            swapButton.Click += SwapButton_Click;
            // 
            // pictureLayoutPanel
            // 
            pictureLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
            pictureLayoutPanel.ColumnCount = 2;
            pictureLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pictureLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pictureLayoutPanel.Controls.Add(currentPictureBox, 0, 0);
            pictureLayoutPanel.Controls.Add(newPictureBox, 1, 0);
            pictureLayoutPanel.Location = new Point(12, 12);
            pictureLayoutPanel.Name = "pictureLayoutPanel";
            pictureLayoutPanel.RowCount = 1;
            pictureLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pictureLayoutPanel.Size = new Size(807, 400);
            pictureLayoutPanel.TabIndex = 0;
            // 
            // button1
            // 
            ImportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ImportButton.Location = new Point(120, 420);
            ImportButton.Margin = new Padding(4, 5, 4, 5);
            ImportButton.Name = "importButton";
            ImportButton.Size = new Size(100, 35);
            ImportButton.TabIndex = 38;
            ImportButton.Text = "Import";
            ImportButton.UseVisualStyleBackColor = true;
            ImportButton.Click += ImportButton_Click;
            // 
            // PictureSwap
            // 
            ClientSize = new Size(831, 487);
            Controls.Add(ImportButton);
            Controls.Add(pictureLayoutPanel);
            Controls.Add(swapButton);
            Controls.Add(cancelButton);
            Controls.Add(getPictureButton);
            Controls.Add(OKButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PictureSwap";
            Text = "Swap Picture";
            ((System.ComponentModel.ISupportInitialize)currentPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)newPictureBox).EndInit();
            pictureLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Button ImportButton;
    }
}