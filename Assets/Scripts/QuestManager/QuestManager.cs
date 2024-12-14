using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestScript> questScripts;
    private List<QuestScript> _activeQuests;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _activeQuests = new List<QuestScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveQuest(QuestScript questScript)
    {
        if (_activeQuests.Contains(questScript))
        {
            return;
        }
        
        Debug.Log("New quest: " + questScript.GetQuestName());
        _activeQuests.Add(questScript);
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
