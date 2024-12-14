using UnityEngine;
using UnityEngine.UI;

public class PanicBarController : MonoBehaviour
{
    [Header("Bar Settings")]
    public Image panicBar;
    public float DechargeSpeed = 0.5f;
    public float rechargeSpeed = 0.5f;
    public float transparentAlpha = 0.5f;
    private bool isDecharging = false;
    private bool isRecharging = false;
    private Color originalColor;

    void Start()
    {
        if (panicBar == null)
        {
            Debug.LogError("Aucune Image assignée à 'panicBar' dans l'inspecteur.");
            return;
        }
        originalColor = panicBar.color;
        panicBar.fillAmount = 1f;
        SetBarVisibility(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isDecharging && !isRecharging)
            {
                isDecharging = true;
                SetBarVisibility(true);
                SetBarTransparency(false);
            }
            else if (isDecharging)
            {
                isDecharging = false;
                isRecharging = true;
                SetBarVisibility(true);
                SetBarTransparency(true);
            }
            else if (isRecharging)
            {
                isRecharging = false;
                isDecharging = true;
                SetBarTransparency(false);
                SetBarVisibility(true);
            }
        }
        if (isDecharging)
        {
            DechargeBar();
        }
        else if (isRecharging)
        {
            RechargeBar();
        }
    }
    private void DechargeBar()
    {
        if (panicBar.fillAmount > 0f)
        {
            panicBar.fillAmount -= DechargeSpeed * Time.deltaTime;
        }
        else
        {
            isDecharging = false;
            SetBarVisibility(false);
        }
    }
    private void RechargeBar()
    {
        if (panicBar.fillAmount < 1f)
        {
            panicBar.fillAmount += rechargeSpeed * Time.deltaTime;
        }
        else
        {
            isRecharging = false;
            SetBarVisibility(false);
        }
    }
    private void SetBarTransparency(bool isTransparent)
    {
        Color newColor = panicBar.color;
        newColor.a = isTransparent ? transparentAlpha : originalColor.a;
        panicBar.color = newColor;
    }
    private void SetBarVisibility(bool isVisible)
    {
        panicBar.enabled = isVisible;
    }
}