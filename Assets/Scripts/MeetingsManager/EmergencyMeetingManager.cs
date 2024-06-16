using System.Collections.Generic;
using UnityEngine;

public class EmergencyMeetingManager : MonoBehaviour
{
    [SerializeField] private List<EmergencySeat> emergencySpawnPoints;
    public void TriggerEmergency(PlayerData triggeredBy){
        List<Player> players = Player.GetAllPlayers();
        for(int i = 0; i < players.Count; i++) {
            emergencySpawnPoints[i].FillSeat(players[i]);
        }
        GameManager.Instance.SetGameState(GameState.Emergency);
    }
}
