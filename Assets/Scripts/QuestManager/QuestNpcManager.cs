using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestNpcManager : MonoBehaviour
{
    private QuestManager _questManager;
    
    [SerializeField] private Collider2D trigger;
    [SerializeField] private List<QuestScript> questScripts;
    private bool isPlayerInRange;

    [Header("UI")]
    [SerializeField] private Canvas interactPopup;
    [SerializeField] private string interactText;
    [SerializeField] private List<TMP_Text> texts;

    void Start()
    {
        _questManager = GameObject.Find("/GameManager").GetComponent<QuestManager>();
        
        _questManager.RegisterDialogBeginEventListener(OnDialogBegin);
        _questManager.RegisterQuestBeginEventListener(OnQuestBegin);
        _questManager.RegisterQuestCompletedEventListener(OnQuestCompleted);
        
        interactPopup.gameObject.SetActive(false);

        isPlayerInRange = false;

        foreach (TMP_Text text in texts)
        {
            text.text = interactText;
        }
        HideUI();
    }

    private void HideUI()
    {
        interactPopup.gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        interactPopup.gameObject.SetActive(true);
    }

    private void OnDialogBegin(QuestScript questScript)
    {
        if (!questScripts.Contains(questScript))
        {
            return;   
        }
        interactPopup.gameObject.SetActive(false);
    }

    private void OnQuestBegin(QuestScript questScript)
    {
        if (!questScripts.Contains(questScript))
        {
            return;   
        }
        if (HasUnGivenQuests())
        {
            return;
        }
        
        HideUI();
    }

    private void OnQuestCompleted(QuestScript questScript)
    {
        if (!questScripts.Contains(questScript))
        {
            return;   
        }
        if (!isPlayerInRange || !HasUnGivenQuests())
        {
            return;
        }
        
        ShowUI();
    }
    
    private bool HasUnGivenQuests()
    {
        foreach (var quest in questScripts)
        {
            if (!_questManager.IsQuestCompleted(quest) && !_questManager.IsQuestActive(quest))
            {
                return true;
            }
        }
        return false;
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
        if (!other.CompareTag("Player") || !HasUnGivenQuests())
        {
            return;
        }

        isPlayerInRange = true;

        PlayerInteractionController player = other.GetComponent<PlayerInteractionController>();
        
        player.AddInteractableNpc(this);
        ShowUI();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        isPlayerInRange = true;
        
        PlayerInteractionController player = other.GetComponent<PlayerInteractionController>();
        
        player.RemoveInteractableNpc(this);
        HideUI();
    }

    public void Interact()
    {
        if (!HasUncompletedQuests())
        {
            return;
        }

        foreach (var quest in questScripts)
        {
            if (!_questManager.IsQuestCompleted(quest) && !_questManager.IsQuestActive(quest))
            {
                _questManager.GiveQuest(quest);
                
                if (quest.GetQuestType() == QuestScript.QuestType.Item)
                {
                    gameObject.SetActive(false);
                }
                
                break;
            }
        }
    }
}
