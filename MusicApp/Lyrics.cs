using System.Text.Json;
using System.Xml;
using unirest_net.http;

namespace MusicApp
{
    public static class Lyrics
    {
        public static async Task<(string, bool)> GetLyricsAsync(string artist, string song)
        {
            var lyrics = await LyrcisOvhGet(artist, song);
            if (!string.IsNullOrEmpty(lyrics))
                return (lyrics, false); 
            lyrics = await ChartLyricsGet(artist, song);
            if (!string.IsNullOrEmpty(lyrics))
                return (lyrics, false);

            DialogResult dialogResult = MessageBox.Show("Error: " + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
            if (dialogResult == DialogResult.Abort)
                return (null, true);
            else if (dialogResult == DialogResult.Retry)
            {
                Thread.Sleep(2000);
                return await GetLyricsAsync(artist, song);
            }
            else if (dialogResult == DialogResult.Ignore)
                return (null, false);
            else
                return (null, true);
        }

        public static async Task<string> ChartLyricsGet(string artist, string song)
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
                HttpResponse<string> response = Unirest.get(url).asJson<string>();

                if (response.Code != 200)
                    return null;
                response.Code.ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Body);

                XmlNodeList parentNode = doc.GetElementsByTagName("Lyric");

                return parentNode[0].InnerText;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public static async Task<string> LyrcisOvhGet(string artist, string song)
        {
            try
            {
                artist = (artist ?? string.Empty).Trim();
                song = (song ?? string.Empty).Trim();
                var encArtist = Uri.EscapeDataString(artist);
                var encSong = Uri.EscapeDataString(song);
                string url = $"https://api.lyrics.ovh/v1/{encArtist}/{encSong}";

                HttpResponse<string> response = Unirest.get(url).asJson<string>();

                if (response.Code != 200)
                    return null;

                var json = response.Body;
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("lyrics", out var lyricsElem))
                {
                    var raw = lyricsElem.GetString();
                    return string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
