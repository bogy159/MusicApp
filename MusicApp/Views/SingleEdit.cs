using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MusicApp.Helpers;
using MusicApp.Services;
using System.Text.RegularExpressions;

namespace MusicApp
{
    public partial class SingleEdit : Form
    {
        public string error = "";

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
                ShellObject song = ShellObject.FromParsingName(location);
                // no-op to ensure file is modified/saved after edits
                nameTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));
                aArtistTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));
                titleTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Title));
                albumTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumTitle));
                yearTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Media.Year));
                lyricsTextBox.Text = TagLib.File.Create(location).Tag?.Lyrics ?? Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Music.Lyrics));
                
                // Use WindowsAPICodePack ShellObject for properties where possible
                try
                {
                    var shellObj = ShellObject.FromParsingName(location);
                    // Fall back to Shell32 compatibility wrapper for details not exposed by ShellObject
                    Shell shell = new Shell();
                    Folder objFolder = shell.NameSpace(Path.GetDirectoryName(location));
                    var folderItem = objFolder?.ParseName(Path.GetFileName(location));
                    contributingTextBox.Text = objFolder?.GetDetailsOf(folderItem, 13) ?? string.Empty;
                    genreBox.Text = objFolder?.GetDetailsOf(folderItem, 16) ?? string.Empty;
                }
                catch
                {
                    // Fallback: attempt COM wrapper directly
                    Shell shell = new Shell();
                    Folder objFolder = shell.NameSpace(Path.GetDirectoryName(location));
                    FolderItem folderItem = objFolder.ParseName(Path.GetFileName(location));
                    contributingTextBox.Text = objFolder.GetDetailsOf(folderItem, 13);
                    genreBox.Text = objFolder.GetDetailsOf(folderItem, 16);
                }
            }
            catch (ShellException)
            {
                MessageBox.Show("File not found exception!", "Error");
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                Close();
            }
        }

        public void SetData(string location)
        {            
            var fileshell = ShellFile.FromFilePath(location);
            ShellPropertyWriter propertyWriter = fileshell.Properties.GetPropertyWriter();
            try
            {

                if (yearTextBox.Text != "" && Validators.IsValidYear(yearTextBox.Text))
                    propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { Convert.ToInt16(yearTextBox.Text) });
                else if (yearTextBox.Text == "" || Convert.ToInt16(yearTextBox.Text) == 0)
                    propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { 0 });
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Invalid year!" + Environment.NewLine + "Do you want to continue with the year change?", "Inpropper year!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                        propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { Convert.ToInt16(yearTextBox.Text) });
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                    else if (dialogResult == DialogResult.Cancel)
                        return;
                }

                propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumArtist, new string[] { aArtistTextBox.Text });
                propertyWriter.WriteProperty(SystemProperties.System.Title, new string[] { titleTextBox.Text });
                propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, new string[] { albumTextBox.Text });
                propertyWriter.WriteProperty(SystemProperties.System.Music.Artist, new string[] { contributingTextBox.Text });
                propertyWriter.WriteProperty(SystemProperties.System.Music.Genre, new string[] { genreBox.Text });
                propertyWriter.WriteProperty(SystemProperties.System.Music.Lyrics, new string[] { lyricsTextBox.Text });

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e, "Setting data error!");
            }
            finally
            {
                propertyWriter.Close();
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
            string[] name = Regex.Split(nameTextBox.Text, "-");
            string[] artist = Regex.Split(name[0], "\\.");
            if (artist.Length >= 2 && artist[1] != null)
            {
                aArtistTextBox.Text = artist[1].Trim(); 
                contributingTextBox.Text = artist[1].Trim();
            }
            if (name.Length >= 2 && name[1] != null)
            {
                string[] title = Regex.Split(name[1], "\\.");
                titleTextBox.Text = title[0].Trim();
            }  
        }
    }
}