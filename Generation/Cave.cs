using static CaveGame.Managers.TileManager;
using static CaveGame.Managers.BiomeManager;
using static CaveGame.Generation.MainGeneration;

namespace CaveGame.Generation;

public class Cave : Biome
{
    public Cave()
    {
        ID = "cave";
        Name = "Cave";
        Description = "Default biome";
    }
    public override Tile[,] GenerateChunk(int width, int height, int chunkX, int chunkY, int seed)
    {
        var walls = GenerateSimplex(width, height, chunkX, chunkY, seed);

        var tileChunk = new Tile[height, width];
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (walls[y,x])
                {
                    tileChunk[y, x] = new Stone();
                }
                else
                {
                    tileChunk[y, x] = new Air();
                }
            }
        }
        
        return tileChunk;
    }
}