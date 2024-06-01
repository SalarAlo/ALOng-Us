using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : SingletonPersistent<LobbyManager> 
{
    public Action<Lobby> OnLobbyJoined;
    public Action OnLobbyLeft;
    public Action<LobbyServiceException> OnFailOccured;
    private Lobby currentLobby;
    private const float HEARTBEAT_INTERVAL = 15;
    private const float LOBBY_RELOAD_INTERVAL = 3;
    private float heartbeatTimer;
    private float lobbyReloadTimer;

    public override void Awake() {
        base.Awake();
        InitializeServices();
        heartbeatTimer = HEARTBEAT_INTERVAL;
        lobbyReloadTimer = LOBBY_RELOAD_INTERVAL;
    }

    public async void KickPlayerFromCurrentLobbyAsync(string clientId){
        try {
            if (currentLobby == null) return;
            await Lobbies.Instance.RemovePlayerAsync(currentLobby.Id, clientId);
            OnLobbyLeft?.Invoke();
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }

    private async void InitializeServices(){
        try {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }

    public async void QuickJoinLobby() {
        try {
            currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            OnLobbyJoined?.Invoke(currentLobby);
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }

    public async void CreateLobbyAsync(string name, int maxPlayers, int imposters) {
        try {
            CreateLobbyOptions options = new(){
                Data = new() {
                    { "imposters", new DataObject(DataObject.VisibilityOptions.Public, imposters.ToString()) }
                }
            };
            currentLobby = await Lobbies.Instance.CreateLobbyAsync(name, maxPlayers, options);
            OnLobbyJoined?.Invoke(currentLobby);
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }

    public async Task<List<Lobby>> QueryJoinableLobbies() {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new() {
                Filters = new List<QueryFilter>() {
                    new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.NE)
                }
            };
            return (await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions)).Results;
        } catch (LobbyServiceException e)  {
            OnFailOccured?.Invoke(e);
        }
        return null;
    }

    private void Update() {
        HandleLobbyHeartbeat();
        HandleLobbyReload();
    }

    private void HandleLobbyReload(){
        if (JoinLobbyUI.Instance == null) return;

        lobbyReloadTimer -= Time.deltaTime;
        if (lobbyReloadTimer <= 0) {
            lobbyReloadTimer = LOBBY_RELOAD_INTERVAL;
            JoinLobbyUI.Instance.RefreshLobbiesList();
        }
    }

    private async void HandleLobbyHeartbeat(){
        try {
            if (!IsLobbyHost()) return;
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0) {
                heartbeatTimer = HEARTBEAT_INTERVAL;
                await Lobbies.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            }
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }

    public bool IsLobbyHost() => currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId; 

    public async void JoinLobbyByIdAsync(string lobbyId) {
        try {
            currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
            OnLobbyJoined?.Invoke(currentLobby);
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }
}
