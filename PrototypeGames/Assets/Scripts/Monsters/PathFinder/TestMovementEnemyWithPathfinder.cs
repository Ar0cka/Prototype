using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.PathFinder
{
    public class TestMovementEnemyWithPathfinder : MonoBehaviour
    {
        [SerializeField] private GenerationNodes generationNodes;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Transform player;
        [SerializeField] private float moveSpeed = 2f; // Скорость движения врага
        [SerializeField] private float reachDistance = 0.1f; // Дистанция, при которой считаем, что узел достигнут

        private List<Node> path;
        private int currentPathIndex; // Индекс текущего узла в пути
        private Vector3 currentTargetPosition; // Текущая цель (мировые координаты)

        private void Start()
        {
            if (generationNodes == null) return;

            // Предполагаю, что InitializeNodes заменил GeneraNodes
            generationNodes.InitializeNodes();

            PathFinder pathFinder = new PathFinder(generationNodes);
            Vector2Int start = generationNodes.WorldToNodePosition(transform.position);
            Vector2Int goal = generationNodes.WorldToNodePosition(player.position);

            Debug.Log($"{start.x}/{start.y}, {goal.x}/{goal.y}");

            path = pathFinder.FindPath(start, goal);
            generationNodes.SetPath(path);

            if (path != null && path.Count > 0)
            {
                foreach (Node node in path)
                {
                    Debug.Log($"Path node: ({node.X}, {node.Y})");
                }
                currentPathIndex = 0; // Начинаем с первого узла
                UpdateTargetPosition(); // Устанавливаем первую цель
            }
        }

        private void UpdateTargetPosition()
        {
            if (path == null || currentPathIndex >= path.Count) return;

            Node targetNode = path[currentPathIndex];
            // Преобразуем координаты узла в мировые координаты
            currentTargetPosition = generationNodes.NodeToWorldPosition(targetNode);
        }

        private void FixedUpdate()
        {
            if (path == null || currentPathIndex >= path.Count) return;

            // Проверяем расстояние до текущей цели
            Vector2 currentPos = transform.position;
            float distanceToTarget = Vector2.Distance(currentPos, currentTargetPosition);

            if (distanceToTarget <= reachDistance)
            {
                // Достигли текущего узла, переходим к следующему
                currentPathIndex++;
                if (currentPathIndex < path.Count)
                {
                    UpdateTargetPosition();
                }
                else
                {
                    // Достигли конца пути
                    rb2D.velocity = Vector2.zero;
                    path = null; // Очищаем путь, если достигли цели
                    return;
                }
            }

            // Двигаемся к текущей цели
            Vector2 direction = (currentTargetPosition - transform.position).normalized;
            rb2D.velocity = direction * moveSpeed;
        }

        // Опционально: обновление пути при движении игрока
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Пересчитываем путь при нажатии пробела
            {
                PathFinder pathFinder = new PathFinder(generationNodes);
                Vector2Int start = generationNodes.WorldToNodePosition(transform.position);
                Vector2Int goal = generationNodes.WorldToNodePosition(player.position);

                path = pathFinder.FindPath(start, goal);
                generationNodes.SetPath(path);

                if (path != null && path.Count > 0)
                {
                    currentPathIndex = 0;
                    UpdateTargetPosition();
                }
            }
        }
    }
}