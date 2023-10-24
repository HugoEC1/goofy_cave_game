using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    private static List<Chunk> _loadedChunks = new();

    public static Chunk GetChunk(int chunkY, int chunkX, int layer)
    {
        var chunk = _loadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX);

        if (chunk == null)
        {
            chunk = new Chunk(null, new []{chunkY, chunkX}, layer, new Cave(), GetSeed());
            _loadedChunks.Add(chunk);
        }

        return chunk;
    }

    public static void AddChunk(Chunk chunk)
    {
        _loadedChunks.Add(chunk);
    }

    public static void LoadSurroundingChunks(int chunkY, int chunkX, int layer)
    {
        // northwest
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX - 1, layer));
        // north
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX, layer));
        // northeast
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX + 1, layer));
        // west
        _ = Task.Run(() => LoadChunk(chunkY, chunkX - 1, layer));
        // east
        _ = Task.Run(() => LoadChunk(chunkY, chunkX + 1, layer));
        // southwest
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX - 1, layer));
        // south
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX, layer));
        // southeast
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX + 1, layer));
    }
    
    public static async Task LoadChunk(int chunkY, int chunkX, int layer)
    {
        if (_loadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX) != null) { return; }
        var createChunkTask = Task.Run(() => new Chunk(null, new[] { chunkY, chunkX }, layer, new Cave(), GetSeed()));
        var chunk = await createChunkTask;
        if (_loadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX) != null) { return; }
        _loadedChunks.Add(chunk);
    }

    public static int[] ToLocalPosition(int[] position)
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
            chunkY = (position[0] + 1) / CHUNK_HEIGHT - 1;
        }
        else
        {
            chunkY = position[0] / CHUNK_HEIGHT;
        }
        if (position[1] < 0)
        {
            chunkX = (position[1] + 1) / CHUNK_WIDTH - 1;
        }
        else
        {
            chunkX = position[1] / CHUNK_WIDTH;
        }
        
        System.Console.WriteLine("Chunk Position: " + chunkX + ", " + chunkY);
        return new[] { chunkY, chunkX };
    }
}