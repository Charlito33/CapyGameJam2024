using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private QuestManager _questManager;
    
    private Rigidbody2D _rb;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] private float movementTransitionTime;
    [SerializeField] private float speed;
    [SerializeField] private float badSpeed;
    [SerializeField] private Tilemap[] happyTilemaps;
    [SerializeField] private Tilemap[] sadTilemaps;
    
    [SerializeField] private PlayerStateManager playerStateManager;
    
    [Header("Sprites")]
    [SerializeField] private List<SpriteRenderer> sprites;
    
    private void Start()
    {
        _questManager = GameObject.Find("/GameManager").GetComponent<QuestManager>();
        
        movementTransitionTime = 0.15f;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_questManager.IsDialogActive())
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        float speedyVariable = 0.0f;

        if (playerStateManager.GetPlayerState() == PlayerStateManager.PlayerState.Default)
        {
            speedyVariable = speed;
        }
        if (playerStateManager.GetPlayerState() == PlayerStateManager.PlayerState.Bad)
        {
            speedyVariable = badSpeed;
        }
        
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        
        _movementInput = new Vector2(h, v).normalized;  
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            movementTransitionTime
        );
        _rb.linearVelocity = _smoothedMovementInput * (Time.fixedDeltaTime * speedyVariable);

        if ((_smoothedMovementInput * (Time.fixedDeltaTime * speedyVariable)).x < 0.0f)
        {
            foreach (var s in sprites)
            {
                s.flipX = true;
            }
        }
        if ((_smoothedMovementInput * (Time.fixedDeltaTime * speedyVariable)).x > 0.0f)
        {
            foreach (var s in sprites)
            {
                s.flipX = false;
            }
        }
    }
}