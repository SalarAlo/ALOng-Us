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

    private void SpawnPlayers() {
        List<RoleCount> rolesList = new(playerRoles);
        for(int i = 0; i <  AlongUsMultiplayer.Instance.networkedPlayerDataList.Count; i++) {
            if(rolesList.Count == 0) return;

            int randIndex = Random.Range(0, rolesList.Count-1);
            while (rolesList[randIndex].count == 0) {
                randIndex = Random.Range(0, rolesList.Count);
            }
            rolesList[randIndex].count--;

            PlayerData playerData = AlongUsMultiplayer.Instance.networkedPlayerDataList[i];
            NetworkObject playerSpawned = Instantiate(playerPrefab, spawnPositions[i].position, Quaternion.identity).GetComponent<NetworkObject>();
            playerSpawned.SpawnAsPlayerObject(playerData.clientId, true);

            if(rolesList[randIndex].count == 0) {
                rolesList.RemoveAt(randIndex);
            }
            
            SyncDataOfPlayerClientRpc(playerSpawned, playerData, rolesList[randIndex].role);
        }
    }

    [ClientRpc]
    private void SyncDataOfPlayerClientRpc(NetworkObjectReference playerNetworkObjectReference, PlayerData playerData, PlayerRole role){
        playerNetworkObjectReference.TryGet(out var playerNetworkObject);
        playerNetworkObject.GetComponent<PlayerVisuals>().SetColorTo(playerData.colorIndex);
        playerNetworkObject.GetComponent<PlayerVisuals>().SetPlayerName(playerData.playerName.ToString());
        playerNetworkObject.GetComponent<PlayerRoleManager>().SetRole(role);
    }
}

[System.Serializable]
public class RoleCount {
    public PlayerRole role;
    public int count;
}