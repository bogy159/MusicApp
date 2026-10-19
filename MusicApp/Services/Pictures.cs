using System.Text.Json;
using System.Drawing.Drawing2D;

namespace MusicApp.Services
{
    public static class Pictures
    {
        private const int MaxMetadataDimension = 500;

        private static Bitmap ResizeForMetadata(Image image)
        {
            var scale = Math.Min(1f, (float)MaxMetadataDimension / Math.Max(image.Width, image.Height));
            var width = Math.Max(1, (int)Math.Round(image.Width * scale));
            var height = Math.Max(1, (int)Math.Round(image.Height * scale));
            var resizedImage = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using var graphics = Graphics.FromImage(resizedImage);
            graphics.Clear(Color.White);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(image, 0, 0, width, height);

            return resizedImage;
        }

        public static TagLib.Picture ToTagPicture(Image image)
        {
            using var resizedImage = ResizeForMetadata(image);
            using var stream = new MemoryStream();
            var jpegEncoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
                .First(encoder => encoder.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
            using var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
            resizedImage.Save(stream, jpegEncoder, encoderParameters);

            return new TagLib.Picture
            {
                Type = TagLib.PictureType.FrontCover,
                Description = "Cover",
                MimeType = "image/jpeg",
                Data = stream.ToArray()
            };
        }

        public static async Task<(Image?, bool)> GetPicture(string artist, string song)
        {
            var iTunes = await ITunesGet(artist, song);
            if (iTunes != null)
                return (iTunes, false);

            var musicBrainz = await MusicBrainzGet(artist, song);
            if (musicBrainz != null)
                return (musicBrainz, false);

            DialogResult dialogResult = MessageBox.Show("Error: " + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
            if (dialogResult == DialogResult.Abort)
                return (null, true);
            else if (dialogResult == DialogResult.Retry)
            {
                Thread.Sleep(2000);
                return await GetPicture(artist, song);
            }
            else if (dialogResult == DialogResult.Ignore)
                return (null, false);
            else
                return (null, true);
        }

        public static async Task<Image?> ITunesGet(string artist, string song)
        {
            if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(song))
                return null;

            try
            {
                var query = Uri.EscapeDataString($"{artist} {song}");
                var searchUrl = $"https://itunes.apple.com/search?term={query}&entity=song&limit=10";

                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                using var response = await http.GetAsync(searchUrl).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (!document.RootElement.TryGetProperty("results", out var results) ||
                    results.ValueKind != JsonValueKind.Array)
                    return null;

                static string ReadString(JsonElement element, string name) =>
                    element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String
                        ? value.GetString() ?? string.Empty
                        : string.Empty;

                static string Normalize(string value) => value.Trim().ToLowerInvariant();

                var normalizedArtist = Normalize(artist);
                var normalizedSong = Normalize(song);
                var chosen = results.EnumerateArray().FirstOrDefault(result =>
                    Normalize(ReadString(result, "artistName")) == normalizedArtist &&
                    Normalize(ReadString(result, "trackName")) == normalizedSong);

                if (chosen.ValueKind == JsonValueKind.Undefined)
                {
                    chosen = results.EnumerateArray().FirstOrDefault(result =>
                        Normalize(ReadString(result, "artistName")).Contains(normalizedArtist, StringComparison.Ordinal) &&
                        Normalize(ReadString(result, "trackName")).Contains(normalizedSong, StringComparison.Ordinal));
                }

                if (chosen.ValueKind != JsonValueKind.Object)
                    return null;

                var artworkUrl = ReadString(chosen, "artworkUrl100");
                if (string.IsNullOrWhiteSpace(artworkUrl) || !Uri.TryCreate(artworkUrl, UriKind.Absolute, out var artworkUri))
                    return null;

                var highResolutionUrl = artworkUri.AbsoluteUri.Replace("100x100bb", "600x600bb", StringComparison.OrdinalIgnoreCase);
                using var imageResponse = await http.GetAsync(highResolutionUrl).ConfigureAwait(false);
                if (!imageResponse.IsSuccessStatusCode)
                    return null;

                var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                using var imageStream = new MemoryStream(imageBytes);
                using var decodedImage = Image.FromStream(imageStream);
                return new Bitmap(decodedImage);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<Image?> MusicBrainzGet(string artist, string song)
        {
            try
            {
                static string EscapeQueryValue(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"");
                static string Normalize(string? value) => string.Join(" ", (value ?? string.Empty)
                    .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

                var query = Uri.EscapeDataString($"recording:\"{EscapeQueryValue(song)}\" AND artist:\"{EscapeQueryValue(artist)}\"");
                var searchUrl = $"https://musicbrainz.org/ws/2/recording/?query={query}&fmt=json&limit=10";

                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
                http.DefaultRequestHeaders.UserAgent.ParseAdd("MusicApp/3.0 (https://github.com/bogy159/MusicApp)");

                using var searchResponse = await http.GetAsync(searchUrl).ConfigureAwait(false);
                if (!searchResponse.IsSuccessStatusCode)
                    return null;

                using var searchDocument = JsonDocument.Parse(await searchResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (!searchDocument.RootElement.TryGetProperty("recordings", out var recordings) ||
                    recordings.ValueKind != JsonValueKind.Array)
                    return null;

                static string ReadString(JsonElement element, string propertyName) =>
                    element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
                        ? value.GetString() ?? string.Empty
                        : string.Empty;

                static string GetArtistCredit(JsonElement recording)
                {
                    if (!recording.TryGetProperty("artist-credit", out var credits) || credits.ValueKind != JsonValueKind.Array)
                        return string.Empty;

                    return string.Join(" ", credits.EnumerateArray()
                        .Where(credit => credit.TryGetProperty("artist", out _))
                        .Select(credit => credit.GetProperty("artist"))
                        .Where(creditedArtist => creditedArtist.TryGetProperty("name", out _))
                        .Select(creditedArtist => creditedArtist.GetProperty("name").GetString() ?? string.Empty));
                }

                var normalizedArtist = Normalize(artist);
                var normalizedSong = Normalize(song);
                var chosenRecording = recordings.EnumerateArray().FirstOrDefault(recording =>
                    Normalize(ReadString(recording, "title")) == normalizedSong &&
                    Normalize(GetArtistCredit(recording)) == normalizedArtist);

                if (chosenRecording.ValueKind == JsonValueKind.Undefined)
                {
                    chosenRecording = recordings.EnumerateArray().FirstOrDefault(recording =>
                        Normalize(ReadString(recording, "title")).Contains(normalizedSong, StringComparison.Ordinal) &&
                        Normalize(GetArtistCredit(recording)).Contains(normalizedArtist, StringComparison.Ordinal));
                }

                var recordingId = ReadString(chosenRecording, "id");
                if (string.IsNullOrWhiteSpace(recordingId))
                    return null;

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

                var detailUrl = $"https://musicbrainz.org/ws/2/recording/{Uri.EscapeDataString(recordingId)}?inc=releases&fmt=json";
                using var detailResponse = await http.GetAsync(detailUrl).ConfigureAwait(false);
                if (!detailResponse.IsSuccessStatusCode)
                    return null;

                using var detailDocument = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (!detailDocument.RootElement.TryGetProperty("releases", out var releases) ||
                    releases.ValueKind != JsonValueKind.Array)
                    return null;

                var releaseCandidates = releases.EnumerateArray()
                    .Where(release => !string.IsNullOrWhiteSpace(ReadString(release, "id")))
                    .OrderByDescending(release => release.TryGetProperty("release-group", out var group) && ReadString(group, "primary-type") == "Album")
                    .ThenBy(release => ReadString(release, "date") is { Length: > 0 } date ? date : "9999")
                    .Take(5);

                foreach (var release in releaseCandidates)
                {
                    var releaseId = ReadString(release, "id");
                    var coverUrl = $"https://coverartarchive.org/release/{Uri.EscapeDataString(releaseId)}/front-500";
                    using var coverResponse = await http.GetAsync(coverUrl).ConfigureAwait(false);
                    if (!coverResponse.IsSuccessStatusCode)
                        continue;

                    var imageBytes = await coverResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    using var imageStream = new MemoryStream(imageBytes);
                    using var decodedImage = Image.FromStream(imageStream);
                    return new Bitmap(decodedImage);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}