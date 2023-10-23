using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    private static List<Chunk> LoadedChunks = new List<Chunk>();

    public static Chunk GetChunk(int[] position, int layer)
    {
        var chunkY = position[0] / CHUNK_HEIGHT;
        var chunkX = position[1] / CHUNK_WIDTH;
        Chunk? chunk = null;
        
        System.Console.WriteLine("sex");
        
        foreach (var loadedChunk in LoadedChunks)
        {
            System.Console.WriteLine(loadedChunk.Position[0] + ", " + loadedChunk.Position[1]);
            if (loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX) continue;
            chunk = loadedChunk;
            break;
        }

        if (chunk == null)
        {
            chunk = new Chunk(null, new []{chunkY, chunkX}, layer, new Cave(), GetSeed());
            LoadedChunks.Add(chunk);
        }

        return chunk;
    }

    public static void AddChunk(Chunk chunk)
    {
        LoadedChunks.Add(chunk);
    }
}