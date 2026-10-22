public partial class DarkMessageBox : DarkForm
{
    private Label lblMessage;

    public DarkMessageBox(string message, string title, MessageBoxButtons buttons)
    {
        InitializeComponent(message, title, buttons);
    }

    private void InitializeComponent(string message, string title, MessageBoxButtons buttons)
    {
        Text = title;
        Width = 450; // Made slightly wider for longer button text like "Abort" / "Retry" / "Ignore"
        Height = 200;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        // Message Label
        lblMessage = new Label()
        {
            Text = message,
            Left = 20,
            Top = 20,
            Width = 390,
            Height = 85,
            Font = new Font("Segoe UI", 10F)
        };
        Controls.Add(lblMessage);

        // Position buttons dynamically based on what was requested
        SetupButtons(buttons);
    }

    private void SetupButtons(MessageBoxButtons buttons)
    {
        int yPos = 120;
        int btnWidth = 85;
        int btnHeight = 30;
        int spacing = 10;

        switch (buttons)
        {
            case MessageBoxButtons.OK:
                var btnOK = CreateButton("OK", DialogResult.OK, 325, yPos, btnWidth, btnHeight);
                Controls.Add(btnOK);
                AcceptButton = btnOK;
                break;

            case MessageBoxButtons.OKCancel:
                var btnCancel = CreateButton("Cancel", DialogResult.Cancel, 325, yPos, btnWidth, btnHeight);
                var btnOK2 = CreateButton("OK", DialogResult.OK, 325 - btnWidth - spacing, yPos, btnWidth, btnHeight);
                Controls.Add(btnOK2);
                Controls.Add(btnCancel);
                AcceptButton = btnOK2;
                CancelButton = btnCancel;
                break;

            case MessageBoxButtons.YesNo:
                var btnNo = CreateButton("No", DialogResult.No, 325, yPos, btnWidth, btnHeight);
                var btnYes = CreateButton("Yes", DialogResult.Yes, 325 - btnWidth - spacing, yPos, btnWidth, btnHeight);
                Controls.Add(btnYes);
                Controls.Add(btnNo);
                AcceptButton = btnYes;
                break;

            case MessageBoxButtons.YesNoCancel:
                // Arranges Yes (left), No (middle), and Cancel (right)
                var btnCancel2 = CreateButton("Cancel", DialogResult.Cancel, 325, yPos, btnWidth, btnHeight);
                var btnNo2 = CreateButton("No", DialogResult.No, 325 - btnWidth - spacing, yPos, btnWidth, btnHeight);
                var btnYes22 = CreateButton("Yes", DialogResult.Yes, 325 - (btnWidth * 2) - (spacing * 2), yPos, btnWidth, btnHeight);

                Controls.Add(btnYes22);
                Controls.Add(btnNo2);
                Controls.Add(btnCancel2);
                AcceptButton = btnYes22;
                CancelButton = btnCancel2;
                break;

            case MessageBoxButtons.AbortRetryIgnore:
                // Three buttons need a bit more width/space layout adjustment
                var btnIgnore = CreateButton("Ignore", DialogResult.Ignore, 325, yPos, btnWidth, btnHeight);
                var btnRetry = CreateButton("Retry", DialogResult.Retry, 325 - btnWidth - spacing, yPos, btnWidth, btnHeight);
                var btnAbort = CreateButton("Abort", DialogResult.Abort, 325 - (btnWidth * 2) - (spacing * 2), yPos, btnWidth, btnHeight);

                Controls.Add(btnAbort);
                Controls.Add(btnRetry);
                Controls.Add(btnIgnore);
                AcceptButton = btnAbort;
                break;
        }
    }

    private Button CreateButton(string text, DialogResult result, int left, int top, int width, int height)
    {
        return new Button()
        {
            Text = text,
            DialogResult = result,
            Left = left,
            Top = top,
            Width = width,
            Height = height
        };
    }

    // Static helper method to match MessageBox.Show syntax
    public static DialogResult Show(string message, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
    {
        using var box = new DarkMessageBox(message, title, buttons);
        return box.ShowDialog();
    }
}