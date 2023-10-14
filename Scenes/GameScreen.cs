using SadConsole.UI.Controls;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

public class GameScreen : ScreenObject
{
    private Console _gameView;

    public GameScreen()
    {
        _gameView = new Console(GAMEVIEW_WIDTH, GAMEVIEW_HEIGHT);
        
        writer.ReadEntireDocument();
        Children.Add(_title);
        
        Children.Add(new StartMenu() { Position = new Point(0, START_HEIGHT - STARTMENU_HEIGHT)});
    }
    
    public class GameView : Console
    {
        public GameView() : base(GAMEVIEW_WIDTH, GAMEVIEW_HEIGHT)
        {
            
        }
    }
    public class StartMenu : SadConsole.UI.ControlsConsole
    {
        public StartMenu() : base(STARTMENU_WIDTH, STARTMENU_HEIGHT)
        {
            var startButton = new Button(10)
            {
                Text = "Play",
                Position = new Point(HorCentered(this, 10), 3)
            };
            startButton.Click += (_, _) => Program.Start();
            var exitButton = new Button(10)
            {
                Text = "Exit",
                Position = new Point(HorCentered(this, 10), 6)
            };
            exitButton.Click += (_, _) => Program.Exit();
            
            Controls.Add(startButton);
            Controls.Add(exitButton);
        }
    }
}