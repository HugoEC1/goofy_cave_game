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
            PrintHorCentered(this, 1, "World Size:");
            PrintHorCentered(this, 2, "(200 recommended)");
            var worldSize = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 3)
            };
            PrintHorCentered(this, 5, "Minimum World Area:");
            PrintHorCentered(this, 6, "(20000 recommended, ignored if seed is set)");
            var minWorldArea = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 7)
            };
            PrintHorCentered(this, 9, "Seed:");
            PrintHorCentered(this, 10, "(leave empty for random)");
            var seed = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 11)
            };
            PrintHorCentered(this, 13, "Number of Enemies:");
            var enemyCount = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 14)
            };
            var confirmButton = new Button(20)
            {
                Text = "Generate World",
                Position = new Point(HorCentered(this, 20), 18)
            };
            confirmButton.Click += (_, _) =>
            {
                this.Erase(0, 16, Width);
                if (int.TryParse(worldSize.Text, out var i) && int.TryParse(minWorldArea.Text, out var j) && int.TryParse(enemyCount.Text, out var k))
                {
                    // check if world size can't be larger than min area
                    if (i * i / 2 < j)
                    {
                        PrintHorCentered(this, 16, "Max minimum area is half of size squared!", Color.Red);
                        return;
                    }

                    // checks if seed is empty
                    if (seed.Text == "")
                    {
                        Program.GenerateWorld(i, j, k, null);
                    }
                    else if (int.TryParse(seed.Text, out var l))
                    {
                        Program.GenerateWorld(i, j, k, l);
                    }
                    else
                    {
                        PrintHorCentered(this, 16, "Inputs must be integers!", Color.Red);
                    }
                }
                else
                {
                    PrintHorCentered(this, 16, "Inputs must be integers!", Color.Red);
                }
            };
            
            Controls.Add(worldSize);
            Controls.Add(minWorldArea);
            Controls.Add(seed);
            Controls.Add(enemyCount);
            Controls.Add(confirmButton);
        }
    }
}