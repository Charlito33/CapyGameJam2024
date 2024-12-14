using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TileSoundController : MonoBehaviour
{
    [System.Serializable]
    public class TileSoundPair
    {
        public TileBase tile;
        public AudioClip stepSound;
    }
    public Tilemap tilemap;
    public List<TileSoundPair> tileSoundPairs;
    public Transform playerTransform;
    public float stepFrequency = 0.5f;
    private AudioSource audioSource;
    private float lastStepTime = 0f;
    private Vector3 previousPosition;

    void Start()
    {
        if (tilemap == null || playerTransform == null)
            return;

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        previousPosition = playerTransform.position;
    }
    void Update()
    {
        if (Vector3.Distance(playerTransform.position, previousPosition) > 0.01f)
        {
            if (Time.time - lastStepTime >= stepFrequency)
            {
                PlayFootstepSound();
                lastStepTime = Time.time;
            }
        }

        previousPosition = playerTransform.position;
    }
    private void PlayFootstepSound()
    {
        Vector3Int gridPosition = tilemap.WorldToCell(playerTransform.position);
        TileBase currentTile = tilemap.GetTile(gridPosition);

        if (currentTile != null)
        {
            AudioClip soundToPlay = GetSoundForTile(currentTile);
            if (soundToPlay != null && !audioSource.isPlaying)
            {
                audioSource.clip = soundToPlay;
                audioSource.Play();
            }
        }
    }
    private AudioClip GetSoundForTile(TileBase tile)
    {
        foreach (TileSoundPair pair in tileSoundPairs)
        {
            if (pair.tile == tile)
                return pair.stepSound;
        }
        return null;
    }
}