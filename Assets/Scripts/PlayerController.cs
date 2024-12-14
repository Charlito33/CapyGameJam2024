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
    [SerializeField] private PlayerState state;
    [SerializeField] private float speed;
    [SerializeField] private Tilemap[] happy_tilemaps;
    [SerializeField] private Tilemap[] sad_tilemaps;
    private SceneManager _sceneManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sceneManager = GameObject.Find("GameManager").GetComponent<SceneManager>();
        _rb = GetComponent<Rigidbody2D>();
        state = PlayerState.HAPPY;
        SwitchTiles();
    }

    private void SwitchTiles()
    {
        switch (state)
        {
            case PlayerState.HAPPY:
                foreach (var tilemap in happy_tilemaps)
                {
                    tilemap.gameObject.SetActive(true);
                }
                foreach (var tilemap in sad_tilemaps)
                {
                    tilemap.gameObject.SetActive(false);
                }
                break;
            case PlayerState.SAD:
                foreach (var tilemap in happy_tilemaps)
                {
                    tilemap.gameObject.SetActive(false);
                }
                foreach (var tilemap in sad_tilemaps)
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

    private void FixedUpdate()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        
        _rb.linearVelocity = new Vector2(h, v).normalized * (Time.fixedDeltaTime * speed);
    }
}
