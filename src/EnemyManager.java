import java.lang.reflect.Array;
import java.util.*;
public class EnemyManager{
    ArrayList<Enemy> enemyList = new ArrayList<>();
    ArrayList<Enemy> spawnQueue = new ArrayList<>();
    Enemy[] enemyInit = {new Swarmer(), new SwarmerNest(), new GoldenFreddy()};
    int turnSpeed = Main.TURN_SPEED;
    double totalSpawnWeight = 0;

    public EnemyManager(int numberOfEnemies, String[][] grid){
        for (Enemy enemy : enemyInit) {
            totalSpawnWeight += enemy.spawnWeight;
        }
        boolean[][] occupiedTiles = Hutil.stringMapToBool(grid);
        for (int i = 0; i < numberOfEnemies; i++) {
            int[] spawnPosition;
            while(true){
                spawnPosition = new int[] {Hutil.random(0, grid.length - 1), Hutil.random(0, grid.length - 1)};
                if(!occupiedTiles[spawnPosition[0]][spawnPosition[1]]) {
                    occupiedTiles[spawnPosition[0]][spawnPosition[1]] = true;
                    break;
                }
            }
            int j = 0;
            for (double rand = Math.random() * totalSpawnWeight; j < enemyInit.length - 1; j++) {
                rand -= enemyInit[j].spawnWeight;
                if (rand <= 0) {
                    break;
                }
            }
            Enemy spawnedEnemy = enemyInit[j].copy(); //enemyList won't just refer to objects in enemyInit but copies of them
            spawnQueue.add(spawnedEnemy);
            spawnedEnemy.spawn(spawnPosition);
        }

        spawnEnemies();
    }
    public void spawnEnemies() {
        enemyList.addAll(spawnQueue);
        spawnQueue.clear();
    }
    public boolean tileEmpty(int[] position, int[] playerPosition, String[][] grid){
        for (Enemy enemy : enemyList) {
            if(Hutil.equals(position, enemy.position)){
                return false;
            }
        }

        if(Hutil.equals(position, playerPosition)){
            return false;
        }
        if(grid[position[0]][position[1]] == "X"){
            return false;
        }

        return true;
    }
    public Enemy getEnemyFromLocation(int[] position) {
        for (Enemy enemy : enemyList) {
            if(Hutil.equals(position, enemy.position)){
                return enemy;
            }
        }
        return null;
    }
    protected void removeEnemy(Enemy enemy){
        enemyList.remove(enemy);
    }

    public abstract class Enemy {
        public int maxHealth;
        public int hp;
        protected int speed;
        protected int damage;
        protected double spawnWeight;
        public int[] position;
        public abstract void turn(Player player, String[][] grid);
        public abstract void spawn(int[] spawnPosition);
        public abstract Enemy copy();
        public void takeDamage(int x){
            hp -= x;
            if(hp <= 0){
                die();
            }
        }
        protected int[][] pathfind(int[] start, int[] end, String[][] grid, ArrayList<Enemy> enemies) {
            Pathfinding pathfinding = new Pathfinding();
            boolean[][] occupiedTiles = Hutil.stringMapToBool(grid);
            if (enemies != null) {
                for (Enemy enemy : enemies) {
                    occupiedTiles[enemy.position[0]][enemy.position[1]] = true;
                }
            }
            return pathfinding.findPath(start, end, occupiedTiles);
        }
        protected int[] chooseWanderSpot(String[][] grid) {
            int y;
            int x;
            while(true){
                y = Hutil.random(0, grid.length - 1);
                x = Hutil.random(0, grid.length - 1);
                if(grid[y][x] != "X"){
                    break;
                }
            }
            return new int[]{y, x};
        }
        protected void die(){
            removeEnemy(this);
        }
    }

    class Swarmer extends Enemy{
        int[] targetPosition;
        int[][] path;
        int turnIndex;
        int turnsStuck;

        public Swarmer(){
            this.maxHealth = 10;
            this.hp = maxHealth;
            this.speed = 10;
            this.damage = 5;
            this.spawnWeight = 10.0;
            turnIndex = 0;
            turnsStuck = 0;
        }

