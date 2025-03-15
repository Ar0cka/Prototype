namespace Monsters.PathFinder
{
    public class Node
    {
        public int X { get; }
        public int Y { get; }
        public bool IsWalkable { get;}
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;
        
        public Node Parent { get; set; }

        public Node(int x, int y, bool isWalkable = true)
        {
            X = x;
            Y = y;
            IsWalkable = isWalkable;
            GCost = 0;
            HCost = 0;
            Parent = null;
        }
    }
}