using MusicApp.Services;

namespace MusicApp.Views
{
    public partial class PictureSwap : Form
    {
        private TagLib.Tag _tag = null;
        public Image SelectedImage { get; private set; }

        private TableLayoutPanel pictureLayoutPanel;
        private PictureBox currentPictureBox;
        private PictureBox newPictureBox;
        private Button OKButton;
        private Button getPictureButton;
        private Button cancelButton;
        private Button swapButton;

        public PictureSwap(TagLib.Tag tag)
        {
            _tag = tag;
            InitializeComponent();
            var picture = tag.Pictures?.FirstOrDefault();

            if (picture != null && picture.Type != TagLib.PictureType.NotAPicture)
            {
                using var stream = new MemoryStream(picture?.Data?.Data);
                using var decodedImage = Image.FromStream(stream);
                currentPictureBox.Image = new Bitmap(decodedImage);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new(typeof(PictureSwap));
            currentPictureBox = new PictureBox();
            newPictureBox = new PictureBox();
            OKButton = new Button();
            getPictureButton = new Button();
            cancelButton = new Button();
            swapButton = new Button();
            pictureLayoutPanel = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)currentPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)newPictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureLayoutPanel
            // 
            pictureLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
            pictureLayoutPanel.ColumnCount = 2;
            pictureLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pictureLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pictureLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureLayoutPanel.Location = new Point(12, 12);
            pictureLayoutPanel.Name = "pictureLayoutPanel";
            pictureLayoutPanel.RowCount = 1;
            pictureLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pictureLayoutPanel.Size = new Size(807, 400);
            pictureLayoutPanel.TabIndex = 0;
            // 
            // currentPictureBox
            // 
            currentPictureBox.BorderStyle = BorderStyle.FixedSingle;
            currentPictureBox.Dock = DockStyle.Fill;
            currentPictureBox.Location = new Point(0, 0);
            currentPictureBox.Margin = new Padding(0, 0, 6, 0);
            currentPictureBox.Name = "currentPictureBox";
            currentPictureBox.Size = new Size(831, 487);
            currentPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            currentPictureBox.TabIndex = 32;
            currentPictureBox.TabStop = false;
            // 
            // newPictureBox
            // 
            newPictureBox.BorderStyle = BorderStyle.FixedSingle;
            newPictureBox.Dock = DockStyle.Fill;
            newPictureBox.Location = new Point(0, 0);
            newPictureBox.Margin = new Padding(6, 0, 0, 0);
            newPictureBox.Name = "newPictureBox";
            newPictureBox.Size = new Size(831, 487);
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
            swapButton.Location = new Point(120, 420);
            swapButton.Margin = new Padding(4, 5, 4, 5);
            swapButton.Name = "swapButton";
            swapButton.Size = new Size(100, 35);
            swapButton.TabIndex = 37;
            swapButton.Text = "Swap";
            swapButton.UseVisualStyleBackColor = true;
            swapButton.Click += SwapButton_Click;
            swapButton.Enabled = false;
            // 
            // PictureSwap
            // 
            ClientSize = new Size(831, 487);
            pictureLayoutPanel.Controls.Add(currentPictureBox, 0, 0);
            pictureLayoutPanel.Controls.Add(newPictureBox, 1, 0);
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
            ResumeLayout(false);

        }

        private async void GetPictureButton_Click(object sender, EventArgs e)
        {
            swapButton.Enabled = false;
            OKButton.Enabled = false;
            getPictureButton.Enabled = false;
            cancelButton.Enabled = false;
            var newPicture = await Pictures.GetPicture(_tag.AlbumArtists?.FirstOrDefault(), _tag.Title);

            if (newPicture.Item1 == null)
                MessageBox.Show("Picture of the song was not found.", "No Results.");
            else
            {
                newPictureBox.Image = newPicture.Item1;
                swapButton.Enabled = true;
            }
            OKButton.Enabled = true;
            getPictureButton.Enabled = true;
            cancelButton.Enabled = true;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SelectedImage = currentPictureBox.Image;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SwapButton_Click(object sender, EventArgs e)
        {
            if (newPictureBox.Image != null)
            {
                currentPictureBox.Image = newPictureBox.Image;
                swapButton.Enabled = false;
            }
        }
    }
}