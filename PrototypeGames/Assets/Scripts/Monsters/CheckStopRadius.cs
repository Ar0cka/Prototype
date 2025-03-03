using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CheckStopRadius : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider2D stopCircle;

        public bool IsStop { get; private set; }

        private void Awake()
        {
            stopCircle.isTrigger = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                IsStop = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                IsStop = false;
            }
        }
    }
}