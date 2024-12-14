using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private List<QuestNpcController> _interactableNpcList;
    private QuestManager _questManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _questManager = GameObject.Find("GameManager").GetComponent<QuestManager>();
        _interactableNpcList = new List<QuestNpcController>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
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
        
        foreach (var npcController in _interactableNpcList)
        {
            npcController.Interact();
        }
    }

    public void AddInteractableNpc(QuestNpcController npc)
    {
        _interactableNpcList.Add(npc);
    }

    public void RemoveInteractableNpc(QuestNpcController npc)
    {
        _interactableNpcList.Remove(npc);
    }
}
