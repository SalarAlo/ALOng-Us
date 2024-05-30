using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private CharacterSelectPlayerPosition[] playerPositions;
    [SerializeField] private CharacterSelectPlayerPosition ownPlayerPos;
    [SerializeField] private Color[] chosableColors;

    private void Start() {
        AlongUsMultiplayer.Instance.OnNetworkedPlayerDataListChanged += AlongUsMultiplayer_OnNetworkedPlayerDataListChanged;
        UpdatePlayers();
    }

    private void AlongUsMultiplayer_OnNetworkedPlayerDataListChanged() {
        UpdatePlayers();
    }

    private void UpdatePlayers(){
        foreach (var playerPos in playerPositions) 
            playerPos.Clear();

        foreach (PlayerData playerData in AlongUsMultiplayer.Instance.networkedPlayerDataList) {
            CharacterSelectPlayerPosition playerPosition = playerPositions[AlongUsMultiplayer.Instance.networkedPlayerDataList.IndexOf(playerData)];
            if (NetworkManager.Singleton.LocalClientId == playerData.clientId) ownPlayerPos = playerPosition;
            playerPosition.PopulateWithPlayer(playerData);
        }
    }

    private CharacterSelectPlayerPosition GetNextEmptyPlayerPosition() => playerPositions.First(playerPos => playerPos.IsEmpty());

    public Color[] GetChoosableColors() {
        return chosableColors;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            UpdatePlayers();
        }
    }
}
