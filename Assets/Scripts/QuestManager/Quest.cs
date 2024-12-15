using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest Script")]
public class QuestScript : ScriptableObject
{
    [Serializable]
    public struct Dialog
    {
        public string actor;
        public string text;
    }

    public enum QuestType
    {
        Manual,
        Dialog,
        Item,
        DestroyGameObjects,
        TeleportPlayer
    }
    
    public ulong _questId;
    public string _questName = "Quest Name";
    public string _questDescription = "Quest Description";
    public QuestType _questType = QuestType.Manual;
    [SerializeField] public List<QuestScript> requiredQuests;
    public bool _autoGiveQuest;
    [SerializeField] public List<QuestScript> onCompleteGiveQuests;
    [SerializeField] public List<Dialog> dialogs;
    [SerializeField] public List<string> destroyGameObjects;
    [SerializeField] public Vector2 teleportPosition;

    public void Log()
    {
        Debug.Log("Quest ID: " + _questId + ", OnComplete: " + onCompleteGiveQuests.Count + ", Dialogs: " + dialogs.Count);
    }
    
    public ulong GetQuestId()
    {
        return _questId;
    }

    public bool AreQuestsRequired()
    {
        return requiredQuests.Count > 0;
    }

    public List<QuestScript> GetRequiredQuests()
    {
        return requiredQuests;
    }

    public bool IsQuestAutoGiven()
    {
        return _autoGiveQuest;
    }
    
    public QuestType GetQuestType()
    {
        return _questType;
    }

    public string GetQuestName()
    {
        return _questName;
    }

    public string GetQuestDescription()
    {
        return _questDescription;
    }

    public List<QuestScript> OnCompleteGiveQuests()
    {
        return onCompleteGiveQuests;
    }

    public List<Dialog> GetDialogs()
    {
        return dialogs;
    }

    public List<GameObject> GetOnCompleteDestroyGameObjects()
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (var path in destroyGameObjects)
        {
            gameObjects.Add(GameObject.Find(path));
        }
        
        return gameObjects;
    }

    public Vector2 GetTeleportPosition()
    {
        return teleportPosition;
    }
}
