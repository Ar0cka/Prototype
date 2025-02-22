using System;
using System.Collections.Generic;
using Player.Interface;
using Player.PlayerStats.Interface;
using ScriptableObjects.Player;
using UnityEngine;

namespace Player
{
    public class PlayerData : MonoBehaviour   //Общая логика игрока
    {
        [SerializeField] private GameObject playerGameObject;
        
        [Header("baseStatsObject")] [SerializeField] private PlayerStartData baseStats;

        [Header("InitializeScripts")] 
        [SerializeField] private PlayerDamage playerDamage;
        [SerializeField] private PlayerMainStats mainStats;
        [SerializeField] private PlayerManaPool playerManaPool;
        [SerializeField] private PlayerStamina playerStamina;
        [SerializeField] private PlayerHeath playerHeath;
        
        #region Initialize

        private void Start()
        {
            DontDestroyOnLoad(playerGameObject);
            if (baseStats != null) InitializePlayer();
            else
            {
                Debug.LogError("Component baseStats not founded. Please restart game!!");
                enabled = false;
            }
        }

        private void InitializePlayer()
        {
            CheckComponents();

            if (!AreDependenciesValid())
            {
                Debug.LogError("Failed to initialize PlayerData due to missing dependencies!");
                enabled = false;
                return;
            }
            
            mainStats.InitializePlayerStats(baseStats);
            playerDamage.InitializeDamage(mainStats, baseStats);
            playerManaPool.InitializeManaPool(mainStats, baseStats);
            playerStamina.InitializeStamina(mainStats, baseStats);
            playerHeath.InitializeHeathPlayer(baseStats, mainStats);
        }
        #endregion

        private void CheckComponents()
        {
            if (playerDamage == null) playerDamage = GetComponent<PlayerDamage>();
            if (mainStats == null) mainStats = GetComponent<PlayerMainStats>();
            if (playerManaPool == null) playerManaPool = GetComponent<PlayerManaPool>();
            if (playerStamina == null) playerStamina = GetComponent<PlayerStamina>();
            if (playerHeath == null) playerHeath = GetComponent<PlayerHeath>();
        }
        
        private bool AreDependenciesValid()
        {
            if (playerDamage == null || mainStats == null || playerManaPool == null || playerStamina == null || playerHeath == null)
            {
                Debug.LogError("Missing dependencies: " +
                               (playerDamage == null ? "PlayerDamage, " : "") +
                               (mainStats == null ? "MainStats, " : "") +
                               (playerManaPool == null ? "ManaPool, " : "") +
                               (playerStamina == null ? "Stamina, " : "") +
                               (playerHeath == null ? "Heath" : ""));
                return false;
            }
            return true;
        }
    }

    public class GlobalPlayerStats
    {
        private static GlobalPlayerStats _instance;

        public static GlobalPlayerStats Instance
        {
            get
            {
                if (_instance == null) _instance = new GlobalPlayerStats();
                return _instance;
            }
        }

        public int AttackDamage { get; private set; }
        public int MagicDamage { get; private set; }

        public void CalculateDamagePlayer(int resultDamage)
        {
            AttackDamage = resultDamage;
        }
        public void CalculateMagicDamage(int resultMagicDamage)
        {
            MagicDamage = resultMagicDamage;
        }
    }
}