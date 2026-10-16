using System.Collections.Concurrent;
using System.Data;
namespace MusicApp
{
    // Reusable helper that reads music metadata using TagLib# in parallel and
    // exposes convenience helpers to get a list of Song objects or a DataTable.
    public static class BulkSongsReader
    {
        /// <summary>
        /// Reads metadata for the provided files in parallel.
        /// </summary>
        /// <param name="files">Enumerable of file paths to read.</param>
        /// <param name="maxDegreeOfParallelism">If <=0, Environment.ProcessorCount is used.</param>
        /// <param name="progress">Optional progress reporter (reports processed count).</param>
        public static async Task<List<SongExtended>> ReadSongsAsync(IEnumerable<string> files, int maxDegreeOfParallelism = 10, IProgress<int>? progress = null)
        {
            var fileArray = files as string[] ?? files.ToArray();
            if (maxDegreeOfParallelism <= 0) maxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount);

            var bag = new ConcurrentBag<SongExtended>();
            int processed = 0;
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };

            // Progress reporting: aim for ~100 updates and also throttle by time to keep UI smooth.
            int total = fileArray.Length;
            const int targetUpdates = 10;
            int chunk = Math.Max(1, total / targetUpdates);
            var sw = System.Diagnostics.Stopwatch.StartNew();
            long lastReportMs = 0;

            await Parallel.ForEachAsync(fileArray, options, async (file, ct) =>
            {
                try
                {
                    // Run the (synchronous) TagLib file open on a thread-pool thread so we don't block the caller
                    await Task.Run(() =>
                    {
                        try
                        {
                            using var tfile = TagLib.File.Create(file);
                            var tag = tfile.Tag;
                            var props = tfile.Properties;

                            var song = new SongExtended
                            {
                                Location = file,
                                FileName = Path.GetFileName(file),
                                FixName = Validators.NameCheck(Path.GetFileName(file)),
                                AlbumArtist = (tag.AlbumArtists?.FirstOrDefault() ?? tag.FirstAlbumArtist) ?? string.Empty,
                                ContributingArtist = (tag.Performers != null && tag.Performers.Length > 0) ? string.Join("; ", tag.Performers) : string.Empty,
                                SongTitle = tag.Title ?? string.Empty,
                                AlbumTitle = tag.Album ?? string.Empty,
                                Year = tag.Year > 0 ? tag.Year.ToString() : string.Empty,
                                Genre = tag.FirstGenre ?? string.Empty,
                                Lyrics = string.IsNullOrEmpty(tag.Lyrics) ? "No" : "Yes",
                                Length = props.Duration != TimeSpan.Zero ? props.Duration.ToString(props.Duration.Hours > 0 ? "h\\:mm\\:ss" : "mm\\:ss") : string.Empty,
                                Size = (Convert.ToDouble(new FileInfo(file).Length) / 1024 / 1024).ToString() ?? string.Empty,
                                BitRate = props.AudioBitrate > 0 ? props.AudioBitrate.ToString() : string.Empty
                            };
                            song.ChangeArtist = Validators.ArtistCheck(song);
                            song.ChangeTitle = Validators.TitleCheck(song);

                            bag.Add(song);
                        }
                        catch
                        {
                            // ignore single-file failures
                        }
                    }, ct).ConfigureAwait(false);
                }
                finally
                {
                    var count = Interlocked.Increment(ref processed);

                    // Decide whether to report: every 'chunk' items, always on final item, or at most once every 200ms
                    bool shouldReport = false;
                    if (count == total) shouldReport = true;
                    else if (count % chunk == 0) shouldReport = true;
                    else
                    {
                        var elapsed = sw.ElapsedMilliseconds;
                        var last = Interlocked.Read(ref lastReportMs);
                        if (elapsed - last >= 200)
                        {
                            Interlocked.Exchange(ref lastReportMs, elapsed);
                            shouldReport = true;
                        }
                    }

                    if (shouldReport)
                    {
                        int percent = total == 0 ? 100 : (int)((count * 100L) / total);
                        progress?.Report(percent);
                    }
                }
            }).ConfigureAwait(false);

            return bag.OrderBy(s => s.FileName).ToList();
        }

        /// <summary>
        /// Converts a Song list to a DataTable compatible with existing UI code.
        /// </summary>
        public static DataTable ToDataTable(IEnumerable<SongExtended> songs)
        {
            var dt = new DataTable();
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("File Name");
            dt.Columns.Add("Fix Name");
            dt.Columns.Add("Needed Changes in Artists");
            dt.Columns.Add("Needed Changes in Title");
            dt.Columns.Add("Album Artist");
            dt.Columns.Add("Contribution Artist");
            dt.Columns.Add("Song Title");
            dt.Columns.Add("Album");
            dt.Columns.Add("Year");
            dt.Columns.Add("Genre");
            dt.Columns.Add("Length");
            dt.Columns.Add("Size in MB");
            dt.Columns.Add("BitRate");
            dt.Columns.Add("File Path");
            dt.Columns.Add("Has Lyrics");

            var i = 0;
            foreach (var s in songs)
            {
                i++;
                dt.Rows.Add(i, s.FileName, s.FixName, s.ChangeArtist, s.ChangeTitle, s.AlbumArtist, s.ContributingArtist, s.SongTitle, s.AlbumTitle, s.Year, s.Genre, s.Length, s.Size, s.BitRate, s.Location, s.Lyrics);
            }

            return dt;
        }
    }
}