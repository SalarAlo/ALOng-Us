using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameRoleManager : Singleton<GameRoleManager>
{
    public List<GameRoleData> gameRoleDatas;
    public Dictionary<PlayerGameRole, GameRoleData> playerRoleDataDict;

    public override void Awake() {
        base.Awake();
        playerRoleDataDict = new();

        foreach (GameRoleData data in gameRoleDatas) {
            playerRoleDataDict[data.role] = data;
        }
    }

    public GameRoleData GetDataForRole(PlayerGameRole playerGameRole) {
        return playerRoleDataDict[playerGameRole];
    }
}

[Serializable]
public class GameRoleData {
    public PlayerGameRole role;
    public Color color;
    public string description;
    public bool canCompleteTasks;
}