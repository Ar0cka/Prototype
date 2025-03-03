using Monsters;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Monsters
{
    public class MonsterAbstractData : ScriptableObject
    {
        [SerializeField] private int maxHitPoints;
        [SerializeField] private float monsterRegeneration;
        [SerializeField] private float cooldownAttack = 2f;

        [FormerlySerializedAs("monsterAttackType")] [SerializeField] private MonsterType monsterType;

        public int MaxHitPoints => maxHitPoints;
        public MonsterType MonsterType => monsterType;
        public float MonsterRegeneration => monsterRegeneration;
        public float CooldownAttack => cooldownAttack;

    }
}