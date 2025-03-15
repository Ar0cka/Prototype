using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Monsters.PathFinder
{
    public class GenerationNodes : MonoBehaviour
    {
        [SerializeField] private Tilemap groundMap;
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private List<TileBase> obstacleTileBase;
        [SerializeField] private int nodeStep = 1;

        private Node[,] _nodes;
        private List<Node> currentPath;

        public void InitializeNodes()
        {
            Debug.Log("Nodes generation");
            GeneraNodes();
        }

        private void GeneraNodes()
        {
            groundMap.CompressBounds();
            BoundsInt boundsInt = groundMap.cellBounds;

            int weight = boundsInt.size.x;
            int height = boundsInt.size.y;

            _nodes = new Node[weight, height];
            
            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int tilePos =
                        new Vector3Int(boundsInt.xMin + x * nodeStep, boundsInt.yMin + y * nodeStep, 0);

                    bool isWalkable = IsTileWalkable(tilePos);
                    
                    _nodes[x, y] = new Node(x, y, isWalkable);
                }
            }
        }
        
        public Vector3 NodeToWorldPosition(Node node)
        {
            Vector3Int tilePos = new Vector3Int(
                groundMap.cellBounds.xMin + node.X * nodeStep,
                groundMap.cellBounds.yMin + node.Y * nodeStep,
                0
            );
            return groundMap.CellToWorld(tilePos) + groundMap.cellSize * 0.5f; // Центр тайла
        }
        
        private bool IsTileWalkable(Vector3Int position)
        {
            bool hasGround = groundMap.HasTile(position);

            TileBase tileAtPosition = obstacleMap.GetTile(position);
            
            bool hasObstacle = tileAtPosition != null && obstacleTileBase.Contains(tileAtPosition);

            return hasGround && !hasObstacle;
        }

        public Node GetNode(int x, int y)
        {
            if (x >= 0 && x < _nodes.GetLength(0) && y >= 0 && y < _nodes.GetLength(1))
            {
                return _nodes[x, y];
            }
            return null;
        }

        // Опционально: метод для перевода мировых координат в координаты узлов
        public Vector2Int WorldToNodePosition(Vector3 worldPos)
        {
            Vector3Int cellPos = groundMap.WorldToCell(worldPos);
            Vector2Int nodePos = new Vector2Int(
                (cellPos.x - groundMap.cellBounds.xMin) / nodeStep,
                (cellPos.y - groundMap.cellBounds.yMin) / nodeStep
            );
            return nodePos;
        }
        public void SetPath(List<Node> path)
        {
            currentPath = path;
        }
        private void OnDrawGizmos()
        {
            if (_nodes == null) return;

            for (int x = 0; x < _nodes.GetLength(0); x++)
            {
                for (int y = 0; y < _nodes.GetLength(1); y++)
                {
                    Node node = _nodes[x, y];
                    if (node != null)
                    {
                        // Точно вычисляем мировую позицию центра тайла
                        Vector3Int tilePos = new Vector3Int(
                            groundMap.cellBounds.xMin + x * nodeStep,
                            groundMap.cellBounds.yMin + y * nodeStep,
                            0
                        );
                        Vector3 worldPos = groundMap.CellToWorld(tilePos) + groundMap.cellSize * 0.5f; // Центр тайла

                        // Задаём цвет: зелёный для проходимых, красный для непроходимых
                        Gizmos.color = node.IsWalkable ? Color.green : Color.red;
                        Gizmos.DrawSphere(worldPos, 0.2f); // Рисуем сферу радиусом 0.2
                    }
                }
            }   
        }
    }
}