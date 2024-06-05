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
}