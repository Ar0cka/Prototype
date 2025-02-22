using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class AttackPlayer : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        [SerializeField] private List<String> namesTriggers;
        
        [SerializeField] private float delayExitFromCombo;
        [SerializeField] private float lastAnimationTime = 0.4f;
        [SerializeField] private int maxComboAttackCount = 2;

        private const int MouseButton = 0;
        
        private int _countAttack;
        private float lastClicked;

        private void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();
                
            if (animator == null || namesTriggers == null)
            {
                Debug.LogError("Missing components: " +
                               (animator == null ? " Animator" : "") +
                               (namesTriggers == null ? " Triggers" : ""));
                
                enabled = false;
            }
        }

        private void Update()
        {
            if (Time.time - lastClicked > delayExitFromCombo) EndCombo();
            
            if (Input.GetMouseButtonDown(MouseButton) && _countAttack < maxComboAttackCount)
            {
                lastClicked = Time.time;
                Attack();
            }
        }
        
        private void Attack()
        {
            animator.SetTrigger(namesTriggers[_countAttack]);
            
            _countAttack++;

            if (_countAttack >= maxComboAttackCount)
            {
                StartCoroutine(ExitFromAttacks());
            }
        }

        private IEnumerator ExitFromAttacks()
        {
            yield return new WaitForSeconds(lastAnimationTime);
        }
        
        private void EndCombo()
        {
            for (int i = 0; i < namesTriggers.Count; i++)
            {
                animator.ResetTrigger(namesTriggers[i]);
            }

            _countAttack = 0;
            lastClicked = 0;
        }
    }
}