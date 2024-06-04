using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleLobbyUI : MonoBehaviour, IPointerDownHandler
{
    private Lobby lobby;

    [SerializeField] private TextMeshProUGUI playersAmountTextField;
    [SerializeField] private TextMeshProUGUI lobbyNameTextField;

    public void OnPointerDown(PointerEventData _) {
        LobbyManager.Instance.JoinLobbyByIdAsync(lobby.Id);
    }

    public void SetLobby(Lobby lobby) {
        Debug.Log("Setting lobby to " + lobby.Name + " with " + lobby.Id + " ID of the instance " + name);
        this.lobby = lobby;
        lobbyNameTextField.text = lobby.Name;
        playersAmountTextField.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }

}
