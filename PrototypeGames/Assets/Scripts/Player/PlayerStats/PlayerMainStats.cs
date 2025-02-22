using Player.PlayerStats.Interface;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMainStats : MonoBehaviour
    {
        [SerializeField] private PlayerDamage playerDamage;
        private PlayerStartData baseStats;
        
        [Header("UpCountsWherePlayerUpLevel")] 
        [SerializeField] private int upMaxHitPoints = 10;
        [SerializeField] private int upStamina = 10;
        [SerializeField] private int upManaPool = 10;
        
        private IUpManaPool _manaPool;
        private IUpStamina _stamina;
        private IUpHeathPlayer _heathPlayer;
        
        [Header("Stats")]
        private int _strength;
        private int _agility;
        private int _intelligence;

        public void InitializePlayerStats(PlayerStartData startData)
        {
            baseStats = startData;
            
            _manaPool = GetComponent<PlayerManaPool>();
            _stamina = GetComponent<PlayerStamina>();
            _heathPlayer = GetComponent<PlayerHeath>();

            if (baseStats == null || playerDamage == null || _manaPool == null || _stamina == null || _heathPlayer == null)
            {
                Debug.LogError("Missing component " +
                               (baseStats == null ? "Base stats" : "") +
                               (playerDamage == null ? "Player damage" : "") + 
                               (_manaPool == null ? "Mana pool" : "") + 
                               (_stamina == null ? "Stamina" :  "") +
                               (_heathPlayer == null ? "Player heath" : ""));
                enabled = false;
                return;
            }
            
            _strength = baseStats.Strength;
            _agility = baseStats.Agility;
            _intelligence = baseStats.Intelligence;
        }
        
        #region UpgradesStats

        public void UpStrength(int count)
        {
            _strength += count;
            
            _heathPlayer.CalculateHitPoint(upMaxHitPoints);

            if (playerDamage._isHaveWeapon) playerDamage.DamageWithWeapon();
            else playerDamage.DamageWithoutWeapon();
            
            //Улучшение брони
        }

        public void UpAgility(int count)
        {
            _agility += count;

            _stamina.CalculateStamina(upStamina);

            //Метод для обнавления спринт значений
            //Улучшение регенерации
        }

        public void UpIntelligence(int count)
        {
            _intelligence += count;
            
            _manaPool.CalculateManaPoolStats(upManaPool);
            playerDamage.CalculateMagicDamage();
        }

        #endregion
        
        #region Getters
        public int Strength => _strength;
        public int Agility => _agility;
        public int Intelligence => _intelligence;
        #endregion
    }
}