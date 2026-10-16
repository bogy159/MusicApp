using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using unirest_net.http;

namespace MusicApp
{
    public class DataEnrichment
    {
        public static async Task<(Song, bool)> EnrichDataAsync(string artist, string song)
        { 
            try
            {
                var newData = new Song();
                newData = await ITunesGet(artist, song);
                if (!string.IsNullOrEmpty(newData.Genre) && !string.IsNullOrEmpty(newData.AlbumTitle) && !string.IsNullOrEmpty(newData.Year))
                    return (newData, false);

                /*
                var extraData = MusixmatchGet(artist, song);
                if (extraData.Item1 != null)
                    newData = await MusixmatchFormat(extraData.Item1);
                */
                throw new Exception();
            }
            catch (Exception ex)
            {
                DialogResult dialogResult = MessageBox.Show("Error: " + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
                if (dialogResult == DialogResult.Abort)
                    return (null, true);
                else if (dialogResult == DialogResult.Retry)
                {
                    Thread.Sleep(2000);
                    return await EnrichDataAsync(artist, song);
                }
                else if (dialogResult == DialogResult.Ignore)
                    return (null, false);
                else
                    return (null, true);
            }
        }

        private static async Task<Song> ITunesGet(string artist, string song)
        {
            var query = Uri.EscapeDataString($"{artist} {song}");
            var url = $"https://itunes.apple.com/search?term={query}&entity=song&limit=5";

            try
            {
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
                using var resp = await http.GetAsync(url).ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                    return null;

                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                    return null;

                static string Normalize(string s) => Regex.Replace((s ?? string.Empty).ToLowerInvariant(), @"\s+", " ").Trim();

                var targetArtist = Normalize(artist);
                var targetSong = Normalize(song);

                JsonElement chosen = default;
                bool chosenSet = false;

                foreach (var r in results.EnumerateArray())
                {
                    string getStr(string name) => r.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : string.Empty;

                    var trackName = getStr("trackName");
                    var artistName = getStr("artistName");

                    if (string.IsNullOrEmpty(trackName) || string.IsNullOrEmpty(artistName))
                        continue;

                    var nTrack = Normalize(trackName);
                    var nArtist = Normalize(artistName);

                    if ((!string.IsNullOrEmpty(targetSong) && nTrack.Contains(targetSong) || string.IsNullOrEmpty(targetSong)) &&
                        (!string.IsNullOrEmpty(targetArtist) && nArtist.Contains(targetArtist) || string.IsNullOrEmpty(targetArtist)))
                    {
                        chosen = r;
                        chosenSet = true;
                        break;
                    }

                    if (!chosenSet)
                    {
                        chosen = r;
                        chosenSet = true;
                    }
                }

                if (!chosenSet)
                    return null;

                string pick(string name) => chosen.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : string.Empty;

                var result = new Song
                {
                    AlbumTitle = pick("collectionName") ?? string.Empty,
                    Genre = pick("primaryGenreName") ?? string.Empty
                };

                var release = pick("releaseDate");
                if (!string.IsNullOrEmpty(release) && DateTime.TryParse(release, out var dt))
                    result.Year = dt.Year.ToString();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static (TrackSearchJSON.Track, bool) MusixmatchGet(string artist, string song)
        {
            try
            {
                artist = artist.Trim();
                artist = artist.Replace(' ', '+');
                song = song.Trim();
                song = song.Replace(' ', '+');
                string APIKey = "";
                string url = "https://api.musixmatch.com/ws/1.1/track.search?format=json&callback=callback&q_track=" + song + "&q_artist=" + artist + "&s_track_rating=desc&quorum_factor=1&page_size=1&apikey=" + APIKey;

                HttpResponse<string> response = Unirest.get(url).asJson<string>();

                if (response.Code != 200)
                {
                    artist = artist.Replace('+', ' ');
                    song = song.Replace('+', ' ');
                    DialogResult dialogResult = MessageBox.Show("Response code: " + response.Code + Environment.NewLine + "When searching: " + artist + " - " + song, "Server error!", MessageBoxButtons.AbortRetryIgnore);
                    if (dialogResult == DialogResult.Abort)
                        return (null, true);
                    else if (dialogResult == DialogResult.Retry)
                    {
                        Thread.Sleep(2000);
                        return MusixmatchGet(artist, song);
                    }
                    else if (dialogResult == DialogResult.Ignore)
                        return (null, false);
                }
                response.Code.ToString();
                TrackSearchJSON.Main result = JsonConvert.DeserializeObject<TrackSearchJSON.Main>(response.Body.ToString());

                return (result.Message.Body.TrackList[0].Track, false);
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
                    return (null, true);
                else if (dialogResult == DialogResult.Retry)
                {
                    Thread.Sleep(2000);
                    return MusixmatchGet(artist, song);

                }
                else if (dialogResult == DialogResult.Ignore)
                    return (null, false);
                else
                    return (null, true);
            }
        }
    
        private static async Task<Song> MusixmatchFormat(TrackSearchJSON.Track trackJSON)
        {
            return new Song()
            {
                AlbumTitle = trackJSON.AlbumName?.ToString(),
                //Year = trackJSON.FirstReleaseDate?.Year.ToString(),
                Genre = trackJSON.PrimaryGenres?.MusicGenreList[0]?.MusicGenre?.MusicGenreName?.ToString()
            };
        }
    }
}