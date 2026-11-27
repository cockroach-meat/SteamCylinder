namespace UI;

partial class Logon
{
    private System.ComponentModel.IContainer components = null;

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.App = Gtk.Application.New(
            "pl.ryzh.Cylider",
            Gio.ApplicationFlags.FlagsNone
        );
        //
        // window
        //
        this.Window = Gtk.ApplicationWindow.New(App);
        this.Window.Title = "Cylider";
        this.Window.SetDefaultSize(800, 300);
    }
    private Gtk.Application App;
    private Gtk.ApplicationWindow Window;
}
