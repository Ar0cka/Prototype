using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Player.PlayerStats.Stamina;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;

    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private int sprintCost = 1;
    [SerializeField] private int minStamina = 10;

    public Vector2 MoveDirection { get; private set; }
    
    private ISubtractionStamina _subtractionStamina;
    private bool _isSprint;

    private void Start()
    {
        if (rb2D == null) rb2D = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        _subtractionStamina = GetComponentInChildren<PlayerStamina>();

        if (_subtractionStamina == null)
        {
            Debug.LogError("Not founded player stamin");
        }
    }

    private void FixedUpdate()
    {
        if (_isSprint && _subtractionStamina.CurrentStamina > sprintCost)
        {
            _subtractionStamina.SubtractionStamina(sprintCost);
        }
    }

    private void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        #region FlipHero

        if (horizontalMove > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalMove < 0)
        {
            spriteRenderer.flipX = true;
        }

        #endregion
        
        animator.SetBool("Walk", ActivatingWalkAnimation(horizontalMove, verticalMove));
        
        MoveDirection = new Vector2(horizontalMove, verticalMove);

        rb2D.velocity = MoveDirection * Speed();
    }

    private float Speed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _subtractionStamina.CurrentStamina > minStamina) // Изменить на button 
        {
            _isSprint = true;
            return runSpeed;
        }

        _isSprint = false;
        return walkSpeed;
    }

    private bool ActivatingWalkAnimation(float horizontalMove, float verticalMove)
    {
        if (horizontalMove != 0 || verticalMove != 0)
        {
            return true;
        }

        return false;
    }
}
