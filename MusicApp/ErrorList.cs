using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp
{
    public partial class ErrorList : Form
    {
        public ErrorList(string errorList)
        {
            InitializeComponent();
            richTextBox1.Text = errorList;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
