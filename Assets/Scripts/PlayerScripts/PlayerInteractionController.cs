using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private List<QuestNpcManager> _interactableNpcList;
    private QuestManager _questManager;
    
    void Start()
    {
        _questManager = GameObject.Find("GameManager").GetComponent<QuestManager>();
        _interactableNpcList = new List<QuestNpcManager>();   
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (_questManager.IsDialogActive())
            {
                return;
            }
            
            Interact();
        }
    }

    private void Interact()
    {
        if (_questManager.IsDialogActive())
        {
            _questManager.ContinueDialog();
            return;
        }
        
        foreach (var npcController in _interactableNpcList)
        {
            npcController.Interact();
        }
    }

    public void AddInteractableNpc(QuestNpcManager npc)
    {
        _interactableNpcList.Add(npc);
    }

    public void RemoveInteractableNpc(QuestNpcManager npc)
    {
        _interactableNpcList.Remove(npc);
    }
}
