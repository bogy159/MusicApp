namespace MusicApp
{
    public partial class ErrorList : DarkForm
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