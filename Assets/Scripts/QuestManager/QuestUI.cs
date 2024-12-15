using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> questNameTexts;
    [SerializeField] private List<TMP_Text> questNameDescriptions;

    private void Start()
    {
        if (questNameTexts.Count != questNameDescriptions.Count)
        {
            Debug.LogError("Not the same amount of quest texts and descriptions.");
        }
        
        SetQuests(null);
    }

    public void SetQuests(List<QuestScript> questScripts)
    {
        int i;

        for (i = 0; i < questNameTexts.Count; i++)
        {
            questNameTexts[i].gameObject.SetActive(false);
            questNameDescriptions[i].gameObject.SetActive(false);
        }
        
        if (questScripts == null)
        {
            return;
        }

        i = 0;

        foreach (QuestScript q in questScripts)
        {
            if (i >= questScripts.Count)
            {
                break;
            }

            questNameTexts[i].gameObject.SetActive(true);
            questNameDescriptions[i].gameObject.SetActive(true);
            
            questNameTexts[i].text = q.GetQuestName();
            questNameDescriptions[i].text = q.GetQuestDescription();

            i++;
        }
    }
}
