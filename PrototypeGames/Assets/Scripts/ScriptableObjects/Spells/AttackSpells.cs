using UnityEngine;

namespace Player.CastSpeelSystem
{
    [CreateAssetMenu(fileName = "AttackSpell", menuName = "ScriptableObject/Spells/AttackSpell", order = 0)]
    public class AttackSpells : AbstractSpellSettings
    {
        [SerializeField] private GameObject spellPrefab;
        
        [SerializeField] private int damage;
        [SerializeField] private float spellSpeed;
        [SerializeField] private float timeAlive;

        [SerializeField] private MagicDamageType typeDamage;

        public int SpellDamage => damage;
        public float SpellSpeed => spellSpeed;
        public float TimeAlive => timeAlive;
        public GameObject SpellPrefab => spellPrefab;
        public MagicDamageType TypeDamage => typeDamage;
       
    }
}