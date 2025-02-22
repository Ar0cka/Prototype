using Player.Interface;
using ScriptableObjects.Player;
using UnityEngine;

namespace Player
{
    public class PlayerDamage : MonoBehaviour, IEquipWeapon
    {
        private PlayerStartData baseStats;
        private PlayerMainStats playerStats;

        [Header("Damage")] 
        private int _weaponDamage = 0;

        public bool _isHaveWeapon { get; private set; }

        public void InitializeDamage(PlayerMainStats mainStats, PlayerStartData stats)
        {
            baseStats = stats;
            playerStats = mainStats;
            
            if (playerStats == null) playerStats = GetComponent<PlayerMainStats>();
            
            if (baseStats == null || playerStats == null)
            {
                Debug.LogError("Missing component" + 
                               (baseStats == null ? "baseStats" : "") +
                               (playerStats == null ? "PlayerStats":""));
                return;
            }
            
            
            DamageWithoutWeapon();
            CalculateMagicDamage();
        }
        
        #region DamageOperation

        public void CalculateMagicDamage()
        {
            float resultMagicDamage = baseStats.MagicDamage + playerStats.Intelligence * 0.55f;
            GlobalPlayerStats.Instance.CalculateMagicDamage((int) resultMagicDamage);
        }
        
        public void DamageWithoutWeapon()
        {
            float resultDamage = baseStats.Damage + GetDamageFromStrength();
            GlobalPlayerStats.Instance.CalculateDamagePlayer((int)resultDamage);
        }

        public void DamageWithWeapon() //В будущем поменяется на другую реализацию // Вызываться данный метод будет только когда одевается оружие
        {
            float resultDamage = baseStats.Damage + _weaponDamage + GetDamageFromStrength();
            GlobalPlayerStats.Instance.CalculateDamagePlayer((int)resultDamage);
        }

        public void EquipWeapon(int weaponDamage)
        {
            _weaponDamage = weaponDamage;
            _isHaveWeapon = true;
            DamageWithWeapon();
        }

        public void UnEquipWeapon()
        {
            _isHaveWeapon = false;
            DamageWithoutWeapon();
        }

        private float GetDamageFromStrength()
        {
            return playerStats.Strength * 0.1f;
        }

        #endregion
    }
}