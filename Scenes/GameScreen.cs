using SadConsole.UI;
using SadConsole.UI.Controls;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

public class GameScreen : ScreenObject
{
    private Console _gameView;

    public GameScreen()
    {
        // add game display console
        _gameView = new Console(GAMEVIEW_WIDTH, GAMEVIEW_HEIGHT);
        
        Children.Add(_gameView);
        
    }
    
    public class GameView : Console
    {
        private IFont _font = Game.Instance.Fonts["mdcurses16.font"];
        public GameView() : base(GAMEVIEW_WIDTH, GAMEVIEW_HEIGHT)
        {
            Font = _font;
            Game.Instance.FocusedScreenObjects.Push(this);
        }

        public static void UpdateView(ColoredGlyph[,] view)
        {
            
        } 
    }

    public class SkillMenu : ControlsConsole
    {
        public SkillMenu() : base()
        {
            
        }
    }
}