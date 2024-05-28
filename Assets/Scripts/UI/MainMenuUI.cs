using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [Header("Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsBackButton;
    
    [Header("Windows")]
    [SerializeField] private LobbyMenuUI lobbyUI;
    [SerializeField] private Transform creditsUI;
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;
    
    // [Header("Config")]

    private void Awake() {
        quitGameButton.onClick.AddListener(QuitGameButton_OnClick);
        startGameButton.onClick.AddListener(StartGameButton_OnClick);
        creditsButton.onClick.AddListener(CreditsButton_OnClick);
        creditsBackButton.onClick.AddListener(CreditsBackButton_OnClick);
        
        HideWindows();
        Show();
    }

    private void HideWindows() {
        creditsUI.gameObject.SetActive(false);
    }

    private void CreditsBackButton_OnClick() {
        creditsUI.gameObject.SetActive(false);
        Show();
    }

    private void CreditsButton_OnClick() {
        Hide();
        creditsUI.gameObject.SetActive(true);
    }

    private void StartGameButton_OnClick() {
        lobbyMenuCam.gameObject.SetActive(true);
        mainMenuCam.gameObject.SetActive(false);
        Hide();
        lobbyUI.Show();
    }

    private void QuitGameButton_OnClick(){
        Application.Quit();
    }

    public override void Hide() {
        HideWindows();
        base.Hide();
    }
}
