using SadConsole.UI;
using SadConsole.UI.Controls;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

public class GameScreen : ScreenObject
{
    private ScreenSurface _gameSurface;
    private Console _gameView;
    private Console _gameLog;
    private Console _skillMenu;

    public GameScreen()
    {
        // create overall surface to hold consoles
        _gameSurface = new ScreenSurface(GAME_WIDTH, GAME_HEIGHT);
        
        // add game display console
        _gameView = new GameView();
        
        // add scrollable log console
        _gameLog = new GameLog() { Position = new Point(GAMEVIEW_WIDTH * 2, 0) };
        
        // add skill menu console
        //_skillMenu = new SkillMenu();
        
        Children.Add(_gameSurface);
        Children.Add(_gameView);
        Children.Add(_gameLog);
        //Children.Add(_skillMenu);
    }
    public class GameView : Console
    {
        public GameView() : base(GAMEVIEW_WIDTH, GAMEVIEW_HEIGHT, CHUNK_WIDTH, CHUNK_HEIGHT)
        {
            var font = Game.Instance.Fonts["mdcurses16"];
            Font = font;
            Game.Instance.FocusedScreenObjects.Push(this);
        }
    }
    public void UpdateChunk(ColoredGlyph[,] glyphChunk)
    {
        for (var y = 0; y < glyphChunk.GetLength(0); y++)
        {
            for (var x = 0; x < glyphChunk.GetLength(1); x++)
            {
                glyphChunk[y,x].CopyAppearanceTo(_gameView.Surface[x,y]);
            }
        }
    }

    public class GameLog : Console
    {
        private ScrollBar _scrollBar;
        private ControlsConsole _scrollComponent;
        public GameLog() : base(GAMELOG_WIDTH - 1, GAMELOG_HEIGHT, GAMELOG_WIDTH - 1, GAMELOG_MAXHEIGHT)
        {
            _scrollBar = new ScrollBar(Orientation.Vertical, GAMELOG_HEIGHT);
            _scrollComponent = new ControlsConsole(1, GAMELOG_HEIGHT) { Position = new Point(GAMELOG_WIDTH-1, 0) };
            Cursor.Print("epicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepicepic");
            
            _scrollComponent.Controls.Add(_scrollBar);
            Children.Add(_scrollComponent);
        }
    }
}