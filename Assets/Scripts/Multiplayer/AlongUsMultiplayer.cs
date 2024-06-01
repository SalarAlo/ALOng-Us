using System;
using System.Collections.Generic;
using Unity;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AlongUsMultiplayer : SingletonNetworkPersistent<AlongUsMultiplayer>
{
    public const int MAX_PLAYERS_PER_LOBBY = 10;
    public Action OnPlayerDataListChanged;
    public NetworkList<PlayerData> networkedPlayerDataList;
    private string playerName = "";

    public override void Awake() {
        base.Awake();   
        networkedPlayerDataList = new();
        networkedPlayerDataList.OnListChanged += NetworkedPlayerDataList_OnListChanged;
    }

    private void Start() {
        NetworkManager.Singleton.OnServerStarted += NetworkManager_OnServerStarted;
        LobbyManager.Instance.OnLobbyJoined += LobbyManager_OnLobbyJoined;
        NameMenuUI.Instance.OnNameSet += NameMenuUI_OnNameSet;
    }

    public void SetColorOfPlayer(ulong clientId, int colorIndex){
        for(int i = 0;  i < networkedPlayerDataList.Count; i++) {
            var data = networkedPlayerDataList[i];
            if(data.clientId == clientId) {
                data.colorIndex = colorIndex;
                networkedPlayerDataList[i] = data;
                break;
            }
        }
    }

    public PlayerData GetLocalPlayerData(){ 
        foreach(var playerData in networkedPlayerDataList) {
            if (playerData.clientId == NetworkManager.LocalClientId) return playerData;
        }
        return default;
    }


    private void RegisterPlayer(ulong clientId) {
        int avaiableColorIndex = ColorSelectionManager.Instance.GetAvaiableColorIndex();
        networkedPlayerDataList.Add(new() {
            clientId = clientId,
            playerName = playerName,
            colorIndex = avaiableColorIndex
        });
        ColorSelectionManager.Instance.SetColorInavaiable(avaiableColorIndex);
    }


    private void NetworkManager_OnServerStarted() {
        RegisterPlayer(OwnerClientId);
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void NetworkedPlayerDataList_OnListChanged(NetworkListEvent<PlayerData> changeEvent) {
        OnPlayerDataListChanged?.Invoke();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        RegisterPlayer(clientId);
    }
    private void LobbyManager_OnLobbyJoined(Lobby lobby) {
        Loader.Instance.LoadScene(Loader.Scene.CharacterSelectionScene);
        Loader.Instance.OnSceneChanged += Loader_OnSceneChanged;
    }

    private void Loader_OnSceneChanged(Loader.Scene scene){
        if(LobbyManager.Instance.IsLobbyHost()) {
            StartHost();
        } else {
            StartClient();
        }

        Loader.Instance.OnSceneChanged += Loader_OnSceneChanged;
    }

    private void StartHost() {
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient() {
        NetworkManager.Singleton.StartClient();
    }

    private void NameMenuUI_OnNameSet(string playerName) => this.playerName = playerName;
    public string GetPlayerName() => playerName;
}

public struct PlayerData : INetworkSerializable, IEquatable<PlayerData> {
        public ulong clientId;
        public FixedString64Bytes playerName;
        public int colorIndex;

        public bool Equals(PlayerData other)
        {
            return clientId == other.clientId && 
                other.playerName == playerName && 
                other.colorIndex == colorIndex;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref playerName);
            serializer.SerializeValue(ref colorIndex);
        }
    }