        public void spawn(int[] spawnPosition) {
            this.position = spawnPosition;
            this.targetPosition = position; //this just makes the target position its current position so on the next turn it starts wandering.
            // this is to make the spawn method only need the one param
        }
        public Swarmer copy() {
            return new Swarmer();
        }
        public void turn(Player player, String[][] grid){
            turnIndex += speed;

            while (turnIndex >= turnSpeed) {
                turnIndex -= turnSpeed;
                if (position == null) {
                    return;
                }

                if (path == null) {
                    path = pathfind(position, targetPosition, grid, null);
                }

                if (Hutil.equals(targetPosition, position)) {
                    targetPosition = chooseWanderSpot(grid);
                    path = pathfind(position, targetPosition, grid, null);
                }
                if (Vision.hasSightline(player.position[0], player.position[1], position, grid)) {
                    //as long as player is in sight swarmer tells all other swarmers where the player is
                    for (Enemy swarmer : enemyList) {
                        if (swarmer instanceof Swarmer) {
                            ((Swarmer) swarmer).targetPosition = Hutil.copy(player.position);
                            swarmer.pathfind(position, targetPosition, grid, null);
                        }
                    }
                }

                if (turnsStuck >= 3) {
                    path = pathfind(position, targetPosition, grid, enemyList);
                }

                if (path != null) {
                    move(path[path.length - 1], player, grid);
                }
            }
        }
        private void move(int[] desiredPosition, Player player, String[][] grid) {
            boolean onGrid = (Hutil.inRange(desiredPosition[0], 0, grid.length - 1) && Hutil.inRange(desiredPosition[1], 0, grid.length - 1));
            if(onGrid){
                if(Hutil.equals(desiredPosition, player.position)){
                    attack(player);
                    return;
                }
                if(tileEmpty(desiredPosition, player.position, grid)){
                    position = desiredPosition;
                    turnsStuck = 0;
                    path = Hutil.removeEnd(path);
                    return;
                }
            }
            turnsStuck += 1;
        }
        private void attack(Player player){
            player.takeDamage(damage, "swarmer");
        }
        public String toString(){
            return colour.RED + "*" + colour.RESET;
        }

    }
    class SwarmerNest extends Enemy {
        int turnIndex;
        int swarmersLeft;

        public SwarmerNest(){
            this.maxHealth = 100;
            this.hp = maxHealth;
            this.speed = Hutil.random(4, 12);
            this.spawnWeight = 2.0;
            swarmersLeft = Hutil.random(3, 8);
            turnIndex = 0;
        }
        public void spawn(int[] spawnPosition) {
            this.position = spawnPosition;
        }
        public SwarmerNest copy() {
            return new SwarmerNest();
        }
        public void turn(Player player, String[][] grid){
            turnIndex += speed;

            while (turnIndex >= turnSpeed) {
                turnIndex -= turnSpeed;
                if (position == null) {
                    return;
                }

                if (Vision.hasSightline(player.position[0], player.position[1], position, grid)) {
                    //as long as player is in sight swarmer nest spawns swarmers until it runs out
                    if (swarmersLeft > 0) {
                        spawnSwarmer(grid);
                    }
                }
            }
        }
        private void spawnSwarmer(String[][] grid) {
            int y;
            int x;
            boolean[][] occupiedTiles = Hutil.stringMapToBool(grid);
            for (Enemy enemy : enemyList) {
                occupiedTiles[enemy.position[0]][enemy.position[1]] = true;
            }
            for (int i = 0; i < swarmersLeft; i++) {
                y = Hutil.random(position[0] - 1, position[0] + 1);
                x = Hutil.random(position[1] - 1, position[1] + 1);
                if(Hutil.random(0, 5) != 5 && (y >= 0 && y < grid[0].length) && (x >= 0 && x < grid[1].length) && !occupiedTiles[y][x]){
                    Swarmer spawnedSwarmer = new Swarmer();
                    spawnQueue.add(spawnedSwarmer);
                    spawnedSwarmer.spawn(new int[]{y, x});
                    swarmersLeft -= 1;
                    return;
                }
            }
        }

        public String toString(){
            return colour.RED + "0" + colour.RESET;
        }

    }
    class GoldenFreddy extends Enemy {
        int[] targetPosition;
        int[][] path;
        int turnIndex;
        int turnsStuck;

        public GoldenFreddy() {
            this.maxHealth = 100;
            this.hp = maxHealth;
            this.speed = 100;
            this.damage = 100;
            this.spawnWeight = 0.01;
            turnIndex = 0;
            turnsStuck = 0;
        }

        public void spawn(int[] spawnPosition) {
            this.position = spawnPosition;
            this.targetPosition = position;
        }
        public GoldenFreddy copy() {
            return new GoldenFreddy();
        }
        public void turn(Player player, String[][] grid) {
            turnIndex += speed;

            while (turnIndex >= turnSpeed) {
                turnIndex -= turnSpeed;
                if (position == null) {
                    return;
                }

                //golden freddy jumpscare!!!!!!!!!
                targetPosition = Hutil.copy(player.position);
                path = pathfind(position, targetPosition, grid, null);

                if (turnsStuck >= 3) {
                    pathfind(position, targetPosition, grid, enemyList);
                }

                if (path != null) {
                    move(path[path.length - 1], player, grid);
                }
            }
        }

        private void move(int[] desiredPosition, Player player, String[][] grid){
            boolean onGrid = (Hutil.inRange(desiredPosition[0], 0, grid.length - 1) && Hutil.inRange(desiredPosition[1], 0, grid.length - 1));
            if(onGrid){
                if(Hutil.equals(desiredPosition, player.position)){
                    attack(player);
                    return;
                }
                if(tileEmpty(desiredPosition, player.position, grid)){
                    position = desiredPosition;
                    turnsStuck = 0;
                    path = Hutil.removeEnd(path);
                    return;
                }
            }
            turnsStuck += 1;
        }
        private void attack(Player player){
            player.takeDamage(damage, "goldenFreddy");
        }
        public String toString(){
            return colour.YELLOW + "F" + colour.RESET;
        }
    }
}