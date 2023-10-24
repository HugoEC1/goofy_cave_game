using static CaveGame.GameSettings;
using static CaveGame.Managers.BiomeManager;
using static CaveGame.Managers.TileManager;

namespace CaveGame;

public class Chunk
{
    public Tile[,] Tiles;
    public bool[,] Blocking;
    public int Width;
    public int Height;
    public int[] Position;
    public int Layer;
    public int Seed;
    public Biome Biome;

    public Chunk(Tile[,]? tiles, int[] position, int layer, Biome biome, int seed, int height = CHUNK_WIDTH, int width = CHUNK_HEIGHT)
    {
        Height = height;
        Width = width;
        Tiles = new Tile[Height, Width];
        Position = position;
        Layer = layer;
        Seed = seed;
        Biome = biome;
        if (tiles == null)
        {
            Tiles = biome.GenerateChunk(Height, Width, Position[0], Position[1], Seed);
        }
        else
        {
            Tiles = tiles;
        }
        ToBlocking();
    }
    
    public void ToBlocking()
    {
        var blocking = new bool[Height, Width];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                blocking[y,x] = Tiles[y,x].Blocking;
            }
        }

        Blocking =  blocking;
    }
    /*public Task<Dictionary<int[], string[,]>> GenerateSurroundings()
    {
        var SurroundingChunks = new Dictionary<int[], string[,]>();
        for (var x = -1 * LOAD_DISTANCE; x <= LOAD_DISTANCE; x++)
        {
            for (var y = -1 * LOAD_DISTANCE; y <= LOAD_DISTANCE; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                SurroundingChunks.Add(new[] {x,y}, Generate(width, height, chunkX, chunkY, biome, seed));
            }
        }

        return SurroundingChunks;
    }*/
}