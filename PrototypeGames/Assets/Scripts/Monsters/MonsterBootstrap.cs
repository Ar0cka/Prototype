using System;
using Monsters.PathFinder;
using UnityEngine;

namespace Monsters
{
    public class MonsterBootstrap : MonoBehaviour
    {
        [SerializeField] private GridGen gridCreater;
        [SerializeField] private EnemyData enemyData;

        private void Start()
        {
            enemyData.InitializeEnemyData();
            gridCreater.InitializeGridCreater();
        }
    }
}