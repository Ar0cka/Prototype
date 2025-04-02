using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.PathFinder
{
    public class PathFinding : MonoBehaviour
    {
        [SerializeField] private GridGen gridCreater;

        [SerializeField] private Transform currentPos;
        [SerializeField] private Transform seeker;

        private List<Node> _path;

        private void Update()
        {
            if(Input.GetButtonDown("Jump"))
                FindPath(currentPos.position, seeker.position);
        }

        private void FindPath(Vector2 startPos, Vector2 goalPos)
        {
            Node startNode = gridCreater.NodeFromWorldPosition(startPos);
            Node goalNode = gridCreater.NodeFromWorldPosition(goalPos);

            Debug.Log($"startPos = {startPos.x}/{startPos.y} and startNode = {startNode.gridX}/{startNode.gridY} " +
                      $"\n endPos = {goalPos.x}/{goalPos.y} amd goalNode = {goalNode.gridX}/{goalNode.gridY}");
            
            List<Node> openList = new List<Node>();
            HashSet<Node> closeList = new HashSet<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost &&
                        openList[i].HCost < currentNode.HCost)
                    {
                        currentNode = openList[i];
                    }
                }
                
                openList.Remove(currentNode);
                closeList.Add(currentNode);

                if (currentNode == goalNode) //Не находит нужный путь 
                {
                    RequestPath(startNode, goalNode);
                    return;
                }

                foreach (var neighbours in gridCreater.GetNeighbour(currentNode))
                {
                    if (!neighbours.isWalkable || closeList.Contains(neighbours))
                    { 
                        continue;
                    }
                    
                    
                    int newMovementCost = currentNode.GCost + GetDistance(currentNode, neighbours);

                    if (newMovementCost < neighbours.GCost || !openList.Contains(neighbours))
                    {
                        neighbours.GCost = newMovementCost;
                        neighbours.HCost = GetDistance(neighbours, goalNode);
                        neighbours.parent = currentNode;
                        
                        if (!openList.Contains(neighbours))
                        {
                            openList.Add(neighbours);
                        }
                    }
                }
            }
        }

        private void RequestPath(Node startNode, Node endNode)
        {
            Debug.Log("requestPath");
            List<Node> currentPath = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                currentPath.Add(currentNode);
                currentNode = currentNode.parent;
            }
            
            _path = currentPath;
            
        }

        private int GetDistance(Node nodeA, Node NodeB)
        {
            int distX = Mathf.Abs(nodeA.gridX - NodeB.gridX);
            int distY = Mathf.Abs(nodeA.gridY - NodeB.gridY);

            if (distX > distY) return 14 * distY + 10 * (distX - distY);  
  
            return 14 * distX + 10 * (distY - distX);  
        }

        private void OnDrawGizmos()
        {
            if (_path != null)
            {
                foreach (var p in _path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(p.worldPosition, 0.2f);
                }
            }
        }
    }
}