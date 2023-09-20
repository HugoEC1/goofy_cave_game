import java.util.*;

public class Player{
    final Scanner input = new Scanner(System.in);
    int turnSpeed = Main.TURN_SPEED;

    String[][] grid = Main.grid;

    int maxHealth = 100;
    int hp;
    int speed;
    int hunger;
    int fuel;
    int turnIndex;

    public int[] position;
    
    public Player(int y, int x){
        this.hp = this.maxHealth;
        this.speed = 10;
        this.hunger = 100;
        this.fuel = 100;
        this.position = new int[]{y, x};
        turnIndex = 0;
    }

    public String toString(){
        return colour.BLUE + "@" + colour.RESET;
    }

    public void takeDamage(int x, String enemy){
        //this.hp -= x;
        if(this.hp <= 0){
            die(enemy);
        }
    }

    private void die(String enemy){
	    switch (enemy) {
		    case "swarmer" -> System.out.println(colour.RED + "You were torn apart by a swarmer." + colour.RESET);
		    case "goldenFreddy" -> System.out.println(colour.RED + "WAS THAT THE BITE OF 87???" + colour.RESET);
	    }

        System.out.println("\n--- YOU DIED ---");
        Main.onEnter();
        Main.main(null);
    }

    public void turn(EnemyManager enemyManager) {
        turnIndex += speed;

        while (turnIndex >= turnSpeed) {
            turnIndex -= turnSpeed;
            if (hp < 100) {
                hp += 1;
            }

            this.grid = Main.grid;
            String move = input.nextLine().toLowerCase();
            String action;
            int[] direction = {0, 0};

	        switch (move) {
		        case "w" -> {
			        action = "walk";
			        direction = new int[]{-1, 0};
		        }
		        case "a" -> {
			        action = "walk";
			        direction = new int[]{0, -1};
		        }
		        case "s" -> {
			        action = "walk";
			        direction = new int[]{1, 0};
		        }
		        case "d" -> {
			        action = "walk";
			        direction = new int[]{0, 1};
		        }
		        case "f" -> action = "shoot";
		        default -> action = "wait";
	        }

	        switch (action) {
		        case "walk" -> {
			        int[] wantedPosition = {position[0] + direction[0], position[1] + direction[1]};
			        if (Hutil.inRange(wantedPosition[0], 0, grid.length) && Hutil.inRange(wantedPosition[1], 0, grid.length)) {
				        if (grid[wantedPosition[0]][wantedPosition[1]] != "X" && enemyManager.tileEmpty(wantedPosition, position, grid)) {
					        this.position = wantedPosition;
				        }
			        }
		        }
		        case "shoot" -> shoot(enemyManager);
		        default -> {
		        }
	        }
        }
    }

    private void shoot(EnemyManager enemyManager){
        int[] aimPosition = {Main.RENDER_DISTANCE, Main.RENDER_DISTANCE};
        boolean aim = true;
		String[][] render = Hutil.copy(Main.currentRender);
	    label:
	    while(true){
	        render[aimPosition[0]][aimPosition[1]] = colour.BLUE + "X" + colour.RESET;
	        Main.printGame(render, hp);
			render[aimPosition[0]][aimPosition[1]] = Main.currentRender[aimPosition[0]][aimPosition[1]];
	        String move = input.nextLine().toLowerCase();
	        int[] direction = {0, 0};
		    switch (move) {
			    case "w":
				    direction = new int[]{-1, 0};
				    break;
			    case "a":
				    direction = new int[]{0, -1};
				    break;
			    case "s":
				    direction = new int[]{1, 0};
				    break;
			    case "d":
				    direction = new int[]{0, 1};
				    break;
			    case "f":
				    int[] aimGridPosition = {0, 0};
					for (int i = 0; i < aimGridPosition.length; i++) {
						aimGridPosition[i] = Main.toGrid(aimPosition[i], position[i]);
					}
				    boolean notPlayer = !Hutil.equals(aimPosition, position);
				    EnemyManager.Enemy hitEnemy = enemyManager.getEnemyFromLocation(aimGridPosition);
				    if (notPlayer && hitEnemy != null) {
					    hitEnemy.takeDamage(20);
					    System.out.println("hit");
					    Main.onEnter();
					    break label;
				    } else {
					    System.out.println("cant shoot at nothing!");
					    Main.onEnter();
				    }
				    break;
			    case "c":
				    break label;
		    }

	        boolean onRender = Hutil.inRange(aimPosition[0] + direction[0], 0, render.length-1) && Hutil.inRange(aimPosition[1] + direction[1], 0, render.length-1);
	        if(onRender){
	            boolean visible = render[aimPosition[0] + direction[0]][aimPosition[1] + direction[1]] != " ";
	            if(visible){
	                aimPosition[0] += direction[0];
	                aimPosition[1] += direction[1];
	            }
	        }
	    }
    }
}