using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest Script")]
public class QuestScript : ScriptableObject
{
    public enum QuestType
    {
        Manual,
        Dialog
    }

    public enum QuestCompleteAction
    {
        DoNothing,
        GiveQuest
    }
    
    [SerializeField] private ulong questId = 0;
    [SerializeField] private long requireQuestId = -1;
    [SerializeField] private bool autoGiveQuest = false;
    [SerializeField] private QuestType questType = QuestType.Manual;
    [SerializeField] private string questName = "Quest Name";
    [SerializeField] private string questDescription = "Quest Description";
    [SerializeField] private QuestCompleteAction onComplete = QuestCompleteAction.DoNothing;
    [SerializeField] private ulong onCompleteGiveQuestId = 0;
    
    // If QuestType is Dialog
    [SerializeField] private List<string> dialogs;

    public ulong GetQuestId()
    {
        return questId;
    }

    public bool IsQuestRequired()
    {
        return requireQuestId > 0;
    }

    public ulong GetRequiredQuestId()
    {
        return (ulong) requireQuestId;
    }

    public bool IsQuestAutoGiven()
    {
        return autoGiveQuest;
    }
    
    public QuestType GetQuestType()
    {
        return questType;
    }

    public string GetQuestName()
    {
        return questName;
    }

    public string GetQuestDescription()
    {
        return questDescription;
    }

    public QuestCompleteAction GetQuestCompleteAction()
    {
        return onComplete;
    }
    
    public ulong GetQuestCompleteGiveQuestId()
    {
        return onCompleteGiveQuestId;
    }

    public List<string> GetDialogs()
    {
        return dialogs;
    }
}
