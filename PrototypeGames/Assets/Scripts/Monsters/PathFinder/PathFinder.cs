using System.Collections.Generic;
using UnityEngine;

namespace Monsters.PathFinder
{
    public class PathFinder
    {
        private GenerationNodes nodeGenerator;

        public PathFinder(GenerationNodes generator)
        {
            nodeGenerator = generator;
        }

        public List<Node> FindPath(Vector2Int startPos, Vector2Int endPos)
        {
            Node startNode = nodeGenerator.GetNode(startPos.x,startPos.y);
            Node endNode = nodeGenerator.GetNode(endPos.x, endPos.y);

            if (startNode == null || endNode == null || !startNode.IsWalkable || !endNode.IsWalkable)
            {
                return null; // Невозможно найти путь
            }

            List<Node> openList = new List<Node> { startNode };
            HashSet<Node> closedList = new HashSet<Node>();

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openList);

                if (currentNode == endNode)
                {
                    return ReconstructPath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (Node neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                        continue;

                    int newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                    {
                        neighbor.GCost = newGCost;
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;

                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
            }

            return null; // Путь не найден
        }

        private Node GetLowestFCostNode(List<Node> nodes)
        {
            Node lowest = nodes[0];
            foreach (Node node in nodes)
            {
                if (node.FCost < lowest.FCost || (node.FCost == lowest.FCost && node.HCost < lowest.HCost))
                    lowest = node;
            }
            return lowest;
        }

        private List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            int[,] directions = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } }; // Вверх, вправо, вниз, влево

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int checkX = node.X + directions[i, 0];
                int checkY = node.Y + directions[i, 1];
                Node neighbor = nodeGenerator.GetNode(checkX, checkY);
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }
            return neighbors;
        }

        private int GetDistance(Node a, Node b)
        {
            int distX = Mathf.Abs(a.X - b.X);
            int distY = Mathf.Abs(a.Y - b.Y);
            return distX + distY; // Манхэттенское расстояние (без диагоналей)
        }

        private List<Node> ReconstructPath(Node endNode)
        {
            List<Node> path = new List<Node>();
            Node current = endNode;
            while (current != null)
            {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}