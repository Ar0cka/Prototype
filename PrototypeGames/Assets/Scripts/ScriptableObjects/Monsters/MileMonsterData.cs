using Monsters;
using UnityEngine;

namespace ScriptableObjects.Monsters
{
    [CreateAssetMenu(fileName = "MileMonster", menuName = "ScriptableObject/Monster/MileMonster", order = 0)]
    public class MileMonsterData : MonsterAbstractData
    {
        [SerializeField] private int weaponDamage;
        [SerializeField] private MonsterWeaponTypeDamage monsterWeaponTypeDamage;

        public int WeaponDamage => weaponDamage;
        public MonsterWeaponTypeDamage MonsterWeaponTypeDamage => monsterWeaponTypeDamage;
    }
}