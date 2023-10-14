using CaveGame.Generation;
using CaveGame.Scenes;
using static CaveGame.GameSettings;
using static CaveGame.GenerationManager;

// :3

namespace CaveGame;
static class Program
{
    private static int seed;
    private static void Main()
    {
        Settings.WindowTitle = "Goofy Cave Game";

        var gameStartup = new Game.Configuration()
            .SetScreenSize(GAME_WIDTH, GAME_HEIGHT)
            .SetStartingScreen<StartScreen>();

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
        // uncomment to enable custom config
        // Game.Instance.Screen = new CustomConfigScreen();
        seed = new Random().Next(int.MinValue, int.MaxValue);
        var idChunk = Generate(CHUNK_WIDTH, CHUNK_HEIGHT, 0, 0, new Cave(), seed);
        
    }

    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}