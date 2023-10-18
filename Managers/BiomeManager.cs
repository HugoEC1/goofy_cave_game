using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;
using static CaveGame.Managers.TileManager;

namespace CaveGame.Managers;

public static class BiomeManager
{
    public abstract class Biome
    {
        public string ID = "";
        public string Name = "";
        public string Description = "";
        public abstract Tile[,] GenerateChunk(int width, int height, int chunkX, int chunkY, int seed);
    }
}