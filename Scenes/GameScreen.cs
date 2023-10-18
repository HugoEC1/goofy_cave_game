using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using static CaveGame.Program;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;
using static CaveGame.Managers.TileManager;
using static CaveGame.Player;

namespace CaveGame.Scenes;

public class GameScreen : ScreenObject
{
    private ScreenSurface _gameSurface;
    private Console _gameView;
    private Console _gameLog;
    private Console _skillMenu;
    private Console _inputConsole;

    public GameScreen()
    {
        // create overall surface to hold consoles
        _gameSurface = new ScreenSurface(GAME_WIDTH, GAME_HEIGHT);
        
        // add game display console
        _gameView = new GameView() { Position = new Point(1, 1) };
        
        // add scrollable log console
        _gameLog = new GameLog() { Position = new Point(GAMEVIEW_WIDTH * 4, 0) };
        
        // add skill menu console
        //_skillMenu = new SkillMenu();
        
        // add console to take inputs
        _inputConsole = new InputConsole() { Position = new Point(0, 0) };
        
        Children.Add(_gameSurface);
        Children.Add(_gameView);
        Children.Add(_gameLog);
        //Children.Add(_skillMenu);
        Children.Add(_inputConsole);
    }
    public class GameView : Console
    {
        public GameView() : base(GAMEVIEW_WIDTH - 1, GAMEVIEW_HEIGHT - 1, CHUNK_WIDTH * 3, CHUNK_HEIGHT * 3)
        {
            var font = Game.Instance.Fonts["mdcurses16"];
            Font = font;
            FontSize = Font.GetFontSize(IFont.Sizes.Two);
        }
    }
    public void UpdateChunk(Chunk chunk)
    {
        for (var y = 0; y < chunk.Height; y++)
        {
            for (var x = 0; x < chunk.Width; x++)
            {
                chunk.Tiles[y,x].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
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
            
            _scrollComponent.Controls.Add(_scrollBar);
            Children.Add(_scrollComponent);
        }
    }
    public void PrintLog(string msg, Color color)
    {
        _gameLog.Cursor.SetPrintAppearance(color);
        _gameLog.Cursor.Print(msg);
    }

    public class InputConsole : Console
    {
        public InputConsole() : base(1, 1)
        {
            Game.Instance.FocusedScreenObjects.Push(this);
            UseKeyboard = true;
        }
        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            if (keyboard.KeysPressed.Count == 0) return false;
            
            foreach (var asciiKey in keyboard.KeysPressed)
            {
                GetInputHandler().Input(asciiKey.Key);
                return true;
            }
            
            return false;
        }
    }
}