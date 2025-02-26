using System;
using Monsters;
using UnityEngine;

namespace Player.CastSpeelSystem
{
    public class SpellProjectile : MonoBehaviour
    {
        private float _speed;
        private float _timeAlive;
        private int _damage;
        private Vector2 _moveDirection;

        public void Initialize(int projectileDamage, float projectileSpeed, Vector2 projectileMoveDirection, float timeAliveProjectile)
        {
            _speed = projectileSpeed;
            _damage = projectileDamage;
            _moveDirection = projectileMoveDirection.normalized;
            _timeAlive = timeAliveProjectile;
        }

        private void Update()
        {
            transform.Translate(_moveDirection * _speed * Time.deltaTime, Space.World);

            if (_timeAlive <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _timeAlive -= Time.deltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("FindPlayer");
                EnemyDataTest enemyDataTest = other.GetComponent<EnemyDataTest>();
                enemyDataTest.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}