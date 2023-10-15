using CaveGame.Generation;
using CaveGame.Scenes;
using static CaveGame.GameSettings;
using static CaveGame.GenerationManager;

// :3

namespace CaveGame;
static class Program
{
    private static int _seed;
    private static BiomeManager.Biome _biome;
    
    private static void Main()
    {
        Settings.WindowTitle = "Goofy Cave Game";

        var gameStartup = new Game.Configuration()
            .SetScreenSize(START_WIDTH, START_HEIGHT)
            .SetStartingScreen<StartScreen>()
            .ConfigureFonts((f) =>
            {
                f.AddExtraFonts("Fonts/mdcurses16.font");
            });

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
        _seed = new Random().Next(int.MinValue, int.MaxValue);
        _biome = new Cave();
        var idChunk = Generate(CHUNK_WIDTH, CHUNK_HEIGHT, 0, 0, _biome, _seed);
        Game.Instance.Screen = new GameScreen();

    }

    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}