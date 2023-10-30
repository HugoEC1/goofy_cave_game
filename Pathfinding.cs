namespace CaveGame;

public class Pathfinding
{
    private List<Node> Open;

    private List<Node> Closed;
    
    private Node StartNode;
    private Node EndNode;

    public int[,] FindPath(int startY, int startX, int endY, int endX, int hWeight)
    {
        StartNode = new Node(startY, startX, hWeight) { G = 0 };
        StartNode.SetH(endY, endX);
        
        Open.Add(StartNode);

        while (Open.Count != 0)
        {
            var currentNode = GetCheapest();
            Open.Remove(currentNode);
            Closed.Add(currentNode);

            if (currentNode.Y == endY && currentNode.X == endX)
            {
                return null; // make this return path
            }
            
            for (var y = 0; y < 2; y++)
            {
                for (var x = 0; x < 2; x++)
                {
                    if (y == 0 && x == 0)
                    {
                        continue;
                    }
                    
                }
            }
        }
    }

    private Node GetCheapest()
    {
        var cheapest = Open[0];
        foreach (var node in Open)
        {
            if (cheapest.GetCost() > node.GetCost())
            {
                cheapest = node;
            }

            if (cheapest.GetCost() == node.GetCost())
            {
                if (cheapest.H > node.H)
                {
                    cheapest = node;
                }
            }
        }

        return cheapest;
    }
    
    private class Node
    {
        public int Y, X;
        public int G, H;
        public int HWeight;
        public Node? ParentNode = null;

        public Node(int y, int x, int hWeight)
        {
            Y = y;
            X = x;
            HWeight = hWeight;
            G = int.MaxValue;
        }

        public int GetCost()
        {
            return G + HWeight * H;
        }

        public void SetH(int y, int x)
        {
            var dY = Y - y;
            var dX = X - x;
            H = dY * dY + dX * dX;
        }
    }
}