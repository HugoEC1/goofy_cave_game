public class Vision{
    private static final double VISION_BOUNDS = 0.49;
    private static double slope(double y1, double x1, double y2, double x2){
        try {
            double m = (y2 - y1) / (x2 - x1);
            return m;
        } catch (Exception e) {
            throw new ArithmeticException("Slope is undefined");
        }
    }

    private static double pointDistance(double[] p1, double[] p2){
        double a = Math.abs(p1[0] - p2[0]);
        double b = Math.abs(p1[1] - p2[1]);
        double c = Math.sqrt(a*a + b*b);
        return c;
    }

    private static double[] lineIntersection(double m1, double b1, double m2, double b2){
        double x = (b2-b1) / (m1-m2);
        double y = (m1 * x) + b1;
        double[] intersectPoint = {y, x};
        return intersectPoint;
    }

    private static double perpendiculerSlope(double m){
        try {
            double pm = (1 / m) * -1;
            return pm;
        } catch (Exception e) {
            throw new ArithmeticException("Slope is undefined");
        }
    }

    private static int[] closestPoint(int[] p1, int[][] p_arr){
        int[] closest = p_arr[0];
        double closestDistance = pointDistance(Hutil.doubleArr(p1), Hutil.doubleArr(closest));
        for(int[] i : p_arr){
            double d = pointDistance(Hutil.doubleArr(p1), Hutil.doubleArr(i));
            if(d < closestDistance);
            closestDistance = d;
            closest = i;
        }
        return closest;
    }

    private static double yInt(int y, int x, double m){
        return y - (m * x);
    }
    private static double yInt(double y, double x, double m){
        return y - (m * x);
    }

    private static boolean blocked(double[] intersect, int[] blocker, int y, int x){
        double sightlineDistanceToWall = pointDistance(intersect, Hutil.doubleArr(blocker)); // a wall can be seen as a cricle with a raduis of 0.5, enough to touch its neighbors
        boolean sightlineGoesThroughWall = (sightlineDistanceToWall < 0.5);

        int[] spot = {y, x};
        boolean selfBlocking = Hutil.equals(spot, blocker);

        if(sightlineGoesThroughWall && !selfBlocking){
            return true;
        }
        else{
            return false;
        }
    }
    private static boolean blocked(double[] intersect, int[] blocker, double y, double x){
        double sightlineDistanceToWall = pointDistance(intersect, Hutil.doubleArr(blocker)); // a wall can be seen as a cricle with a raduis of 0.5, enough to touch its neighbors
        boolean sightlineGoesThroughWall = (sightlineDistanceToWall < 0.5);

        boolean selfBlocking = ( Hutil.inRange(y, blocker[0]-VISION_BOUNDS, blocker[0]+VISION_BOUNDS) && Hutil.inRange(x, blocker[1]-VISION_BOUNDS, blocker[1]+VISION_BOUNDS) );

        if(sightlineGoesThroughWall && !selfBlocking){
            return true;
        }
        else{
            return false;
        }
    }

    public static boolean canSee(int y, int x, int[] observerPosition, String[][] grid, int RENDER_DISTANCE){
        // checks if there is a sightline to any of the 4 edges
        boolean topEdge = hasSightline(y-VISION_BOUNDS, x, observerPosition, grid, RENDER_DISTANCE);
        boolean bottomEdge = hasSightline(y+VISION_BOUNDS, x, observerPosition, grid, RENDER_DISTANCE);
        boolean rightEdge = hasSightline(y, x+VISION_BOUNDS, observerPosition, grid, RENDER_DISTANCE);
        boolean leftEdge = hasSightline(y, x-VISION_BOUNDS, observerPosition, grid, RENDER_DISTANCE);
        if(topEdge || bottomEdge || rightEdge || leftEdge){
            return true;
        }
        else{
            return false;
        }
    }

    public static boolean hasSightline(double spotY, double spotX, int[] observerPosition, String[][] grid, int RENDER_DISTANCE){
        double[] intersect = {};

        // works based on grade 9 analytical geometry
        // m is slope and b is y-intercept

        boolean m_isUndefined = (spotX - observerPosition[1] == 0);
        if(!m_isUndefined){

            double m = slope(spotY, spotX, observerPosition[0], observerPosition[1]);
            double b = yInt(spotY, spotX, m);
            // check for walls within camera
            for (int renderY = 0; renderY < RENDER_DISTANCE*2+1; renderY++) {
                for (int renderX = 0; renderX < RENDER_DISTANCE*2+1; renderX++) {
                    // convert relative position to grid. same as Main.toGrid()
                    int wallY = (observerPosition[0] - RENDER_DISTANCE) + renderY;
                    int wallX = (observerPosition[1] - RENDER_DISTANCE) + renderX;

                    boolean inbetweenObserverAndSpot = Hutil.inRange(wallY, spotY, observerPosition[0]) && Hutil.inRange(wallX, spotX, observerPosition[1]);
                    if(inbetweenObserverAndSpot){
                        if(grid[wallY][wallX] == "X"){
                            int[] blocker = {wallY, wallX}; // position of wall that MAY be blocking sightline
                        
                            if(m != 0){

                                // line that originating from wall running perpidicular to sightline
                                double m2 = perpendiculerSlope(m);
                                double b2 = yInt(wallY, wallX, m2);

                                intersect = lineIntersection(m, b, m2, b2); // intersection of sightline and line from wall to sightline

                                if(blocked(intersect, blocker, spotY, spotX)){
                                    return false;
                                }

                            }
                            else{
                                intersect = new double[]{spotY, wallX};

                                if(blocked(intersect, blocker, spotY, spotX)){
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

        }
        else{
            // check for walls within camera
            for (int renderY = 0; renderY < RENDER_DISTANCE*2+1; renderY++) {
                for (int renderX = 0; renderX < RENDER_DISTANCE*2+1; renderX++) {
                    // convert relative position to grid. same as Main.toGrid()
                    int wallY = (observerPosition[0] - RENDER_DISTANCE) + renderY;
                    int wallX = (observerPosition[1] - RENDER_DISTANCE) + renderX;

                    boolean inbetweenObserverAndSpot = Hutil.inRange(wallY, spotY, observerPosition[0]) && Hutil.inRange(wallX, spotX, observerPosition[1]);
                    if(inbetweenObserverAndSpot){

                        if(grid[wallY][wallX] == "X"){
                            int[] blocker = {wallY, wallX}; // position of wall that MAY be blocking sightline
                            intersect = new double[]{wallY, spotX}; // intersection of sightline and line from wall to sightline

                            if(blocked(intersect, blocker, spotY, spotX)){
                                return false;
                            }

                        }
                    }
                }
            }
        }

        return true;
    }

    public static boolean hasSightline(int spotY, int spotX, int[] observerPosition, String[][] grid){
        double[] intersect = {};

        boolean m_isUndefined = (spotX - observerPosition[1] == 0);
        if(!m_isUndefined){

            double m = slope(spotY, spotX, observerPosition[0], observerPosition[1]);
            double b = yInt(spotY, spotX, m);
            for (int wallY = 0; wallY < grid.length; wallY++) {
                for (int wallX = 0; wallX < grid.length; wallX++) {
                    if(Hutil.inRange(wallY, spotY, observerPosition[0]) && Hutil.inRange(wallX, spotX, observerPosition[1])){
                        if(grid[wallY][wallX] == "X"){
                            int[] blocker = {wallY, wallX};
                        
                            if(m != 0){

                                double m2 = perpendiculerSlope(m);
                                double b2 = yInt(wallY, wallX, m2);

                                intersect = lineIntersection(m, b, m2, b2);

                                if(blocked(intersect, blocker, spotY, spotX)){
                                    return false;
                                }

                            }
                            else{
                                intersect = new double[]{spotY, wallX};

                                if(blocked(intersect, blocker, spotY, spotX)){
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

        }
        else{
            for (int wallY = 0; wallY < grid.length; wallY++) {
                for (int wallX = 0; wallX < grid.length; wallX++) {
                    if(Hutil.inRange(wallY, spotY, observerPosition[0]) && Hutil.inRange(wallX, spotX, observerPosition[1])){

                        if(grid[wallY][wallX] == "X"){
                            int[] blocker = {wallY, wallX};
                            intersect = new double[]{wallY, spotX};

                            if(blocked(intersect, blocker, spotY, spotX)){
                                return false;
                            }

                        }
                    }
                }
            }
        }

        return true;
    }
}