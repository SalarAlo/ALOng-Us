using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private LobbyMenuUI lobbyUI;
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;

    private void Awake() {
        quitGameButton.onClick.AddListener(QuitGameButton_OnClick);
        startGameButton.onClick.AddListener(StartGameButton_OnClick);

        Show();
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
}
