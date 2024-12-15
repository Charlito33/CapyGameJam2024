using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private List<QuestNpcManager> _interactableNpcList;
    private QuestManager _questManager;
    
    void Start()
    {
        _questManager = GameObject.Find("/GameManager").GetComponent<QuestManager>();
        
        _interactableNpcList = new List<QuestNpcManager>();   
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
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
        
        // Copy to avoid editing while foreach
        List<QuestNpcManager> npcList = new List<QuestNpcManager>(_interactableNpcList);
        
        foreach (var npcController in npcList)
        {
            Debug.Log(npcController.gameObject.name);
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
