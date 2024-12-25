using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Rigidbody2D playerRigidBody2D;
    
    [Header("Base Stats")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Movement")] 
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Vector2 moveInput;
    
    [Header("Animator")] 
    [SerializeField] private Animator animator;
    
    private int _currentState;
    private float _lockedTill;
    
    private static readonly int IdleDown = Animator.StringToHash("IdleDown");
    private static readonly int IdleRight = Animator.StringToHash("IdleRight");
    private static readonly int IdleUp = Animator.StringToHash("IdleUp");
    private static readonly int MoveDown = Animator.StringToHash("MoveDown");
    private static readonly int MoveRight = Animator.StringToHash("MoveRight");
    private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    private static readonly int RollDown = Animator.StringToHash("RollDown");
    private static readonly int RollRight = Animator.StringToHash("RollRight");
    private static readonly int RollUp = Animator.StringToHash("RollUp");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Flip();
        
        playerRigidBody2D.velocity = moveInput * moveSpeed;
       
        
        var state = GetState();
        
        if (state == _currentState) return;
        animator.CrossFade(state,0,0);
        _currentState = state;
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        if (moveInput.y == 0) 
            return moveInput.x == 0 ? IdleRight : MoveRight;
        switch (moveInput.y)
        {
            case >= 0:
                return moveInput.y >= 0 ? MoveUp : IdleUp;
            case <= 0:
                return moveInput.y <= 0 ?  MoveDown: IdleDown;
        }

        return -1;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    // FixedUpdate is called 50 times a second
    private void FixedUpdate()
    {
        
    }
    
    // LateUpdate is called after every other update function has finished
    private void LateUpdate()
    {
        
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Roll(InputAction.CallbackContext context)
    {
        
    }
    private void Flip()
    {
        if ((!(moveInput.x < 0) || !isFacingRight) && (!(moveInput.x > 0) || isFacingRight)) return;
        isFacingRight = !isFacingRight;
        var localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
