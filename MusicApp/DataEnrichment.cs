using System.Text.Json;
using System.Text.RegularExpressions;

namespace MusicApp
{
    public class DataEnrichment
    {
        public static async Task<(Song, bool)> EnrichDataAsync(string artist, string song)
        { 
            try
            {
                var result = new Song();
                var iTunes = await ITunesGet(artist, song);
                if (!string.IsNullOrWhiteSpace(iTunes.Genre) && !string.IsNullOrWhiteSpace(iTunes.AlbumTitle) && !string.IsNullOrWhiteSpace(iTunes.Year) && IsValidYear(iTunes.Year))
                    return (iTunes, false);

                var discogs = await DiscogsGet(artist, song);
                if (!string.IsNullOrWhiteSpace(discogs.Genre) && !string.IsNullOrWhiteSpace(discogs.AlbumTitle) && !string.IsNullOrWhiteSpace(discogs.Year) && IsValidYear(discogs.Year))
                    return (discogs, false);

                var musicBrainz = await MusicBrainzGet(artist, song);
                if (!string.IsNullOrWhiteSpace(musicBrainz.Genre) && !string.IsNullOrWhiteSpace(musicBrainz.AlbumTitle) && !string.IsNullOrWhiteSpace(musicBrainz.Year) && IsValidYear(musicBrainz.Year))
                    return (musicBrainz, false);

                result.AlbumTitle = new[] { iTunes.AlbumTitle, discogs.AlbumTitle, musicBrainz.AlbumTitle }
                    .FirstOrDefault(title => !string.IsNullOrWhiteSpace(title)) ?? string.Empty;
                result.Genre = new[] { iTunes.Genre, discogs.Genre, musicBrainz.Genre }
                    .FirstOrDefault(genre => !string.IsNullOrWhiteSpace(genre)) ?? string.Empty;
                result.Year = new[] { iTunes.Year, discogs.Year, musicBrainz.Year }
                    .FirstOrDefault(year => !string.IsNullOrWhiteSpace(year) && IsValidYear(year)) ?? string.Empty;

                if (string.IsNullOrWhiteSpace(result.AlbumTitle) || string.IsNullOrWhiteSpace(result.Genre) || string.IsNullOrWhiteSpace(result.Year) || !IsValidYear(result.Year))
                    throw new Exception();
                return (result, false);
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

        private static async Task<Song> DiscogsGet(string artist, string song)
        {
            var result = new Song();

            try
            {
                static string ReadText(JsonElement element, string propertyName)
                {
                    if (!element.TryGetProperty(propertyName, out var value))
                        return string.Empty;

                    return value.ValueKind switch
                    {
                        JsonValueKind.String => value.GetString() ?? string.Empty,
                        JsonValueKind.Number => value.ToString(),
                        _ => string.Empty
                    };
                }

                static string ReadFirstArrayValue(JsonElement element, string propertyName)
                {
                    if (element.TryGetProperty(propertyName, out var values) &&
                        values.ValueKind == JsonValueKind.Array && values.GetArrayLength() > 0)
                        return values[0].GetString() ?? string.Empty;

                    return string.Empty;
                }

                var searchUrl = $"https://api.discogs.com/database/search?type=release&per_page=5&artist={Uri.EscapeDataString(artist)}&track={Uri.EscapeDataString(song)}";

                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                http.DefaultRequestHeaders.UserAgent.ParseAdd("MusicApp/1.0 (https://github.com/bogy159/MusicApp)");

                using var searchResponse = await http.GetAsync(searchUrl).ConfigureAwait(false);
                if (!searchResponse.IsSuccessStatusCode)
                    return result;

                using var searchDoc = JsonDocument.Parse(await searchResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (!searchDoc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                    return result;

                var release = results[0];
                if (!release.TryGetProperty("id", out var idValue))
                    return result;

                var releaseUrl = $"https://api.discogs.com/releases/{Uri.EscapeDataString(idValue.ToString())}";
                using var releaseResponse = await http.GetAsync(releaseUrl).ConfigureAwait(false);
                if (!releaseResponse.IsSuccessStatusCode)
                    return result;

                using var releaseDoc = JsonDocument.Parse(await releaseResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                var releaseDetails = releaseDoc.RootElement;

                result.AlbumTitle = ReadText(releaseDetails, "title");
                result.Year = ReadText(releaseDetails, "year");
                result.Genre = ReadFirstArrayValue(releaseDetails, "genres");
                if (string.IsNullOrEmpty(result.Genre))
                    result.Genre = ReadFirstArrayValue(releaseDetails, "styles");

                if (string.IsNullOrEmpty(result.Year))
                    result.Year = ReadText(release, "year");
                if (string.IsNullOrEmpty(result.Genre))
                    result.Genre = ReadFirstArrayValue(release, "genre");

                return result;
            }
            catch
            {
                return result;
            }
        }

        private static async Task<Song> MusicBrainzGet(string artist, string song)
        {
            var result = new Song();

            try
            {
                static string EscapeQueryValue(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"");
                static string Normalize(string value) => Regex.Replace((value ?? string.Empty).ToLowerInvariant(), @"\s+", " ").Trim();

                var query = Uri.EscapeDataString($"recording:\"{EscapeQueryValue(song)}\" AND artist:\"{EscapeQueryValue(artist)}\"");
                var searchUrl = $"https://musicbrainz.org/ws/2/recording/?query={query}&fmt=json&limit=5";

                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                http.DefaultRequestHeaders.UserAgent.ParseAdd("MusicApp/1.0 (music metadata lookup)");

                using var searchResponse = await http.GetAsync(searchUrl).ConfigureAwait(false);
                if (!searchResponse.IsSuccessStatusCode)
                    return result;

                using var searchDoc = JsonDocument.Parse(await searchResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (!searchDoc.RootElement.TryGetProperty("recordings", out var recordings) || recordings.GetArrayLength() == 0)
                    return result;

                var targetArtist = Normalize(artist);
                var targetSong = Normalize(song);
                JsonElement chosen = recordings[0];

                foreach (var recording in recordings.EnumerateArray())
                {
                    var title = recording.TryGetProperty("title", out var titleValue) ? Normalize(titleValue.GetString()) : string.Empty;
                    var artistNames = new List<string>();
                    if (recording.TryGetProperty("artist-credit", out var credits))
                    {
                        foreach (var credit in credits.EnumerateArray())
                        {
                            if (credit.ValueKind == JsonValueKind.Object && credit.TryGetProperty("artist", out var creditedArtist) &&
                                creditedArtist.TryGetProperty("name", out var artistName))
                                artistNames.Add(artistName.GetString() ?? string.Empty);
                        }
                    }

                    var creditedArtists = Normalize(string.Join(" ", artistNames));
                    if (title.Contains(targetSong) && creditedArtists.Contains(targetArtist))
                    {
                        chosen = recording;
                        break;
                    }
                }

                if (!chosen.TryGetProperty("id", out var idValue))
                    return result;

                var detailUrl = $"https://musicbrainz.org/ws/2/recording/{Uri.EscapeDataString(idValue.GetString() ?? string.Empty)}?inc=releases+tags&fmt=json";
                using var detailResponse = await http.GetAsync(detailUrl).ConfigureAwait(false);
                if (!detailResponse.IsSuccessStatusCode)
                    return result;

                using var detailDoc = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                var detail = detailDoc.RootElement;

                if (detail.TryGetProperty("releases", out var releases) && releases.GetArrayLength() > 0)
                {
                    var albumRelease = releases.EnumerateArray()
                        .Where(release => release.TryGetProperty("release-group", out var releaseGroup) &&
                                          releaseGroup.TryGetProperty("primary-type", out var primaryType) &&
                                          primaryType.GetString() == "Album")
                        .OrderBy(release => release.TryGetProperty("date", out var date) ? date.GetString() ?? "9999" : "9999")
                        .FirstOrDefault();

                    if (albumRelease.ValueKind == JsonValueKind.Undefined)
                        albumRelease = releases.EnumerateArray()
                            .OrderBy(release => release.TryGetProperty("date", out var date) ? date.GetString() ?? "9999" : "9999")
                            .FirstOrDefault();

                    if (albumRelease.ValueKind == JsonValueKind.Object)
                    {
                        if (albumRelease.TryGetProperty("title", out var albumTitle))
                            result.AlbumTitle = albumTitle.GetString() ?? string.Empty;

                        if (albumRelease.TryGetProperty("date", out var releaseDate))
                        {
                            var date = releaseDate.GetString();
                            if (!string.IsNullOrEmpty(date) && date.Length >= 4)
                                result.Year = date[..4];
                        }
                    }
                }

                if (detail.TryGetProperty("tags", out var tags) && tags.GetArrayLength() > 0)
                {
                    var topTag = tags.EnumerateArray()
                        .OrderByDescending(tag => tag.TryGetProperty("count", out var count) ? count.GetInt32() : 0)
                        .FirstOrDefault();
                    if (topTag.ValueKind == JsonValueKind.Object && topTag.TryGetProperty("name", out var tagName))
                        result.Genre = tagName.GetString() ?? string.Empty;
                }

                return result;
            }
            catch
            {
                return result;
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

        private static bool IsValidYear(string? value) => int.TryParse(value, out var year) && year > 0;
    }
}