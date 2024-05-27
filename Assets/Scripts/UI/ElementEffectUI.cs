using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UI;

public class ElementEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Vector3 hoverOffset = new Vector3(20f, 0f, 0f);
    [SerializeField] private Color hoverColor = Color.green;

    private Vector3 originalPosition;
    private Color originalColor;

    private RectTransform rectTransform;
    private bool isTransitioning = false;
    private TextMeshProUGUI textField;
    private Image image;
    private bool isHovered = false;
    private float transitionTimer = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
        TryGetComponent(out image);
        TryGetComponent(out textField);
        if (textField != null)
            originalColor = textField.color;
        else 
            originalColor = image.color;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;

            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            if (isHovered)
            {
                rectTransform.localPosition = Vector3.Lerp(originalPosition, originalPosition + hoverOffset, t);
                
                if (textField != null)
                    textField.color = Color.Lerp(originalColor, hoverColor, t);
                else 
                    image.color = Color.Lerp(originalColor, hoverColor, t);
            }
            else
            {
                rectTransform.localPosition = Vector3.Lerp(originalPosition + hoverOffset, originalPosition, t);
                if (textField != null)
                    textField.color = Color.Lerp(hoverColor, originalColor, t);
                else 
                    image.color = Color.Lerp(hoverColor, originalColor, t);
            }

            if (transitionTimer >= transitionDuration)
            {
                isTransitioning = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        StartTransition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        StartTransition();
    }

    private void StartTransition()
    {
        isTransitioning = true;
        transitionTimer = 0f;
    }
}
