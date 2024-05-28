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
    public Action<LobbyServiceException> OnFailOccured;
    private Lobby currentLobby;
    private const float HEARTBEAT_TIMER_MAX = 15;
    private float heartbeatTimer;

    public override void Awake() {
        base.Awake();
        InitializeServices();
        heartbeatTimer = HEARTBEAT_TIMER_MAX;
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
        } catch (LobbyServiceException e) {
            OnFailOccured?.Invoke(e);
        }
    }

    public async void CreateLobbyAsync(string name, int maxPlayers, int imposters) {
        try {
            CreateLobbyOptions options = new(){
                Data = new Dictionary<string, DataObject>() {
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
    }

    private async void HandleLobbyHeartbeat(){
        try {
            if (!IsLobbyHost()) return;
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0) {
                heartbeatTimer = HEARTBEAT_TIMER_MAX;
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
