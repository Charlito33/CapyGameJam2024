using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Default,
        Bad
    }

    [SerializeField] private float badTimerSeconds = 30.0f;
    [SerializeField] private float badTimerMinimum = 5.0f;
    private float _badTimer;
    private TilemapManager _tilemapManager;
    
    private PlayerState _state = PlayerState.Default;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tilemapManager = GameObject.Find("GameManager").GetComponent<TilemapManager>();
        _badTimer = badTimerSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Toggle Player State"))
        {
            TogglePlayerState();
        }

        if (_state == PlayerState.Default)
        {
            _badTimer += Time.deltaTime;
            if (_badTimer > badTimerSeconds)
            {
                _badTimer = badTimerSeconds;
            }
        }
        if (_state == PlayerState.Bad)
        {
            _badTimer -= Time.deltaTime;
            if (_badTimer < 0)
            {
                SetPlayerState(PlayerState.Default);
            }
        }
    }

    private void SetPlayerState(PlayerState state)
    {
        _state = state;

        if (state == PlayerState.Default)
        {
            _tilemapManager.SwitchTilemap(TilemapManager.TilemapType.Default);
        }
        if (state == PlayerState.Bad)
        {
            _tilemapManager.SwitchTilemap(TilemapManager.TilemapType.Bad);
        }
    }

    public bool TogglePlayerState()
    {
        if (_state == PlayerState.Default)
        {
            if (_badTimer < badTimerMinimum)
            {
                return false;
            }
            SetPlayerState(PlayerState.Bad);
            return true;
        }
        if (_state == PlayerState.Bad)
        {
            SetPlayerState(PlayerState.Default);
            return true;
        }
        return false;
    }

    public PlayerState GetPlayerState()
    {
        return _state;
    }
}
