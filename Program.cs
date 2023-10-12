using CaveGame.Scenes;

// :3

namespace CaveGame;
static class Program
{
    private static void Main()
    {
        Settings.WindowTitle = "Goofy Cave Game";

        var gameStartup = new Game.Configuration()
            .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
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
        Game.Instance.Screen = new StartConfigScreen();
    }
    public static void GenerateWorld(int size, int minArea, int enemyCount, int? seed)
    {
        if (seed == null)
        {
            seed = new Random().Next(-2147483648, 2147483647);
        }
        else
        {
            minArea = 0; // ignore minArea if seed is entered
        }
        WorldGeneration.Generate(size, minArea, seed.GetValueOrDefault());
    }
    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}