using System;
using System.Collections;
using DefaultNamespace;
using ScriptableObjects.Monsters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Monsters
{
    public class MonsterAttack : MonoBehaviour
    {
        [SerializeField] private MonsterAbstractData monsterData;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CheckStopRadius stopRadius;
        
        [Header("Delays")]
        [SerializeField] private float delayForAttack = 1f;
        [SerializeField] private float delayForSwitchBehavior = 2f;
        [SerializeField] private Animator animator;
        
        [Header("Count attack")]
        [SerializeField] private int countAnimationAttack = 1;

        private float _cooldownAttack;
        private float _delayForAttack;
        private bool canAttack;

        private void Awake()
        {
            if (animator == null) animator = GetComponentInParent<Animator>();
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();
            if (stopRadius == null) stopRadius = GetComponentInChildren<CheckStopRadius>();

            if (animator == null || stopRadius == null || monsterData == null || spriteRenderer == null) 
            {
                Debug.LogError("Not found components");
                enabled = false;
                return;
            }

            _cooldownAttack = monsterData.CooldownAttack;
            _delayForAttack = delayForAttack;
        }

        private void Update()
        {
            // Проверяем, остановился ли монстр перед игроком
            canAttack = stopRadius.IsStop;

            if (canAttack)
            {
                _delayForAttack -= Time.deltaTime;
                
                if (_delayForAttack < 0)
                {
                    if (_cooldownAttack < 0)
                    {
                        Debug.Log("Attacking player!");
                        TakeMethod();
                    }
                    else
                    {
                        _cooldownAttack -= Time.deltaTime;
                    }
                }
            }
            else
            {
                // Сбрасываем задержку и переключаем поведение, если монстр вышел из боя
                if (MonsterGlobalValues.Instance.IsMonsterInFight)
                {
                    _delayForAttack = delayForAttack;
                    StartCoroutine(DelayForSwitchBehavior());
                }
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
        
        private void AttackPlayer()
        {
            animator.SetTrigger("Attack");
            MonsterGlobalValues.Instance.SwitchMonsterBehavior(true);
            _cooldownAttack = monsterData.CooldownAttack;
        }

        private void ComboAttack()
        {
            // Логика комбо-атаки, если нужна
        }

        private IEnumerator DelayForSwitchBehavior()
        {
            yield return new WaitForSeconds(delayForSwitchBehavior);
            MonsterGlobalValues.Instance.SwitchMonsterBehavior(false);
        }
    }
}
