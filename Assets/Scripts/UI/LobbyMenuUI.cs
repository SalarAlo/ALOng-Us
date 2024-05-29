using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEditor.VersionControl;
using TMPro;
using System;

public class LobbyMenuUI : BaseUI
{

    [Header("Buttons")]
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button backToMainMenuButton;
    
    [Header("Windows")]
    [SerializeField] private CreateLobbyMenuUI createLobbyUI;
    [SerializeField] private JoinLobbyUI joinLobbyUI;
    [SerializeField] private MainMenuUI mainMenu;
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;

    private void Start() {
        createLobbyButton.onClick.AddListener(CreateLobbyButton_OnClick);
        joinLobbyButton.onClick.AddListener(JoinLobbyButton_OnClick);
        quickJoinButton.onClick.AddListener(QuickJoinButton_OnClick);
        backToMainMenuButton.onClick.AddListener(BackToMainMenuButton_OnClick);

        LobbyManager.Instance.OnFailOccured += LobbyManager_OnFailOccured;
    }

    private void LobbyManager_OnFailOccured(LobbyServiceException e) { 
        HideWindows();
        ErrorViewUI.Instance.RenderError(e.Message);
    }

    private void QuickJoinButton_OnClick() {
        LobbyManager.Instance.QuickJoinLobby();
    }

    private void HideWindows() {
        createLobbyUI.Hide();
        joinLobbyUI.Hide();
    }

    private void CreateLobbyButton_OnClick() {
        HideWindows();
        createLobbyUI.Show();
    }
    
    private void JoinLobbyButton_OnClick() {
        HideWindows();
        joinLobbyUI.Show();
    }

    private void BackToMainMenuButton_OnClick() {
        mainMenuCam.gameObject.SetActive(true);
        lobbyMenuCam.gameObject.SetActive(true);
        mainMenu.Show();
        Hide();
    }

    public override void Show(){
        base.Show();
        HideWindows();
    }

}
