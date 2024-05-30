using System;
using Unity;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AlongUsMultiplayer : SingletonNetworkPersistent<AlongUsMultiplayer>
{
    public Action OnNetworkedPlayerDataListChanged;
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

    private void NetworkManager_OnServerStarted() {
        Debug.Log("Now the server has started");
        // This player is the host so we first register this player
        RegisterPlayer(OwnerClientId);
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientJoined;
    }

    private void NetworkedPlayerDataList_OnListChanged(NetworkListEvent<PlayerData> changeEvent) {
        Debug.Log("Triggering Network Player Data change event");
        OnNetworkedPlayerDataListChanged?.Invoke();
    }


    private void NetworkManager_OnClientJoined(ulong clientId) {
        Debug.Log("Adding a new player to the List");
        RegisterPlayer(clientId);
    }

    private void RegisterPlayer(ulong clientId) {
        networkedPlayerDataList.Add(new() {
            clientId = clientId,
            playerName = playerName,
        });
    }

    private void LobbyManager_OnLobbyJoined(Lobby lobby) {
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
        NetworkManager.StartClient();
    }

    private void NameMenuUI_OnNameSet(string playerName) => this.playerName = playerName;
    public string GetPlayerName() => playerName;
}

public struct PlayerData : INetworkSerializable, IEquatable<PlayerData> {
        public ulong clientId;
        public FixedString64Bytes playerName;

        public bool Equals(PlayerData other)
        {
            return clientId == other.clientId && 
                other.playerName == playerName;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref playerName);
        }
    }