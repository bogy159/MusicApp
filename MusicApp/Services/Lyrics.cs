using System.Text.Json;
using System.Xml;

namespace MusicApp.Services
{
    public static class Lyrics
    {
        private static readonly HttpClient SharedHttpClient = new() { Timeout = TimeSpan.FromSeconds(10) };

        static Lyrics()
        {
            SharedHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MusicApp/1.0");
        }

        public static async Task<(string, bool)> GetLyricsAsync(string artist, string song)
        {
            var lyrcisOvh = await LyrcisOvhGet(artist, song);
            if (!string.IsNullOrEmpty(lyrcisOvh))
                return (lyrcisOvh, false);

            var lRCLIB = await LRCLIBGet(artist, song);
            if (!string.IsNullOrEmpty(lRCLIB))
                return (lRCLIB, false);

            var chartLyrics = await ChartLyricsGet(artist, song);
            if (!string.IsNullOrEmpty(chartLyrics))
                return (chartLyrics, false);

            DialogResult dialogResult = DarkMessageBox.Show("Error: " + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
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

        private static async Task<string?> LRCLIBGet(string artist, string song)
        {
            try
            {
                artist = (artist ?? string.Empty).Trim();
                song = (song ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(song))
                    return null;

                static string Normalize(string value) => string.Join(" ", value.ToLowerInvariant().Split((char[])null, StringSplitOptions.RemoveEmptyEntries));
                static string GetLyrics(JsonElement track)
                {
                    if (track.TryGetProperty("plainLyrics", out var plainLyrics) &&
                        plainLyrics.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(plainLyrics.GetString()))
                        return plainLyrics.GetString().Trim();

                    if (track.TryGetProperty("syncedLyrics", out var syncedLyrics) &&
                        syncedLyrics.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(syncedLyrics.GetString()))
                        return syncedLyrics.GetString().Trim();

                    return null;
                }

                var url = $"https://lrclib.net/api/search?artist_name={Uri.EscapeDataString(artist)}&track_name={Uri.EscapeDataString(song)}";
                using var response = await SharedHttpClient.GetAsync(url).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    return null;

                var targetArtist = Normalize(artist);
                var targetSong = Normalize(song);

                foreach (var track in doc.RootElement.EnumerateArray())
                {
                    var trackArtist = track.TryGetProperty("artistName", out var artistName) ? Normalize(artistName.GetString() ?? string.Empty) : string.Empty;
                    var trackTitle = track.TryGetProperty("trackName", out var trackName) ? Normalize(trackName.GetString() ?? string.Empty) : string.Empty;
                    if (!string.IsNullOrEmpty(trackArtist) && !string.IsNullOrEmpty(trackTitle) &&
                        (trackArtist == targetArtist || trackArtist.Contains(targetArtist) || targetArtist.Contains(trackArtist)) &&
                        (trackTitle == targetSong || trackTitle.Contains(targetSong) || targetSong.Contains(trackTitle)))
                    {
                        var lyrics = GetLyrics(track);
                        if (!string.IsNullOrEmpty(lyrics))
                            return lyrics;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private static async Task<string> ChartLyricsGet(string artist, string song)
        {
            try
            {
                artist = artist.Trim();
                song = song.Trim();
                string url = $"http://api.chartlyrics.com/apiv1.asmx/SearchLyricDirect?artist={Uri.EscapeDataString(artist)}&song={Uri.EscapeDataString(song)}";

                using var response = await SharedHttpClient.GetAsync(url).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                XmlNodeList parentNode = doc.GetElementsByTagName("Lyric");

                return parentNode.Count > 0 ? parentNode[0]?.InnerText : null;
            }
            catch
            {
            }
            return null;
        }

        private static async Task<string> LyrcisOvhGet(string artist, string song)
        {
            try
            {
                artist = (artist ?? string.Empty).Trim();
                song = (song ?? string.Empty).Trim();
                var encArtist = Uri.EscapeDataString(artist);
                var encSong = Uri.EscapeDataString(song);
                string url = $"https://api.lyrics.ovh/v1/{encArtist}/{encSong}";

                using var response = await SharedHttpClient.GetAsync(url).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("lyrics", out var lyricsElem))
                {
                    var raw = lyricsElem.GetString();
                    return string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();
                }
            }
            catch
            {
            }
            return null;
        }
    }
}