using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monsters.PathFinderV2
{
    public class PathFinding : MonoBehaviour
    {
        [SerializeField] private GridGen gridGen;

        [SerializeField] private Transform seeker;
        [SerializeField] private Transform targetPos;

        private void Awake()
        {
            if (gridGen == null) gridGen = FindObjectOfType<GridGen>();
        }

        private void Update()
        {
            FindPath(seeker.position, targetPos.position);
        }

        private void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            WorldNode startNode = gridGen.NodeFromWorldPosition(startPos);
            WorldNode targetNode = gridGen.NodeFromWorldPosition(targetPos);

            List<WorldNode> openList = new List<WorldNode>();
            HashSet<WorldNode> closeNode = new HashSet<WorldNode>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                WorldNode currentNode = openList[0];
                
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost &&
                        openList[i].HCost < currentNode.HCost)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closeNode.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (var neighbours in gridGen.GetNeighbours(currentNode))
                {
                    if (!neighbours.IsWalkable || closeNode.Contains(neighbours))
                    {
                        continue;
                    }
                    
                    
                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbours);
                    if (newMovementCostToNeighbour < neighbours.GCost || !openList.Contains(neighbours))
                    {
                        neighbours.GCost = newMovementCostToNeighbour;
                        neighbours.HCost = GetDistance(neighbours, targetNode);
                        neighbours.parent = currentNode;

                        if (!openList.Contains(neighbours))
                        {
                            openList.Add(neighbours);
                        }
                    }
                }
            }
        }

        private void RetracePath(WorldNode startNode, WorldNode endNode)
        {
            Debug.Log("retracePath");
            
            List<WorldNode> path = new List<WorldNode>();
            WorldNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            gridGen.path = path;
        }

        private int GetDistance(WorldNode nodeA, WorldNode nodeB)
        {
            int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (distX > distY) return 14 * distY + 10 * (distX - distY);

            return 14 * distX + 10 * (distY - distX);
        }
    }
}