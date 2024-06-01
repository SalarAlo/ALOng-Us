using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ColorSelectionManager : SingletonNetwork<ColorSelectionManager>
{
    // sort of acts like a dict where the key is just the index of the color
    public NetworkList<bool> networkedAvaibleColorList;
    [SerializeField] private Color[] chosableColors;
    
    [SerializeField] private Transform colorButtonsParent;
    [SerializeField] private CharacterColorSelectionButton colorSelectionButtonPrefab;

    public override void Awake() {
        base.Awake();
        networkedAvaibleColorList = new();
        InstantiateColorSelectionButtons();
    }

    public override void OnNetworkSpawn() {
        if(!IsServer) return;

        for (int i = 0; i < AlongUsMultiplayer.MAX_PLAYERS_PER_LOBBY; i++) {
            networkedAvaibleColorList.Add(true);
        }
    }

    public Transform GetColorButtonsParent() => colorButtonsParent;
    
    private void InstantiateColorSelectionButtons(){
        for(int i = 0; i < chosableColors.Length; i++) {
            CharacterColorSelectionButton colorSelectionButton = Instantiate(colorSelectionButtonPrefab, colorButtonsParent);
            colorSelectionButton.SetColor(i);
        }
    }
    
    public Color GetColorAtIndex(int index){
        return chosableColors[index];
    }

    
    public void SetColorOfLocalClient(int colorIndex) {
        SetColorOfClientServerRpc(NetworkManager.Singleton.LocalClientId, colorIndex);
    }

    
    public bool IsColorAvaible(int colorIndex) {
        return networkedAvaibleColorList[colorIndex];
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetColorOfClientServerRpc(ulong clientId, int newColorIndex) {
        if(!IsColorAvaible(newColorIndex)) return;

        for(int i = 0; i < AlongUsMultiplayer.Instance.networkedPlayerDataList.Count; i++) {
            PlayerData playerData = AlongUsMultiplayer.Instance.networkedPlayerDataList[i];
            int previousColorIndex = playerData.colorIndex;
            if (playerData.clientId == clientId) {
                networkedAvaibleColorList[previousColorIndex] = true;
                networkedAvaibleColorList[newColorIndex] = false;
                
                playerData.colorIndex = newColorIndex;
                AlongUsMultiplayer.Instance.SetColorOfPlayer(playerData.clientId, playerData.colorIndex);
                return;
            }
        }
    }
    
    public int GetAvaiableColorIndex() {
        for (int i = 0; i < AlongUsMultiplayer.MAX_PLAYERS_PER_LOBBY; i++) {
            if (IsColorAvaible(i)) return i;
        }

        return -1;
    }

    public void SetColorInavaiable(int colorIndex) {
        networkedAvaibleColorList[colorIndex] = false;
    }

    public void SetColorAvaiable(int colorIndex) {
        networkedAvaibleColorList[colorIndex] = true;
    } 
}
