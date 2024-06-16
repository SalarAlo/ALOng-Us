using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EmergencyMeetingManager : SingletonNetwork<EmergencyMeetingManager>
{
    [SerializeField] private List<EmergencySeat> emergencySpawnPoints;
    public void TriggerEmergency(PlayerData triggeredBy){
        TriggerEmergencyServerRpc(triggeredBy);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TriggerEmergencyServerRpc(PlayerData triggeredBy){
        GameManager.Instance.SetGameState(GameState.Emergency);
        TriggerEmergencyClientRpc(triggeredBy);
    }

    [ClientRpc]
    public void TriggerEmergencyClientRpc(PlayerData triggeredBy){
        List<Player> players = Player.GetAllPlayers();
        players.Sort(
            (player1, player2) => player1.GetPlayerData().playerName.ToString().CompareTo(
                player2.GetPlayerData().playerName.ToString()
            )
        );
        for(int i = 0; i < players.Count; i++) {
            emergencySpawnPoints[i].FillSeat(players[i]);
        }
    }

}
