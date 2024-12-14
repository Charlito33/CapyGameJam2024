using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestScript> questScripts;
    [SerializeField] private QuestDialogUIManager questDialogUIManager;
    private List<QuestScript> _activeQuests;
    private List<QuestScript> _completedQuests;
    private bool _inDialog;
    private QuestScript _activeDialog;
    private int _dialogIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _activeQuests = new List<QuestScript>();
        _completedQuests = new List<QuestScript>();
        
        questDialogUIManager.SetActive(false);
        
        UpdateQuests();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update every quest interdependencies
    // Use it when you update a quest state
    private void UpdateQuests()
    {
        foreach (var questScript in questScripts)
        {
            if (!questScript.IsQuestAutoGiven())
                return;
            if (!questScript.IsQuestRequired() || IsQuestCompleted(questScript.GetRequiredQuestId()))
            {
                GiveQuest(questScript);
            }
        }
    }

    public bool IsQuestCompleted(ulong questId)
    {
        return false;
    }

    public void GiveQuest(QuestScript questScript)
    {
        if (_activeQuests.Contains(questScript))
        {
            return;
        }
        
        Debug.Log("New quest: " + questScript.GetQuestName());
        _activeQuests.Add(questScript);
        
        if (questScript.GetQuestType() == QuestScript.QuestType.Dialog)
        {
            _inDialog = true;
            _activeDialog = questScript;
            _dialogIndex = -1;
            ContinueDialog();
        }
    }

    public void ContinueDialog()
    {
        _dialogIndex++;
        
        if (_dialogIndex >= _activeDialog.GetDialogs().Count)
        {
            CompleteQuest(_activeDialog);
            _inDialog = false;
            _activeDialog = null;
            questDialogUIManager.SetActive(false);
            return;
        }
        
        questDialogUIManager.SetTitle(_activeDialog.GetQuestName());
        questDialogUIManager.SetDialog(_activeDialog.GetDialogs()[_dialogIndex]);
        questDialogUIManager.SetActive(true);
    }

    public bool IsDialogActive()
    {
        return _inDialog;
    }

    public void CompleteQuest(QuestScript questScript)
    {
        if (!_activeQuests.Contains(questScript))
        {
            Debug.LogError("Cannot complete a quest that is not active.");
            return;
        }
        
        _completedQuests.Add(questScript);
        
        if (questScript.GetQuestCompleteAction() == QuestScript.QuestCompleteAction.GiveQuest)
        {
            GiveQuest(GetQuestById(questScript.GetQuestCompleteGiveQuestId()));   
        }
    }

    public bool CanCompleteQuest(QuestScript questScript)
    {
        return false;
    }

    public QuestScript GetQuestById(ulong id)
    {
        foreach (var questScript in questScripts)
        {
            if (questScript.GetQuestId() == id)
            {
                return questScript;
            }
        }
        Debug.LogError("Unknown quest id: " + id);
        return null;
    }
}
