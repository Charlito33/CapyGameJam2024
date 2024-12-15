using UnityEditor;

[CustomEditor(typeof(QuestScript))]
public class QuestInspector : Editor
{
    private QuestScript _quest;
    
    private void Awake()
    {
        _quest = target as QuestScript;
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
        serializedObject.Update();
        _quest._questId = ULongField("Id", _quest._questId);
        _quest._questName = EditorGUILayout.TextField("Name", _quest._questName);
        _quest._questDescription = EditorGUILayout.TextField("Description", _quest._questDescription);
        _quest._questType = (QuestScript.QuestType) EditorGUILayout.EnumPopup("Type", _quest._questType);
        _quest._autoGiveQuest = EditorGUILayout.Toggle("Auto-Give", _quest._autoGiveQuest);
        if (_quest._questType == QuestScript.QuestType.Dialog)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogs"));
        }
        if (_quest._questType == QuestScript.QuestType.DestroyGameObjects)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyGameObjects"));
        }
        if (_quest._questType == QuestScript.QuestType.TeleportPlayer)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("teleportPosition"));
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredQuests"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompleteGiveQuests"));
        EditorUtility.SetDirty(_quest);
        EditorUtility.SetDirty(serializedObject.targetObject);
        serializedObject.ApplyModifiedProperties();
    }
}