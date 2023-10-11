namespace CaveGame;

public static class WorldGeneration
{
    public static string[,] Generate(int size, int minCaveArea, int seed)
    {
        var scale = 0.10f;
        
        var cave = new string[size,size];
        
        SimplexNoise.Seed = seed;
        var noiseGrid = SimplexNoise.Calc2D(size, size, scale);

        while(true)
        {
            var walls = new bool[size,size];
            System.Console.WriteLine("sex1");
            
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (noiseGrid[y,x] > 0)
                    {
                        System.Console.Write("sex2");
                        walls[y,x] = true;
                    }
                    else
                    {
                        walls[y,x] = false;
                    }
                }
            }
            
            System.Console.WriteLine("sex3");
    
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
            
            int startY;
            int startX;
            while (true)
            {
                System.Console.WriteLine("sex4");
                startY = SHUtil.Random(0, size - 1);
                startX = SHUtil.Random(0, size - 1);
                if (walls[startY,startX] == false)
                {
                    System.Console.WriteLine("sex5");
                    break;
                }
            }
            
            var visited = new bool[size,size];

            var caveArea = FillCount(startY, startX, visited, walls);

            if(caveArea >= minCaveArea)
            {
                for (var y = 0; y < visited.Length; y++) {
                    for (var x = 0; x < visited.Length; x++) {
                        if(visited[y,x]){
                            System.Console.WriteLine("sex5");
                            cave[y,x] = "·";
                        }
                        else{
                            cave[y,x] = "X";
                        }
                    }
                }
                break;
            }
            System.Console.WriteLine("cave rejected");
        }


        System.Console.WriteLine("\033[H\033[2J");  
        System.Console.Clear();
        System.Console.WriteLine("Region Generated");
        System.Console.WriteLine("Seed: " + seed);
        return cave;
    }
    
    private static int FillCount(int y, int x, bool[,] visited, bool[,] walls){
        var count = 1;
        var onGrid = (0 <= y && y <= walls.GetLength(0) - 1 && 0 <= x && x <= walls.GetLength(1) - 1);
        if(!onGrid){return 0;}
        if(visited[y,x] || walls[y,x]) {return 0;}
        visited[y,x] = true;
        count += FillCount(y+1, x, visited, walls);
        count += FillCount(y-1, x, visited, walls);
        count += FillCount(y, x+1, visited, walls);
        count += FillCount(y, x-1, visited, walls);
        return count;
    }
}