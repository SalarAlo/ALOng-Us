using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorViewUI : SingletonPersistent<ErrorViewUI>
{
    [SerializeField] private GameObject errorViewUIParent;
    [SerializeField] private TextMeshProUGUI errorMessageTextField;
    [SerializeField] private Button closeButton;

    private void Start() {
        closeButton.onClick.AddListener(CloseButton_OnClick);
        errorViewUIParent.SetActive(false);
        BaseUI.OnAnyWindowOpened += BaseUI_OnAnyWindowOpened;
    }

    private void BaseUI_OnAnyWindowOpened() {
        Hide();
    }

    private void CloseButton_OnClick(){
        Hide();
    }

    private void Hide() {
        errorViewUIParent.SetActive(false);
        errorMessageTextField.text = "";
    }

    public void RenderError(string errorMessage) {
        errorViewUIParent.SetActive(true);
        errorMessageTextField.text = errorMessage;
    }
}
