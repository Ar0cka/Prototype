using System;
using System.Collections;
using System.Collections.Generic;
using Monsters;
using Player;
using UnityEngine;

public class CheckHit : MonoBehaviour
{
    private bool hit = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hit && other.CompareTag("Enemy"))
        {
            IDamageable enemy = other.GetComponent<EnemyData>();
            enemy.TakeDamage(GlobalPlayerStats.Instance.AttackDamage);
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (hit)
        {
            hit = false;
        }
    }
}
