using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class SingleLobbyUI : MonoBehaviour
{
    private Lobby lobby;

    [SerializeField] private TextMeshProUGUI playersAmountTextField;
    [SerializeField] private TextMeshProUGUI lobbyNameTextField;
    [SerializeField] private TextMeshProUGUI imposterAmountTextField;

    public void SetLobby(Lobby lobby) {
        this.lobby = lobby;
        lobbyNameTextField.text = lobby.Name;
        playersAmountTextField.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        imposterAmountTextField.text = $"{lobby.Data["imposters"].Value}";
    }

}
