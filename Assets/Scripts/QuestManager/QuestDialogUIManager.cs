using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text dialogText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetTitle(string title)
    {
        titleText.SetText(title);
    }

    public void SetDialog(string dialog)
    {
        dialogText.SetText(dialog);        
    }
}
