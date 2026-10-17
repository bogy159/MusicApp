using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MusicApp
{
    public partial class Stats : Form
    {
        public Stats()
        {
            InitializeComponent();
            comboBox1.SelectedItem = "Album Artists";
            Sorting("Album Artist");
        }
        
        private DataTable GetArtists(string column)
        {
            List<string> collection = Filtering(column);
            collection.Sort();
            DataTable stats = new DataTable();
            stats.Columns.Add("No", typeof(int));
            stats.Columns.Add("Value");
            stats.Columns.Add("Count", typeof(int));

            int j = 1;
            for (int i = 1; i < collection.Count; i++)
            {
                if (collection[i] != collection[i - 1])
                {
                    DataRow row = stats.NewRow();
                    row["Value"] = collection[i - 1];
                    row["Count"] = j;
                    j = 1;
                    stats.Rows.Add(row);
                }
                else if (collection[i] == collection[i - 1])
                {
                    j++;                    
                }

                if (i == collection.Count - 1)
                {
                    DataRow row = stats.NewRow();
                    row["Value"] = collection[i];
                    row["Count"] = j;
                    stats.Rows.Add(row);
                }
                progressBar2.Value = Convert.ToInt32(100 * (i + 1) / collection.Count);

                if (i == collection.Count - 1)
                {
                    progressBar2.Value = 100;
                }
            }
            return stats;
        }

        private List<string> Filtering(string column)
        {
            List<string> collection = Index.tableFin.AsEnumerable().Select(r => r.Field<string>(column)).ToList();

            if (column == "Contribution Artist")
            {
                List<string> newCollection = new List<string>(); ;
                foreach (string element in collection)
                {
                    if (element.Contains("&"))
                    {
                        string[] words = element.Split(',', '&');
                        foreach (string word in words)
                        {
                            newCollection.Add(word.Trim());
                        }
                    }
                    else
                        newCollection.Add(element);
                }
                return newCollection;
            }
            else if (column == "Length")
            {
                List<string> newCollection = new List<string>(); ;
                foreach (string element in collection)
                {
                    TimeSpan time = TimeSpan.Parse(element);
                    newCollection.Add(Math.Round(time.TotalHours).ToString());
                }
                return newCollection;
            }
            else if (column == "Year" && comboBox1.SelectedItem.ToString() == "Decades")
            {
                List<string> newCollection = new List<string>(); ;
                foreach (string element in collection)
                {
                    if (element.Length == 4)
                        newCollection.Add("The " + element[0].ToString() + element[1].ToString() + element[2].ToString() + "0's");
                    else
                        newCollection.Add(element);
                }
                return newCollection;
            }
            else if (column == "File Name")
            {
                List<string> newCollection = new List<string>(); ;
                foreach (string element in collection)
                {
                    newCollection.Add(element[0].ToString() + element[1].ToString());
                }
                collection = newCollection;
            }
            else if (column == "File Path")
            {
                List<string> newCollection = new List<string>(); ;
                foreach (string element in collection)
                {
                    string[] words = element.Split('\\');                    
                    newCollection.Add(words[words.Length - 2]);
                }
                return newCollection;
            }
            return collection;
        }

        private void Sorting(string column)
        {
            DataView dv = GetArtists(column).DefaultView;
            dv.Sort = "Count DESC";
            DataTable sortedDT = dv.ToTable();
            sortedDT.Rows[0].SetField(0, 1);
            for (int i = 1; i < sortedDT.Rows.Count; i++)
            {
                if (sortedDT.Rows[i][2].ToString() == sortedDT.Rows[i - 1][2].ToString())
                    sortedDT.Rows[i].SetField(0, sortedDT.Rows[i - 1][0]);
                else
                    sortedDT.Rows[i].SetField(0, i + 1);
                progressBar2.Value = Convert.ToInt32(100 * (i + 1) / sortedDT.Rows.Count);

                if (i == sortedDT.Rows.Count - 1)
                    progressBar2.Value = 100;
            }
            statsDataGridView.DataSource = sortedDT;
            textBox1.Text = sortedDT.Rows.Count.ToString();
        }
        
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selected = (comboBox1.SelectedItem as string)?.Trim();
                var sortKey = selected switch
                {
                    "Album Artists" => "Album Artist",
                    "Contribution Artists" => "Contribution Artist",
                    "Albums" => "Album",
                    "Genres" => "Genre",
                    "Years" => "Year",
                    "Decades" => "Year",
                    "Have Lyrics" => "Has Lyrics",
                    "Average Durations" => "Length",
                    "ID Numbers" => "File Name",
                    "Locations" => "File Path",
                    _ => null
                };

                if (sortKey != null)
                    Sorting(sortKey);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString(), "Error!");
            }
        }
    }
}