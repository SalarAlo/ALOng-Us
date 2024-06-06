using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Image actionImage;

    public void SetAction(ActionData actionData) {
        gameObject.SetActive(true);
        actionImage.sprite = actionData.sprite;
        actionText.text = actionData.action.ToString();
    }
}
