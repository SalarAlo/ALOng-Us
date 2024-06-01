using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelection : SingletonNetwork<CharacterSelection>
{
    [SerializeField] private CharacterSelectPlayerPosition[] playerPositions;

    public override void Awake() {
        base.Awake();
    }

    private void Start() {
        AlongUsMultiplayer.Instance.OnPlayerDataListChanged += AlongUsMultiplayer_OnPlayerDataListChanged;
        UpdatePlayers();
        UpdateButtons();
    }

    private void AlongUsMultiplayer_OnPlayerDataListChanged() {
        UpdatePlayers();
        UpdateButtons();
    }

    private void UpdateButtons() {
        foreach (Transform colorButtonTransform in ColorSelectionManager.Instance.GetColorButtonsParent()) {
            CharacterColorSelectionButton colorButtonColorSelectionComponent = colorButtonTransform.GetComponent<CharacterColorSelectionButton>();
            colorButtonColorSelectionComponent.UpdateButtonAvaiability();
        }
    }

    private void UpdatePlayers(){
        // Clear every playerPos so that no playerPos is occupied
        foreach (var playerPos in playerPositions) 
            playerPos.Clear();

        // iterate over each connected client data 
        for (int i = 0; i < AlongUsMultiplayer.Instance.networkedPlayerDataList.Count; i++){
            PlayerData playerData = AlongUsMultiplayer.Instance.networkedPlayerDataList[i];
            playerPositions[i].PopulateWithPlayer(playerData);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            UpdatePlayers();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            DebugEverything();
        }
    }

    private void DebugEverything(){
        Debug.Log($"there are currently {AlongUsMultiplayer.Instance.networkedPlayerDataList.Count} Clients connected");
        foreach(PlayerData playerData in AlongUsMultiplayer.Instance.networkedPlayerDataList) {
            Debug.Log($"this player with the name, {playerData.playerName} has an id of {playerData.clientId} and the colorIndex {playerData.colorIndex}");
        }
    }
}
