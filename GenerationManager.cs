using static CaveGame.BiomeManager;
using static CaveGame.GameSettings;

namespace CaveGame;

public static class GenerationManager
{
    /*public static Task<Dictionary<int[], string[,]>> GenerateSurroundings(int width, int height, int chunkX, int chunkY, Biome biome, int seed)
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
    public static bool[,] GenerateSimplex(int width, int height, int chunkX, int chunkY, int seed)
    {
        var xOffset = chunkX * width;
        var yOffset = chunkY * height;
        
        var walls = new bool[height,width];
        
        SimplexNoise.Seed = seed;
        var noiseGrid = SimplexNoise.Calc2D(width, height, xOffset, yOffset, 0.10f);
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (noiseGrid[y,x] > 100) // making this number smaller will make more walls as noise outputs number from 0 - 128
                {
                    walls[y,x] = false;
                }
                else
                {
                    walls[y,x] = true;
                }
            }
        }
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (walls[y,x])
                {
                    System.Console.Write("X");
                }
                else
                {
                    System.Console.Write(" ");
                }
            }
            System.Console.WriteLine("");
        }

        System.Console.WriteLine("Region Generated");
        System.Console.WriteLine("Seed: " + seed);
        return walls;
    }
    
    // bad at method names
    // example use: find large enough area to spawn player/staircase
    // TODO: make the method that finds starting coords recursive and to keep finding coords from unvisited tiles until entire chunk has been checked
    // TODO: also find all empty tiles and randomly select from an array of them instead of randomly brute forcing all tiles (while excluding already visited tiles)
    public static bool[,]? FindAreaThatIsOfProvidedArea(bool[,] walls, int area)
    {
        var height = walls.GetLength(0);
        var width = walls.GetLength(1);
        int startY;
        int startX;
        
        while (true)
        {
            startY = SHutil.Random(0, height - 1);
            startX = SHutil.Random(0, width - 1);
            if (walls[startY,startX] == false)
            {
                break;
            }
        }
            
        var visited = new bool[height,width];

        var areaCheckResult = AreaCheck(startY, startX, visited, walls, area);
        _visitedArea = 0;

        if (areaCheckResult)
        {
            return visited;
        }
        return null;
    }
    
    private static int _visitedArea;
    
    private static bool AreaCheck(int y, int x, bool[,] visited, bool[,] walls, int area) 
    {
        if (_visitedArea >= area) { return true; }
        if (visited[y,x] || walls[y,x]) { return false; }
        var onGrid = (0 <= y && y <= walls.GetLength(0) - 1 && 0 <= x && x <= walls.GetLength(1) - 1);
        if (!onGrid) { return false; }
        _visitedArea++;
        visited[y,x] = true;
        if (AreaCheck(y + 1, x, visited, walls, area) || AreaCheck(y-1, x, visited, walls, area) || AreaCheck(y, x+1, visited, walls, area) || AreaCheck(y, x-1, visited, walls, area))
        {
            return true;
        }
        return false;
    }
}