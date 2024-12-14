using UnityEngine;
using UnityEngine.UI;

public class QuestPanelController : MonoBehaviour
{
    [Header("Panel Settings")]
    public RectTransform questPanel;
    public Button toggleButton;
    [Header("Animation Settings")]
    public float slideSpeed = 500f;
    public float hiddenPositionX = 0f;
    public float visiblePositionX = -300f;

    private bool isPanelVisible = true;
    private Vector2 targetPosition;
    void Start()
    {
        toggleButton.onClick.AddListener(TogglePanel);
        targetPosition = questPanel.anchoredPosition;
        SetPanelPosition(isPanelVisible);
    }
    void Update()
    {
        questPanel.anchoredPosition = Vector2.Lerp(
            questPanel.anchoredPosition,
            targetPosition,
            Time.deltaTime * slideSpeed
        );
    }
    public void TogglePanel()
    {
        isPanelVisible = !isPanelVisible;
        SetPanelPosition(isPanelVisible);
    }
    private void SetPanelPosition(bool visible)
    {
        if (visible)
        {
            targetPosition = new Vector2(visiblePositionX, questPanel.anchoredPosition.y);
        }
        else
        {
            targetPosition = new Vector2(hiddenPositionX, questPanel.anchoredPosition.y);
        }
    }
}