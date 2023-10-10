using SadConsole.Ansi;
using SadConsole.UI;
using SadConsole.UI.Controls;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

public class StartConfigScreen : ScreenObject
{
    private ScreenSurface _startConfig;
    private Console _startConfigMenu;

    public StartConfigScreen()
    {
        // create a screen for config background
        _startConfig = new ScreenSurface(GAME_WIDTH, GAME_HEIGHT);
        // add config background
        Children.Add(_startConfig);
        
        // create a config menu
        _startConfigMenu = new StartConfigMenu() { Position = new Point(HorCentered(_startConfig, STARTCONFIGMENU_WIDTH), 2) };
        // add config menu
        Children.Add(_startConfigMenu);
        
        // create menu border
        var borderParams = Border.BorderParameters.GetDefault()
            .AddTitle("Game Configuration", Color.DarkCyan, Color.Transparent)
            .ChangeBorderColors(Color.White, Color.Black)
            .AddShadow();

        // add menu border
        Border border = new(_startConfigMenu, borderParams);
    }
    public class StartConfigMenu : SadConsole.UI.ControlsConsole
    {
        public StartConfigMenu() : base(STARTCONFIGMENU_WIDTH, STARTCONFIGMENU_HEIGHT)
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
            exitButton.Click += (s, a) => CaveGame.Program.Exit();
            Controls.Add(startButton);
            Controls.Add(exitButton);
        }
    }
}