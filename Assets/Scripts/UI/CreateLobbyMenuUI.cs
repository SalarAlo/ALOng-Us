using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CreateLobbyMenuUI : BaseUISingleton<CreateLobbyMenuUI>
{
    [Header("Configuration")]
    [SerializeField] private Button createLobbyButton;

    [Header("Create Lobby Options")]
    [SerializeField] private TMP_InputField lobbyNameTextField;
    [SerializeField] private Slider playersAmountSlider;

    private void Start() {
        createLobbyButton.onClick.AddListener(CreateLobbyButton_OnClick);
    }

    private void CreateLobbyButton_OnClick() {
        string lobbyName = lobbyNameTextField.text;
        int maxPlayers = (int)playersAmountSlider.value;
        
        if(String.IsNullOrWhiteSpace(lobbyName) || lobbyName == "") {
            Hide();
            ErrorViewUI.Instance.RenderError("Lobby name shouldn't only consist of whitespaces");
            return;
        }
        
        LobbyManager.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
        Hide();
    }
}
