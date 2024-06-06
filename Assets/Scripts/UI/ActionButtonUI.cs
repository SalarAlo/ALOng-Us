using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Image actionImage;
    [SerializeField] private TextMeshProUGUI coolDownText;
    private CanvasGroup canvasGroup;

    private void Awake() {
        if(TryGetComponent(out canvasGroup)) return;
        canvasGroup = transform.AddComponent<CanvasGroup>();
    }

    public void SetAction(ActionData actionData) {
        gameObject.SetActive(true);
        actionImage.sprite = actionData.sprite;
        actionText.text = actionData.action.ToString();
    }
}
