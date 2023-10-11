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
    public class StartConfigMenu : ControlsConsole
    {
        public StartConfigMenu() : base(STARTCONFIGMENU_WIDTH, STARTCONFIGMENU_HEIGHT)
        {
            PrintHorCentered(this, 3, "World Size:");
            PrintHorCentered(this, 4, "(200 recommended)");
            var worldSize = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 5)
            };
            PrintHorCentered(this, 8, "Minimum World Area:");
            PrintHorCentered(this, 9, "(20000 recommended)");
            var minWorldArea = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 10)
            };
            PrintHorCentered(this, 13, "Number of Enemies:");
            var enemyCount = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 14)
            };
            var confirmButton = new Button(20, 1)
            {
                Text = "Generate World",
                Position = new Point(HorCentered(this, 20), 18)
            };
            confirmButton.Click += (s, a) =>
            {
                if (int.TryParse(worldSize.Text, out var i) && int.TryParse(minWorldArea.Text, out var j) && int.TryParse(enemyCount.Text, out var k))
                {
                    Program.GenerateWorld(i, j, k);
                }
                else
                {
                    PrintHorCentered(this, 16, "Inputs must be integers!", Color.Red);
                }
            };
            
            Controls.Add(worldSize);
            Controls.Add(minWorldArea);
            Controls.Add(enemyCount);
            Controls.Add(confirmButton);
        }
    }
}