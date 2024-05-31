using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelection : SingletonNetwork<CharacterSelection>
{
    [SerializeField] private CharacterSelectPlayerPosition[] playerPositions;
    [SerializeField] private Transform colorButtonsParent;
    [SerializeField] private CharacterColorSelectionButton colorSelectionButtonPrefab;
    [SerializeField] private Color[] chosableColors;

    public override void Awake() {
        base.Awake();
        InstantiateColorSelectionButtons();
    }

    private void Start() {
        UpdatePlayers();
        
        AlongUsMultiplayer.Instance.OnPlayerDataListLengthChanged += AlongUsMultiplayer_OnPlayerDataListLengthChanged;
        AlongUsMultiplayer.Instance.OnAnyPlayerColorChanged += AlongUsMultiplayer_OnAnyPlayerColorChanged;
    }

    private void InstantiateColorSelectionButtons(){
        for(int i = 0; i < chosableColors.Length; i++) {
            CharacterColorSelectionButton colorSelectionButton = Instantiate(colorSelectionButtonPrefab, colorButtonsParent);
            colorSelectionButton.SetColor(i);
        }
    }
    
    private void AlongUsMultiplayer_OnAnyPlayerColorChanged() {
        UpdatePlayers();
    }


    private void AlongUsMultiplayer_OnPlayerDataListLengthChanged() {
        UpdatePlayers();
    }

    public Color GetColorAtIndex(int index){
        return chosableColors[index];
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
