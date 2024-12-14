using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpcManager : MonoBehaviour
{
    [SerializeField] private Collider2D talkTrigger;
    [SerializeField] private Canvas interactPopup;
    [SerializeField] private List<QuestScript> questScripts;
    private QuestManager _questManager;
    private bool isPlayerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _questManager = GameObject.Find("GameManager").GetComponent<QuestManager>();
        interactPopup.gameObject.SetActive(false);
        
        _questManager.RegisterDialogBeginEvent(OnDialogBegin);
        _questManager.RegisterQuestCompletedEvent(OnQuestCompleted);

        isPlayerInRange = false;
    }

    private void OnDialogBegin(QuestScript questScript)
    {
        interactPopup.gameObject.SetActive(false);
    }

    private void OnQuestCompleted(QuestScript questScript)
    {
        if (!isPlayerInRange || !HasUncompletedQuests())
        {
            return;
        }
        
        interactPopup.gameObject.SetActive(true);
    }

    private bool HasUncompletedQuests()
    {
        foreach (var quest in questScripts)
        {
            if (!_questManager.IsQuestCompleted(quest))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !HasUncompletedQuests())
        {
            return;
        }

        isPlayerInRange = true;

        PlayerInteractionController player = other.GetComponent<PlayerInteractionController>();
        
        interactPopup.gameObject.SetActive(true);
        player.AddInteractableNpc(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        isPlayerInRange = true;
        
        PlayerInteractionController player = other.GetComponent<PlayerInteractionController>();
        
        interactPopup.gameObject.SetActive(false);
        player.RemoveInteractableNpc(this);
    }

    public void Interact()
    {
        if (!HasUncompletedQuests())
        {
            return;
        }

        foreach (var quest in questScripts)
        {
            if (!_questManager.IsQuestCompleted(quest))
            {
                _questManager.GiveQuest(quest);
                break;
            }
        }
    }
}
