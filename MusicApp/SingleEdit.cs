using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Windows.Forms;
using unirest_net.http;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml;
using System.Threading;
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
                nameTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));
                aArtistTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));
                titleTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Title));
                albumTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumTitle));
                yearTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Media.Year));
                lyricsTextBox.Text = Index.GetValue(song.Properties.GetProperty(SystemProperties.System.Music.Lyrics));

                Shell32.Shell shell = new Shell32.Shell();
                Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(location));
                Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(location));
                contributingTextBox.Text = objFolder.GetDetailsOf(folderItem, 13);
                genreBox.Text = objFolder.GetDetailsOf(folderItem, 16);
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

                if (yearTextBox.Text != "" && Convert.ToInt16(yearTextBox.Text) <= Convert.ToInt16(DateTime.Now.Year) && Convert.ToInt16(yearTextBox.Text) > 1900)
                {
                    propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { Convert.ToInt16(yearTextBox.Text) });
                }
                else if (yearTextBox.Text == "" || Convert.ToInt16(yearTextBox.Text) == 0)
                {
                    propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { 0 });
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Invalid year!" + Environment.NewLine + "Do you want to continue with the year change?", "Inpropper year!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { Convert.ToInt16(yearTextBox.Text) });
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
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
                string message = Index.NameCheck(nameTextBox.Text);
                if (message == "")
                {
                    System.IO.File.Move(location, System.IO.Path.GetDirectoryName(location) + "\\" + nameTextBox.Text);
                    Close();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Name check returned: " + message + Environment.NewLine + "Do you want to continue with the name change?", "Inpropper name!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        System.IO.File.Move(location, System.IO.Path.GetDirectoryName(location) + "\\" + nameTextBox.Text);
                        Close();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e, "Renaming error!");                
            }
        }

        public static (TrackSearchJSON.Main, bool)  MusixmatchGet(string artist, string song)
        {
            try
            {
                artist = artist.Trim();
                artist = artist.Replace(' ', '+');
                song = song.Trim();
                song = song.Replace(' ', '+');
                string url = "https://musixmatchcom-musixmatch.p.mashape.com/wsr/1.1/track.search?q_track_artist=" + song + "&s_track_rating=desc&page=1&q_track=" + song + "&q_artist=" + artist + "&page_size=1";
                //HttpResponse<string> response = Unirest.get("https://musixmatchcom-musixmatch.p.mashape.com/wsr/1.1/track.search?q_track_artist=Hotel+California&s_track_rating=desc&page=1&q_track=Hotel+California&q_artist=Eagles&page_size=1")
                HttpResponse<string> response = Unirest.get(url)
                   .header("X-Mashape-Key", "GGLEQ6xnQ5mshNEFjdypEff5Rv80p1NdFdMjsna08Vm5HN7ccb")
                    .header("X-Mashape-Host", "musixmatchcom-musixmatch.p.mashape.com")
                    .asJson<string>();

                if(response.Code != 200)
                {
                    artist = artist.Replace('+', ' ');
                    song = song.Replace('+', ' ');
                    DialogResult dialogResult = MessageBox.Show("Response code: " + response.Code + Environment.NewLine + "When searching: " + artist + " - " + song, "Server error!", MessageBoxButtons.AbortRetryIgnore);
                    if (dialogResult == DialogResult.Abort)
                    {
                        return (null, true);
                    }
                    else if (dialogResult == DialogResult.Retry)
                    {
                        Thread.Sleep(2000);
                        return MusixmatchGet(artist, song);

                    }
                    else if (dialogResult == DialogResult.Ignore)
                    {
                        return (null, false);
                    }
                }
                response.Code.ToString();
                //Unirest.shut
                TrackSearchJSON.Main[] result = JsonConvert.DeserializeObject<TrackSearchJSON.Main[]>(response.Body.ToString());

                return (result[0], false);
            }
            //catch(HttpRequestException e)
            //{                
            //    MessageBox.Show("Error: " + e, "Http Request Failed!");
            //    return (null, true); 
            //}
            //catch (WebException e)
            //{
            //    MessageBox.Show("Error: " + e, "Web Exception!");
            //    return (null, true);
            //}
            //catch (AggregateException e)
            //{
            //    MessageBox.Show("Error: " + e, "Aggregate Exception!");
            //    return (null, true);
            //}
            catch (IndexOutOfRangeException)
            {
                //MessageBox.Show("The search did not return any results for: " + artist + " - " + song, "No Results.");
                return (null, false);
            }
            catch (Exception e)
            {
                artist = artist.Replace('+', ' ');
                song = song.Replace('+', ' ');
                DialogResult dialogResult = MessageBox.Show("Error: " + e + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
                if (dialogResult == DialogResult.Abort)
                {
                    return (null, true);
                }
                else if (dialogResult == DialogResult.Retry)
                {
                    Thread.Sleep(2000);
                    return MusixmatchGet(artist, song);

                }
                else if (dialogResult == DialogResult.Ignore)
                {
                    return (null, false);
                }
                else
                {
                    return (null, true);
                }
            }
        }

        public static (string, bool) ChartLyricsGet (string artist, string song)
        {
            try
            {
                artist = artist.Trim();
                artist = artist.Replace(' ', '+');
                artist = artist.Replace("'", "");
                song = song.Trim();
                song = song.Replace(' ', '+');
                song = song.Replace("'", "");
                string url = "http://api.chartlyrics.com/apiv1.asmx/SearchLyricDirect?artist=" + artist + "&song=" + song;
                HttpResponse<string> response = Unirest.get(url)
                    .asJson<string>();

                if (response.Code != 200)
                {
                    artist = artist.Replace('+', ' ');
                    song = song.Replace('+', ' ');
                    DialogResult dialogResult = MessageBox.Show("Response code: " + response.Code + Environment.NewLine + "When searching: " + artist + " - " + song, "Server error!", MessageBoxButtons.AbortRetryIgnore);
                    if (dialogResult == DialogResult.Abort)
                    {
                        return (null, true);
                    }
                    else if (dialogResult == DialogResult.Retry)
                    {
                        Thread.Sleep(2000);
                        return ChartLyricsGet(artist, song);

                    }
                    else if (dialogResult == DialogResult.Ignore)
                    {
                        return (null, false);
                    }

                }
                response.Code.ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Body);

                XmlNodeList parentNode = doc.GetElementsByTagName("Lyric");
                
                return (parentNode[0].InnerText, false);
            }
            catch (IndexOutOfRangeException)
            {
                return (null, false);
            }

            catch (Exception e)
            {
                artist = artist.Replace('+', ' ');
                song = song.Replace('+', ' ');
                DialogResult dialogResult = MessageBox.Show("Error: " + e + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
                if (dialogResult == DialogResult.Abort)
                {
                    return (null, true);
                }
                else if (dialogResult == DialogResult.Retry)
                {
                    Thread.Sleep(2000);
                    return ChartLyricsGet(artist, song);

                }
                else if (dialogResult == DialogResult.Ignore)
                {
                    return (null, false);
                }
                else
                {
                    return (null, true);
                }
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
                {
                    e.Handled = e.KeyChar != (char)Keys.Back;
                }
            }
        }
        
        private void APIbutton_Click(object sender, EventArgs e)
        {
            var trackJSON = MusixmatchGet(aArtistTextBox.Text, titleTextBox.Text);
            if (trackJSON.Item1 != null)
            {
                //var trackJSON = MusixmatchGet(aArtistTextBox.Text, titleTextBox.Text);                
                albumTextBox.Text = trackJSON.Item1.AlbumName.ToString();
                yearTextBox.Text = trackJSON.Item1.FirstReleaseDate.Year.ToString();
                try
                {
                    genreBox.Text = trackJSON.Item1.PrimaryGenres.MusicGenre[0].MusicGenreName.ToString();
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show("Genre of the song was not found.", "No Results.");
                }
            }
            else
            {
                MessageBox.Show("The search did not return any results.", "No Results.");
            }
        }

        private void LyricsButton_Click(object sender, EventArgs e)
        {
            string lyricsValues = ChartLyricsGet(aArtistTextBox.Text, titleTextBox.Text).Item1;
            if (lyricsValues == "")
            {
                MessageBox.Show("Lyrics of the song were not found.", "No Results.");
            }
            else
            {
                lyricsTextBox.Text = lyricsValues;
            }
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
