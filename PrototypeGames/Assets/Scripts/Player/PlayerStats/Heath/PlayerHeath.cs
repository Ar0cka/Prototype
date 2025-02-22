using System.Collections.Generic;
using Player.PlayerStats.Interface;
using ScriptableObjects.Player;
using UnityEngine;

namespace Player
{
    public class PlayerHeath : MonoBehaviour, IDamageable, IUpHeathPlayer
    {
        private PlayerStartData baseStats;
        private PlayerMainStats playerStats;
        
        private int _maxHitPoints;
        private int _currentHitPoints;
        
        [SerializeField] private float ArmorEffectCoefficient = 100;
        private float _armour;

        public void InitializeHeathPlayer(PlayerStartData startStats, PlayerMainStats stats)
        {
            baseStats = startStats;
            playerStats = stats;

            if (playerStats == null) playerStats = GetComponent<PlayerMainStats>();
            
            if (baseStats == null || stats == null)
            {
                Debug.LogError("Missing component" + 
                               (baseStats == null ? "baseStats" : "") +
                               (stats == null ? "PlayerStats":""));
                return;
            }
            
            _maxHitPoints = baseStats.MaxHitPoints;
            _armour = baseStats.Armour;
            _currentHitPoints = _maxHitPoints;
        }
        
        public void TakeDamage(int damage)
        {
            if (_currentHitPoints <= 0) return;

            float finalDamage = damage * (1 - DamageReducer());
            _currentHitPoints -= (int)finalDamage;

            if (_currentHitPoints <= 0) PlayerDie();
        }

        private float DamageReducer()
        {
            return _armour / (_armour + ArmorEffectCoefficient);
        }
        
        private void PlayerDie()
        {
            //Возврат на первую сцену с начинанием респавна героя
        }
        
        #region UpgradeStats

        public void CalculateHitPoint(int countUpgrade)
        {
            _maxHitPoints += countUpgrade;
            _currentHitPoints = _maxHitPoints;
        }

        public void CalculateArmour(List<float> armourBuffs) //Будет проверять все надетые вещи, и в зависимости от того, складывать все бафы к армору 
        {
            float result = playerStats.Strength * 0.05f;
            
            foreach (var item in armourBuffs)
            {
                result += item;
            }

            _armour = result;
        }

        #endregion
        
        public int MaxHitPoints => _maxHitPoints;

        public int CurrentHitPoints => _currentHitPoints;
        
    }
}