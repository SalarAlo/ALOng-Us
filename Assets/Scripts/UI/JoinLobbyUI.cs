using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUI : BaseUISingleton<JoinLobbyUI>
{

    [Header("Prefabs")]
    [SerializeField] private SingleLobbyUI singleLobbyUIPrefab;

    [Header("Configuration")]
    [SerializeField] private Transform lobbyParentUI;


    public async void RefreshLobbiesList(){
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
}
