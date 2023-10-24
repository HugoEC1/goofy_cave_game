using SadConsole.Entities;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using static CaveGame.Program;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;
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
        _gameLog = new GameLog() { Position = new Point(GAMEVIEW_WIDTH * 4 - 1, 0) };
        
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
        private EntityManager _entityManager;
        public GameView() : base(GAMEVIEW_WIDTH - 1, GAMEVIEW_HEIGHT - 1, CHUNK_WIDTH * 3, CHUNK_HEIGHT * 3)
        {
            var font = Game.Instance.Fonts["mdcurses16"];
            Font = font;
            FontSize = Font.GetFontSize(IFont.Sizes.Two);
            
            _entityManager = new EntityManager();
            SadComponents.Add(_entityManager);

            var borderParams = Border.BorderParameters.GetDefault()
                .ChangeBorderGlyph(ICellSurface.ConnectedLineThick, Color.White, Color.Black);
            
            Border border = new(this, borderParams);
            _entityManager.Add(GetPlayer().Entity);
        }
    }
    public void UpdateView(Player player)
    {
        var chunkPosition = GetChunkPosition(player.Position);
        var chunk = GetChunk(chunkPosition[0], chunkPosition[1], player.Layer);
        Chunk? chunkN = null;
        Chunk? chunkNE = null;
        Chunk? chunkE = null;
        Chunk? chunkSE = null;
        Chunk? chunkS = null;
        Chunk? chunkSW = null;
        Chunk? chunkW = null;
        Chunk? chunkNW = null;
        var yOffset = player.Position[0] - GAMEVIEW_HEIGHT / 2;
        var xOffset = player.Position[1] - GAMEVIEW_WIDTH / 2;
        for (var y = 0; y < GAMEVIEW_HEIGHT; y++)
        {
            for (var x = 0; x < GAMEVIEW_WIDTH; x++)
            {
                var chunkOffset = ToChunkPosition(new[] { y + yOffset, x + xOffset });
                var chunkYOffset = chunkOffset[0];
                var chunkXOffset = chunkOffset[1];
                if (y + yOffset < chunkPosition[0] * CHUNK_HEIGHT)
                {
                    // northwest
                    if (x + xOffset < chunkPosition[1] * CHUNK_WIDTH)
                    {
                        if (chunkNW == null) { chunkNW = GetChunk(chunkPosition[0] - 1, chunkPosition[1] - 1, player.Layer); }
                        chunkNW.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                    }
                    // northeast
                    else if (x + xOffset >= chunkPosition[1] * CHUNK_WIDTH + CHUNK_WIDTH)
                    {
                        if (chunkNE == null) { chunkNE = GetChunk(chunkPosition[0] - 1, chunkPosition[1] + 1, player.Layer); }
                        chunkNE.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                    }
                    // north
                    else
                    {
                        if (chunkN == null) { chunkN = GetChunk(chunkPosition[0] - 1, chunkPosition[1], player.Layer); }
                        chunkN.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                    }
                }
                else if (y + yOffset >= chunkPosition[0] * CHUNK_HEIGHT + CHUNK_HEIGHT)
                {
                    // southwest
                    if (x + xOffset < chunkPosition[1] * CHUNK_WIDTH)
                    {
                        if (chunkSW == null) { chunkSW = GetChunk(chunkPosition[0] + 1, chunkPosition[1] - 1, player.Layer); }
                        chunkSW.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                    }
                    // southeast
                    else if (x + xOffset >= chunkPosition[1] * CHUNK_WIDTH + CHUNK_WIDTH)
                    {
                        if (chunkSE == null) { chunkSE = GetChunk(chunkPosition[0] + 1, chunkPosition[1] + 1, player.Layer); }
                        chunkSE.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                    }
                    // south
                    else
                    {
                        if (chunkS == null) { chunkS = GetChunk(chunkPosition[0] + 1, chunkPosition[1], player.Layer); }
                        chunkS.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                    }
                }
                // west
                else if (x + xOffset < chunkPosition[1] * CHUNK_WIDTH)
                {
                    if (chunkW == null) { chunkW = GetChunk(chunkPosition[0], chunkPosition[1] - 1, player.Layer); }
                    chunkW.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                }
                // east
                else if (x + xOffset >= chunkPosition[1] * CHUNK_WIDTH + CHUNK_WIDTH)
                {
                    if (chunkE == null) { chunkE = GetChunk(chunkPosition[0], chunkPosition[1] + 1, player.Layer); }
                    chunkE.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                }
                // center
                else
                {
                    chunk.Tiles[chunkYOffset,chunkXOffset].Glyph.CopyAppearanceTo(_gameView.Surface[x,y]);
                }
            }
        }
        _gameView.IsDirty = true;
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
            keyboard.InitialRepeatDelay = 0.3f;
            //keyboard.RepeatDelay = 0.2f;
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