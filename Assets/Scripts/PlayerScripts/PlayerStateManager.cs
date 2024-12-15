using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Default,
        Bad
    }

    private QuestManager _questManager;
    private TilemapManager _tilemapManager;
    [SerializeField] private float badTimerSeconds = 30.0f;
    [SerializeField] private float badTimerMinimum = 5.0f;
    private float _badTimer;
    
    private PlayerState _state = PlayerState.Default;
    
    [Header("UI")]
    [SerializeField] private Image bar;

    void Start()
    {
        _questManager = GameObject.Find("/GameManager").GetComponent<QuestManager>();
        _tilemapManager = GameObject.Find("/GameManager").GetComponent<TilemapManager>();
        
        _badTimer = badTimerSeconds;
    }

    void Update()
    {
        if (Input.GetButtonDown("Toggle Player State"))
        {
            if (_questManager.IsDialogActive())
            {
                return;
            }
            
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
        
        float percent = _badTimer / badTimerSeconds;
        bar.fillAmount = percent;
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
