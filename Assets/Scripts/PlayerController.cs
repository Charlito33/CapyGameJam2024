using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    enum PlayerState
    {
        HAPPY,
        SAD
    }

    private Rigidbody2D _rb;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] private float movementTransitionTime;
    [SerializeField] private PlayerState state;
    [SerializeField] private float speed;
    [SerializeField] private Tilemap[] happyTilemaps;
    [SerializeField] private Tilemap[] sadTilemaps;
    private QuestManager _questManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementTransitionTime = 0.15f;
        _rb = GetComponent<Rigidbody2D>();
        state = PlayerState.HAPPY;
//        _questManager = GameObject.Find("GameManager").GetComponent<QuestManager>();
        SwitchTiles();
    }

    private void SwitchTiles()
    {
        switch (state)
        {
            case PlayerState.HAPPY:
                foreach (var tilemap in happyTilemaps)
                {
                    tilemap.gameObject.SetActive(true);
                }
                foreach (var tilemap in sadTilemaps)
                {
                    tilemap.gameObject.SetActive(false);
                }
                break;
            case PlayerState.SAD:
                foreach (var tilemap in happyTilemaps)
                {
                    tilemap.gameObject.SetActive(false);
                }
                foreach (var tilemap in sadTilemaps)
                {
                    tilemap.gameObject.SetActive(true);
                }
                break;
            default:
                Debug.LogError("Invalid player state");
                break;
        }
    }

    private void ToggleState()
    {
        switch (state)
        {
            case PlayerState.HAPPY:
                state = PlayerState.SAD;
                break;
            case PlayerState.SAD:
                state = PlayerState.HAPPY;
                break;
            default:
                Debug.LogError("Invalid player state");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ToggleState();
            SwitchTiles();
        }
    }
//IsDialogAcvite
    private void FixedUpdate()
    {
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