using CaveGame.Generation;
using CaveGame.Scenes;
using SadConsole.Configuration;
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

        Settings.ResizeMode = Settings.WindowResizeOptions.None;
        
        var gameStartup = new Builder()
            .SetScreenSize(GAME_WIDTH, GAME_HEIGHT)
            .SetStartingScreen<StartScreen>()
            .OnStart(Init)
            .ConfigureFonts((f, g) =>
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
    private static void Init(object? sender, GameHost e)
    {
        SadConsole.Host.Global.GraphicsDeviceManager.PreferredBackBufferWidth =
            Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        SadConsole.Host.Global.GraphicsDeviceManager.PreferredBackBufferHeight =
            Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        SadConsole.Host.Global.GraphicsDeviceManager.ApplyChanges();
        
        Game.Instance.ToggleFullScreen();
    }
    public static void Start()
    {
        // uncomment to enable custom config
        // Game.Instance.Screen = new CustomConfigScreen();
        _seed = new Random().Next(int.MinValue, int.MaxValue);
        _biome = new Cave();
        var idChunk = Generate(CHUNK_WIDTH, CHUNK_HEIGHT, 0, 0, _biome, _seed);
        var gameScreen = new GameScreen();
        Game.Instance.Screen = gameScreen;
        gameScreen.UpdateChunk(TileManager.GetGlyphChunk(idChunk));
    }

    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}