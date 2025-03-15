using System;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjects.Monsters;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class MonsterController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CheckStopRadius stopRadius;

        [SerializeField] private float speed = 3f;
        [SerializeField] private float moveInX = 2;
        
        [Header("Transform")] 
        [SerializeField] private float correctCoefByY= 0.4f; 
        
        [Header("Collider")]
        [SerializeField] private CircleCollider2D followRadius;
        [SerializeField] private float radiusCollider;

        [Header("CheckPlayerPosition")] 
        [SerializeField] private CheckPlayerPosition playerPosition;
        
        private Transform _playerTransform;

        private PlayerMovementController _playerMovementController;

        private void Awake()
        {
            if (rb2D == null) rb2D = GetComponentInParent<Rigidbody2D>();
            if (animator == null) animator = GetComponentInParent<Animator>();
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();
            if (playerPosition == null) playerPosition = GetComponentInChildren<CheckPlayerPosition>();

            if (rb2D == null || animator == null || spriteRenderer == null || stopRadius == null || playerPosition == null)
            {
                Debug.LogError((rb2D == null ? "Rigidbody2D" : "") +
                               (animator == null ? "Animator" : "") +
                               (spriteRenderer == null ? "Sprite renderer" : "") +
                               (stopRadius == null ? "Stop radius" : "") + 
                               (playerPosition == null ? "player position" : ""));
                enabled = false;
                Destroy(gameObject);
            }
            
            followRadius.radius = radiusCollider;
            followRadius.isTrigger = true;
        }

        private void Update()
        {
            if (stopRadius.IsStop)
            {
                spriteRenderer.flipX = _playerTransform.position.x < transform.position.x;
            }
            
            if (CanUseDefaultMove() && !playerPosition.NeedWalkAroundPlayer)
            {
                Vector2 moveDirection = (_playerTransform.position - transform.position).normalized; //Добавить отклонения во время ходьбы чтобы не было вида, что он идет по прямой
                spriteRenderer.flipX = moveDirection.x < 0;
                MoveMonster(moveDirection);
            }
            else if (playerPosition.NeedWalkAroundPlayer)
            {
                MoveMonster(SideStepPlayer());
            }
            
            SetRunAnimation();
        }

        #region MoveEnemy

        private void MoveMonster(Vector2 moveDirection)
        {
            CheckYPosition(ref moveDirection);    
            rb2D.MovePosition(rb2D.position + moveDirection * speed * Time.deltaTime);
        }
        
        private void CheckYPosition(ref Vector2 moveDirection)
        {
            if (_playerTransform.position.y > transform.position.y)
            {
                moveDirection.y += correctCoefByY;
                
            }
            else if (_playerTransform.position.y < moveDirection.y)
            {
                moveDirection.y -= correctCoefByY;
            }
        }

        private Vector2 SideStepPlayer()
        {
            if (_playerTransform == null) return Vector2.zero;

            if (_playerTransform.position.x > transform.position.x)
            {
                return new Vector2(-moveInX, 0).normalized;
            }

            return new Vector2(moveInX, 0).normalized;
        }

        #endregion
        
        private void SetRunAnimation()
        {
            if (!stopRadius.IsStop)
            {
                animator.SetBool("Run", MonsterGlobalValues.Instance.IsSeePlayer);
            }
            else
            {
                animator.SetBool("Run", false);
            }
        }

        #region Triggers

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                MonsterGlobalValues.Instance.SwitchSeePlayer(true);
                _playerMovementController = other.GetComponent<PlayerMovementController>();
                _playerTransform = other.transform;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerTransform = other.transform;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                MonsterGlobalValues.Instance.SwitchSeePlayer(false); // Нужно продумать лучше метод связанный с выходом, чтобы дать небольшую задержку для выхода из боя
            }
        }

        #endregion
       

        #region RaycastCheckers

        private bool CanUseDefaultMove()
        {
            return MonsterGlobalValues.Instance.IsSeePlayer && !stopRadius.IsStop;
        }
        
        #endregion
    }
}