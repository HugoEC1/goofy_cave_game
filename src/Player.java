import java.util.Scanner;

class Player{
    final Scanner input = new Scanner(System.in);

    String[][] grid = Main.grid;

    int maxHealth = 100;
    int hp;
    int hunger;
    int fuel;

    public int[] position = {-1, -1};
    
    public Player(int y, int x){
        this.hp = this.maxHealth;
        this.hunger = 100;
        this.fuel = 100;
        this.position = new int[]{y, x};
    }

    public String toString(){
        return colour.BLUE + "@" + colour.RESET;
    }

    public void takeDamage(int x){
        this.hp -= x;
        if(this.hp <= 0){
            die();
        }
    }

    private void die(){
        System.out.println("you died");
        Main.onEnter();
    }

    public void turn(EnemyManager enemyManager){
        if(hp < 100){
            hp += 1;
        }

        this.grid = Main.grid;
        String move = null;
        String action = "";
        move = input.nextLine().toLowerCase();
        int[] direction = {0, 0};

        if(move.equals("w")){
            action = "walk";
            direction = new int[]{-1, 0};
        }
        else if(move.equals("a")){
            action = "walk";
            direction = new int[]{0, -1};
        }
        else if(move.equals("s")){
            action = "walk";
            direction = new int[]{1, 0};
        }
        else if(move.equals("d")){
            action = "walk";
            direction = new int[]{0, 1};
        }
        else if(move.equals("f")){
            action = "shoot";
        }
        
        if(action.equals("walk")){
            int[] wantedPosition = {position[0] + direction[0], position[1] + direction[1]};
            if(Hutil.inRange(wantedPosition[0], 0, grid.length) && Hutil.inRange(wantedPosition[1], 0, grid.length)){
                if(grid[wantedPosition[0]][wantedPosition[1]] != "X" && !enemyManager.tileOccupied(wantedPosition, position, grid)){
                    this.position = wantedPosition;
                }
            }
        }
        else if(action.equals("shoot")){
            shoot(enemyManager);
        }
    }

    private void shoot(EnemyManager enemyManager){
        int[] aimPosisiton = {Main.RENDER_DISTANCE, Main.RENDER_DISTANCE};
        boolean aim = true;
        while(true){
            String[][] render = Main.renderGame(this, enemyManager.enemyList);
            render[aimPosisiton[0]][aimPosisiton[1]] = colour.BLUE + "X" + colour.RESET;
            Main.printGame(render, hp);
            String move = input.nextLine().toLowerCase();
            int[] direction = {0, 0};
            if(move.equals("w")){
                direction = new int[]{-1, 0};
            }
            else if(move.equals("a")){
                direction = new int[]{0, -1};
            }
            else if(move.equals("s")){
                direction = new int[]{1, 0};
            }
            else if(move.equals("d")){
                direction = new int[]{0, 1};
            }
            else if(move.equals("f")){
                int[] aimGridPosition = {Main.toGrid(aimPosisiton[0], position[0]), Main.toGrid(aimPosisiton[1], position[1])};
                boolean notPlayer = !Hutil.equals(aimPosisiton, position);
                boolean hitEnemy = enemyManager.tileOccupied(aimGridPosition, position, grid);
                if(notPlayer && hitEnemy){
                    enemyManager.takeDamage(aimGridPosition, 100);
                    System.out.println("hit");
                    Main.onEnter();
                    break;
                }
                else{
                    System.out.println("cant shoot at nothing!");
                    Main.onEnter();
                }
            }
            else if(move.equals(("c"))){
                break;
            }

            boolean onRender = Hutil.inRange(aimPosisiton[0] + direction[0], 0, render.length-1) && Hutil.inRange(aimPosisiton[1] + direction[1], 0, render.length-1);
            if(onRender){
                boolean visible = render[aimPosisiton[0] + direction[0]][aimPosisiton[1] + direction[1]] != " ";
                if(visible){
                    aimPosisiton[0] += direction[0];
                    aimPosisiton[1] += direction[1];
                }
            }
        }
    }
}