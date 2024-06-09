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
    private bool actionSet;
    private CanvasGroup canvasGroup;

    private void Awake() {
        if(TryGetComponent(out canvasGroup)) return;
        canvasGroup = transform.AddComponent<CanvasGroup>();
    }

    public void Hide(){
        transform.localScale = Vector3.zero;
        actionSet = false;
    }

    public void SetAction(PlayerAction action) {
        var actionData = GameRoleManager.Instance.GetDataForAction(action);
        actionSet = true;

        gameObject.SetActive(true);
        actionImage.sprite = actionData.sprite;
        actionText.text = actionData.action.ToString();
    }

    public bool IsActionSet(){
        return actionSet;
    }
}
