using System;
using Player.PlayerStats.Interface;
using Player.PlayerStats.ManaPool;
using ScriptableObjects.Player;
using UnityEngine;

namespace Player
{
    public class PlayerManaPool : MonoBehaviour, IUpManaPool, ISubtractionMana
    {
        private PlayerStartData baseStats;
        private PlayerMainStats stats;
        
        private int _manaPool;
        private float _regenerationMana;
        
        [Header("RunTime stats")]
        private int _currentManaPool;

        private float currentRegnerationCount;

        private void FixedUpdate()
        {
            if (_currentManaPool < _manaPool)
            {
                currentRegnerationCount += _regenerationMana;

                if (Mathf.Approximately(currentRegnerationCount, Mathf.Round(currentRegnerationCount)))
                {
                    RegenerationMana((int)currentRegnerationCount);
                }
            }
        }

        public void InitializeManaPool(PlayerMainStats mainStats, PlayerStartData playerStats)
        {
            baseStats = playerStats;
            stats = mainStats;

            if (stats == null) stats = GetComponent<PlayerMainStats>();
            
            if (baseStats == null || stats == null)
            {
                Debug.LogError("Missing component" + 
                               (baseStats == null ? "baseStats" : "") +
                               (stats == null ? "PlayerStats":""));
                return;
            }
            
            _manaPool = baseStats.ManaPool;
            _currentManaPool = _manaPool;
        }

        public void CalculateManaPoolStats(int countUpdateManaPool)
        {
            _manaPool += countUpdateManaPool;
            _currentManaPool = _manaPool;
            
            //Добавить улучшение восстановление маны
        }

        private void RegenerationMana(int count)
        {
            _currentManaPool += count;
        }

        public void SubtractionMana(int spellCost)
        {
            if (_currentManaPool < spellCost)
            {
                Debug.Log("Dont have mana");
                return;
            }
            
            _currentManaPool -= spellCost;
        }
        
        public int CurrentManaPool => _currentManaPool;
    }
}