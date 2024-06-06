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
        LobbyManager.Instance.OnLobbyJoined += LobbyManager_OnLobbyJoined;
        NameMenuUI.Instance.OnNameSet += NameMenuUI_OnNameSet;
        NetworkManager.Singleton.OnServerStarted += NetworkManager_OnServerStarted;
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
    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerInvisibleServerRpc(NetworkObjectReference networkedPlayerObjectRef){
        SetPlayerInvisibleClientRpc(networkedPlayerObjectRef);
    }

    [ClientRpc]
    private void SetPlayerInvisibleClientRpc(NetworkObjectReference networkedPlayerObjectRef){
        networkedPlayerObjectRef.TryGet(out NetworkObject networkedPlayerObject);
        PlayerController playerController = networkedPlayerObject.GetComponent<PlayerController>();
        foreach (Transform child in playerController.GetVisualsParent()){
            child.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
        }
    }

    [ClientRpc]
    private void RegisterPlayerClientRpc(ClientRpcParams clientRpcReceiveParams = default) {
        RegisterPlayerServerRpc(playerName);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RegisterPlayerServerRpc(string playerName, ServerRpcParams serverRpcParams = default){
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        int avaiableColorIndex = ColorSelectionManager.Instance.GetAvaiableColorIndex();
        
        networkedPlayerDataList.Add(new() {
            clientId = clientId,
            playerName = playerName,
            colorIndex = avaiableColorIndex
        });
        
        ColorSelectionManager.Instance.SetColorInavaiable(avaiableColorIndex);
    }

    private void UnregisterPlayer(ulong clientId) {
        PlayerData playerData = default; 
        int i;
        for(i = 0; i < networkedPlayerDataList.Count; i++) {
            var data = networkedPlayerDataList[i];
            if(data.clientId == clientId) {
                playerData = data;
                break;
            }
        }
        ColorSelectionManager.Instance.SetColorAvaiable(playerData.colorIndex);
        networkedPlayerDataList.RemoveAt(i);
    }


    // Only invoked on the server bc. OnServerStarted is only invoked on server
    private void NetworkManager_OnServerStarted() {
        ClientRpcParams clientRpcParams = new() {
                Send = new ClientRpcSendParams() { 
                    TargetClientIds = new List<ulong>() {
                        NetworkManager.LocalClientId
                    }
                }
        };

        RegisterPlayerClientRpc(
            clientRpcParams
        );
        
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkedPlayerDataList_OnListChanged(NetworkListEvent<PlayerData> changeEvent) {
        OnPlayerDataListChanged?.Invoke();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        ClientRpcParams clientRpcParams = new() {
                Send = new ClientRpcSendParams() { 
                    TargetClientIds = new List<ulong>() {
                        clientId
                    }
                }
        };
        RegisterPlayerClientRpc(
            clientRpcParams
        );
    }
    
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
        UnregisterPlayer(clientId);
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

        Loader.Instance.OnSceneChanged -= Loader_OnSceneChanged;
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