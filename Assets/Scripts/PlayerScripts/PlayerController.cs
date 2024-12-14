using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] private float movementTransitionTime;
    [SerializeField] private float speed;
    [SerializeField] private Tilemap[] happyTilemaps;
    [SerializeField] private Tilemap[] sadTilemaps;
    private QuestManager _questManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementTransitionTime = 0.15f;
        _rb = GetComponent<Rigidbody2D>();
        _questManager = GameObject.Find("GameManager").GetComponent<QuestManager>();
    }
    
   // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_questManager.IsDialogActive())
        {
            return;
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
        _rb.linearVelocity = _smoothedMovementInput * (Time.fixedDeltaTime * speed);
    }
}