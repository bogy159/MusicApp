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
            Sort(5);
        }
        
        private DataTable GetArtists(int item)
        {
            List<string> artists = Index.tableFin.AsEnumerable().Select(r => r.Field<string>(item)).ToList();
            artists.Sort();
            DataTable stats = new DataTable();
            stats.Columns.Add("No", typeof(int));
            stats.Columns.Add("Name");
            stats.Columns.Add("Count", typeof(int));

            int j = 1;
            for (int i = 1; i < artists.Count; i++)
            {
                if (artists[i] != artists[i - 1])
                {
                    DataRow row = stats.NewRow();
                    row["Name"] = artists[i - 1];
                    row["Count"] = j;
                    j = 1;
                    stats.Rows.Add(row);
                }
                else if (artists[i] == artists[i - 1])
                {
                    j++;                    
                }

                if (i == artists.Count - 1)
                {
                    DataRow row = stats.NewRow();
                    row["Name"] = artists[i];
                    row["Count"] = j;
                    stats.Rows.Add(row);
                }
                progressBar2.Value = Convert.ToInt32(100 * (i + 1) / artists.Count);

                if (i == artists.Count - 1)
                {
                    progressBar2.Value = 100;
                }
            }
            return stats;
        }

        private void Sort(int item)
        {
            DataView dv = GetArtists(item).DefaultView;
            dv.Sort = "Count DESC";
            DataTable sortedDT = dv.ToTable();
            sortedDT.Rows[0].SetField(0, 1);
            for (int i = 1; i < sortedDT.Rows.Count; i++)
            {
                if (sortedDT.Rows[i][2].ToString() == sortedDT.Rows[i - 1][2].ToString())
                {
                    sortedDT.Rows[i].SetField(0, sortedDT.Rows[i - 1][0]);
                }
                else
                {
                    sortedDT.Rows[i].SetField(0, i + 1);
                }
                progressBar2.Value = Convert.ToInt32(100 * (i + 1) / sortedDT.Rows.Count);

                if (i == sortedDT.Rows.Count - 1)
                {
                    progressBar2.Value = 100;
                }
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
            if (comboBox1.SelectedItem.ToString() == "Album Artists")
            {
                Sort(5);
            }
            else if (comboBox1.SelectedItem.ToString() == "Contribution Artists")
            {
                Sort(2);
            }
            else if (comboBox1.SelectedItem.ToString() == "Albums")
            {
                Sort(7);
            }
            else if (comboBox1.SelectedItem.ToString() == "Genres")
            {
                Sort(3);
            }
            else if (comboBox1.SelectedItem.ToString() == "File Names")
            {
                Sort(1);
            }
        }
    }
}
