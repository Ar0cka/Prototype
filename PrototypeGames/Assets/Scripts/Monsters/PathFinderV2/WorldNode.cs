

using UnityEngine;

namespace Monsters.PathFinderV2
{
    public class WorldNode
    {
        public Vector3 WorldPosition { get; }
        public bool IsWalkable { get; }

        public int gridX, gridY;
        
        public int GCost, HCost;

        public int FCost => GCost + HCost;

        public WorldNode parent;

        public WorldNode(Vector3 worldPosition, bool isWalkable, int _gridX, int _gridY)
        {
            WorldPosition = worldPosition;
            IsWalkable = isWalkable;
            gridX = _gridX;
            gridY = _gridY;

            parent = null;
        }
    }
}