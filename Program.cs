// :3

namespace CaveGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Settings.WindowTitle = "Goofy Cave Game";

            Game.Configuration gameStartup = new Game.Configuration()
                .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
                .SetStartingScreen<CaveGame.Scenes.StartScreen>();

            Game.Create(gameStartup);
            Game.Instance.FrameUpdate += Update;
            Game.Instance.Run();
            Game.Instance.Dispose();

        }

        private static void Update(object? sender, GameHost e)
        {
            //System.Console.WriteLine(e);
        }
    }
}