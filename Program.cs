using CaveGame.Scenes;

// :3

namespace CaveGame;
static class Program
{
    private static void Main(string[] args)
    {
        Settings.WindowTitle = "Goofy Cave Game";

        var gameStartup = new Game.Configuration()
            .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
            .SetStartingScreen<CaveGame.Scenes.StartScreen>();

        Game.Create(gameStartup);
        Game.Instance.FrameUpdate += Update;
        Game.Instance.Run();
        Game.Instance.Dispose();

    }
    private static void Update(object? sender, GameHost e)
    {
        
    }
    public static void Start()
    {
        Game.Instance.Screen = new StartConfigScreen();
    }
    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}