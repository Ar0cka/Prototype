using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CheckStopRadius : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("RaySettings")] 
        [SerializeField] private Transform rayPosition;
        [SerializeField] private float distanceRay;
        
        
        public bool IsStop { get; private set; }

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        private void Update()
        {
            CheckPlayerRay();
        }

        private void CheckPlayerRay()
        {
            Vector2 side = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            RaycastHit2D rayHit2D =
                Physics2D.Raycast(rayPosition.position, side, distanceRay, LayerMask.GetMask("Player"));
            
            Debug.DrawRay(rayPosition.position, side * distanceRay, Color.red);

            if (rayHit2D.collider != null)
            {
                if (rayHit2D.collider.CompareTag("Player"))
                {
                    IsStop = true;
                    return;
                }
            }

            IsStop = false;
        }
    }
}