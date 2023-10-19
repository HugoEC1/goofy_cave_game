using CaveGame.Generation;
using CaveGame.Managers;
using CaveGame.Scenes;
using SadConsole.Configuration;
using static CaveGame.GameSettings;
using static CaveGame.Generation.MainGeneration;
using static CaveGame.Managers.BiomeManager;
using static CaveGame.Managers.ChunkManager;
using static CaveGame.Managers.TileManager;

// :3

namespace CaveGame;
public static class Program
{
    private static int _seed;
    private static Player player;
    private static InputHandler inputHandler;
    private static Chunk currentChunk;
    private static GameScreen gameScreen;
    
    private static void Main()
    {
        Settings.WindowTitle = "Goofy Cave Game";

        Settings.ResizeMode = Settings.WindowResizeOptions.None;
        
        var gameStartup = new Builder()
            .SetScreenSize(1920 / 8, 1080 / 16)
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
    private static void Init(object? sender, GameHost e)
    {
        SadConsole.Host.Global.GraphicsDeviceManager.PreferredBackBufferWidth =
            Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        SadConsole.Host.Global.GraphicsDeviceManager.PreferredBackBufferHeight =
            Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        SadConsole.Host.Global.GraphicsDeviceManager.ApplyChanges();
        
        Game.Instance.ToggleFullScreen();
    }
    private static void Update(object? sender, GameHost e)
    {
        
    }
    public static void Start()
    {
        // uncomment to enable custom config
        // Game.Instance.Screen = new CustomConfigScreen();
        _seed = new Random().Next(int.MinValue, int.MaxValue);
        currentChunk = new Chunk(null, new []{0,0}, 0, new Cave(), _seed);
        gameScreen = new GameScreen();
        Game.Instance.Screen = gameScreen;
        gameScreen.UpdateChunk(currentChunk);
        inputHandler = new InputHandler();
        player = new Player(0, 0, currentChunk, inputHandler);
    }
    public static Player GetPlayer()
    {
        return player;
    }
    public static InputHandler GetInputHandler()
    {
        return inputHandler;
    }
    public static Chunk GetCurrentChunk()
    {
        return currentChunk;
    }
    public static GameScreen GetGameScreen()
    {
        return gameScreen;
    }
    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}