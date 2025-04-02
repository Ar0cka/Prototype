using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Monsters.PathFinder
{
    public class GridGen : MonoBehaviour
    {
        [SerializeField] private Vector2 gridWorldSize; 
        [SerializeField] private LayerMask unwalkable;
        [SerializeField] private float nodeRadius = 0.5f;
        [SerializeField] private float overlapRadius = 0.3f;

        private float nodeDiameter;
        private int gridSizeX, gridSizeY;
        private Node[,] grid;
        
        public void InitializeGridCreater()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateNodes();
        }

        private void CreateNodes()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 -
                                      Vector2.up * gridWorldSize.y / 2;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 waypoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) +
                                       Vector2.up * (y * nodeDiameter + nodeRadius);

                    bool walkable = !Physics2D.OverlapCircle(waypoint, overlapRadius, unwalkable);
                    grid[x, y] = new Node(x, y, walkable, waypoint);
                }
            }
        }

        public Node NodeFromWorldPosition(Vector2 currentPosition)
        {
            float perX = (currentPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float perY = (currentPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

            perX = Mathf.Clamp01(perX);
            perY = Mathf.Clamp01(perY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * perX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * perY);
            
            return grid[x, y];
        }

        public Vector2 WorldPositionFromNode(int x, int y)
        {
            float cellSizeX = gridWorldSize.x / gridSizeX;
            float cellSizeY = gridWorldSize.y / gridSizeY;

            Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right+ gridWorldSize / 2 - Vector2.up * gridWorldSize.y / 2;

            float worldX = worldBottomLeft.x + (x + 0.5f) * cellSizeX;
            float worldY = worldBottomLeft.y + (y + 0.5f) * cellSizeY;

            return new Vector2(worldX, worldY);
        }
        
        public List<Node> GetNeighbour(Node node)
        {
            List<Node> neighbours = new List<Node>();

            int[] dx = { 1, -1, 0, 0};
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int checkX = node.gridX + dx[i];
                int checkY = node.gridY + dy[i];

                if (checkX < gridSizeX && checkY < gridSizeY && checkX >= 0 && checkY >= 0)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
            
            return neighbours;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
            
            if (grid != null)
            {
                foreach (var n in grid)
                {
                    Gizmos.color = n.isWalkable ? Color.green : Color.red;
                    Gizmos.DrawSphere(n.worldPosition, 0.2f); 
                } 
            }
        }
    }
}