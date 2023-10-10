using SadConsole.Ansi;
using SadConsole.UI.Controls;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

class StartScreen : ScreenObject
{
    private ScreenSurface _title;

    public StartScreen()
    {
        // create a screen for title art
        _title = new ScreenSurface(GAME_WIDTH, GAME_HEIGHT);
        
        var doc = new Document($"Art/god.ans");
        var writer = new AnsiWriter(doc, _title.Surface);
        writer.ReadEntireDocument();
        Children.Add(_title);
        
        //Children.Add(new StartMenu() { Position = new Point(0, GAME_HEIGHT - STARTMENU_HEIGHT)});
    }
    internal class StartMenu : SadConsole.UI.ControlsConsole
    {
        public StartMenu() : base(STARTMENU_WIDTH, STARTMENU_HEIGHT)
        {
            var startButton = new Button(10, 1)
            {
                Text = "Play",
                Position = new Point(HorCentered(this, 10), 3)
            };
            var exitButton = new Button(10, 1)
            {
                Text = "Exit",
                Position = new Point(HorCentered(this, 10), 6)
            };
            Controls.Add(startButton);
            Controls.Add(exitButton);
        }
    }
}
