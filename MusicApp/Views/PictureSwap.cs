using MusicApp.Services;

namespace MusicApp.Views
{
    public partial class PictureSwap : Form
    {
        public Image SelectedImage { get; private set; }
        private TagLib.Tag _tag = null;
        private string path;

        private int apiUsed = 0;

        public PictureSwap()
        {
            InitializeComponent();
            CancelButton = cancelButton;
        }

        public PictureSwap(TagLib.Tag tag)
            : this()
        {
            _tag = tag;
            var picture = tag.Pictures?.FirstOrDefault();

            if (picture != null && picture.Type != TagLib.PictureType.NotAPicture)
            {
                using var stream = new MemoryStream(picture?.Data?.Data);
                using var decodedImage = Image.FromStream(stream);
                currentPictureBox.Image = new Bitmap(decodedImage);
            }
        }



        private async void GetPictureButton_Click(object sender, EventArgs e)
        {
            swapButton.Enabled = false;
            OKButton.Enabled = false;
            getPictureButton.Enabled = false;
            cancelButton.Enabled = false;
            swapButton.Enabled = false;
            var newPicture = await Pictures.GetPicture(_tag.AlbumArtists?.FirstOrDefault(), _tag.Title, apiUsed);

            if (newPicture.Item1 == null)
                MessageBox.Show("Picture of the song was not found.", "No Results.");
            else
            {
                newPictureBox.Image = newPicture.Item1;
                swapButton.Enabled = true;
                apiUsed++;
            }
            OKButton.Enabled = true;
            getPictureButton.Enabled = true;
            getPictureButton.Text = $"Get ({apiUsed})";
            cancelButton.Enabled = true;
            swapButton.Enabled = true;
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

        private void ImportButton_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = path;
            openFileDialog.Title = "Select Album Artwork";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Load via stream to avoid locking the file on disk
                    using var stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    using var decodedImage = Image.FromStream(stream);
                    // Assign to your picture box
                    newPictureBox.Image = new Bitmap(decodedImage);
                    swapButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not load the selected image.\n\nError: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}