using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    public static List<Chunk> LoadedChunks = new();

    public static Chunk GetChunk(int chunkY, int chunkX, int layer)
    {
        var chunk = LoadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX);

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

    public static void LoadChunk(int chunkY, int chunkX, int layer)
    {
        LoadedChunks.Add(new Chunk(null, new [] { chunkY, chunkX }, layer, new Cave(), GetSeed()));
    }

    public static int[] ToChunkPosition(int[] position)
    {
        var chunkPositionY = position[0] % CHUNK_HEIGHT;
        var chunkPositionX = position[1] % CHUNK_WIDTH;

        if (chunkPositionY < 0)
        {
            chunkPositionY += CHUNK_HEIGHT;
        }
        if (chunkPositionX < 0)
        {
            chunkPositionX += CHUNK_WIDTH;
        }

        return new[] { chunkPositionY, chunkPositionX };
    }

    public static int[] GetChunkPosition(int[] position)
    {
        int chunkY;
        int chunkX;
        if (position[0] < 0)
        {
            chunkY = position[0] / CHUNK_HEIGHT - 1;
        }
        else
        {
            chunkY = position[0] / CHUNK_HEIGHT;
        }
        if (position[1] < 0)
        {
            chunkX = position[1] / CHUNK_WIDTH - 1;
        }
        else
        {
            chunkX = position[1] / CHUNK_WIDTH;
        }
        
        System.Console.WriteLine("Chunk Position: " + chunkX + ", " + chunkY);
        return new[] { chunkY, chunkX };
    }
}