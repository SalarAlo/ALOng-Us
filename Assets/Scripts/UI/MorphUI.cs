using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MorphUI : BaseUISingleton<MorphUI>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private SinglePlayerMorphUI singlePlayerMorphUIPrefab;
    [SerializeField] private Transform playerSelectionParent;

    public override void Awake() {
        base.Awake();
        closeButton.onClick.AddListener(Hide);
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised(){
        FillSelectionWithPlayers();
    }

    private void FillSelectionWithPlayers(){
        if(Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole() != PlayerRole.Morpher) return;

        foreach(PlayerData playerData in AlongUsMultiplayer.Instance.networkedPlayerDataList) {
            Debug.Log($"Currently iterating over {playerData.playerName}");
            if(playerData.clientId == NetworkManager.Singleton.LocalClientId) continue;

            var singlePlayerMorphUI = Instantiate(singlePlayerMorphUIPrefab, playerSelectionParent);
            singlePlayerMorphUI.SetPlayerMorphUI(playerData);
        }
    }

    public override void Show() {
        base.Show();
        PlayerCam.Instance.SetCanLookAround(false);
        Player.LocalInstance.GetComponent<PlayerController>().SetCanMove(false);
    }

    public override void Hide() {
        base.Hide();
        PlayerCam.Instance.SetCanLookAround(true);
        if(Player.LocalInstance != null)
            Player.LocalInstance.GetComponent<PlayerController>().SetCanMove(true);
    }
}
