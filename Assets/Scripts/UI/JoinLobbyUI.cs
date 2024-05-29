using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUI : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private SingleLobbyUI singleLobbyUIPrefab;

    [Header("Configuration")]
    [SerializeField] private Transform joinLobbyWindow;
    [SerializeField] private Transform lobbyParentUI;
    [SerializeField] private Button refreshLobbiesButton;


    private void Start() {
        refreshLobbiesButton.onClick.AddListener(RefreshLobbiesButton_OnClick);
    }

    private void RefreshLobbiesButton_OnClick() {
        RefreshLobbiesList();
    }

    private async void RefreshLobbiesList(){
        try { 
            foreach (Transform child in lobbyParentUI) child.gameObject.SetActive(false);

            List<Lobby> lobbies = await LobbyManager.Instance.QueryJoinableLobbies();
            foreach (Lobby lobby in lobbies) {
                SingleLobbyUI singleLobbyUI = Instantiate(singleLobbyUIPrefab, lobbyParentUI);
                singleLobbyUI.SetLobby(lobby);
            }
        } catch (LobbyServiceException e) {
            Debug.Log(e.Message);
        }
    }

    public void Hide() => joinLobbyWindow.gameObject.SetActive(false);
    public void Show() => joinLobbyWindow.gameObject.SetActive(true);
}
