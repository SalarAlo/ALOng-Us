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
    [Header("Prefabs")]
    [SerializeField] private SingleLobbyUI singleLobbyUIPrefab;

    [Header("Buttons")]
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button refreshLobbiesButton;
    [SerializeField] private Button createLobbyButtonFinal;
    [SerializeField] private Button quickJoinButton;
    
    [Header("Windows")]
    [SerializeField] private Transform createLobbyWindow;
    [SerializeField] private Transform joinLobbyWindow;
    [SerializeField] private MainMenuUI mainMenu;
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainMenuCam;
    [SerializeField] private CinemachineVirtualCamera lobbyMenuCam;
    
    [Header("Config")]
    [SerializeField] private Transform lobbyParentUI;
    [SerializeField] private TMP_InputField lobbyNameTextField;
    [SerializeField] private Slider playersAmountSlider;
    [SerializeField] private Slider impostersAmountSlider;

    private void Start() {
        createLobbyButton.onClick.AddListener(CreateLobbyButton_OnClick);
        joinLobbyButton.onClick.AddListener(JoinLobbyButton_OnClick);
        backToMainMenuButton.onClick.AddListener(BackToMainMenuButton_OnClick);
        refreshLobbiesButton.onClick.AddListener(RefreshLobbiesButton_OnClick);
        createLobbyButtonFinal.onClick.AddListener(CreateLobbyButtonFinal_OnClick);
        quickJoinButton.onClick.AddListener(QuickJoinButton_OnClick);
        Hide();
    }

    private void QuickJoinButton_OnClick() {
        LobbyManager.Instance.QuickJoinLobby();
    }

    private  void CreateLobbyButtonFinal_OnClick(){
        string lobbyName = lobbyNameTextField.text;
        int maxPlayers = (int)playersAmountSlider.value;
        int imposters = (int)impostersAmountSlider.value;
        // TODO: Render some error message using ui...
        if (lobbyName == "") return;
        
        LobbyManager.Instance.CreateLobbyAsync(lobbyName, maxPlayers, imposters);
        RefreshLobbiesList();
        HideWindows();
    }
    private void RefreshLobbiesButton_OnClick() {
        RefreshLobbiesList();
    }

    private async void RefreshLobbiesList(){
        try { 
            foreach (Transform child in lobbyParentUI) child.gameObject.SetActive(false);

            List<Lobby> lobbies = await LobbyManager.Instance.QueryLobbies();
            foreach (Lobby lobby in lobbies) {
                SingleLobbyUI singleLobbyUI = Instantiate(singleLobbyUIPrefab, lobbyParentUI);
                singleLobbyUI.SetLobby(lobby);
            }
        } catch (LobbyServiceException e) {
            Debug.Log(e.Message);
        }
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
