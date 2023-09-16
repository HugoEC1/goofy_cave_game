public class Hutil {
    public static boolean inRange(double x, double limit1, double limit2){
        double min = Math.min(limit1, limit2);
        double max = Math.max(limit1, limit2);

        if(x >= min && x <= max){
            return true;
        }
        else{
            return false;
        }
    }
    public static boolean inRange(int x, int limit1, int limit2){
        int min = Math.min(limit1, limit2);
        int max = Math.max(limit1, limit2);

        if(x >= min && x <= max){
            return true;
        }
        else{
            return false;
        }
    }

    public static double[] doubleArr(int[] arr){
        double[] new_arr = new double[arr.length];

        for(int i = 0; i < arr.length; i++){
            new_arr[i] = (double) arr[i];
        }

        return new_arr;
    }

    public static double[] append(double[] arr, double x){
        double[] new_arr = new double[arr.length + 1];

        for(int i = 0; i < arr.length; i++){
            new_arr[i] = arr[i];
        }

        new_arr[arr.length] = x;

        return new_arr;
    }
    public static int[] append(int[] arr, int x){
        int[] new_arr = new int[arr.length + 1];

        for(int i = 0; i < arr.length; i++){
            new_arr[i] = arr[i];
        }

        new_arr[arr.length] = x;

        return new_arr;
    }

    public static double[][] append(double[][] arr, double[] x){
        double[][] new_arr = new double[arr.length + 1][arr[0].length];

        for(int i = 0; i < arr.length; i++){
            for (int j = 0; j < arr[0].length; j++) {
                new_arr[i][j] = arr[i][j];
            }
        }

        new_arr[arr.length] = copy(x);

        return new_arr;
    }
    public static int[][] append(int[][] arr, int[] x){
        int[][] new_arr = new int[arr.length + 1][arr[0].length];

        for(int i = 0; i < arr.length; i++){
            for (int j = 0; j < arr[0].length; j++) {
                new_arr[i][j] = arr[i][j];
            }
        }


        new_arr[arr.length] = copy(x);

        return new_arr;
    }
    public static int[][] removeEnd(int[][] arr){
        int[][] new_arr = new int[arr.length - 1][arr[0].length];

        for (int i = 0; i < new_arr.length; i++) {
            new_arr[i] = arr[i];
        }
        return new_arr;
    }

    public static int[] copy(int[] x){
        int[] new_arr = new int[x.length];

        for (int i = 0; i < new_arr.length; i++) {
            new_arr[i] = x[i];
        }

        return new_arr;
    }
    public static double[] copy(double[] x){
        double[] new_arr = new double[x.length];

        for (int i = 0; i < new_arr.length; i++) {
            new_arr[i] = x[i];
        }

        return new_arr;
    }
    public static String[] copy(String[] x){
        String[] new_arr = new String[x.length];

        for (int i = 0; i < new_arr.length; i++) {
            new_arr[i] = x[i];
        }

        return new_arr;
    }
    public static int[][] copy(int[][] x){
        int[][] new_arr = new int[x.length][x[0].length];

        for (int i = 0; i < new_arr.length; i++) {
            new_arr[i] = copy(x[i]);
        }

        return new_arr;
    }
    public static String[][] copy(String[][] x){
        String[][] new_arr = new String[x.length][x[0].length];

        for (int i = 0; i < new_arr.length; i++) {
            new_arr[i] = copy(x[i]);
        }

        return new_arr;
    }

    public static boolean equals(double[] x, double[] y){
        if(x.length != y.length){
            return false;
        }

        for (int i = 0; i < x.length; i++) {
            if(x[i] != y[i]){
                return false;
            }
        }
        return true;
    }
    public static boolean equals(int[] x, int[] y){
        if(x.length != y.length){
            return false;
        }

        for (int i = 0; i < x.length; i++) {
            if(x[i] != y[i]){
                return false;
            }
        }
        return true;
    }

    public static int random(int min, int max){
        return (int)Math.floor(Math.random() *(max - min + 1) + min);
    }

    public static double round(double x, int step){
        return x - (x%step);
    }

    public static boolean[][] stringMapToBool(String[][] grid){
        boolean[][] walls = new boolean[grid.length][grid[0].length];
        for (int y = 0; y < walls.length; y++) {
            for (int x = 0; x < walls.length; x++) {
                boolean isWall = (grid[y][x] == "X");
                walls[y][x] = isWall;
            }
        }
        return walls;
    }

    public static ArrayList<> getAllChildren()
}
