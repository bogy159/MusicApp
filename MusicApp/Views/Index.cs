using MusicApp.Helpers;
using MusicApp.Services;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using Application = System.Windows.Forms.Application;

namespace MusicApp
{
    public partial class Index : Form
    {
        public static DataTable tableFin;
        private string path;
        private int PgSize = 10;
        private int CurrentPageIndex = 1;
        private string errorList;

        public Index()
        {
            InitializeComponent();

            //mandatory. Otherwise will throw an exception when calling ReportProgress method  
            backgroundWorker1.WorkerReportsProgress = true;

            //mandatory. Otherwise we would get an InvalidOperationException when trying to cancel the operation  
            backgroundWorker1.WorkerSupportsCancellation = true;

            backgroundWorker1.DoWork +=
                 new DoWorkEventHandler(Bw_DoWork);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(Bw_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(Bw_RunWorkerCompleted);

        }

        private int CalculateTotalPages()
        {
            int rowCount = tableFin.Rows.Count;
            int TotalPage = rowCount / PgSize;
            // if any row left after calculated pages, add one more page 
            if (rowCount % PgSize > 0)
                TotalPage += 1;
            return TotalPage;
        }

        private DataTable GetCurrentRecords(int page, DataTable whole)
        {
            DataTable dt = whole.Clone();
            CalculateTotalPages();
            if (page != CalculateTotalPages())
            {
                for (int i = page * PgSize - PgSize; i < page * PgSize; i++)
                {
                    DataRow row = whole.Rows[i];
                    dt.ImportRow(row);
                }
            }
            else if (page == CalculateTotalPages())
            {
                for (int i = page * PgSize - PgSize; i < tableFin.Rows.Count; i++)
                {
                    DataRow row = whole.Rows[i];
                    dt.ImportRow(row);
                }

            }
            return dt;
        }

        private DataTable SeachFileName(string word, DataTable whole)
        {
            whole.DefaultView.RowFilter = "[File Name] LIKE '%" + word + "%'";
            DataTable dtOutput = whole.DefaultView.ToTable();

            return dtOutput;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                DataTable table = new DataTable();
                table.Columns.Add("No", typeof(int));
                table.Columns.Add("File Name", typeof(string));
                table.Columns.Add("Fix Name");
                table.Columns.Add("Needed Changes in Album Artist");
                table.Columns.Add("Needed Changes in Title");
                table.Columns.Add("Album Artist");
                table.Columns.Add("Title");
                table.Columns.Add("Album");
                table.Columns.Add("Year");
                table.Columns.Add("Length");
                table.Columns.Add("Bit Rate");
                table.Columns.Add("Size in MB", typeof(double));
                table.Columns.Add("Has Picture");
                table.Columns.Add("File Path");
                table.Columns.Add("Has Lyrics");

                for (int i = 0; i < files.Length; i++)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    FileInfo file = new FileInfo(files[i]);
                    using (TagLib.File tagFile = TagLib.File.Create(file.FullName))
                    {
                        TagLib.Tag tag = tagFile.Tag;
                        var properties = tagFile.Properties;
                        string albumArtist = tag.AlbumArtists?.FirstOrDefault() ?? string.Empty;
                        string title = tag.Title ?? string.Empty;
                        string filename = file.Name;
                        string year = tag.Year > 0 ? tag.Year.ToString() : string.Empty;
                        string lyrics = tag.Lyrics ?? string.Empty;

                        DataRow row = table.NewRow();
                        row["No"] = i + 1;
                        row["Album Artist"] = albumArtist;
                        row["File Name"] = filename;
                        row["Title"] = title;
                        row["Album"] = tag.Album ?? string.Empty;
                        row["Year"] = year;
                        row["Length"] = properties.Duration.ToString(@"mm\:ss");
                        row["Bit Rate"] = properties.AudioBitrate.ToString();
                        row["Size in MB"] = file.Length / (1024d * 1024d);
                        string falseName = Validators.NameCheck(filename);
                        row["Fix Name"] = falseName;
                        row["Has Picture"] = Validators.IsValidPicture(tag.Pictures?.FirstOrDefault()) ? "Yes" : "No";
                        row["File Path"] = file.FullName;
                        row["Has Lyrics"] = string.IsNullOrEmpty(lyrics) ? "No" : "Yes";

                        if (string.IsNullOrEmpty(albumArtist) || albumArtist.Contains("ntitled") || albumArtist.Contains("various") || albumArtist.Contains("Various") || albumArtist.Contains("nknown"))
                        {
                            string[] newName = Regex.Split(filename, "-");
                            string[] newest = Regex.Split(newName[0], "\\.");

                            if (falseName != "")
                                row["Needed Changes in Album Artist"] = "Improper file name";
                            else if (newest.Length >= 2 && newest[1] != null)
                                row["Needed Changes in Album Artist"] = newest[1];
                        }

                        if (title == "" || title.Contains("ntitled") || title.Contains("nknown") || title.Contains("Track"))
                        {
                            string[] newName = Regex.Split(filename, "-");

                            if (falseName != "")
                                row["Needed Changes in Title"] = "Improper file name";
                            else if (newName.Length >= 2 && newName[1] != null)
                            {
                                string[] newest = Regex.Split(newName[1], "\\.");
                                row["Needed Changes in Title"] = newest[0];
                            }
                        }

                        table.Rows.Add(row);
                        tableFin = table;
                    }
                    worker.ReportProgress(i);
                }
            }

            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File not found exception!", ex.Message);
                path = "";
            }
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            double j = 100 * (e.ProgressPercentage + 1) / files.Length;
            j = Math.Round(j);
            this.label1.Text = (e.ProgressPercentage + 1 + "/" + files.Length.ToString() + "=> " + Convert.ToInt32(j) + "%");
            progressBar1.Value = Convert.ToInt32(j);
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                MessageBox.Show("Cancelled!", "Cancelled");
                if (tableFin != null)
                    dataGridView1.DataSource = GetCurrentRecords(1, tableFin);
                CancelButton2.Enabled = false;
                changePgSizeButton.Enabled = false;
                searchButton.Enabled = false;
                changeArtistsInfoToolStripMenuItem.Enabled = false;
                changeTitleToolStripMenuItem.Enabled = false;
                updateEnrichmentToolStripMenuItem.Enabled = false;
                lyricsUpdateToolStripMenuItem.Enabled = false;
                picturesUpdateToolStripMenuItem.Enabled = false;
            }
            else if (!(e.Error == null))
            {
                MessageBox.Show("Error!", "Error");
                DisableButtons();
            }
            else
                SuccessfulDataGridLoad();
        }

        private void SuccessfulDataGridLoad()
        {
            if (tableFin != null)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = GetCurrentRecords(1, tableFin);
            }
            CancelButton2.Enabled = false;
            if (path != "")
            {
                changeArtistsInfoToolStripMenuItem.Enabled = true;
                changeTitleToolStripMenuItem.Enabled = true;
                updateEnrichmentToolStripMenuItem.Enabled = true;
                lyricsUpdateToolStripMenuItem.Enabled = true;
                picturesUpdateToolStripMenuItem.Enabled = true;
                CurrentPageIndex = 1;
                pageTextBox.Text = CurrentPageIndex.ToString() + "/" + CalculateTotalPages().ToString();
                itemsNumTextBox.Text = PgSize.ToString();
                if (CalculateTotalPages() > 1)
                {
                    nextButton.Enabled = true;
                    lastButton.Enabled = true;
                }
                changePgSizeButton.Enabled = true;
                searchButton.Enabled = true;
                refreshButton.Enabled = true;
                refreshExtendedButton.Enabled = true;
                statsButton.Enabled = true;
            }
        }

        private void DisableButtons()
        {
            changeArtistsInfoToolStripMenuItem.Enabled = false;
            changeTitleToolStripMenuItem.Enabled = false;
            updateEnrichmentToolStripMenuItem.Enabled = false;
            lyricsUpdateToolStripMenuItem.Enabled = false;
            picturesUpdateToolStripMenuItem.Enabled = false;
            searchButton.Enabled = false;
            CancelButton2.Enabled = false;
            lastButton.Enabled = false;
            previousButton.Enabled = false;
            nextButton.Enabled = false;
            lastButton.Enabled = false;
            changePgSizeButton.Enabled = false;
            refreshButton.Enabled = false;
            refreshExtendedButton.Enabled = false;
            statsButton.Enabled = false;
        }

        #region Buttons and Clicks

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            path = null;
            tableFin = null;
            Application.Exit();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = path;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath, "*.mp3", SearchOption.AllDirectories);

                DialogResult dialogResult = MessageBox.Show("Files found: " + files.Length.ToString() + Environment.NewLine + "This Process can be slow once started. Do you want to continue?", "Countinue?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    tableFin = null;
                    DisableButtons();
                    if (files.Length != 0)
                    {
                        path = fbd.SelectedPath;
                        if (backgroundWorker1.IsBusy != true)
                        {
                            CancelButton2.Enabled = true;
                            backgroundWorker1.RunWorkerAsync();
                        }
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
        }

        private void ChangeArtistsInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                var counter = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    if (Validators.NameCheck(file.Name) == "")
                    {
                        using TagLib.File tagFile = TagLib.File.Create(file.FullName);
                        TagLib.Tag tag = tagFile.Tag;
                        string contrArtist = tag.Performers?.FirstOrDefault() ?? string.Empty;
                        string albumArtist = tag.AlbumArtists?.FirstOrDefault() ?? string.Empty;
                        string[] nameParts = Regex.Split(file.Name, " - ");
                        string[] artistParts = Regex.Split(nameParts[0], "\\.");
                        if (artistParts.Length >= 2)
                        {
                            string inferredArtist = artistParts[1];
                            bool changed = false;

                            if (string.IsNullOrEmpty(contrArtist) || contrArtist.Contains("ntitled") || contrArtist.Contains("various") || contrArtist.Contains("Various") || contrArtist.Contains("nknown") || contrArtist.Contains("http") || contrArtist.Contains("www.") || contrArtist.Contains(".com"))
                            {
                                tag.Performers = [inferredArtist];
                                changed = true;
                            }

                            if (string.IsNullOrEmpty(albumArtist) || albumArtist.Contains("ntitled") || albumArtist.Contains("various") || albumArtist.Contains("Various") || albumArtist.Contains("nknown") || contrArtist.Contains("nknown") || contrArtist.Contains("http") || contrArtist.Contains("www.") || contrArtist.Contains(".com"))
                            {
                                tag.AlbumArtists = [inferredArtist];
                                changed = true;
                            }

                            if (changed)
                            {
                                tagFile.Save();
                                counter++;
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                        progressBar1.Value = 100;
                }
                MessageBox.Show($"{counter} files changed successfully!", "Success");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Files not found exception!", ex.Message);
            }
        }

        private void ChangeTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                var counter = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    if (Validators.NameCheck(file.Name) == "")
                    {
                        using TagLib.File tagFile = TagLib.File.Create(file.FullName);
                        TagLib.Tag tag = tagFile.Tag;
                        string title = tag.Title ?? string.Empty;

                        if (title == "" || title.Contains("ntitled") || title.Contains("nknown") || title.Contains("http") || title.Contains("www.") || title.Contains(".com") || title.Contains("Track"))
                        {
                            string[] newName = Regex.Split(file.Name, " - ");

                            if (newName.Length >= 2 && newName[1] != null)
                            {
                                string[] newest = Regex.Split(newName[1], "\\.");
                                tag.Title = newest[0];
                                tagFile.Save();
                                counter++;
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                        progressBar1.Value = 100;
                }
                MessageBox.Show($"{counter} files changed successfully!", "Success");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Files not found exception!", ex.Message);
            }
        }

        private void GetHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableButtons();
            DataTable table = new DataTable();
            table.Columns.Add("No");
            table.Columns.Add("Header Name");

            string[] headers = ["Album Artist", "Artist", "Title", "Album", "Year", "Genre", "Lyrics", "Duration", "Bitrate"];
            for (int i = 0; i < headers.Length; i++)
            {
                DataRow row = table.NewRow();
                row["No"] = i + 1;
                row["Header Name"] = headers[i];
                table.Rows.Add(row);
            }
            dataGridView1.DataSource = table;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
                backgroundWorker1.CancelAsync();
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex - 1, tableFin);
            CurrentPageIndex--;
            nextButton.Enabled = true;
            lastButton.Enabled = true;
            pageTextBox.Text = CurrentPageIndex.ToString() + "/" + CalculateTotalPages().ToString();
            if (CurrentPageIndex == 1)
            {
                previousButton.Enabled = false;
                beginningButton.Enabled = false;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex + 1, tableFin);
            CurrentPageIndex++;
            previousButton.Enabled = true;
            beginningButton.Enabled = true;
            pageTextBox.Text = CurrentPageIndex.ToString() + "/" + CalculateTotalPages().ToString();
            if (CurrentPageIndex == CalculateTotalPages())
            {
                nextButton.Enabled = false;
                lastButton.Enabled = false;
            }
        }

        private void BeginningButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetCurrentRecords(1, tableFin);
            CurrentPageIndex = 1;
            previousButton.Enabled = false;
            beginningButton.Enabled = false;
            nextButton.Enabled = true;
            lastButton.Enabled = true;
            pageTextBox.Text = CurrentPageIndex.ToString() + "/" + CalculateTotalPages().ToString();

        }

        private void LastButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetCurrentRecords(CalculateTotalPages(), tableFin);
            CurrentPageIndex = CalculateTotalPages();
            previousButton.Enabled = true;
            beginningButton.Enabled = true;
            nextButton.Enabled = false;
            lastButton.Enabled = false;
            pageTextBox.Text = CurrentPageIndex.ToString() + "/" + CalculateTotalPages().ToString();
        }

        private void ChangePgSizeButton_Click(object sender, EventArgs e)
        {
            if (itemsNumTextBox.Text != "")
            {
                PgSize = Convert.ToInt16(itemsNumTextBox.Text);
                dataGridView1.DataSource = GetCurrentRecords(1, tableFin);
                CurrentPageIndex = 1;
                previousButton.Enabled = false;
                beginningButton.Enabled = false;
                if (CalculateTotalPages() > 1)
                {
                    nextButton.Enabled = true;
                    lastButton.Enabled = true;
                }
                pageTextBox.Text = CurrentPageIndex.ToString() + "/" + CalculateTotalPages().ToString();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

            DialogResult dialogResult = MessageBox.Show("Files found: " + files.Length.ToString() + Environment.NewLine + "This Process can be slow once started. Do you want to continue?", "Countinue?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                tableFin = null;
                DisableButtons();
                if (files.Length != 0)
                {
                    if (backgroundWorker1.IsBusy != true)
                    {
                        CancelButton2.Enabled = true;
                        backgroundWorker1.RunWorkerAsync();
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private void RefreshExtended_Click(object sender, EventArgs e)
        {
            tableFin = null;
            DisableButtons();
            string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

            MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
            if (files.Length != 0)
            {
                if (GetAllFiles() == true)
                    SuccessfulDataGridLoad();
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SingleEdit editForm = new SingleEdit(dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 2].Value.ToString())
            {
                Text = dataGridView1.SelectedCells[1].Value.ToString()
            };
            try
            {
                editForm.ShowDialog();
            }
            catch
            {
                //MessageBox.Show("Something happened", "Message");
            }
        }

        private void StatsButton_Click(object sender, EventArgs e)
        {
            Stats statsForm = new();
            statsForm.ShowDialog();
        }

        private async void EnrichmentUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                errorList = "No data for the following: ";

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    string artist;
                    string title;
                    string year;
                    string album;
                    string genre;
                    using (TagLib.File tagFile = TagLib.File.Create(file.FullName))
                    {
                        TagLib.Tag tag = tagFile.Tag;
                        artist = tag.AlbumArtists?.FirstOrDefault() ?? tag.FirstAlbumArtist ?? string.Empty;
                        title = tag.Title ?? string.Empty;
                        year = tag.Year > 0 ? tag.Year.ToString() : string.Empty;
                        album = tag.Album ?? string.Empty;
                        genre = tag.FirstGenre ?? string.Empty;
                    }

                    if (artist != "" && title != "")
                    {
                        if (year == "" || year == "0" || album == "" || genre == "")
                        {
                            var newData = await DataEnrichment.EnrichDataAsync(artist, title);
                            if (newData.Item1 != null)
                            {
                                bool changed = false;
                                using TagLib.File tagFile = TagLib.File.Create(file.FullName);
                                TagLib.Tag tag = tagFile.Tag;
                                if ((year == "" || year == "0") && Validators.IsValidYear(newData.Item1.Year) && uint.TryParse(newData.Item1.Year, out uint enrichedYear))
                                {
                                    tag.Year = enrichedYear;
                                    changed = true;
                                }
                                if (album == "" && !string.IsNullOrEmpty(newData.Item1.AlbumTitle))
                                {
                                    tag.Album = newData.Item1.AlbumTitle;
                                    changed = true;
                                }
                                if (genre == "" && !string.IsNullOrEmpty(newData.Item1.Genre))
                                {
                                    tag.Genres = [newData.Item1.Genre];
                                    changed = true;
                                }

                                if (changed)
                                    tagFile.Save();
                            }
                            else if (newData.Item2 == true)
                                break;
                            else
                                errorList += Environment.NewLine + file.Name;
                        }
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                        progressBar1.Value = 100;
                }
                ErrorList errorForm = new ErrorList(errorList);
                errorForm.ShowDialog();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Files not found exception!", "Message");
            }
        }

        private async void LyricsUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                errorList = "No lyrics for the following: ";

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    string artist;
                    string title;
                    string lyrics;
                    using (TagLib.File tagFile = TagLib.File.Create(file.FullName))
                    {
                        TagLib.Tag tag = tagFile.Tag;
                        artist = tag.AlbumArtists?.FirstOrDefault() ?? tag.FirstAlbumArtist ?? string.Empty;
                        title = tag.Title ?? string.Empty;
                        lyrics = tag.Lyrics ?? string.Empty;
                    }

                    if (artist != "" && title != "" && (lyrics == "" || lyrics.Contains(".com") || lyrics.Contains("www.") || lyrics.Contains("http")))
                    {
                        var lyricsValues = await Lyrics.GetLyricsAsync(artist, title);

                        if (lyricsValues.Item1 != null && lyricsValues.Item1 != "")
                        {
                            using TagLib.File tagFile = TagLib.File.Create(file.FullName);
                            tagFile.Tag.Lyrics = lyricsValues.Item1;
                            tagFile.Save();
                        }
                        else if (lyricsValues.Item2 == true)
                            break;
                        else
                            errorList += Environment.NewLine + file.Name;
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                        progressBar1.Value = 100;
                }
                ErrorList errorForm = new ErrorList(errorList);
                errorForm.ShowDialog();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Files not found exception!", ex.Message);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = SeachFileName(searchTextBox.Text, tableFin);
        }

        private void OpenExtendedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = path;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                tableFin = null;
                DisableButtons();
                string[] files = Directory.GetFiles(fbd.SelectedPath, "*.mp3", SearchOption.AllDirectories);
                MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

                if (files.Length != 0)
                {
                    path = fbd.SelectedPath;
                    if (GetAllFiles() == true)
                        SuccessfulDataGridLoad();
                }
            }
        }

        #endregion

        private void ItemsNumTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
                e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) && searchButton.Enabled == true)
                dataGridView1.DataSource = SeachFileName(searchTextBox.Text, tableFin);
        }

        private bool GetAllFiles()
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                var table = BulkSongsReader.ReadSongsAsync(files, 10, new Progress<int>(percent => progressBar1.Value = percent)).Result;
                tableFin = BulkSongsReader.ToDataTable(table);
                return true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found exception!", "Error");
                path = "";
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
                path = "";
                return false;
            }
        }

        private async void PicturesUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                errorList = "No pictures for the following: ";

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new(files[i]);
                    string artist;
                    string title;
                    TagLib.IPicture picture;
                    using (TagLib.File tagFile = TagLib.File.Create(file.FullName))
                    {
                        TagLib.Tag tag = tagFile.Tag;
                        artist = tag.AlbumArtists?.FirstOrDefault() ?? tag.FirstAlbumArtist ?? string.Empty;
                        title = tag.Title ?? string.Empty;
                        picture = tag.Pictures?.FirstOrDefault() ?? null;
                    }

                    if (!Validators.IsValidPicture(picture))
                    {
                        var pictureValues = await Pictures.GetPicture(artist, title);

                        if (pictureValues.Item1 != null)
                        {
                            using TagLib.File tagFile = TagLib.File.Create(file.FullName);
                            tagFile.Tag.Pictures = [Pictures.ToTagPicture(pictureValues.Item1)];
                            tagFile.Save();
                        }
                        else if (pictureValues.Item2 == true)
                            break;
                        else
                            errorList += Environment.NewLine + file.Name;
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                        progressBar1.Value = 100;
                }
                ErrorList errorForm = new ErrorList(errorList);
                errorForm.ShowDialog();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Files not found exception!", ex.Message);
            }
        }

        #region Unused
        private void SomethingElseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        #endregion Unused        
    }
}