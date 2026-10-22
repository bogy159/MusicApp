public class DarkForm : Form
{
    public DarkForm()
    {
        // Automatically apply dark theme when any form inheriting from this is loaded
        Load += (s, e) => ThemeManager.ApplyDarkTheme(this);
    }
}