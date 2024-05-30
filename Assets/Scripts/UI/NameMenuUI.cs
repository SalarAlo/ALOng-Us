using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameMenuUI : BaseUISingleton<NameMenuUI>
{
    public event Action<string> OnNameSet;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button readyButton;
    [SerializeField] private CinemachineVirtualCamera nameCam;

    public override void Awake() {
        base.Awake();

        Show();
    }

    private void Start() {
        readyButton.onClick.AddListener(ReadyButton_OnClick);
    }

    private void ReadyButton_OnClick(){
        string potentialName = nameInput.text;
        string errorString = "Name needs to consist of at least 1 non white space character!";
        if (String.IsNullOrWhiteSpace(potentialName) || potentialName == "") {
            ErrorViewUI.Instance.RenderError(errorString);
            return;
        }
        OnNameSet?.Invoke(potentialName);
        nameCam.gameObject.SetActive(false);
        MainMenuUI.Instance.Show();
        Hide();
    }

}
