using System;
using Unity;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AlongUsMultiplayer : SingletonNetworkPersistent<AlongUsMultiplayer>
{
    private string playerName = "";

    private void Start() {
        NameMenuUI.Instance.OnNameSet += NameMenuUI_OnNameSet;

        LobbyManager.Instance.OnLobbyJoined += LobbyManager_OnLobbyJoined;
    }

    private void LobbyManager_OnLobbyJoined(Lobby lobby) {
        if (LobbyManager.Instance.IsLobbyHost()) {
            Loader.Instance.LoadScene(Loader.Scene.CharacterSelectionScene);
        } else {
            
        }
    }

    private void NameMenuUI_OnNameSet(string playerName) => this.playerName = playerName;
    public string GetPlayerName() => playerName;
}
