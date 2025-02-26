using UnityEngine;

namespace Monsters
{
    public class EnemyDataTest : MonoBehaviour
    {
        [SerializeField] private int enemyCurrentHp;

        public void TakeDamage(int damage)
        {
            enemyCurrentHp -= damage;
            Debug.Log($"Полученное урона: {damage}, осталось {enemyCurrentHp} хп");
        }
    }
}