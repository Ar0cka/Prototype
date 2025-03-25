using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace Monsters.PathFinderV2
{
    public class GridGen : MonoBehaviour
    {
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private LayerMask unwalkableMask;
        [SerializeField] private float nodeRadius;
        [SerializeField] private Transform playerPosition;
        

        public WorldNode[,] grid;

        private float diameterNode => nodeRadius * 2;
        private int gridSizeX, gridSizeY;


        private void Start()
        {
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / diameterNode);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / diameterNode);
            CreateNode();
        }

        private void CreateNode()
        {
            grid = new WorldNode[gridSizeX, gridSizeY];
            Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 -
                                      Vector2.up * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector2.right * (x * diameterNode + nodeRadius) +
                                         Vector2.up * (y * diameterNode + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    grid[x, y] = new WorldNode(worldPoint, walkable, x, y);
                }
            }
        }

        public List<WorldNode> GetNeighbours(WorldNode node)
        {
            List<WorldNode> neighbours = new List<WorldNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x==0 && y==0) continue;
                    
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
            
            return neighbours;
        }

        public WorldNode NodeFromWorldPosition(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x,y];
        }

        public List<WorldNode> path;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

            if (grid != null)
            {
                WorldNode playerNode = playerPosition != null ? NodeFromWorldPosition(playerPosition.position) : null;

                foreach (WorldNode node in grid)
                {
                    Gizmos.color = node.IsWalkable ? Color.green : Color.red;

                    if (playerNode != null && node == playerNode)
                    {
                        Gizmos.color = Color.yellow;
                    }

                    if (path != null)
                    {
                        if (path.Contains(node))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                    else
                    {
                        Debug.LogError("not path");
                    }
                    Gizmos.DrawSphere(node.WorldPosition, 0.2f);
                }
            }
        }
    }
}