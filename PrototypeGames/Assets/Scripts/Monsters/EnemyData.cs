using System;
using Player;
using ScriptableObjects.Monsters;
using UnityEngine;

namespace Monsters
{
    public class EnemyData : MonoBehaviour, IDamageable
    {
        [SerializeField] private MonsterAbstractData monsterAbstractData;

        private int _enemyCurrentHitPoint;
        
        private float _countRegeneration;

        public void InitializeEnemyData()
        {
            if (monsterAbstractData == null)
            {
                enabled = false;
                Destroy(gameObject);
            }
        }
        
        public void TakeDamage(int damage)
        {
            _enemyCurrentHitPoint -= damage;
            Debug.Log($"Полученное урона: {damage}, осталось {_enemyCurrentHitPoint} хп");
        }

        public int GetDamage()
        {
            if (monsterAbstractData is MileMonsterData monsterMile)
            {
                return monsterMile.WeaponDamage;
            }

            return 0;
        }
    }

    public class MonsterGlobalValues
    {
        private static MonsterGlobalValues _instance;

        public static MonsterGlobalValues Instance
        {
            get
            {
                if (_instance == null) _instance = new MonsterGlobalValues();
                return _instance;
            }
        }
        
        public bool IsMonsterInFight { get; private set; }
        
        public bool IsSeePlayer { get; private set; }

        public void SwitchMonsterBehavior(bool value)
        {
            IsMonsterInFight = value;
        }

        public void SwitchSeePlayer(bool value)
        {
            IsSeePlayer = value;
        }
        
        
    }
}