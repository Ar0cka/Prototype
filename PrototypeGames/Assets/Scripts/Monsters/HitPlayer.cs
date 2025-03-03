using System;
using Monsters;
using Player;
using Player.PlayerStats.Heath;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class HitPlayer : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private BoxCollider2D hitCollider;
        private bool hit = false;

        private void Awake()
        {
            if (hitCollider == null) hitCollider = GetComponent<BoxCollider2D>();
            if (enemyData == null) enemyData = GetComponentInParent<EnemyData>();

            if (hitCollider == null || enemyData == null)
            {
                Debug.LogError("Not find components");
                enabled = false;
            }

            hitCollider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !hit)
            {
                Debug.Log("HitPlayer");
                
                hit = true;
                IPlayerTakeDamage playerHeath = other.GetComponentInChildren<PlayerHeath>();
                
                if (playerHeath != null && enemyData != null)
                {
                    playerHeath.TakeDamage(enemyData.GetDamage());
                } 
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && hit)
            {
                hit = false;
            }
        }
    }
}