using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionPlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshPro nameText;    
    [SerializeField] private Button kickButton;
    private PlayerData data;

    private void Start() {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        bool characterIsOwnedByHost = data.clientId == NetworkManager.ServerClientId;
        bool isHostGameInstance = NetworkManager.LocalClientId == NetworkManager.ServerClientId;
        
        if (characterIsOwnedByHost || !isHostGameInstance) {
            kickButton.gameObject.SetActive(false);
        }
    }

    public void SetClient(PlayerData data) {
        nameText.text = data.playerName.ToString();
        this.data = data;
        kickButton.onClick.AddListener(KickButton_OnClick);
    }

    private void KickButton_OnClick() {
        LobbyManager.Instance.KickPlayerFromCurrentLobbyAsync(AuthenticationService.Instance.PlayerId);
    }
}
