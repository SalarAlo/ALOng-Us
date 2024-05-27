using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;

public class LobbyUI : BaseUI
{
    
    [Header("Buttons")]
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private Button backToMainMenuButton;
    
    [Header("Windows")]
    [SerializeField] private Transform createLobbyWindow;
    [SerializeField] private Transform joinLobbyWindow;
    [SerializeField] private MainMenuUI mainMenu;
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;

    private void Start() {
        createLobbyButton.onClick.AddListener(CreateLobbyButton_OnClick);
        joinLobbyButton.onClick.AddListener(JoinLobbyButton_OnClick);
        backToMainMenuButton.onClick.AddListener(BackToMainMenuButton_OnClick);
        Hide();
    }

    private void HideWindows() {
        createLobbyWindow.gameObject.SetActive(false);
        joinLobbyWindow.gameObject.SetActive(false);
    }

    private void CreateLobbyButton_OnClick() {
        HideWindows();
        createLobbyWindow.gameObject.SetActive(true);
    }
    
    private void JoinLobbyButton_OnClick() {
        HideWindows();
        joinLobbyWindow.gameObject.SetActive(true);
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
