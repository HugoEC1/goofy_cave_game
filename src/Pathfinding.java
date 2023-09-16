import java.util.ArrayList;

public class Pathfinding {
    private ArrayList<Node> openList;

    Node[][] nodes;

    Node startNode;
    Node endNode;

    public int[][] findPath(int[] startPosition, int[] endPosition, boolean[][] grid){

        this.startNode = new Node(startPosition[0], startPosition[1]);
        startNode.GToZero();
        this.endNode = new Node(endPosition[0], endPosition[1]);
        startNode.setH(endNode);

        nodes = new Node[grid.length][grid.length];
        for (int y = 0; y < grid.length; y++) {
            for (int x = 0; x < grid.length; x++) {
                nodes[y][x] = new Node(y, x);
            }
        }
        nodes[startNode.y][startNode.x] = startNode;
        nodes[endNode.y][endNode.x] = endNode;

        openList = new ArrayList<>();

        openList.add(startNode);

        if(startNode.y == endNode.y && startNode.x == endNode.x){
            return null;
        }

        while(!openList.isEmpty()){
            Node current = cheapestNode();

            openList.remove(current);
            current.setClosed();

            if(current.equals(endNode)){
                endNode.setParentNode(current.getParentNode());
                return retracePath();
            }

            createNeighbors(current, grid);
        }

        return null; // could not find path
    }

    private void createNeighbors(Node current, boolean[][] grid){
        for(int y = current.y-1; y <= current.y+1; y++){
            for(int x = current.x-1; x <= current.x+1; x++){
                boolean notCurrent = (y != current.y || x != current.x);
                boolean onGrid = (Hutil.inRange(y, grid.length-1, 0) && Hutil.inRange(x, grid[0].length-1, 0));
                if(notCurrent && onGrid){
                    boolean wall = grid[y][x];
                    Node neighborNode = nodes[y][x];
                    if(!wall && neighborNode.closed == false){

                        neighborNode.setG(current, AdjacentTravelCost(current, neighborNode));
                        neighborNode.setH(endNode);

                        if(!openList.contains(neighborNode)){
                            openList.add(neighborNode);
                        }
                    }
                }
            }
        }
    }

    private Node cheapestNode(){
        Node cheapest = openList.get(0);
        for (Node node : openList) {
            if(cheapest.getCost() > node.getCost()){
                cheapest = node;
            }
            if(cheapest.getCost() == node.getCost()){
                if(node.getH() < cheapest.getH()){
                    cheapest = node;
                }
            }
        }
        return cheapest;
    }

    private int AdjacentTravelCost(Node node1, Node node2){
        double a = node1.y - node2.y;
        double b = node1.x - node2.x;
        double distance = Math.sqrt( a*a + b*b );
        return (int) (distance * 10);
    }

    private int[][] retracePath(){
        ArrayList<Node> path = new ArrayList<>();
        path.add(endNode);

        Node current = endNode.getParentNode();
        while(current.getParentNode() != null){
            path.add(current);
            current = current.getParentNode();
        }

        int[][] pathArr = new int[path.size()][2];

        for (int i = 0; i < pathArr.length; i++) {
            pathArr[i] = new int[]{path.get(i).y, path.get(i).x};
        }

        return pathArr;
    }

    class Node{
        public int y, x;
        private Node parentNode;

        boolean closed;

        private int g, h;

        public Node(int y, int x){
            this.y = y;
            this.x = x;
            this.g = 2147483647;
            this.parentNode = null;
            this.closed = false;
        }

        public boolean equals(Node node){
            if(this.y == node.y && this.x == node.x){
                return true;
            }
            else{
                return false;
            }
        }

        public int getCost() {
            return g + h;
        }

        public void setH(Node endNode){
            double a = y - endNode.y;
            double b = x - endNode.x;
            double distance = Math.sqrt(a*a + b*b);
            h = (int) (distance * 10);
        }

        public int getH() {
            return h;
        }

        public void setG(Node neighborNode, int g){
            // if the newly opened adjecent node can offer a faster path, take it.
            int neighborToThis = neighborNode.getG() + g;
            boolean fasterPath = (neighborToThis < this.g);
            if(fasterPath){
                this.g = neighborToThis;
                setParentNode(neighborNode);
            }
        }

        public void GToZero(){
            this.g = 0;
        }

        public int getG() {
            return g;
        }

        public Node getParentNode() {
            return parentNode;
        }
        public void setParentNode(Node parentNode) {
            this.parentNode = parentNode;
        }

        public void setClosed(){
            this.closed = true;
        }
    }
}
