using static CaveGame.GameSettings;

namespace CaveGame;

public static class BiomeManager
{
    public abstract class Biome
    {
        public string ID = "";
        public string Name = "";
        public string Description = "";
        public abstract TileManager.Tile[,] GenerateChunk(int width, int height, int chunkX, int chunkY, int seed);
    }
}