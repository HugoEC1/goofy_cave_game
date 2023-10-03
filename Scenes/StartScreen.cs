using SadConsole.Ansi;

namespace CaveGame.Scenes;

class StartScreen : ScreenObject
{
    private ScreenSurface _mainSurface;

    public StartScreen()
    {
        // Create a surface that's the same size as the screen.
        _mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        var doc = new Document($"Art/goofy.ans");
        var writer = new AnsiWriter(doc, _mainSurface.Surface);
        writer.ReadEntireDocument();
        Children.Add(_mainSurface);
    }
}
