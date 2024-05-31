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
    public Action OnColorAvaiabilityListChanged;
    
    public NetworkList<PlayerData> networkedPlayerDataList;
    public NetworkList<bool> networkedAvaibleColorList;
    private string playerName = "";

    public override void Awake() {
        base.Awake();   
        networkedPlayerDataList = new();
        networkedAvaibleColorList = new() ;
        networkedPlayerDataList.OnListChanged += NetworkedPlayerDataList_OnListChanged;
        
    }

    public override void OnNetworkSpawn() {
        if (networkedAvaibleColorList.Count == 0) {
            Debug.Log("Initialiing List");
            InitNetworkedAvaiableColorIndexListServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InitNetworkedAvaiableColorIndexListServerRpc() {
        Debug.Log("Salars Brain is slowly melting");
        for (int i = 0; i < MAX_PLAYERS_PER_LOBBY; i++) {
            networkedAvaibleColorList.Add(true);
            Debug.Log("color with index " + i + " is " + (networkedAvaibleColorList[i] ? "avaiable" : "not avaiable"));
        }
    }

    private void Start() {
        NetworkManager.Singleton.OnServerStarted += NetworkManager_OnServerStarted;
        LobbyManager.Instance.OnLobbyJoined += LobbyManager_OnLobbyJoined;
        NameMenuUI.Instance.OnNameSet += NameMenuUI_OnNameSet;
    }

    public void SetColorOfLocalClient(int colorIndex) {
        SetColorOfClientServerRpc(NetworkManager.Singleton.LocalClientId, colorIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetColorOfClientServerRpc(ulong clientId, int newColorIndex) {
        if(!IsColorAvaible(newColorIndex)) return;

        for(int i = 0; i < networkedPlayerDataList.Count; i++) {
            PlayerData playerData = networkedPlayerDataList[i];
            int previousColorIndex = playerData.colorIndex;
            if (playerData.clientId == clientId) {
                networkedAvaibleColorList[previousColorIndex] = true;
                networkedAvaibleColorList[newColorIndex] = false;
                
                playerData.colorIndex = newColorIndex;
                networkedPlayerDataList[i] = playerData;
                return;
            }
        }
    }

    public PlayerData GetLocalPlayerData(){ 
        foreach(var playerData in networkedPlayerDataList) {
            if (playerData.clientId == NetworkManager.LocalClientId) return playerData;
        }
        return default;
    }

    public bool IsColorAvaible(int colorIndex) {
        return networkedAvaibleColorList[colorIndex];
    }
    
    public int GetAvaiableColorIndex() {
        for (int i = 0; i < MAX_PLAYERS_PER_LOBBY; i++) {
            if (IsColorAvaible(i)) return i;
        }

        return -1;
    }


    private void NetworkManager_OnServerStarted() {
        RegisterPlayer(OwnerClientId);
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void NetworkedPlayerDataList_OnListChanged(NetworkListEvent<PlayerData> changeEvent) {
        OnPlayerDataListChanged?.Invoke();
    }


    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        Debug.Log("Adding a new player to the List");
        RegisterPlayer(clientId);
    }

    private void RegisterPlayer(ulong clientId) {
        Debug.Log("Registering Client...");
        int avaiableColorIndex = GetAvaiableColorIndex();
        networkedPlayerDataList.Add(new() {
            clientId = clientId,
            playerName = playerName,
            colorIndex = avaiableColorIndex
        });
        networkedAvaibleColorList[avaiableColorIndex] = false;
    }

    private void LobbyManager_OnLobbyJoined(Lobby lobby) {
        // Error must be here
        if (LobbyManager.Instance.IsLobbyHost()) {
            Loader.Instance.LoadScene(Loader.Scene.CharacterSelectionScene);
            StartHost();
        } else {
            StartClient();
        }
    }

    private void StartHost() {
        NetworkManager.StartHost();
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