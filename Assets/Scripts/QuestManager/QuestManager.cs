using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestScript> questScripts;
    [SerializeField] private QuestDialogUIManager questDialogUIManager;
    private Dictionary<ulong, QuestScript> _quests;
    private List<ulong> _activeQuests;
    private List<ulong> _completedQuests;
    private bool _inDialog;
    private QuestScript _activeDialog;
    private int _dialogIndex;

    private List<Action<QuestScript>> _onDialogBeginCallbacks;
    private List<Action<QuestScript>> _onQuestCompletedCallbacks;
    
    void Start()
    {
        _quests = new Dictionary<ulong, QuestScript>();
        _activeQuests = new List<ulong>();
        _completedQuests = new List<ulong>();

        _onDialogBeginCallbacks = new List<Action<QuestScript>>();
        _onQuestCompletedCallbacks = new List<Action<QuestScript>>();

        foreach (var quest in questScripts)
        {
            _quests.Add(quest.GetQuestId(), quest);
            quest.Log();
        }
        
        questDialogUIManager.SetActive(false);

        UpdateQuests();
    }
    
    // Update every quest interdependencies
    // Use it when you update a quest state
    private void UpdateQuests()
    {
        foreach (var questScript in questScripts)
        {
            if (!questScript.IsQuestAutoGiven() || !AreQuestsCompleted(questScript.GetRequiredQuests()))
            {
                return;
            }
            GiveQuest(questScript);
        }
    }
    
    public bool IsQuestCompleted(ulong questId)
    {
        return _completedQuests.Contains(questId);
    }

    public bool IsQuestCompleted(QuestScript questScript)
    {
        return IsQuestCompleted(questScript.GetQuestId());
    }

    public bool AreQuestsCompleted(List<QuestScript> quests)
    {
        foreach (var quest in quests)
        {
            if (!IsQuestCompleted(quest))
            {
                return false;
            }
        }
        return true;
    }

    public void GiveQuest(ulong questId)
    {
        if (!_quests.ContainsKey(questId))
        {
            Debug.LogWarning("Trying to an unregistered quest.");
            return;
        }

        if (_activeQuests.Contains(questId))
        {
            Debug.LogWarning("Trying to give an already active quest.");
            return;
        }
        
        if (_completedQuests.Contains(questId))
        {
            Debug.LogWarning("Trying to give an already completed quest.");
            return;
        }
        
        QuestScript quest = _quests[questId];
        
        _activeQuests.Add(questId);
        
        // Show dialog
        if (quest.GetQuestType() == QuestScript.QuestType.Dialog)
        {
            _inDialog = true;
            _activeDialog = quest;
            _dialogIndex = -1;
            ContinueDialog();

            foreach (var callback in _onDialogBeginCallbacks)
            {
                callback(quest);
            }
        }
        
        Debug.Log("New quest: #" + quest.GetQuestId() + ", " + quest.GetQuestName());
    }

    public void GiveQuest(QuestScript questScript)
    {
        GiveQuest(questScript.GetQuestId());
    }

    public void ContinueDialog()
    {
        if (!_inDialog)
        {
            Debug.LogWarning("Trying to continue dialog but no dialog is active.");    
        }
        
        _dialogIndex++;

        if (_dialogIndex >= _activeDialog.GetDialogs().Count)
        {
            CompleteQuest(_activeDialog);
            _inDialog = false;
            _activeDialog = null;
            questDialogUIManager.SetActive(false);
            return;
        }

        questDialogUIManager.SetActive(true);
        questDialogUIManager.SetTitle(_activeDialog.GetDialogs()[_dialogIndex].actor);
        questDialogUIManager.SetDialog(_activeDialog.GetDialogs()[_dialogIndex].text);
    }

    public bool IsDialogActive()
    {
        return _inDialog;
    }

    public void CompleteQuest(ulong questId)
    {
        if (!_quests.ContainsKey(questId))
        {
            Debug.LogError("Trying to complete an invalid quest.");
            return;
        }
        
        if (!_activeQuests.Contains(questId))
        {
            Debug.LogWarning("Trying to complete a quest that is not active.");
            return;
        }
        
        QuestScript quest = _quests[questId];
        
        _completedQuests.Add(questId);
        _activeQuests.Remove(questId);

        foreach (var questToGive in quest.OnCompleteGiveQuests())
        {
            GiveQuest(questToGive);
        }
    }

    public void CompleteQuest(QuestScript questScript)
    {
        CompleteQuest(questScript.GetQuestId());
        
        foreach (var callback in _onQuestCompletedCallbacks)
        {
            callback(questScript);
        }
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

    public void RegisterDialogBeginEvent(Action<QuestScript> callback)
    {
        _onDialogBeginCallbacks.Add(callback);
    }

    public void RegisterQuestCompletedEvent(Action<QuestScript> callback)
    {
        _onQuestCompletedCallbacks.Add(callback);
    }
}
