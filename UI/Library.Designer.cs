namespace UI;

partial class Library
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
        // GamesList
        //
        this.GamesList = Gtk.ListView.New(
            Gtk.SingleSelection.New(
                Gtk.StringList.New(
                    new [] {"teto", "pear"}
                )
            ), Gtk.SignalListItemFactory.New()
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
    private Gtk.ListView GamesList;
}
