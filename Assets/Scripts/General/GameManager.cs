using System;
using Unity.Netcode;

public class GameManager : SingletonNetwork<GameManager>
{
    public Action<GameState> OnGameStateChanged;
    private NetworkVariable<GameState> networkedGameState;

    public override void Awake() {
        base.Awake();
        networkedGameState = new();
    }

    public override void OnNetworkSpawn(){
        networkedGameState.OnValueChanged += (previousState, newState) => OnGameStateChanged?.Invoke(newState);
    }

    public GameState GetGameState() => networkedGameState.Value;
    public void SetGameState(GameState newState) {
        SetGameStateServerRpc(newState);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetGameStateServerRpc(GameState state){
        networkedGameState.Value = state;
    }
}