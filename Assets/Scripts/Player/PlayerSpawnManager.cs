using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform playerPrefab;
    
    public override void OnNetworkSpawn() {
        if(!IsServer) return;

        SpawnPlayers();
    }

    private void SpawnPlayers() {
        for(int i = 0; i <  AlongUsMultiplayer.Instance.networkedPlayerDataList.Count; i++) {
            PlayerData playerData = AlongUsMultiplayer.Instance.networkedPlayerDataList[i];
            Debug.Log($"SpawnManager, Spawning Player, for loop ({i}, {playerData.playerName})");
            NetworkObject playerSpawned = Instantiate(playerPrefab, spawnPositions[i].position, Quaternion.identity).GetComponent<NetworkObject>();
            playerSpawned.SpawnAsPlayerObject(playerData.clientId, true);
            SyncColorOfPlayerClientRpc(playerSpawned, playerData.colorIndex);
        }
    }

    [ClientRpc]
    private void SyncColorOfPlayerClientRpc(NetworkObjectReference playerNetworkObjectReference, int colorIndex){
        playerNetworkObjectReference.TryGet(out var playerNetworkObject);
        playerNetworkObject.GetComponent<PlayerVisuals>().SetColorTo(ColorSelectionManager.Instance.GetColorAtIndex(colorIndex));
    }
}
