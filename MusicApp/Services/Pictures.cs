using System.Drawing.Drawing2D;
using System.Text.Json;

namespace MusicApp.Services
{
    public static class Pictures
    {
        private const int MaxMetadataDimension = 500;

        public static async Task<(Image?, bool)> GetPicture(string artist, string song, int apiUsed = 0)
        {
            var providers = new Func<Task<Image?>>[]
            {
                () => ITunesGet(artist, song),
                () => DeezerGet(artist, song),
                () => TheAudioDBGet(artist, song),
            };
            var firstProvider = ((apiUsed % providers.Length) + providers.Length) % providers.Length;

            for (var offset = 0; offset < providers.Length; offset++)
            {
                var picture = await providers[(firstProvider + offset) % providers.Length]();
                if (picture != null)
                    return (picture, false);
            }

            DialogResult dialogResult = MessageBox.Show("Error: " + Environment.NewLine + "When searching: " + artist + " - " + song, "Exception!", MessageBoxButtons.AbortRetryIgnore);
            if (dialogResult == DialogResult.Abort)
                return (null, true);
            else if (dialogResult == DialogResult.Retry)
            {
                Thread.Sleep(2000);
                return await GetPicture(artist, song, apiUsed);
            }
            else if (dialogResult == DialogResult.Ignore)
                return (null, false);
            else
                return (null, true);
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

        private static async Task<Image?> ITunesGet(string artist, string song)
        {
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

        private static async Task<Image?> DeezerGet(string artist, string song)
        {
            try
            {
                static string Normalize(string? value) => string.Join(" ", (value ?? string.Empty)
                    .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

                static string ReadString(JsonElement element, string propertyName) =>
                    element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
                        ? value.GetString() ?? string.Empty
                        : string.Empty;

                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                var query = Uri.EscapeDataString($"{artist} {song}");
                var searchUrl = $"https://api.deezer.com/search?q={query}&limit=10";

                using var searchResponse = await http.GetAsync(searchUrl).ConfigureAwait(false);
                if (!searchResponse.IsSuccessStatusCode)
                    return null;

                using var searchDocument = JsonDocument.Parse(await searchResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (!searchDocument.RootElement.TryGetProperty("data", out var tracks) ||
                    tracks.ValueKind != JsonValueKind.Array)
                    return null;

                var normalizedArtist = Normalize(artist);
                var normalizedSong = Normalize(song);

                bool Matches(JsonElement track, bool allowPartialMatch)
                {
                    if (!track.TryGetProperty("artist", out var trackArtist) || trackArtist.ValueKind != JsonValueKind.Object ||
                        !track.TryGetProperty("album", out var album) || album.ValueKind != JsonValueKind.Object)
                        return false;

                    var candidateArtist = Normalize(ReadString(trackArtist, "name"));
                    var candidateSong = Normalize(ReadString(track, "title"));
                    if (candidateArtist.Length == 0 || candidateSong.Length == 0)
                        return false;

                    return allowPartialMatch
                        ? candidateArtist.Contains(normalizedArtist, StringComparison.Ordinal) &&
                          candidateSong.Contains(normalizedSong, StringComparison.Ordinal)
                        : candidateArtist == normalizedArtist && candidateSong == normalizedSong;
                }

                var chosen = tracks.EnumerateArray().FirstOrDefault(track => Matches(track, allowPartialMatch: false));
                if (chosen.ValueKind == JsonValueKind.Undefined)
                    chosen = tracks.EnumerateArray().FirstOrDefault(track => Matches(track, allowPartialMatch: true));
                if (chosen.ValueKind != JsonValueKind.Object ||
                    !chosen.TryGetProperty("album", out var chosenAlbum) || chosenAlbum.ValueKind != JsonValueKind.Object)
                    return null;

                var coverUrl = new[] { "cover_xl", "cover_big", "cover_medium", "cover" }
                    .Select(propertyName => ReadString(chosenAlbum, propertyName))
                    .FirstOrDefault(value => Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp));

                if (string.IsNullOrWhiteSpace(coverUrl))
                    return null;

                using var imageResponse = await http.GetAsync(coverUrl).ConfigureAwait(false);
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

        private static async Task<Image?> TheAudioDBGet(string artist, string song)
        {
            try
            {
                static string Normalize(string? value) => string.Join(" ", (value ?? string.Empty)
                    .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

                static string ReadString(JsonElement element, string propertyName) =>
                    element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
                        ? value.GetString() ?? string.Empty
                        : string.Empty;

                var apiKey = Program.AudioDbApiKey;
                if (string.IsNullOrWhiteSpace(apiKey))
                    return null;

                var searchUrl = $"https://www.theaudiodb.com/api/v1/json/{Uri.EscapeDataString(apiKey)}/searchtrack.php?s={Uri.EscapeDataString(artist)}&t={Uri.EscapeDataString(song)}";
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                using var response = await http.GetAsync(searchUrl).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                using var document = JsonDocument.Parse(jsonString);
                if (!document.RootElement.TryGetProperty("track", out var tracks) || tracks.ValueKind != JsonValueKind.Array)
                    return null;

                var normalizedArtist = Normalize(artist);
                var normalizedSong = Normalize(song);

                bool Matches(JsonElement track, bool allowPartialMatch)
                {
                    var candidateArtist = Normalize(ReadString(track, "strArtist"));
                    var candidateSong = Normalize(ReadString(track, "strTrack"));
                    if (candidateArtist.Length == 0 || candidateSong.Length == 0)
                        return false;

                    return allowPartialMatch
                        ? candidateArtist.Contains(normalizedArtist, StringComparison.Ordinal) &&
                          candidateSong.Contains(normalizedSong, StringComparison.Ordinal)
                        : candidateArtist == normalizedArtist && candidateSong == normalizedSong;
                }

                var chosen = tracks.EnumerateArray().FirstOrDefault(track => Matches(track, allowPartialMatch: false));
                if (chosen.ValueKind == JsonValueKind.Undefined)
                    chosen = tracks.EnumerateArray().FirstOrDefault(track => Matches(track, allowPartialMatch: true));

                // Fallback: If no strict/partial artist+song match worked, take the first track if the array isn't empty
                if (chosen.ValueKind == JsonValueKind.Undefined)
                    chosen = tracks.EnumerateArray().FirstOrDefault();

                if (chosen.ValueKind != JsonValueKind.Object)
                    return null;

                // Expanded property list to check track-specific and album-specific artwork keys
                var coverUrl = new[] { "strTrackThumb", "strAlbumThumbHQ", "strAlbumThumb", "strTrackBitmap" }
                    .Select(propertyName => ReadString(chosen, propertyName))
                    .FirstOrDefault(value => Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp));

                if (string.IsNullOrWhiteSpace(coverUrl))
                    return null;

                using var imageResponse = await http.GetAsync(coverUrl).ConfigureAwait(false);
                if (!imageResponse.IsSuccessStatusCode)
                    return null;

                var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                using var imageStream = new MemoryStream(imageBytes);
                using var decodedImage = Image.FromStream(imageStream);
                return new Bitmap(decodedImage);
            }
            catch (Exception ex)
            {
                // Optional: Console.WriteLine(ex); to catch potential Image.FromStream platform exceptions (like missing GDI+ on non-Windows environments)
                return null;
            }
        }

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
    }
}