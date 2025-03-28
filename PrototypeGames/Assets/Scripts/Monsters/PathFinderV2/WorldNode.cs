

using UnityEngine;

namespace Monsters.PathFinderV2
{
    public class WorldNode : IHeapItem<WorldNode>
    {
        public Vector3 WorldPosition { get; }
        public bool IsWalkable { get; }

        public int gridX, gridY;
        
        public int GCost, HCost;

        public int FCost => GCost + HCost;

        public WorldNode parent;

        private int heapIdnex;

        public WorldNode(Vector3 worldPosition, bool isWalkable, int _gridX, int _gridY)
        {
            WorldPosition = worldPosition;
            IsWalkable = isWalkable;
            gridX = _gridX;
            gridY = _gridY;

            parent = null;
        }

        public int HeapIndex
        {
            get
            {
                return heapIdnex;
            }
            set
            {
                heapIdnex = value;
            }
        }

        public int CompareTo(WorldNode nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }

            return -compare;
        }
    }
}