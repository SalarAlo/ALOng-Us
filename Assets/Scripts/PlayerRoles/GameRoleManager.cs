using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class GameRoleManager : Singleton<GameRoleManager>
{
    public List<PlayerRoleDataSO> gameRoleDatas;
    public Dictionary<PlayerRole, PlayerRoleDataSO> playerRoleDataDict;
    private const int reach = 5;

    public override void Awake() {
        base.Awake();
        playerRoleDataDict = new();

        foreach (PlayerRoleDataSO data in gameRoleDatas) {
            playerRoleDataDict[data.playerRole] = data;
        }
    }

    public PlayerRoleDataSO GetDataForRole(PlayerRole playerGameRole) {
        return playerRoleDataDict[playerGameRole];
    }

    public ActionDataSO GetDataForAction(PlayerAction action) {
        foreach(PlayerRoleDataSO gameRoleData in playerRoleDataDict.Values){
            if(gameRoleData.actions.Select(i => i.action).Contains(action)){
                return gameRoleData.actions.First(act => act.action == action);
            }
        }
        return null;
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