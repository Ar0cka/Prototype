using Player.PlayerStats.Interface;
using ScriptableObjects.Player;
using UnityEngine;

namespace Player
{
    public class PlayerManaPool : MonoBehaviour, IUpManaPool
    {
        private PlayerStartData baseStats;
        private PlayerMainStats stats;
        
        private int _manaPool;
        private float _regenerationMana;
        
        [Header("RunTime stats")]
        private int _currentManaPool;

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
        
        public int CurrentManaPool => _currentManaPool;
    }
}