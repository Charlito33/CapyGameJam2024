using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestScript> questScripts;
    
    private Dictionary<ulong, QuestScript> _quests;
    private List<ulong> _activeQuests;
    private List<ulong> _completedQuests;
    private bool _inDialog;
    private QuestScript _activeDialog;
    private int _dialogIndex;

    private List<Action<QuestScript>> _onDialogBeginCallbacks;
    private List<Action<QuestScript>> _onQuestBeginCallbacks;
    private List<Action<QuestScript>> _onQuestCompletedCallbacks;
    
    [Header("UI")]
    [SerializeField] private QuestUI questUI;
    [SerializeField] private QuestDialogUIManager questDialogUIManager;

    private void Awake()
    {
        _quests = new Dictionary<ulong, QuestScript>();
        _activeQuests = new List<ulong>();
        _completedQuests = new List<ulong>();

        _onDialogBeginCallbacks = new List<Action<QuestScript>>();
        _onQuestBeginCallbacks = new List<Action<QuestScript>>();
        _onQuestCompletedCallbacks = new List<Action<QuestScript>>();
    }

    void Start()
    {
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

    private void UpdateUI()
    {
        List<QuestScript> uiQuests = new List<QuestScript>();
        foreach (var qId in _activeQuests)
        {
            QuestScript q = _quests[qId];

            if (q.GetQuestType() == QuestScript.QuestType.Dialog || q.GetQuestType() == QuestScript.QuestType.Item)
            {
                continue;
            }
            
            uiQuests.Add(_quests[qId]);
        }
        questUI.SetQuests(uiQuests);
    }
    
    public bool IsQuestCompleted(ulong questId)
    {
        return _completedQuests.Contains(questId);
    }

    public bool IsQuestCompleted(QuestScript questScript)
    {
        return IsQuestCompleted(questScript.GetQuestId());
    }

    public bool IsQuestActive(ulong questId)
    {
        return _activeQuests.Contains(questId);
    }

    public bool IsQuestActive(QuestScript questScript)
    {
        return IsQuestActive(questScript.GetQuestId());
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
        
        foreach (var callback in _onQuestBeginCallbacks)
        {
            callback(quest);
        }
        
        if (quest.GetQuestType() == QuestScript.QuestType.Item || quest.GetQuestType() == QuestScript.QuestType.DestroyGameObjects)
        {
            CompleteQuest(quest);
        }
        
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
        
        UpdateUI();
        
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
            return;
        }

        Debug.Log("Continue dialog");
        
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

        if (quest.GetQuestType() == QuestScript.QuestType.DestroyGameObjects)
        {
            foreach (var gameObject in quest.GetOnCompleteDestroyGameObjects())
            {
                Destroy(gameObject);
            }
        }
        
        UpdateUI();
        
        Debug.Log("Completed quest: #" + quest.GetQuestId() + ", " + quest.GetQuestName());
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

    public void RegisterDialogBeginEventListener(Action<QuestScript> callback)
    {
        _onDialogBeginCallbacks.Add(callback);
    }

    public void RegisterQuestBeginEventListener(Action<QuestScript> callback)
    {
        _onQuestBeginCallbacks.Add(callback);
    }

    public void RegisterQuestCompletedEventListener(Action<QuestScript> callback)
    {
        _onQuestCompletedCallbacks.Add(callback);
    }
}
