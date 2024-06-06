using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform playerPrefab;
    public int maxPlayerAmount;
    
    public List<RoleCount> playerRoles = new List<RoleCount>();
    
    
    public override void OnNetworkSpawn() {
        if(!IsServer) return;

        SpawnPlayers();
    }
    
    private void OnValidate() {
        // Ensure all roles are present
        foreach (PlayerRole role in System.Enum.GetValues(typeof(PlayerRole))) {
            if (!playerRoles.Exists(r => r.role == role)) {
                playerRoles.Add(new RoleCount { role = role, count = 0 });
            }
        }

        // Ensure no extra roles are present
        playerRoles.RemoveAll(r => !System.Enum.IsDefined(typeof(PlayerRole), r.role));
    }

    private void SpawnPlayers() {
        List<RoleCount> rolesList = new(playerRoles);
        for(int i = 0; i <  AlongUsMultiplayer.Instance.networkedPlayerDataList.Count; i++) {
            RoleCount roleToUse = rolesList[Random.Range(0, rolesList.Count-1)];
            roleToUse.count--;
            // No player should obtain that role anymore!
            if(roleToUse.count == 0) {
                int roleIndex = rolesList.FindIndex(r => r.role == roleToUse.role);
                rolesList.RemoveAt(roleIndex);
            }

            PlayerData playerData = AlongUsMultiplayer.Instance.networkedPlayerDataList[i];
            NetworkObject playerSpawned = Instantiate(playerPrefab, spawnPositions[i].position, Quaternion.identity).GetComponent<NetworkObject>();
            playerSpawned.SpawnAsPlayerObject(playerData.clientId, true);
            SyncDataOfPlayerClientRpc(playerSpawned, playerData.colorIndex, roleToUse.role);
        }
    }

    [ClientRpc]
    private void SyncDataOfPlayerClientRpc(NetworkObjectReference playerNetworkObjectReference, int colorIndex, PlayerRole role){
        playerNetworkObjectReference.TryGet(out var playerNetworkObject);
        playerNetworkObject.GetComponent<PlayerVisuals>().SetColorTo(colorIndex);
        playerNetworkObject.GetComponent<PlayerRoleManager>().SetRole(role);
    }
}

[System.Serializable]
public class RoleCount {
    public PlayerRole role;
    public int count;
}