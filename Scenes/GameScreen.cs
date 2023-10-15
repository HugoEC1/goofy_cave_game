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
        _gameView = new GameView();
        
        Children.Add(_gameView);
        
        
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
}