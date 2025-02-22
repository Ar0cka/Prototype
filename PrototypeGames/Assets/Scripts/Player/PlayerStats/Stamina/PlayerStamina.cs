using System;
using Player.PlayerStats.Interface;
using Player.PlayerStats.Stamina;
using ScriptableObjects.Player;
using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour, IUpStamina, ISubtractionStamina
    {  
        private PlayerStartData baseStats;
        private PlayerMainStats playerStats;

        private int _stamina;
        private int _currentStamina;

        private float _regenerationStamina;
        private float currentRegenerationCount; 

        private void FixedUpdate()
        {
            if (_currentStamina != _stamina)
            {
                currentRegenerationCount += _regenerationStamina;

                if (Mathf.Approximately(currentRegenerationCount, Mathf.Round(currentRegenerationCount)))
                {
                    RegenerationStamina((int)currentRegenerationCount);
                    currentRegenerationCount = 0;
                }
            }
        }

        private void RegenerationStamina(int count)
        {
            _currentStamina += count;

            if (_currentStamina > _stamina)
            {
                _currentStamina = _stamina;
            }
        }
        
        public void InitializeStamina(PlayerMainStats mainStats, PlayerStartData stats)
        {
            baseStats = stats;
            playerStats = mainStats;
            
            if (playerStats == null) playerStats = GetComponent<PlayerMainStats>();
            
            if (baseStats == null || playerStats == null)
            {
                Debug.LogError("Missing component" + 
                               (baseStats == null ? "baseStats" : "") +
                               (playerStats == null ? "PlayerStats":""));
                enabled = false;
                return;
            }

            _stamina = baseStats.Stamina;
            _currentStamina = _stamina;
            _regenerationStamina = baseStats.RegenerationStamina;
        }

        public void SubtractionStamina(int countSubtraction)
        {
            _currentStamina -= countSubtraction;
        }
        
        public void CalculateStamina(int countUpgrade)
        {
            _stamina += countUpgrade;
            _currentStamina = _stamina;
            
            //Перерасчет восстанвление стамины
        }
        
        public int CurrentStamina => _currentStamina;
    }
}