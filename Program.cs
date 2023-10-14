using CaveGame.Scenes;
using static CaveGame.GameSettings;

// :3

namespace CaveGame;
static class Program
{
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
        
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, 0, 0, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, -1, 1, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, 0, 1, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, 1, 1, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, -1, 0, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, 1, 0, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, -1, -1, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, 0, -1, MINAREA_CHECK);
        var idChunk = Cave.GenerateChunk(CHUNK_WIDTH, CHUNK_HEIGHT, 1, -1, MINAREA_CHECK);
        
    }

    public static void Exit()
    {
        Game.Instance.Screen = new ExitScreen();
    }
}