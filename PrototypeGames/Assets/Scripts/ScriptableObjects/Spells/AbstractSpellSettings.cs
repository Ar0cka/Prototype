using UnityEngine;
using UnityEngine.Serialization;

namespace Player.CastSpeelSystem
{
    public class AbstractSpellSettings : ScriptableObject
    {
        [SerializeField] protected string nameSpell;
        [SerializeField] protected string description;
        
        [SerializeField] protected int spellCost;
        
        [SerializeField] protected float cooldown;

        [SerializeField] protected SpellType spellType;

        public int SpellCost => spellCost;
        public float CooldownSpell => cooldown;

        public SpellType SpellType => spellType;
    }
}