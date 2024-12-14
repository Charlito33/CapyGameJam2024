using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpcController : MonoBehaviour
{
    [SerializeField] private Collider2D talkTrigger;
    [SerializeField] private Canvas interactPopup;
    [SerializeField] private List<QuestScript> questScripts;
    private QuestManager _questManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _questManager = GameObject.Find("GameManager").GetComponent<QuestManager>();
        interactPopup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        PlayerInteractionController player = other.GetComponent<PlayerInteractionController>();
        
        interactPopup.gameObject.SetActive(true);
        player.AddInteractableNpc(this);
        
        _questManager.GiveQuest(_questManager.GetQuestById(0));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        PlayerInteractionController player = other.GetComponent<PlayerInteractionController>();
        
        interactPopup.gameObject.SetActive(false);
        player.RemoveInteractableNpc(this);
    }

    public void Interact()
    {
        
    }
}
