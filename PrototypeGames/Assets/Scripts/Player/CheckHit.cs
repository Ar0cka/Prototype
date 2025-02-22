using System;
using System.Collections;
using System.Collections.Generic;
using Monsters;
using UnityEngine;

public class CheckHit : MonoBehaviour
{
    private bool hit = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hit && other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyDataTest>();
            enemy.TakeDamage(5);
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
