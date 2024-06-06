using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameRoleManager : Singleton<GameRoleManager>
{
    public List<GameRoleData> gameRoleDatas;
    public Dictionary<PlayerRole, GameRoleData> playerRoleDataDict;

    public override void Awake() {
        base.Awake();
        playerRoleDataDict = new();

        foreach (GameRoleData data in gameRoleDatas) {
            playerRoleDataDict[data.role] = data;
        }
    }

    public GameRoleData GetDataForRole(PlayerRole playerGameRole) {
        return playerRoleDataDict[playerGameRole];
    }

    public Action GetExecutableForAction(PlayerAction playerAction){
        switch(playerAction){
            case PlayerAction.Kill:
                return () => {
                    Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    
                    if (!Physics.Raycast(ray, out RaycastHit hit)) return;
                    if (!hit.transform.TryGetComponent(out PlayerRoleManager playerRoleManager)) return;
                    if (playerRoleManager.GetRole() == PlayerRole.Imposter) return;

                    // Kill the target
                };
            case PlayerAction.Invisible:
                return () => {
                    AlongUsMultiplayer.Instance.SetPlayerInvisibleServerRpc(PlayerController.LocalInstance.NetworkObject);
                };
            default:
                return null;
        }
    }
}