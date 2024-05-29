using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
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
    [SerializeField] private LobbyMenuUI lobbyMenuUI;
    [SerializeField] private Transform creditsWindow;
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;

    protected override void Awake() {
        base.Awake();
        
        quitGameButton.onClick.AddListener(QuitGameButton_OnClick);
        startGameButton.onClick.AddListener(StartGameButton_OnClick);
        creditsButton.onClick.AddListener(CreditsButton_OnClick);
        creditsBackButton.onClick.AddListener(CreditsBackButton_OnClick);
        settingsButton.onClick.AddListener(SettingsButton_OnClick);   

        Show();
    }

    private void SettingsButton_OnClick() {
        SettingsUI.Instance.Show();
    }


    private void CreditsBackButton_OnClick() {
        creditsWindow.gameObject.SetActive(false);
        Show();
    }

    private void CreditsButton_OnClick() {
        Hide();
        creditsWindow.gameObject.SetActive(true);
    }

    private void StartGameButton_OnClick() {
        lobbyMenuCam.gameObject.SetActive(true);
        mainMenuCam.gameObject.SetActive(false);
        Hide();
        lobbyMenuUI.Show();
    }

    private void QuitGameButton_OnClick(){
        Application.Quit();
    }
}
