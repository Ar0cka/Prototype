using System;
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
        
        [Header("Transform")] 
        [SerializeField] private float correctCoefByY= 0.4f; 
        
        [Header("Collider")]
        [SerializeField] private CircleCollider2D followRadius;
        [SerializeField] private float radiusCollider;

        [Header("RaycastSettings")] 
        [SerializeField] private Transform raycastPosition;
        [SerializeField] private float distanceRaycast;
        [SerializeField] private float stopDistance;
        
        private Transform _playerTransform;
        

        private void Awake()
        {
            if (rb2D == null) rb2D = GetComponentInParent<Rigidbody2D>();
            if (animator == null) animator = GetComponentInParent<Animator>();
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();

            if (rb2D == null || animator == null || spriteRenderer == null || stopRadius == null)
            {
                Debug.LogError((rb2D == null ? "Rigidbody2D" : "") +
                               (animator == null ? "Animator" : "") +
                               (spriteRenderer == null ? "Sprite renderer" : "") +
                               (stopRadius == null ? "Stop radius" : ""));
                enabled = false;
                Destroy(gameObject);
            }
            
            followRadius.radius = radiusCollider;
            followRadius.isTrigger = true;
        }

        private void Update()
        {
            if (CanUseDefaultMove() && !CheckPlayerInYVector())
            {
                //Нужно брать постоянное местоположение игрока
                MoveMonster();
            }
            else if (CheckPlayerInYVector())
            {
                
            }
            
            SetRunAnimation();
        }

        #region MoveEnemy

        private void MoveMonster()
        {
            Vector2 moveDirection = (_playerTransform.position - transform.position).normalized; //Добавить отклонения во время ходьбы чтобы не было вида, что он идет по прямой
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

        private void SideStepPlayer()
        {
            if (_playerTransform.position.magnitude > 0.01f)
            {
                //Изменения x противоположно движению игрока
            }
            else
            {
                //Просто монстр идет вправа 
            }
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

        private bool CheckPlayerInYVector()
        {
            RaycastHit2D raycastHit2DDown = Physics2D.Raycast(raycastPosition.position, Vector2.down, distanceRaycast,
                LayerMask.GetMask("Player"));
            
            RaycastHit2D raycastHit2DUp = Physics2D.Raycast(raycastPosition.position, Vector2.up, distanceRaycast,
                LayerMask.GetMask("Player"));

            if (raycastHit2DDown.collider.CompareTag("Player") && raycastHit2DDown.distance <= stopDistance)
            {
                Debug.Log("Игрок находится снизу");
                return true;
            }

            if (raycastHit2DUp.collider.CompareTag("Player") && raycastHit2DUp.distance <= stopDistance)
            {
                Debug.Log("Игрок находится сверху");
                return true;
            }

            return false;
        }

        private bool CanUseDefaultMove()
        {
            return MonsterGlobalValues.Instance.IsSeePlayer && !stopRadius.IsStop;
        }

        #endregion
    }
}