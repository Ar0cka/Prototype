using UnityEngine;

namespace Player.CastSpeelSystem
{
    [CreateAssetMenu(fileName = "HealSpell", menuName = "ScriptableObject/Spells/HealSpell", order = 0)]
    public class HealSpells : AbstractSpellSettings
    {
        [SerializeField] private int healCount;

        public int HealCount => healCount;
    }
}