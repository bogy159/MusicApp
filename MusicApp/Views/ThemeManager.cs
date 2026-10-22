public static class ThemeManager
{
    // Define your core dark palette colors
    private static readonly Color DarkBackground = Color.FromArgb(32, 32, 32);
    private static readonly Color DarkControlBackground = Color.FromArgb(45, 45, 45);
    private static readonly Color DarkBorderColor = Color.FromArgb(64, 64, 64);
    private static readonly Color LightText = Color.White;

    public static void ApplyDarkThemeToGrid(DataGridView dgv)
    {
        // 1. General background (the empty area outside the cells)
        dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
        dgv.GridColor = Color.FromArgb(60, 60, 60);

        // 2. CRITICAL: Must be false, otherwise Windows overrides your header colors
        dgv.EnableHeadersVisualStyles = false;

        // 3. Column Headers Style (Top bar)
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

        // 4. Row Headers Style (The left-most column selector)
        dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
        dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
        dgv.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;

        // 5. Standard Data Cells Style
        dgv.DefaultCellStyle.BackColor = Color.FromArgb(32, 32, 32);
        dgv.DefaultCellStyle.ForeColor = Color.White;

        // Selection colors (when a user clicks a row/cell)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 212); // Windows Accent Blue (or use a dark gray)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White;

        // 6. Optional: Alternating rows for readability
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 38, 38);
        dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
    }

    public static void ApplyDarkTheme(Control container)
    {
        if (container is Form form)
        {
            form.BackColor = DarkBackground;
            form.ForeColor = LightText;
        }

        foreach (Control control in container.Controls)
        {
            StyleControl(control);

            // Recursively style child containers (Panels, GroupBoxes, TabPages, etc.)
            if (control.HasChildren)
            {
                ApplyDarkTheme(control);
            }
        }
    }

    private static void StyleControl(Control c)
    {
        c.ForeColor = LightText;

        if (c is Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = DarkControlBackground;
            btn.ForeColor = LightText;
            btn.FlatAppearance.BorderColor = DarkBorderColor;
        }
        else if (c is TextBox txt)
        {
            txt.BackColor = DarkControlBackground;
            txt.ForeColor = LightText;
        }
        else if (c is ComboBox cb)
        {
            cb.BackColor = DarkControlBackground;
            cb.ForeColor = LightText;
        }
        else if (c is ListBox lb)
        {
            lb.BackColor = DarkControlBackground;
            lb.ForeColor = LightText;
            lb.BorderStyle = BorderStyle.FixedSingle;
        }
        else if (c is CheckBox || c is RadioButton || c is Label)
        {
            c.ForeColor = LightText; // Transparent backgrounds work best for text labels
        }
        else if (c is Panel || c is GroupBox || c is SplitContainer || c is TabControl || c is TabPage)
        {
            c.BackColor = DarkBackground;
        }
        else if (c is MenuStrip menu)
        {
            menu.BackColor = DarkControlBackground;
            menu.ForeColor = LightText;
            foreach (ToolStripItem item in menu.Items)
            {
                StyleToolStripItem(item);
            }
        }
        else
        {
            // Generic fallback for other layout containers
            c.BackColor = DarkBackground;
        }
    }

    private static void StyleToolStripItem(ToolStripItem item)
    {
        item.BackColor = DarkControlBackground;
        item.ForeColor = LightText;
        if (item is ToolStripMenuItem menuItem)
        {
            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                StyleToolStripItem(subItem);
            }
        }
    }
}