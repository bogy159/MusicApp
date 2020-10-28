using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MusicApp
{

    public partial class Index : Form
    {      

        public string path;
        public static DataTable tableFin;
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

        public static string GetValue(IShellProperty value)
        {
            if (value == null || value.ValueAsObject == null)
            {
                return String.Empty;
            }
            return value.ValueAsObject.ToString();
        }

        private int CalculateTotalPages()
        {
            int rowCount = tableFin.Rows.Count;
            int TotalPage = rowCount / PgSize;
            // if any row left after calculated pages, add one more page 
            if (rowCount % PgSize > 0)
            {
                TotalPage += 1;
            }
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

        public static string NameCheck(string filename)
        {

            for (int i = 0; i < filename.Length; i++)
            {
                if (filename[i] > sbyte.MaxValue)
                {
                    return "Not ASCII";
                }
                else if (filename[i] == 32)
                {
                    if (filename[i + 1] == 32)
                    {
                        return "Double Space";
                    }
                    else if (filename[i + 1] != 38 && filename[i + 1] != 45 && ((filename[i + 1] < 65 || filename[i + 1] > 90) && (filename[i + 1] < 48 || filename[i + 1] > 57)))
                    {
                        return "After Space";
                    }
                }
                else if(filename[i] == 40)
                {
                    if (filename[i + 1] < 65 || filename[i + 1] > 90)
                    {
                        return "After (";
                    }
                    //else if (filename[filename.Length - 5] != 41)
                    //{
                    //    return "No )";
                    //}
                }
            }         
            if (!filename.EndsWith(".mp3"))
            {
                return "Not .mp3";
            }            
            else if (filename.Split().Count(r => r == "-") == 0)
            {
                return "No -";
            }
            else if (!filename.Contains('(') && filename.Contains(')'))
            {
                return "No (";
            }
            else if (filename.Contains('(') && !filename.Contains(')'))
            {
                return "No )";
            }
            else if (filename.Contains(".."))
            {
                return "Contains ..";
            }
            else if (filename.Count(x => x == '.') <= 1)
            {
                return "Just one .";
            }
            else if (filename[2] != 46 && filename[3] != 46)
            {
                return "First . missing";
            }
            else if (filename[0] < 48 || filename[0] > 57 || filename[1] < 48 || filename[1] > 57)
            {
                return "No numbers";
            }
            else if (filename[3] == 46 && (filename[2] < 48 || filename[2] > 57))
            {
                return "Wrong numbers";
            }
            else if (filename[2] == 46 && ((filename[3] < 65 || filename[3] > 90) && (filename[3] < 48 || filename[3] > 57)))
            {
                return "Capital letter";
            }
            else if (filename[3] == 46 && ((filename[4] < 65 || filename[4] > 90) && (filename[4] < 48 || filename[4] > 57)))
            {
                return "Capital letter 2";
            }
            else if (filename.Split().Count(r => r == "-") > 1)
            {
                if (filename.Contains(" - Cover"))
                {
                }
                else
                {
                    return "Multiple -";
                }
            }
            return "";
        }
             
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
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
                table.Columns.Add("File Path");
                table.Columns.Add("Has Lyrics");

                for (int i = 0; i < files.Length; i++)
                {

                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break; 
                    }

                    else
                    {
                        FileInfo file = new FileInfo(files[i]);                        
                        var strFileName = file.DirectoryName + "\\" + file.Name;
                                       
                        DataRow row = table.NewRow();
                        row["No"] = i + 1;

                        ShellObject song = ShellObject.FromParsingName(strFileName);
                        var albumArtist = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));
                        row["Album Artist"] = albumArtist;
                        var filename = GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));
                        row["File Name"] = filename;
                        var title = GetValue(song.Properties.GetProperty(SystemProperties.System.Title));
                        row["Title"] = title;
                        row["Album"] = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumTitle));
                        row["Year"] = GetValue(song.Properties.GetProperty(SystemProperties.System.Media.Year));
                        var length = GetValue(song.Properties.GetProperty(SystemProperties.System.Media.Duration));
                        TimeSpan span2 = TimeSpan.FromTicks(Convert.ToInt64(length));
                        row["Length"] = span2.ToString(@"mm\:ss");// + "." + span2.Milliseconds;
                        row["Bit Rate"] = GetValue(song.Properties.GetProperty(SystemProperties.System.Audio.EncodingBitrate));
                        var size = GetValue(song.Properties.GetProperty(SystemProperties.System.Size));
                        row["Size in MB"] = Convert.ToDouble(size) / (1024 * 1024);
                        string falseName = NameCheck(GetValue(song.Properties.GetProperty(SystemProperties.System.FileName)));
                        row["Fix Name"] = falseName;
                        row["File Path"] = strFileName;
                        if (GetValue(song.Properties.GetProperty(SystemProperties.System.Music.Lyrics)) != "")
                        {
                            row["Has Lyrics"] = "Yes";
                        }
                        else
                        {
                            row["Has Lyrics"] = "No";
                        }

                        if (albumArtist == "" || albumArtist.Contains("ntitled") || albumArtist.Contains("various") || albumArtist.Contains("Various") || albumArtist.Contains("nknown"))
                        {
                            string name = GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));                            
                            string[] newName = Regex.Split(name, "-");
                            string[] newest = Regex.Split(newName[0], "\\.");

                            if (falseName != "")
                            {
                                row["Needed Changes in Album Artist"] = "Improper file name";
                            }
                            else if (newest.Length >= 2 && newest[1] != null)
                            {
                                row["Needed Changes in Album Artist"] = newest[1];
                            }                                                       
                        }

                        if (title == "" || title.Contains("ntitled") || title.Contains("nknown") || title.Contains("Track"))
                        {
                            string name = GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));
                            string[] newName = Regex.Split(name, "-");

                            if (falseName != "")
                            {
                                row["Needed Changes in Title"] = "Improper file name";
                            }
                            else if (newName.Length >= 2 && newName[1] != null)
                            {
                                string[] newest = Regex.Split(newName[1], "\\.");
                                row["Needed Changes in Title"] = newest[0];
                            }  
                        }

                        table.Rows.Add(row);
                        worker.ReportProgress(i);
                        tableFin = table;
                    }
                }
            }

            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("File not found exception!", "Error");
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
            if ((e.Cancelled == true))
            {
                System.Windows.Forms.MessageBox.Show("Cancelled!", "Cancelled");
                if (tableFin != null)
                {
                    dataGridView1.DataSource = GetCurrentRecords(1, tableFin);
                }
                CancelButton2.Enabled = false;
                changePgSizeButton.Enabled = false;
                searchButton.Enabled = false;
                changeArtistsInfoToolStripMenuItem.Enabled = false;
                changeTitleToolStripMenuItem.Enabled = false;
                updateMusixmatchToolStripMenuItem.Enabled = false;
                lyricsUpdateToolStripMenuItem.Enabled = false;
            }

            else if (!(e.Error == null))
            {
                System.Windows.Forms.MessageBox.Show("Error!", "Error");
                DisableButtons();
        }

            else
            {
                SuccessfulDataGridLoad();
            }
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
                updateMusixmatchToolStripMenuItem.Enabled = true;
                lyricsUpdateToolStripMenuItem.Enabled = true;
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
            updateMusixmatchToolStripMenuItem.Enabled = false;
            lyricsUpdateToolStripMenuItem.Enabled = false;
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
            System.Windows.Forms.Application.Exit();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = path;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tableFin = null;
                    DisableButtons();
                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*.mp3", SearchOption.AllDirectories);
                    System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
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
            }
        }

        private void ChangeArtistsInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    if (NameCheck(file.Name) == "")
                    {
                        var fileshell = ShellFile.FromFilePath(file.DirectoryName + "\\" + file.Name);

                        Shell32.Shell shell = new Shell32.Shell();
                        var strFileName = file.DirectoryName + "\\" + file.Name;
                        Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(strFileName));
                        Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(strFileName));
                        var contrArtist = objFolder.GetDetailsOf(folderItem, 13);

                        if (contrArtist == "" || contrArtist.Contains("ntitled") || contrArtist.Contains("various") || contrArtist.Contains("Various") || contrArtist.Contains("nknown") || contrArtist.Contains("http") || contrArtist.Contains("www.") || contrArtist.Contains(".com"))
                        {
                            string name = objFolder.GetDetailsOf(folderItem, 0);
                            string[] newName = Regex.Split(name, "-");
                            string[] newest = Regex.Split(newName[0], "\\.");

                            ShellPropertyWriter propertyWriter = fileshell.Properties.GetPropertyWriter();
                            try
                            {
                                if (newest.Length >= 2 && newest[1] != null)
                                {
                                    propertyWriter.WriteProperty(SystemProperties.System.Music.Artist, new string[] { newest[1] });
                                }
                                else if (newest.Length < 2)
                                {
                                }

                            }
                            finally
                            {
                                propertyWriter.Close();
                            }
                        }
                        ShellObject song = ShellObject.FromParsingName(strFileName);
                        var albumArtist = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));

                        if (albumArtist == "" || albumArtist.Contains("ntitled") || albumArtist.Contains("various") || albumArtist.Contains("Various") || albumArtist.Contains("nknown") || contrArtist.Contains("nknown") || contrArtist.Contains("http") || contrArtist.Contains("www.") || contrArtist.Contains(".com"))
                        {
                            string name = objFolder.GetDetailsOf(folderItem, 0);
                            string[] newName = Regex.Split(name, "-");
                            string[] newest = Regex.Split(newName[0], "\\.");

                            ShellPropertyWriter propertyWriter = fileshell.Properties.GetPropertyWriter();
                            try
                            {
                                if (newest.Length >= 2 && newest[1] != null)
                                {
                                    propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumArtist, new string[] { newest[1] });
                                }
                                else if (newest.Length < 2)
                                {
                                }

                            }
                            finally
                            {
                                propertyWriter.Close();
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                    {
                        progressBar1.Value = 100;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Files not found exception!", "Message");
            }
        }

        private void ChangeTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    if (NameCheck(file.Name) == "")
                    {
                        var fileshell = ShellFile.FromFilePath(file.DirectoryName + "\\" + file.Name);

                        Shell32.Shell shell = new Shell32.Shell();
                        var strFileName = file.DirectoryName + "\\" + file.Name;
                        Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(strFileName));
                        Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(strFileName));
                        var title = objFolder.GetDetailsOf(folderItem, 21);

                        if (title == "" || title.Contains("ntitled") || title.Contains("nknown") || title.Contains("nknown") || title.Contains("http") || title.Contains("www.") || title.Contains(".com") || title.Contains("Track"))
                        {
                            string name = objFolder.GetDetailsOf(folderItem, 0);
                            string[] newName = Regex.Split(name, "-");

                            if (newName.Length >= 2 && newName[1] != null)
                            {
                                string[] newest = Regex.Split(newName[1], "\\.");

                                ShellPropertyWriter propertyWriter = fileshell.Properties.GetPropertyWriter();
                                try
                                {
                                    propertyWriter.WriteProperty(SystemProperties.System.Title, new string[] { newest[0] });

                                }
                                finally
                                {
                                    propertyWriter.Close();
                                }
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                    {
                        progressBar1.Value = 100;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Files not found exception!", "Error!");
            }
        }

        private void GetHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableButtons();
            DataTable table = new DataTable();
            table.Columns.Add("No");
            table.Columns.Add("Header Name");

            List<string> arrHeaders = new List<string>();
            List<Tuple<int, string, string>> attributes = new List<Tuple<int, string, string>>();

            Shell32.Shell shell = new Shell32.Shell();
            var strFileName = "C:\\Music";
            Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(strFileName));
            Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(strFileName));


            for (int i = 0; i < short.MaxValue; i++)
            {
                string header = objFolder.GetDetailsOf(null, i);
                if (String.IsNullOrEmpty(header))
                    break;
                arrHeaders.Add(header);
            }

            // The attributes list below will contain a tuple with attribute index, name and value
            // Once you know the index of the attribute you want to get, 
            // you can get it directly without looping, like this:
            var Authors = objFolder.GetDetailsOf(folderItem, 20);

            for (int i = 0; i < arrHeaders.Count; i++)
            {
                var attrName = arrHeaders[i];
                var attrValue = objFolder.GetDetailsOf(folderItem, i);
                var attrIdx = i;

                attributes.Add(new Tuple<int, string, string>(attrIdx, attrName, attrValue));

                //Debug.WriteLine("{0}\t{1}: {2}", i, attrName, attrValue);

                DataRow row = table.NewRow();
                row["No"] = i + 1;
                row["Header Name"] = attrName;
                table.Rows.Add(row);

            }

            dataGridView1.DataSource = table;

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                backgroundWorker1.CancelAsync();
            }
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
            tableFin = null;
            //beginningButton.Enabled = false;
            //previousButton.Enabled = false;
            //nextButton.Enabled = false;
            //lastButton.Enabled = false;
            //refreshButton.Enabled = false;
            //refreshExtendedButton.Enabled = false;
            //statsButton.Enabled = false;
            DisableButtons();
            string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
            if (files.Length != 0)
            {
                if (backgroundWorker1.IsBusy != true)
                {
                    CancelButton2.Enabled = true;
                    backgroundWorker1.RunWorkerAsync();
                }

            }
        }

        private void RefreshExtended_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

            DialogResult dialogResult = MessageBox.Show("Files found: " + files.Length.ToString() + Environment.NewLine + "This Process can not be canceled once started. Do you want to continue?", "Countinue?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                tableFin = null;
                DisableButtons();
                if (files.Length != 0)
                {
                    if (GetAllFiles() == true)
                    {
                        SuccessfulDataGridLoad();
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {

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
            Stats statsForm = new Stats();
            statsForm.ShowDialog();

        }

        private void MusixmatchUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                errorList = "No data for the following: ";

                for (int i = 0; i < files.Length; i++)
                {

                    FileInfo file = new FileInfo(files[i]);
                    var strFileName = file.DirectoryName + "\\" + file.Name;

                    ShellObject song = ShellObject.FromParsingName(strFileName);
                    string artist = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));
                    string title = GetValue(song.Properties.GetProperty(SystemProperties.System.Title));

                    if (artist != "" && title != "")
                    {
                        var year = GetValue(song.Properties.GetProperty(SystemProperties.System.Media.Year));
                        string album = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumTitle));

                        Shell32.Shell shell = new Shell32.Shell();
                        Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(strFileName));
                        Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(strFileName));
                        var genre = objFolder.GetDetailsOf(folderItem, 16);

                        if (year.ToString() == "" || Convert.ToInt16(year) == 0 || album == "" || genre.ToString() == "")
                        {
                            var fileshell = ShellFile.FromFilePath(strFileName);
                            ShellPropertyWriter propertyWriter = fileshell.Properties.GetPropertyWriter();

                            try
                            {
                                var trackJSON = SingleEdit.MusixmatchGet(artist, title);

                                if (trackJSON.Item1 != null)
                                {
                                    try
                                    {
                                        string yearMM = trackJSON.Item1.FirstReleaseDate.Year.ToString();
                                        if ((year.ToString() == "" || Convert.ToInt16(year) == 0) && yearMM != "" && Convert.ToInt16(yearMM) <= Convert.ToInt16(DateTime.Now.Year) && Convert.ToInt16(yearMM) > 1900)
                                        {
                                            propertyWriter.WriteProperty(SystemProperties.System.Media.Year, new int[] { Convert.ToInt16(yearMM) });
                                        }
                                    }
                                    catch
                                    {
                                        errorList += Environment.NewLine + "Year error in: " + file.Name;
                                    }
                                    try
                                    {
                                        string albumMM = trackJSON.Item1.AlbumName.ToString();
                                        if (album == "" && albumMM != "")
                                        {
                                            propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, new string[] { albumMM });
                                        }
                                    }
                                    catch
                                    {
                                        errorList += Environment.NewLine + "Album error in: " + file.Name;
                                    }
                                    if (genre.ToString() == "")
                                    {
                                        try
                                        {
                                            string genreMM = trackJSON.Item1.PrimaryGenres.MusicGenre[0].MusicGenreName.ToString();
                                            if (genreMM.ToString() != "")
                                            {
                                                propertyWriter.WriteProperty(SystemProperties.System.Music.Genre, new string[] { genreMM });
                                            }
                                        }
                                        catch (IndexOutOfRangeException)
                                        {
                                            errorList += Environment.NewLine + "Genre error in: " + file.Name;
                                        }
                                    }
                                }
                                else if (trackJSON.Item2 == true)
                                {
                                    break;
                                }
                                else
                                {
                                    errorList += Environment.NewLine + file.Name;
                                }
                            }
                            finally
                            {
                                propertyWriter.Close();
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                    {
                        progressBar1.Value = 100;
                    }
                }
                ErrorList errorForm = new ErrorList(errorList);
                errorForm.ShowDialog();
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Files not found exception!", "Message");
            }
        }

        private void LyricsUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                errorList = "No lyrics for the following: ";

                for (int i = 0; i < files.Length; i++)
                {

                    FileInfo file = new FileInfo(files[i]);
                    var strFileName = file.DirectoryName + "\\" + file.Name;

                    ShellObject song = ShellObject.FromParsingName(strFileName);
                    string artist = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));
                    string title = GetValue(song.Properties.GetProperty(SystemProperties.System.Title));
                    string lyrics = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.Lyrics));

                    if (artist != "" && title != "" && (lyrics == "" || lyrics.Contains(".com") || lyrics.Contains("www.") || lyrics.Contains("http")))
                    {
                        //string lyrics = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.Lyrics));

                        var fileshell = ShellFile.FromFilePath(strFileName);
                        ShellPropertyWriter propertyWriter = fileshell.Properties.GetPropertyWriter();

                        try
                        {
                            var lyricsValues = SingleEdit.ChartLyricsGet(artist, title);

                            if (lyricsValues.Item1 != null && lyricsValues.Item1 != "")
                            {
                                propertyWriter.WriteProperty(SystemProperties.System.Music.Lyrics, new string[] { lyricsValues.Item1 });
                            }
                            else if (lyricsValues.Item2 == true)
                            {
                                break;
                            }
                            else
                            {
                                errorList += Environment.NewLine + file.Name;
                            }
                        }
                        finally
                        {
                            propertyWriter.Close();
                        }

                    }
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                    {
                        progressBar1.Value = 100;
                    }
                }
                ErrorList errorForm = new ErrorList(errorList);
                errorForm.ShowDialog();
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Files not found exception!", "Error!");
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = SeachFileName(searchTextBox.Text, tableFin);
        }

        private void OpenExtendedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = path;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    
                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*.mp3", SearchOption.AllDirectories);
                    //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

                    DialogResult dialogResult = MessageBox.Show("Files found: " + files.Length.ToString() + Environment.NewLine + "This Process can not be canceled once started. Do you want to continue?", "Countinue?", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        tableFin = null;
                        DisableButtons();
                        if (files.Length != 0)
                        {
                            path = fbd.SelectedPath;
                            if (GetAllFiles() == true)
                            {
                                SuccessfulDataGridLoad();
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }

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
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) && searchButton.Enabled == true)
            {
                dataGridView1.DataSource = SeachFileName(searchTextBox.Text, tableFin);
            }
        }

        private Song GetSingleSong(string location)
        {
            try
            {
                Song item = new Song
                {
                    Location = location
                };

                ShellObject song = ShellObject.FromParsingName(location);
                Shell32.Shell shell = new Shell32.Shell();
                Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(location));
                Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(location));
                item.FileName = objFolder.GetDetailsOf(folderItem, 0);
                item.AlbumArtist = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.AlbumArtist));
                item.ContributingArtist = objFolder.GetDetailsOf(folderItem, 13);
                item.SongTitle = objFolder.GetDetailsOf(folderItem, 21);
                item.AlbumTitle = objFolder.GetDetailsOf(folderItem, 14);
                item.Year = objFolder.GetDetailsOf(folderItem, 15);
                item.Genre = objFolder.GetDetailsOf(folderItem, 16);
                item.Length = objFolder.GetDetailsOf(folderItem, 27);
                item.Size = objFolder.GetDetailsOf(folderItem, 1);
                item.BitRate = objFolder.GetDetailsOf(folderItem, 28);
                item.Lyrics = GetValue(song.Properties.GetProperty(SystemProperties.System.Music.Lyrics));
                return item;
            }
            catch (ShellException)
            {
                MessageBox.Show("File not found exception!", "Error");
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                return null;
            }
        }

        private bool GetAllFiles()
        {
            try
            {
                string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                DataTable table = new DataTable();
                table.Columns.Add("No", typeof(int));
                table.Columns.Add("File Name", typeof(string));
                table.Columns.Add("Fix Name");
                table.Columns.Add("Needed Changes in Artists");
                table.Columns.Add("Needed Changes in Title");
                table.Columns.Add("Album Artist");
                table.Columns.Add("Contribution Artist");
                table.Columns.Add("Title");
                table.Columns.Add("Genre");
                table.Columns.Add("Album");
                table.Columns.Add("Year");
                table.Columns.Add("Length");
                table.Columns.Add("Bit Rate");
                table.Columns.Add("Size in MB", typeof(double));
                table.Columns.Add("File Path");
                table.Columns.Add("Has Lyrics");

                for (int i = 0; i < files.Length; i++)
                {
                    DataRow row = table.NewRow();
                    row["No"] = i + 1;

                    FileInfo file = new FileInfo(files[i]);
                    Song currentSong = GetSingleSong(file.DirectoryName + "\\" + file.Name);

                    row["File Name"] = currentSong.FileName;
                    row["Fix Name"] = NameCheck(currentSong.FileName);

                    if (currentSong.AlbumArtist == "" || currentSong.AlbumArtist.Contains("ntitled") || currentSong.AlbumArtist.Contains("various") || currentSong.AlbumArtist.Contains("Various") || currentSong.AlbumArtist.Contains("nknown"))
                    {
                        //string name = GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));
                        string[] newName = Regex.Split(currentSong.FileName, "-");
                        string[] newest = Regex.Split(newName[0], "\\.");

                        if (newest.Length >= 2 && newest[1] != null)
                        {
                            row["Needed Changes in Artists"] = newest[1];
                        }
                    }

                    else if (currentSong.AlbumArtist != currentSong.ContributingArtist)
                    {
                        row["Needed Changes in Artists"] = "Different Artists";
                    }

                    if (currentSong.SongTitle == "" || currentSong.SongTitle.Contains("ntitled") || currentSong.SongTitle.Contains("nknown") || currentSong.SongTitle.Contains("Track"))
                    {
                        //string name = GetValue(song.Properties.GetProperty(SystemProperties.System.FileName));
                        string[] newName = Regex.Split(currentSong.FileName, "-");

                        if (newName.Length >= 2 && newName[1] != null)
                        {
                            string[] newest = Regex.Split(newName[1], "\\.");
                            row["Needed Changes in Title"] = newest[0];
                        }
                    }

                    row["Album Artist"] = currentSong.AlbumArtist;
                    row["Contribution Artist"] = currentSong.ContributingArtist;
                    row["Title"] = currentSong.SongTitle;
                    row["Genre"] = currentSong.Genre;
                    row["Album"] = currentSong.AlbumTitle;
                    row["Year"] = currentSong.Year;
                    TimeSpan time = TimeSpan.Parse(currentSong.Length);
                    row["Length"] = time.ToString(@"mm\:ss");
                    row["Bit Rate"] = currentSong.BitRate;
                    string[] subStrings = currentSong.Size.Split(' ');
                    row["Size in MB"] = Convert.ToDouble(subStrings[0]);
                    if (currentSong.Lyrics != "")
                    {
                        row["Has Lyrics"] = "Yes";
                    }
                    else
                    {
                        row["Has Lyrics"] = "No";
                    }
                    row["File Path"] = currentSong.Location;


                    table.Rows.Add(row);
                    tableFin = table;
                    progressBar1.Value = Convert.ToInt32(100 * (i + 1) / files.Length);

                    if (i == files.Length - 1)
                    {
                        progressBar1.Value = 100;
                    }
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("File not found exception!", "Error");
                path = "";
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error!");
                path = "";
                return false;
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
