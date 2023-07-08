using Gtk;

namespace NAPS2.EtoForms.Gtk;

public class GtkDarkModeProvider : IDarkModeProvider
{
    public bool IsDarkModeEnabled =>
        Settings.GetForScreen(Gdk.Screen.Default).ApplicationPreferDarkTheme;

    // Not sure if it's possible to detect live changes with Gtk
    public event EventHandler? DarkModeChanged;
}