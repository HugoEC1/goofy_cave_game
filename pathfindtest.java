
import java.util.Scanner;

public class pathfindtest {
    public static final Scanner input = new Scanner(System.in);
    public static void main(String[] args) {
        String[][] grid = CaveGeneration.generate(400, 60000);

        boolean[][] walls = new boolean[grid.length][grid[0].length];
        for (int y = 0; y < walls.length; y++) {
            for (int x = 0; x < walls.length; x++) {
                boolean isWall = (grid[y][x] == "X");
                walls[y][x] = isWall;
            }
        }

        for (int y = 0; y < grid.length; y++) {
            for (int x = 0; x < grid.length; x++) {
                if(grid[y][x] == "X"){
                    grid[y][x] = colour.RED + "X" + colour.RESET;
                }
            }
        }
        

        for (int y = 0; y < grid.length; y++) {
            for (int x = 0; x < grid.length; x++) {
                if(grid[y][x] == "X"){
                    grid[y][x] = colour.RED + "X" + colour.RESET;
                }
            }
        }

        int startY = 1;
        int startX = 6;
        while(true){
            startY = Hutil.random(0, walls.length - 1);
            startX = Hutil.random(0, walls.length - 1);
            if(walls[startY][startX] == false){
                break;
            }
        }
        int targetY = 45;
        int targetX = 4;
        while(true){
                targetY = Hutil.random(0, walls.length - 1);
                targetX = Hutil.random(0, walls.length - 1);
                if(walls[targetY][targetX] == false){
                    break;
            }
        }
        Pathfinding pathfinding = new Pathfinding();
        long start = System.currentTimeMillis();

        int[][] path = pathfinding.findPath(new int[]{startY, startX}, new int[]{targetY, targetX}, walls);
        System.out.println("completed in " + (System.currentTimeMillis()-start) + "ms");

        for (int[] node : path) {
            grid[node[0]][node[1]] = colour.BLUE + "O" + colour.RESET;
        }
        grid[targetY][targetX] = colour.GRENN + "@" + colour.RESET;
        grid[startY][startX] = colour.GRENN + "@" + colour.RESET;

        printTest(grid);
    }

    public static void printTest(String[][] grid){
        for (String[] y : grid) {
            for (String x : y) {
                System.out.print(x + " ");
            }
            System.out.println();
        }
    }
}
