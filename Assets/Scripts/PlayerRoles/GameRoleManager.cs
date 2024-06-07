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
    private const int reach = 5;

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
                    // Kill the target
                };
            case PlayerAction.Invisible:
                return () => {
                    AlongUsMultiplayer.Instance.SetPlayerInvisibleServerRpc(Player.LocalInstance.NetworkObject);
                };
            case PlayerAction.Reveal:
                return () => {
                    if(TryGetRoleInFront(out PlayerRole role)){
                        Debug.Log($"This is a {role}");
                    }
                };
            default:
                return null;
        }
    }

    private bool TryGetRoleInFront(out PlayerRole playerRole){
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        
        if (!Physics.Raycast(ray, out RaycastHit hit, reach)) { 
            playerRole = PlayerRole.Nothing;
            return false; 
        }
        if (!hit.transform.TryGetComponent(out PlayerRoleManager playerRoleManager)) { 
            playerRole = PlayerRole.Nothing;
            return false; 
        }

        playerRole = playerRoleManager.GetRole();
        return true;
    }
}