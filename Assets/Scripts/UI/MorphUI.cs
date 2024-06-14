using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("1");
        if(Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole() != PlayerRole.Morpher) return;
        Debug.Log("2");

        foreach(PlayerData playerData in AlongUsMultiplayer.Instance.networkedPlayerDataList) {
            Debug.Log("3");
            // TODO: implement skipping of localInstance!
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
