using static CaveGame.GameSettings;

namespace CaveGame;

public class EnemyManager
{
    Enemy[] enemyList;
    Enemy[] spawnQueue;
    static readonly Enemy[] EnemyInit = {new Swarmer(), new SwarmerNest(), new GoldenFreddy()};
    int _turnSpeed = TURN_SPEED;
    double _totalSpawnWeight;

    public EnemyManager(int numberOfEnemies, string[,] idGrid)
    {
        foreach (var enemy in EnemyInit)
        {
            _totalSpawnWeight += enemy.SpawnWeight;
        }

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
    protected void removeEnemy(Enemy enemy){
        enemyList.remove(enemy);
    }

    abstract class Enemy
    {
        public int MaxHealth;
        public int Health;
        protected int Speed;
        protected int Damage;
        public double SpawnWeight;
        public int[] Position = {0,0};
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
            int[][] path;
            boolean[][] occupiedTiles = Hutil.stringMapToBool(grid);
            if (enemies != null) {
                for (Enemy enemy : enemies) {
                    occupiedTiles[enemy.position[0]][enemy.position[1]] = true;
                }
            }
            path = pathfinding.findPath(start, end, occupiedTiles);
            if (path != null) {
                return Hutil.copy(path);
            }
            else {
                return null;
            }
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

    class Swarmer : Enemy
    {
        int[] _targetPosition = {0,0};
        int[,] _path = new int[0,0];
        int _turnIndex = 0;
        int _turnsStuck = 0;

        public Swarmer(){
            maxHealth = 10;
            hp = maxHealth;
            speed = 10;
            damage = 5;
            spawnWeight = 10.0;
            _turnIndex = 0;
            _turnsStuck = 0;
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

                if (Vision.canSee(player.position[0], player.position[1], position, grid, -1)) {
                    //as long as player is in sight swarmer tells all other swarmers where the player is
                    for (Enemy swarmer : enemyList) {
                        if (swarmer instanceof Swarmer) {
                            ((Swarmer) swarmer).targetPosition = Hutil.copy(player.position);
                            path = swarmer.pathfind(position, targetPosition, grid, null);
                        }
                    }
                }

                if (Hutil.equals(targetPosition, position)) {
                    targetPosition = chooseWanderSpot(grid);
                    path = pathfind(position, targetPosition, grid, null);
                }

                if (path == null || path.length == 0) {
                    path = pathfind(position, targetPosition, grid, null);
                }

                if (turnsStuck >= 3) {
                    path = pathfind(position, targetPosition, grid, enemyList);
                }

                if (path != null && path.length > 0) {
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
    class SwarmerNest : Enemy
    {
        int _turnIndex = 0;
        int _swarmersLeft = 0;

        public SwarmerNest(){
            maxHealth = 100;
            hp = maxHealth;
            speed = SHutil.Random(4, 12); 
            spawnWeight = 2.0;
            _swarmersLeft = SHutil.Random(3, 8);
            _turnIndex = 0;
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

                if (Vision.canSee(player.position[0], player.position[1], position, grid, -1)) {
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
    class GoldenFreddy : Enemy
    {
        int[] _targetPosition = {0,0};
        int[,] _path = new int[0,0];
        int _turnIndex = 0;
        int _turnsStuck = 0;

        public GoldenFreddy()
        {
            maxHealth = 100;
            hp = maxHealth;
            speed = 100;
            damage = 100;
            spawnWeight = 0.001;
            _turnIndex = 0;
            _turnsStuck = 0;
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
                    path = pathfind(position, targetPosition, grid, enemyList);
                }

                if (path != null && path.length > 0) {
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