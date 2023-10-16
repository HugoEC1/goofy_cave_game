using static CaveGame.TileManager;
using static CaveGame.BiomeManager;

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
        var walls = GenerationManager.GenerateSimplex(width, height, chunkX, chunkY, seed);

        var idChunk = new Tile[walls.GetLength(0), walls.GetLength(1)];
        
        for (var y = 0; y < walls.GetLength(0); y++)
        {
            for (var x = 0; x < walls.GetLength(1); x++)
            {
                if (walls[y,x])
                {
                    idChunk[y, x] = new Stone();
                }
                else
                {
                    idChunk[y, x] = new Air();
                }
            }
        }
        
        return idChunk;
    }
}