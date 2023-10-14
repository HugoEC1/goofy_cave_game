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
    public override string[,] GenerateChunk(int width, int height, int chunkX, int chunkY, int seed)
    {
        var walls = General.GenerateSimplex(width, height, chunkX, chunkY, seed);

        return GenerateBiome(walls);
    }

    private static string[,] GenerateBiome(bool[,] walls)
    {
        var idChunk = new string[walls.GetLength(0), walls.GetLength(1)];
        
        for (var y = 0; y < walls.GetLength(0); y++)
        {
            for (var x = 0; x < walls.GetLength(1); x++)
            {
                if (walls[y,x])
                {
                    idChunk[y, x] = "stone";
                }
                else
                {
                    idChunk[y, x] = "air";
                }
            }
        }

        return idChunk;
    }
}