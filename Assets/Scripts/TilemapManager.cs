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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
