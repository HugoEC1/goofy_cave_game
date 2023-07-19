public class CaveGeneration {
    static final String LOADINGBLOCK = "\u2588";

    public static String[][] generate(int size, int minCaveArea){
        String[][] cave = new String[size][size];
        double[][] noiseGrid = new double[size][size];
        boolean[][] walls = new boolean[size][size];

        double scale = 0.1;

        int rejected = 0;

        double seed;
        int caveArea;
        while(true){
            seed = Math.random() * 1000000;

            double checkpoint = 0;
    
            // creates 2d array of values from -1, 1
            for (int y = 0; y < noiseGrid.length; y++) {
                for (int x = 0; x < noiseGrid.length; x++) {
                    noiseGrid[y][x] = OpenSimplex2S.noise2((long)seed, y*scale, x*scale);

                    double percent = (y/(double)(noiseGrid.length)) * 100;

                    if (percent > checkpoint){
                        percent = Hutil.round(percent, 10);
                        checkpoint = percent + 10;
                        printStatus(rejected, (int)(percent/10));
                    }
                }
            }
    
            walls = new boolean[size][size];
    
            for (int y = 0; y < walls.length; y++) {
                for (int x = 0; x < walls.length; x++) {
                    if(noiseGrid[y][x] > 0){
                        walls[y][x] = true;
                    }
                    else{
                        walls[y][x] = false;
                    }
                }
            }
    
            // creates wall border around map
            for (int y = 0; y < walls.length; y++) {
                walls[y][0] = true;
            }
            for (int y = 0; y < walls.length; y++) {
                walls[y][size-1] = true;
            }
            for (int x = 0; x < walls.length; x++) {
                walls[0][x] = true;
            }
            for (int x = 0; x < walls.length; x++) {
                walls[size-1][x] = true;
            }
            
            int startY;
            int startX;
            while(true){
                startY = Hutil.random(0, walls.length - 1);
                startX = Hutil.random(0, walls.length - 1);
                if(walls[startY][startX] == false){
                    break;
                }
            }

            boolean[][] visited = new boolean[size][size];

            try {
                caveArea = fillCount(startY, startX, visited, walls);

                if(caveArea >= minCaveArea){
                    for (int y = 0; y < visited.length; y++) {
                        for (int x = 0; x < visited.length; x++) {
                            if(visited[y][x] == true){
                                cave[y][x] = "·";
                            }
                            else{
                                cave[y][x] = "X";
                            }
                        }
                    }

                    break;
                }
                else{
                    System.out.println("cave rejected");
                    rejected += 1;
                }
            } 
            catch (Exception e) {
                System.out.println("An error occured during generation");
                return null;
            }
        }


        System.out.print("\033[H\033[2J");  
        System.out.flush();
        System.out.println("Region Generated");
        System.out.println("Seed: " + seed);
        System.out.println(size + "x" + size + ", cave area = " + caveArea + "u^2");
        return cave;
    }
    public static String[][] generate(int size, int minCaveArea, long seed){
        String[][] cave = new String[size][size];
        double[][] noiseGrid = new double[size][size];
        boolean[][] walls = new boolean[size][size];

        double scale = 0.1;

        int rejected = 0;
        while(true){

            double checkpoint = 0;
    
            for (int y = 0; y < noiseGrid.length; y++) {
                for (int x = 0; x < noiseGrid.length; x++) {
                    noiseGrid[y][x] = OpenSimplex2S.noise2((long)seed, y*scale, x*scale);

                    double percent = (y/(double)(noiseGrid.length)) * 100;

                    if (percent > checkpoint){
                        percent = Hutil.round(percent, 10);
                        checkpoint = percent + 10;
                        printStatus(rejected, (int)(percent/10));
                    }
                }
            }
    
            walls = new boolean[size][size];
    
            for (int y = 0; y < walls.length; y++) {
                for (int x = 0; x < walls.length; x++) {
                    if(noiseGrid[y][x] > 0){
                        walls[y][x] = true;
                    }
                    else{
                        walls[y][x] = false;
                    }
                }
            }
    
            for (int y = 0; y < walls.length; y++) {
                walls[y][0] = true;
            }
            for (int y = 0; y < walls.length; y++) {
                walls[y][size-1] = true;
            }
            for (int x = 0; x < walls.length; x++) {
                walls[0][x] = true;
            }
            for (int x = 0; x < walls.length; x++) {
                walls[size-1][x] = true;
            }
            
            int startY;
            int startX;
            while(true){
                startY = Hutil.random(0, walls.length - 1);
                startX = Hutil.random(0, walls.length - 1);
                if(walls[startY][startX] == false){
                    break;
                }
            }

            boolean[][] visited = new boolean[size][size];

            int caveArea = fillCount(startY, startX, visited, walls);

            if(caveArea >= minCaveArea){
                for (int y = 0; y < visited.length; y++) {
                    for (int x = 0; x < visited.length; x++) {
                        if(visited[y][x] == true){
                            cave[y][x] = "·";
                        }
                        else{
                            cave[y][x] = "X";
                        }
                    }
                }

                break;
            }
            else{
                System.out.println("cave rejected");
                rejected += 1;
            }
        }


        System.out.print("\033[H\033[2J");  
        System.out.flush();
        System.out.println("Region Generated");
        System.out.println("Seed: " + seed);
        return cave;
    }

    private static int fillCount(int y, int x, boolean[][] visited, boolean[][] walls){
            int count = 1;
            boolean onGrid = (Hutil.inRange(y, 0, walls.length-1) && Hutil.inRange(x, 0, walls.length-1));
            if(!onGrid){return 0;}
            if(visited[y][x] == true || walls[y][x] == true) {return 0;}
            visited[y][x] = true;
            count += fillCount(y+1, x, visited, walls);
            count += fillCount(y-1, x, visited, walls);
            count += fillCount(y, x+1, visited, walls);
            count += fillCount(y, x-1, visited, walls);
            return count;
    }

    private static void printStatus(int rejected, int progress){
        System.out.print("\033[H\033[2J");  
        System.out.flush();
        System.out.println(rejected + " rejected");
        System.out.println("Generating Region");
        System.out.println("[" + LOADINGBLOCK.repeat(progress) + " ".repeat(10-progress) + "]");
    }
}
