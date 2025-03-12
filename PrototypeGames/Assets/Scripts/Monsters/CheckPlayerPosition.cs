using System;
using UnityEngine;

namespace Monsters
{
    public class CheckPlayerPosition : MonoBehaviour
    {
        public bool NeedWalkAroundPlayer { get; private set; }
        private PlayerMovementController playerMovementController;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                NeedWalkAroundPlayer = true;
                playerMovementController = other.GetComponent<PlayerMovementController>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                NeedWalkAroundPlayer = false;
            }
        }

        public PlayerMovementController GetPlayerMovementController()
        {
            if (playerMovementController != null)
            {
                return playerMovementController;
            }

            return null;
        }
    }
}