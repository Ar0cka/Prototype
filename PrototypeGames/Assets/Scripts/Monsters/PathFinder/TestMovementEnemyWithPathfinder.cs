using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Monsters.PathFinder
{
    public class TestMovementEnemyWithPathfinder : MonoBehaviour
    {
        [SerializeField] private GenerationNodes generationNodes;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Transform player;
        [SerializeField] private float moveSpeed = 2f; // Скорость движения врага

        private List<Node> path;
        private int currentPathIndex; // Индекс текущего узла в пути
        private Vector3 currentTargetPosition; // Текущая цель (мировые координаты)

        private PathFinder _pathFinder;

        public bool IsPlayerMove = false;

        private void Awake()
        {
            if (generationNodes == null) return;
            
            generationNodes.InitializeNodes();

            UpdatePath();
        }

        private void UpdateTargetPosition()
        {
            if (path == null || currentPathIndex >= path.Count) return;

            Node targetNode = path[currentPathIndex];
        
            currentTargetPosition = generationNodes.NodeToWorldPosition(targetNode);
        }

        private void FixedUpdate()
        {
            if (path == null || currentPathIndex >= path.Count)
            {
                Debug.LogError($"path not find");
                return;
            }
            
            currentPathIndex++;
            if (currentPathIndex < path.Count)
            {
                UpdateTargetPosition();
            }
            else
            {
                rb2D.velocity = Vector2.zero;
                path = null;
                return;
            }
            
            Vector2 direction = (currentTargetPosition - transform.position).normalized;
            rb2D.MovePosition(rb2D.position + direction * moveSpeed * Time.deltaTime);
        }

        public void UpdatePath()
        {
            if (_pathFinder == null)
            {
                _pathFinder = new PathFinder(generationNodes);
            }
            
            Vector2Int start = generationNodes.WorldToNodePosition(transform.position);
            Vector2Int goal = generationNodes.WorldToNodePosition(player.position);
            
            path = _pathFinder.FindPath(start, goal);
            foreach (var node in path)
            {
                Debug.Log(node.X + "/" + node.Y);
            }
            generationNodes.SetPath(path);
            
            if (path != null && path.Count > 0)
            {
                foreach (Node node in path)
                {
                    Debug.Log($"Path node: ({node.X}, {node.Y})");
                }

                currentPathIndex = 0;
                UpdateTargetPosition();
            }
        }
    }
}