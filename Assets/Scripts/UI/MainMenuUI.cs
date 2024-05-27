using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private LobbyUI lobbyUI;
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;

    private void Awake() {
        quitGameButton.onClick.AddListener(() => {
            Application.Quit();
        });

        startGameButton.onClick.AddListener(() => {
            lobbyMenuCam.gameObject.SetActive(true);
            mainMenuCam.gameObject.SetActive(false);
            Hide();
            lobbyUI.Show();
        });

        Show();
    }
}
