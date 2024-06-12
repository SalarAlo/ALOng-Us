using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class GameRoleManager : Singleton<GameRoleManager>
{
    public List<PlayerRoleDataSO> gameRoleDatas;
    public Dictionary<PlayerRole, PlayerRoleDataSO> playerRoleDataDict;

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



}