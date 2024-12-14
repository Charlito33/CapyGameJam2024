using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest Script")]
public class QuestScript : ScriptableObject
{
    public enum QuestType
    {
        Dialog
    }
    
    [SerializeField] private ulong questId;
    [SerializeField] private QuestType questType;
    [SerializeField] private string questName;
    [SerializeField] private string questDescription;
    
    // If QuestType is Dialog
    [SerializeField] private List<string> dialogs;

    public ulong GetQuestId()
    {
        return questId;
    }
    
    public QuestType GetQuestType()
    {
        return questType;
    }

    public string GetQuestName()
    {
        return questName;
    }
}
