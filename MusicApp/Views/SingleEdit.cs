using MusicApp.Helpers;
using MusicApp.Services;
using MusicApp.Views;
using System.Text.RegularExpressions;

namespace MusicApp
{
    public partial class SingleEdit : Form
    {
        private TagLib.Tag tag = null;
        private bool isPictureChanged = false;

        public SingleEdit(string location)
        {
            InitializeComponent();
            CancelButton = CancelButton2;
            locationBox.Text = location;
            GetData(location);
        }

        public void GetData(string location)
        {
            try
            {
                using TagLib.File tagFile = TagLib.File.Create(location);
                tag = tagFile.Tag;
                nameTextBox.Text = Path.GetFileName(location);
                aArtistTextBox.Text = tag.AlbumArtists?.FirstOrDefault() ?? tag.FirstAlbumArtist ?? string.Empty;
                contributingTextBox.Text = tag.Performers?.FirstOrDefault() ?? string.Empty;
                titleTextBox.Text = tag.Title ?? string.Empty;
                albumTextBox.Text = tag.Album ?? string.Empty;
                yearTextBox.Text = tag.Year > 0 ? tag.Year.ToString() : string.Empty;
                genreBox.Text = tag.FirstGenre ?? string.Empty;
                lyricsTextBox.Text = tag.Lyrics ?? string.Empty;

                var picture = tag.Pictures?.FirstOrDefault();

                if (Validators.IsValidPicture(picture))
                {
                    using var stream = new MemoryStream(picture?.Data?.Data);
                    using var decodedImage = Image.FromStream(stream);
                    pictureBox.Image = new Bitmap(decodedImage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                Close();
            }
        }

        public void SetData(string location)
        {
            try
            {
                using TagLib.File tagFile = TagLib.File.Create(location);
                TagLib.Tag tag = tagFile.Tag;
                bool hasYear = uint.TryParse(yearTextBox.Text, out uint year);

                if (yearTextBox.Text == "")
                    tag.Year = 0;
                else if (hasYear && Validators.IsValidYear(yearTextBox.Text))
                    tag.Year = year;
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Invalid year!" + Environment.NewLine + "Do you want to continue with the year change?", "Inpropper year!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (hasYear)
                            tag.Year = year;
                    }
                    else if (dialogResult == DialogResult.Cancel)
                        return;
                }

                tag.AlbumArtists = [aArtistTextBox.Text];
                tag.Title = titleTextBox.Text;
                tag.Album = albumTextBox.Text;
                tag.Performers = [contributingTextBox.Text];
                tag.Genres = [genreBox.Text];
                if (!string.IsNullOrWhiteSpace(lyricsTextBox.Text))
                {
                    // File.Tag can be a combined tag when the file contains multiple
                    // tag formats, so inspect the ID3v2 tag directly before updating
                    // the combined Lyrics property.
                    if (tagFile.GetTag(TagLib.TagTypes.Id3v2, false) is TagLib.Id3v2.Tag id3Tag)
                    {
                        foreach (var frame in id3Tag.GetFrames<TagLib.Id3v2.UnsynchronisedLyricsFrame>().ToArray())
                            id3Tag.RemoveFrame(frame);
                    }
                    tag.Lyrics = lyricsTextBox.Text;
                }
                if (isPictureChanged && pictureBox.Image is Image image)
                    tag.Pictures = [Pictures.ToTagPicture(image)];
                tagFile.Save();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e, "Setting data error!");
            }
            try
            {
                string message = Validators.NameCheck(nameTextBox.Text);
                if (message == "")
                {
                    File.Move(location, Path.GetDirectoryName(location) + "\\" + nameTextBox.Text);
                    Close();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Name check returned: " + message + Environment.NewLine + "Do you want to continue with the name change?", "Inpropper name!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        File.Move(location, Path.GetDirectoryName(location) + "\\" + nameTextBox.Text);
                        Close();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                    else if (dialogResult == DialogResult.Cancel)
                        Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Renaming error!");
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            SetData(locationBox.Text);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void YearTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                if (char.IsNumber(e.KeyChar))
                {
                }
                else
                    e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private async void APIbutton_Click(object sender, EventArgs e)
        {
            var data = await DataEnrichment.EnrichDataAsync(aArtistTextBox.Text, titleTextBox.Text);
            if (data.Item1 != null)
            {
                albumTextBox.Text = data.Item1.AlbumTitle;
                ///Enable this incase you want to chnage the year from the one drawn from the API.
                yearTextBox.Text = data.Item1.Year.ToString();
                try
                {
                    genreBox.Text = data.Item1.Genre;
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show("Genre of the song was not found.", "No Results.");
                }
            }
            else
                MessageBox.Show("The search did not return any results.", "No Results.");
        }

        private async void LyricsButton_Click(object sender, EventArgs e)
        {
            var lyrics = await Lyrics.GetLyricsAsync(aArtistTextBox.Text, titleTextBox.Text);

            string lyricsValues = lyrics.Item1;
            if (lyricsValues == "")
                MessageBox.Show("Lyrics of the song were not found.", "No Results.");
            else
                lyricsTextBox.Text = lyricsValues;
        }

        private void QuickGetButton_Click(object sender, EventArgs e)
        {
            string[] name = Regex.Split(nameTextBox.Text, " - ");
            string[] artist = name[0].Split(['.'], 2);
            if (artist.Length >= 2 && artist[1] != null)
            {
                aArtistTextBox.Text = artist[1].Trim();
                contributingTextBox.Text = artist[1].Trim();
            }
            if (name.Length >= 2 && name[1] != null)
            {
                string[] title = Regex.Split(name[1], "\\.mp3");
                titleTextBox.Text = title[0].Trim();
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            using PictureSwap editForm = new(tag);
            if (editForm.ShowDialog(this) == DialogResult.OK && editForm.SelectedImage is Image selectedImage)
            {
                pictureBox.Image = selectedImage;
                isPictureChanged = true;
            }
        }
    }
}