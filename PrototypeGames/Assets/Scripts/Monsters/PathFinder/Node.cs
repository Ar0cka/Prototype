using UnityEngine;

namespace Monsters.PathFinder
{
    public class Node
    {
        public Vector2 worldPosition;
        
        public int gridX;
        public int gridY;

        public bool isWalkable;
        
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public Node parent;

        public Node(int x, int y, bool IsWalkable, Vector2 _worldPosition)
        {
            gridX = x;
            gridY = y;
            isWalkable = IsWalkable;
            parent = null;
            worldPosition = _worldPosition;
        }
    }
}