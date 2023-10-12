namespace CaveGame;

public static class WorldGeneration
{
    public static string[,] Generate(int size, int minCaveArea, int seed)
    {
        var cave = new string[size,size];
        
        while(true)
        {
            SimplexNoise.Seed = seed;
            var noiseGrid = SimplexNoise.Calc2D(size, size, 0.10f);
            
            var walls = new bool[size,size];
            
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (noiseGrid[y,x] > 100) // making this number bigger will make more walls as noise outputs number from 0 - 128
                    {
                        walls[y,x] = false;
                    }
                    else
                    {
                        walls[y,x] = true;
                    }
                }
            }
    
            for (var y = 0; y < size; y++) {
                walls[y,0] = true;
            }
            for (var y = 0; y < size; y++) {
                walls[y,size-1] = true;
            }
            for (var x = 0; x < size; x++) {
                walls[0,x] = true;
            }
            for (var x = 0; x < size; x++) {
                walls[size-1,x] = true;
            }

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (walls[y, x])
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

            int startY;
            int startX;
            while (true)
            {
                startY = SHutil.Random(0, size - 1);
                startX = SHutil.Random(0, size - 1);
                if (walls[startY,startX] == false)
                {
                    break;
                }
            }
            
            var visited = new bool[size,size];

            var caveArea = FillCount(startY, startX, visited, walls);

            if (caveArea >= minCaveArea)
            {
                for (var y = 0; y < size; y++)
                {
                    for (var x = 0; x < size; x++)
                    {
                        if (visited[y,x])
                        {
                            cave[y,x] = "·";
                        }
                        else
                        {
                            cave[y,x] = "X";
                        }
                    }
                }
                break;
            }
            System.Console.WriteLine("cave rejected");
            seed = new Random().Next(-2147483648, 2147483647);
        }
        System.Console.WriteLine("Region Generated");
        System.Console.WriteLine("Seed: " + seed);
        return cave;
    }
    
    private static int FillCount(int y, int x, bool[,] visited, bool[,] walls) 
    {
        var count = 1;
        var onGrid = (0 <= y && y <= walls.GetLength(0) - 1 && 0 <= x && x <= walls.GetLength(1) - 1);
        if (!onGrid) { return 0; }
        if (visited[y,x] || walls[y,x]) { return 0; }
        visited[y,x] = true;
        count += FillCount(y+1, x, visited, walls);
        count += FillCount(y-1, x, visited, walls);
        count += FillCount(y, x+1, visited, walls);
        count += FillCount(y, x-1, visited, walls);
        return count;
    }
}