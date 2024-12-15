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
        DestroyGameObjects
    }
    
    private ulong _questId;
    private string _questName = "Quest Name";
    private string _questDescription = "Quest Description";
    private QuestType _questType = QuestType.Manual;
    [SerializeField] private List<QuestScript> requiredQuests;
    private bool _autoGiveQuest;
    [SerializeField] private List<QuestScript> onCompleteGiveQuests;
    [SerializeField] private List<Dialog> dialogs;
    [SerializeField] private List<string> destroyGameObjects;

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
    
    [CustomEditor(typeof(QuestScript))]
    public class QuestInspector : Editor
    {
        private QuestScript quest;
    
        private void Awake()
        {
            quest = target as QuestScript;
        }

        private ulong ULongField(string label, ulong value)
        {
            long v = (long) value;

            v = EditorGUILayout.LongField(label, v);
            if (v < 0)
            {
                v = 0;
            } 
            return (ulong) v;
        }

        public override void OnInspectorGUI()
        {
            quest._questId = ULongField("Id", quest._questId);
            quest._questName = EditorGUILayout.TextField("Name", quest._questName);
            quest._questDescription = EditorGUILayout.TextField("Description", quest._questDescription);
            quest._questType = (QuestType) EditorGUILayout.EnumPopup("Type", quest._questType);
            quest._autoGiveQuest = EditorGUILayout.Toggle("Auto-Give", quest._autoGiveQuest);
            if (quest._questType == QuestType.Dialog)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogs"));
            }
            if (quest._questType == QuestType.DestroyGameObjects)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyGameObjects"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredQuests"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompleteGiveQuests"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
