using static CaveGame.Managers.TileManager;
using static CaveGame.Managers.BiomeManager;
using static CaveGame.Managers.ChunkManager;
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
    public override Tile[,] GenerateChunk(Chunk chunk)
    {
        var walls = GenerateSimplex(chunk.Width, chunk.Height, chunk.Position[0], chunk.Position[1], chunk.Seed);

        var tileChunk = new Tile[chunk.Height, chunk.Width];
        
        for (var y = 0; y < chunk.Height; y++)
        {
            for (var x = 0; x < chunk.Width; x++)
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