using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerStartData", menuName = "ScriptableObject/Player", order = 0)]
    public class PlayerStartData : ScriptableObject
    {
        [Header("MainStats")]
        [SerializeField] private int strength = 10;
        [SerializeField] private int agility = 10;
        [SerializeField] private int intelligence = 10; 
        
        [Header("Second Stats")]
        [SerializeField] private int maxHitPoints = 100;
        [SerializeField] private int stamina = 100;
        [SerializeField] private int manaPool = 100;
        
        [Header ("Static stats")]
        [SerializeField] private int damage = 3;
        [SerializeField] private int magicDamage = 1;
        [SerializeField] private float walkSpeed = 5;
        [SerializeField] private float sprintSpeed = 7;
        [SerializeField] private float armour = 2;
        [SerializeField] private float regenerationMana = 0.2f;
        [SerializeField] private float regenerationStamina = 0.2f;


        public int Strength => strength;
        public int Agility => agility;
        public int Intelligence => intelligence;

        public int MaxHitPoints => maxHitPoints;
        public int Stamina => stamina;
        public int ManaPool => manaPool;

        public int Damage => damage;
        public int MagicDamage => magicDamage;

        public float WalkSpeed => walkSpeed;
        public float SprintSpeed => sprintSpeed;
        public float Armour => armour;

        public float RegenerationMana => regenerationMana;
        public float RegenerationStamina => regenerationStamina;
    }
}