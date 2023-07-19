
import java.util.Scanner;
// 👍
class Main{
    public static final Scanner input = new Scanner(System.in);
    public static String[][] grid;
    public static final int RENDER_DISTANCE = 30;
    
    public static void main(String[] args) {
        while(true){
            clear();
            printTitle();
            System.out.println();
            System.out.println();

            System.out.println("1 : Play");
            System.out.println("2 : Tech Demo");
            System.out.println("3 : Quit");

            try {
                int choice = Integer.parseInt(input.nextLine());

                if(choice == 1){
                    game();
                }
                else if(choice == 2){
                    demo();
                }
                else if (choice == 3){
                    break;
                }
            } catch (Exception e) {
                // literally do nothing
            }
        }

        System.out.println("Have a nice day!");
    }

    private static void demo(){
        pathfindtest.main(null);
        onEnter();
    }

    private static void printTitle(){
        System.out.println("  ______     ___   ____    ____  _______      _______      ___      .___  ___.  _______ ");
        System.out.println(" /      |   /   \\  \\   \\  /   / |   ____|    /  _____|    /   \\     |   \\/   | |   ____|");
        System.out.println("|  ,----'  /  ^  \\  \\   \\/   /  |  |__      |  |  __     /  ^  \\    |  \\  /  | |  |__   ");
        System.out.println("|  |      /  /_\\  \\  \\      /   |   __|     |  | |_ |   /  /_\\  \\   |  |\\/|  | |   __|  ");
        System.out.println("|  `----./  _____  \\  \\    /    |  |____    |  |__| |  /  _____  \\  |  |  |  | |  |____ ");
        System.out.println(" \\______/__/     \\__\\  \\__/     |_______|    \\______| /__/     \\__\\ |__|  |__| |_______|");

    }

    private static void game(){
        int size;
        int area;
        int numberOfEnemies;
        while(true){
            try {
                System.out.print("World size (400 recomneded): ");
                size = Integer.parseInt(input.nextLine());
                System.out.print("Minumum cave area (50000 recomneded): ");
                area = Integer.parseInt(input.nextLine());
                System.out.println("Enemies: ");
                numberOfEnemies = Integer.parseInt(input.nextLine());
                break;
            } catch (Exception e) {
                System.out.println("invalid");
                onEnter();
                clear();
            }
        }



        grid = CaveGeneration.generate(size, area);
        if(grid == null){
            onEnter();
            return;
        }
        onEnter();

        int startY;
        int startX;
        while(true){
            startY = Hutil.random(0, grid.length - 1);
            startX = Hutil.random(0, grid.length - 1);
            if(grid[startY][startX] == "·"){
                break;
            }
        }
        Player player = new Player(startY, startX);

        EnemyManager enemyManager = new EnemyManager(numberOfEnemies, grid);

        while(true){
            printGame(renderGame(player, enemyManager.enemyList), player.hp);
            player.turn(enemyManager);
            for (EnemyManager.Enemy enemy : enemyManager.enemyList) {
                enemy.turn(player, grid);
            }
            if(player.hp <= 0){
                return;
            }
        }
    }

    public static String[][] renderGame(Player player, EnemyManager.Enemy[] enemyList){
        String[][] render = new String[RENDER_DISTANCE*2+1][RENDER_DISTANCE*2+1];

        // copies part of grid into render
        for (int y = 0; y < render.length; y++) {
            for (int x = 0; x < render.length; x++) {
                int gridY = toGrid(y, player.position[0]);
                int gridX = toGrid(x, player.position[1]);

                boolean onGrid = (Hutil.inRange(gridY, 0, grid.length-1) && Hutil.inRange(gridX, 0, grid.length-1));
                if(onGrid){
                    render[y][x] = grid[gridY][gridX];
                }
                else{
                    render[y][x] = " ";
                }
            }
        }

        // puts enemies on screen
        for (EnemyManager.Enemy enemy : enemyList) {
            int renderY = toRender(enemy.position[0], player.position[0]);
            int renderX = toRender(enemy.position[1], player.position[1]);
            boolean onRender = Hutil.inRange(renderY, 0, render.length-1) && Hutil.inRange(renderX, 0, render.length-1);
            if(onRender){
                render[renderY][renderX] = enemy.toString();
            }
        }

        // checks point on screen to see if visible
        for (int y = 0; y < render.length; y++) {
            for (int x = 0; x < render.length; x++) {
                int gridY = toGrid(y, player.position[0]);
                int gridX = toGrid(x, player.position[1]);
                boolean onGrid = (Hutil.inRange(gridY, 0, grid.length-1) && Hutil.inRange(gridX, 0, grid.length-1));
                if(onGrid){
                    if (!Vision.canSee(gridY, gridX, player.position, grid, RENDER_DISTANCE)){
                        render[y][x] = " ";
                    }
                }
            }
        }
        for (int y = 0; y < render.length; y++) {
            for (int x = 0; x < render.length; x++) {
                if(render[y][x] == "X"){
                    render[y][x] = "" + "#" + colour.RESET;
                }
            }
        }
        render[RENDER_DISTANCE][RENDER_DISTANCE] = player.toString();
        return render;
    }

    // converts coordinates to Grid --> relative to player and vice versa
    public static int toGrid(int coord, int cameraCoord){
        return (cameraCoord - RENDER_DISTANCE) + coord;
    }
    public static int toRender(int coord, int cameraCoord){
        return coord - (cameraCoord - RENDER_DISTANCE);
    }

    public static void printGame(String[][] render, int hp){
        clear();
        for (String[] y : render) {
            for (String x : y) {
                System.out.print(x + " ");
            }
            System.out.println();
        }
        System.out.println(hp + " hp");
    }

    public static void clear(){
        System.out.print("\033[H\033[2J");  
        System.out.flush();  
    }
    public static void onEnter(){
        System.out.println("Any key to continue");
        input.nextLine();
    }
}