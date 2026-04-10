namespace WinGroup;

public sealed class ShellForm : Form
{
    private readonly WindowEmbedder _windowEmbedder;
    private readonly WindowPicker _windowPicker;
    private readonly PaneManager _paneManager;

    public ShellForm()
    {
        FormBorderStyle = FormBorderStyle.Sizable;
        ShowInTaskbar = true;
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.FromArgb(24, 24, 24);

        var workingArea = Screen.PrimaryScreen?.WorkingArea;
        Bounds = workingArea ?? new Rectangle(100, 100, 1280, 720);

        _windowEmbedder = new WindowEmbedder(this);
        _windowPicker = new WindowPicker();
        _paneManager = new PaneManager(this, _windowEmbedder, _windowPicker);

        Load += OnLoad;
        Resize += OnResize;
        Activated += OnActivated;
        FormClosed += OnFormClosed;
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= Win32.WS_EX_APPWINDOW;
            cp.ExStyle &= ~Win32.WS_EX_TOOLWINDOW;
            return cp;
        }
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        ApplyMainWindowTheme();
        _paneManager.Initialize();
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (m.Msg == Win32.WM_SETTINGCHANGE || m.Msg == Win32.WM_THEMECHANGED)
        {
            ApplyMainWindowTheme();
        }
    }

    private void OnResize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            _windowEmbedder.HideAllChildren();
            return;
        }

        _windowEmbedder.ShowAllChildren();
        _paneManager.ResizeEmbeddedWindows();
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        BeginInvoke(new Action(_windowEmbedder.ActivateLastEmbeddedWindow));
    }

    private void OnFormClosed(object? sender, FormClosedEventArgs e)
    {
        _windowEmbedder.Dispose();
    }

    private void ApplyMainWindowTheme()
    {
        if (!IsHandleCreated)
        {
            return;
        }

        var useDark = ShouldUseDarkTitleBar() ? 1u : 0u;
        Win32.DwmSetWindowAttribute(Handle, Win32.DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, 4);
        Win32.DwmSetWindowAttribute(Handle, Win32.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref useDark, 4);
    }

    private static bool ShouldUseDarkTitleBar()
    {
        using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        var value = key?.GetValue("AppsUseLightTheme");
        return value is int intValue && intValue == 0;
    }
}
