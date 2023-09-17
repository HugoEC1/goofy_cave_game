import java.util.*;
public class EnemyManager{
    Enemy[] enemyList;
    Enemy[] enemyTypes = new Enemy[2];
    int totalSpawnWeight = 0;

    public EnemyManager(int numberOfEnemies, String[][] grid){
        enemyList = new Enemy[numberOfEnemies];

        for (int weight : enemyWeights) {
            totalSpawnWeight += weight;
        }

        for (int i = 0; i < enemyList.length; i++) {
            int[] spawnPosition;
            while(true){
                spawnPosition = new int[] {Hutil.random(0, grid.length - 1), Hutil.random(0, grid.length - 1)};
                if(grid[spawnPosition[0]][spawnPosition[1]] != "X"){
                    break;
                }
            }
            int j = 0;
            for (int rand = Hutil.random(0, totalSpawnWeight); j < enemyWeights.length - 1; j++) {
                rand -=
            }

            enemyList[i] = new Swarmer(spawnPosition, grid);
        }
    }

    public boolean tileOccupied(int[] position, int[] playerPosition, String[][] grid){
        for (Enemy enemy : enemyList) {
            if(Hutil.equals(position, enemy.position)){
                return true;
            }
        }

        if(Hutil.equals(position, playerPosition)){
            return true;
        }
        if(grid[position[0]][position[1]] == "X"){
            return true;
        }

        return false;
    }

    public void takeDamage(int[] position, int x){
        for (Enemy enemy : enemyList) {
            if(Hutil.equals(enemy.position, position)){
                enemy.takeDamage(x);
                break;
            }
        }
    }

    protected void removeEnemy(Enemy enemy){
        int index = -1;
        for (int i = 0; i < enemyList.length; i++) {
            if(enemyList[i] == enemy){
                index = i;
                break;
            }
        }

        Enemy[] new_list = new Enemy[enemyList.length - 1];

        int indexSkipper = 0;
        for (int i = 0; i < enemyList.length; i++) {
            if(i == index){
                indexSkipper += 1;
            }
            else{
                new_list[i - indexSkipper] = enemyList[i];
            }
        }

        enemyList = new_list;
    }

    public class Enemy {
        public int maxHealth;
        public int hp;
        protected int speed;
        protected int damage;
        protected int spawnWeight;
        public int[] position;

        public void turn(Player player, String[][] grid){
            if(Vision.hasSightline(player.position[0], player.position[1], position, grid)){
                // do something
            }
        }

        public void takeDamage(int x){
            hp -= x;
            if(hp <= 0){
                die();
            }
        }

        protected void die(){
            removeEnemy(this);
        }
    }

    class Swarmer extends Enemy{
        int[] targetPosition;
        int[][] path;
        int turnsStuck;

        public Swarmer(String[][] grid){
            this.maxHealth = 10;
            this.hp = maxHealth;
            this.speed = 3;
            this.damage = 10;
            this.spawnWeight = 10;
            this.targetPosition = chooseWanderSpot(grid);
            pathfind(position, targetPosition, grid);
            turnsStuck = 0;
        }

        public int getSpawnWeight() {
            return spawnWeight;
        }

        public void setSpawn(int[] spawnPosition) {
            this.position = spawnPosition;
        }
        public void turn(Player player, String[][] grid){
            if (position == null) {
                return;
            }

            if(path == null){
                pathfind(position, targetPosition, grid);
            }

            if(Hutil.equals(targetPosition, position)){
                targetPosition = chooseWanderSpot(grid);
                pathfind(position, targetPosition, grid);
            }
            if(Vision.hasSightline(player.position[0], player.position[1], position, grid)){
                //as long as player is in sight swarmer tells all other swarmers where the player is
                for (Enemy swarmer : enemyList) {
                    ((Swarmer)swarmer).targetPosition = Hutil.copy(player.position);
                    ((Swarmer)swarmer).pathfind(position, targetPosition, grid);
                }
            }

            if(turnsStuck >= 3){
                pathfind(position, targetPosition, grid, enemyList);
            }

            if(path != null){
                move(path[path.length-1], player, grid);
            }
        }

        private void move(int[] desiredPosition, Player player, String[][] grid){
            boolean onGrid = (Hutil.inRange(desiredPosition[0], 0, grid.length - 1) && Hutil.inRange(desiredPosition[1], 0, grid.length - 1));
            if(onGrid){
                if(Hutil.equals(desiredPosition, player.position)){
                    attack(player);
                    return;
                }
                if(!tileOccupied(desiredPosition, player.position, grid)){
                    position = desiredPosition;
                    turnsStuck = 0;
                    path = Hutil.removeEnd(path);
                    return;
                }
            }
            turnsStuck += 1;
        }

        private void pathfind(int[] start, int[] end, String[][] grid){
            Pathfinding pathfinding = new Pathfinding();
            boolean[][] walls = Hutil.stringMapToBool(grid);
            path = pathfinding.findPath(start, end, walls);
        }
        private void pathfind(int[] start, int[] end, String[][] grid, Enemy[] enemies){
            Pathfinding pathfinding = new Pathfinding();
            boolean[][] walls = Hutil.stringMapToBool(grid);
            for (Enemy enemy : enemies) {
                walls[enemy.position[0]][enemy.position[1]] = true;
            }
            path = pathfinding.findPath(start, end, walls);
        }

        private void attack(Player player){
            player.takeDamage(damage);
        }

        private int[] chooseWanderSpot(String[][] grid){
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

        public String toString(){
            return colour.RED + "*" + colour.RESET;
        }

    }
    class SwarmerNest extends Enemy {
        int swarmersLeft;

        public SwarmerNest(String[][] grid){
            this.maxHealth = 100;
            this.hp = maxHealth;
            this.spawnWeight = 1;
            this.position = spawnPosition;
            this.swarmersLeft = Hutil.random(10, 30);
        }

        public int getSpawnWeight() {
            return spawnWeight;
        }
        public void setSpawn(int[] spawnPosition) {
            this.position = spawnPosition;
        }

        public void turn(Player player, String[][] grid){
            if (position == null) {
                return;
            }

            if(Vision.hasSightline(player.position[0], player.position[1], position, grid)){
                //as long as player is in sight swarmer nest spawns swarmers until it runs out

            }
        }

        public String toString(){
            return colour.RED + "0" + colour.RESET;
        }

    }
}