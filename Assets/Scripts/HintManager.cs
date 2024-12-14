using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    [SerializeField] private List<TMP_Text> texts;
    [SerializeField] private string textString;
    [SerializeField] private float textSize;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        
        foreach (var text in texts)
        {
            text.gameObject.SetActive(false);
            text.text = textString;
            text.fontSize = textSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var text in texts)
        {
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (var text in texts)
        {
            text.gameObject.SetActive(false);
        }
    }
}
