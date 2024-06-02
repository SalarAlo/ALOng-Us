using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private PlayerController playerPrefab;
    
    private void Start() {
        if(!IsServer) return;

        SpawnPlayers();
    }

    private void SpawnPlayers() {
        for(int i = 0; i <  AlongUsMultiplayer.Instance.networkedPlayerDataList.Count; i++) {
            PlayerData playerData = AlongUsMultiplayer.Instance.networkedPlayerDataList[i];
            NetworkObject playerSpawned = Instantiate(playerPrefab, spawnPositions[i].position, Quaternion.identity).GetComponent<NetworkObject>();
            playerSpawned.SpawnAsPlayerObject(playerData.clientId);
            playerSpawned.GetComponent<PlayerVisuals>().SetColorTo(ColorSelectionManager.Instance.GetColorAtIndex(playerData.colorIndex));
        }
    }
}
