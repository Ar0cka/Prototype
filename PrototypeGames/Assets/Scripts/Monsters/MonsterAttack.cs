using System;
using System.Collections;
using ScriptableObjects.Monsters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Monsters
{
    public class MonsterAttack : MonoBehaviour
    {
        [SerializeField] private MonsterAbstractData monsterData;
        
        [Header("Delays")]
        [SerializeField] private float delayForAttack = 1f;
        [SerializeField] private float delayForSwitchBehavior = 2f;
        [SerializeField] private Animator animator;
        
        [Header("Count attack")]
        [SerializeField] private int countAnimationAttack = 1;

        [Header("Collider settings")]
        [SerializeField] private CircleCollider2D attackTriggerCollider;
        [SerializeField] private float radiusCollider = 2f;
        
        private float _cooldownAttack;
        private float _delayForAttack;
        private bool canAttack;

        private void Awake()
        {
            if (animator == null) animator = GetComponentInParent<Animator>();
            if (attackTriggerCollider == null) attackTriggerCollider = GetComponent<CircleCollider2D>();

            if (animator == null || attackTriggerCollider == null || monsterData == null) 
            {
                Debug.LogError("Not founded components");
                enabled = false;
                return;
            }

            attackTriggerCollider.radius = radiusCollider;
            attackTriggerCollider.isTrigger = true;

            _cooldownAttack = monsterData.CooldownAttack;
        }

        private void Update()
        {
            if (canAttack)
            {
                _delayForAttack -= Time.deltaTime;
                
                if (_delayForAttack < 0)
                {
                    if (_cooldownAttack < 0 )
                    {
                        Debug.Log("Player stay in attack zone");
                       
                        TakeMethod();
                        
                    }
                    else
                    {
                        _cooldownAttack -= Time.deltaTime;
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canAttack = true;
            }
        }

        private void TakeMethod()
        {
            if (countAnimationAttack == 1)
            {
                AttackPlayer();
            }
            else
            {
                ComboAttack();
            }
        }
        
        private void AttackPlayer() //Активация анимации атаки 
        {
            animator.SetTrigger("Attack");
            MonsterGlobalValues.Instance.SwitchMonsterBehavior(true);
            _cooldownAttack = monsterData.CooldownAttack;
        }

        private void ComboAttack()
        {
            
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && MonsterGlobalValues.Instance.IsMonsterInFight)
            {
                canAttack = false;
                _delayForAttack = delayForAttack;
                StartCoroutine(DelayForSwitchBehavior());
            }
        }

        private IEnumerator DelayForSwitchBehavior()
        {
            yield return new WaitForSeconds(delayForSwitchBehavior);
            MonsterGlobalValues.Instance.SwitchMonsterBehavior(false);
        }
    }
}