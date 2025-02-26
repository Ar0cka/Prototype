using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Monsters;
using Player.PlayerStats.Heath;
using Player.PlayerStats.ManaPool;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Quaternion = System.Numerics.Quaternion;
using Vector2 = UnityEngine.Vector2;

namespace Player.CastSpeelSystem
{
    public class CastSystem : MonoBehaviour
    {
        // В будущем будет вместо опеределнного спела, слоты спелов (их 2)
        [SerializeField] private List<AbstractSpellSettings> spellSlots = new List<AbstractSpellSettings>(2);
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Transform castPosition;

        private ISubtractionMana _subtractionMana;
        private IHealPlayer _healPlayer;
        private int _currentSlot = 0;
        private float _cooldown;

        private void Awake()
        {
            _subtractionMana = GetComponentInChildren<PlayerManaPool>();
            _healPlayer = GetComponentInChildren<PlayerHeath>();

            if (_subtractionMana == null)
                Debug.LogError($"PlayerManaPool not found in children of {gameObject.name}");
            if (_healPlayer == null)
                Debug.LogError($"PlayerHeath not found in children of {gameObject.name}");
            
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            _cooldown -= Time.deltaTime;

            if (_cooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    _currentSlot = 0;
                    CastSpell();
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    _currentSlot = 1;
                    CastSpell();
                }
            }
        }

        #region ChoseNeedSpellCastMethod

        private void CastSpell()
        {
            if (!AreDependenciesValid())
            {
                Debug.LogError("Missing dependencies in CastSystem!");
                return;
            } 
            if (spellSlots[_currentSlot] != null)  
                CheckSpellType(spellSlots[_currentSlot]);
        }

        private void CheckSpellType(AbstractSpellSettings spellSettings)
        {
            if (spellSettings == null)
            {
                Debug.LogError($"Spell at slot {_currentSlot} is null!");
                return;
            }
            
            switch (spellSettings.SpellType)
            {
                case SpellType.Attack:
                    CastAttackSpell(spellSettings);
                    break;
                case SpellType.Heal:
                    CastHealSpell(spellSettings);
                    break;
            }
        }
        
        #endregion
        

        #region DifferentTypeSpellCast 

        private void CastHealSpell(AbstractSpellSettings spellSettings)
        {
            if (spellSettings is HealSpells healSpells)
            {
                if (CanCastSpell(healSpells.SpellCost))
                {
                    _healPlayer.HealPlayer(healSpells.HealCount);
                    AbstractOperation(spellSettings);
                }
            }
        }
        
        private void CastAttackSpell(AbstractSpellSettings spellSettings)
        { 
            if (spellSettings is AttackSpells attackSpells)
            {
                if (CanCastSpell(attackSpells.SpellCost))
                {
                    Vector2 moveVector = spriteRenderer.flipX ? Vector2.left : Vector2.right;
                    GameObject spellPrefab = Instantiate(attackSpells.SpellPrefab, castPosition.position, UnityEngine.Quaternion.identity);
                    SpellProjectile spellProjectile = spellPrefab.GetComponent<SpellProjectile>();
            
                    spellProjectile.Initialize(attackSpells.SpellDamage + GlobalPlayerStats.Instance.MagicDamage, attackSpells.SpellSpeed, moveVector, attackSpells.TimeAlive);
                    AbstractOperation(spellSettings);
                }
            }
        }
        
        private void AbstractOperation(AbstractSpellSettings spellSettings)
        {
            _subtractionMana.SubtractionMana(spellSettings.SpellCost);
            _cooldown = spellSettings.CooldownSpell;
        }

        #endregion
        

        #region CheckerRegion

        private bool CanCastSpell(int spellCost)
        {
            return _subtractionMana.CurrentManaPool > spellCost;
        }
        
        private bool AreDependenciesValid()
        {
            if (spellSlots == null || _subtractionMana == null || _healPlayer == null) 
            {
                Debug.LogError("Missing: " +
                               (spellSlots == null ? "SpellSlots, " : "") +
                               (_subtractionMana == null ? "Subtraction mana" : "") +
                               (_healPlayer == null ? "Heal player" : ""));
                return false;
            }
            return true;
        }
        #endregion
    }
}