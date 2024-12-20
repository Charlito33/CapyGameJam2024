using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public enum TilemapType
    {
        Default,
        Bad
    }
    
    [SerializeField] private List<Tilemap> defaultTilemaps;
    [SerializeField] private List<Tilemap> badTilemaps;
    
    void Start()
    {
        SwitchTilemap(TilemapType.Default);
    }

    public void SwitchTilemap(TilemapType type)
    {
        switch (type)
        {
            case TilemapType.Default:
                foreach (var tilemap in defaultTilemaps)
                {
                    tilemap.gameObject.SetActive(true);
                }
                foreach (var tilemap in badTilemaps)
                {
                    tilemap.gameObject.SetActive(false);
                }
                break;
            case TilemapType.Bad:
                foreach (var tilemap in defaultTilemaps)
                {
                    tilemap.gameObject.SetActive(false);
                }
                foreach (var tilemap in badTilemaps)
                {
                    tilemap.gameObject.SetActive(true);
                }
                break;
            default:
                Debug.LogError("Invalid tilemap type");
                break;
        }
    }
}
