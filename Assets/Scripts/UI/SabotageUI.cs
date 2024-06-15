using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SabotageUI : BaseUISingleton<SabotageUI>
{
    public Button colorBlindSabotage;
    public Button closeSabotage;
    
    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
        closeSabotage.onClick.AddListener(Hide);
    }

    private void Player_OnLocalInstanceInitialised() {
        PlayerRole role = Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        if (role != PlayerRole.Imposter) return;
        colorBlindSabotage.onClick.AddListener(ColorBlindSabotage_OnClick);
    }

    private void ColorBlindSabotage_OnClick() {
        PlayerData playerDataColorblind = new() {
            colorIndex = 0,
            playerName = "X"
        };

        foreach(PlayerData playerData in AlongUsMultiplayer.Instance.networkedPlayerDataList){
            AlongUsMultiplayer.Instance.ChangePlayerAppearanceTo(playerData.clientId, playerDataColorblind, 10, playerData);
        }

        Hide();
    }

    public override void Show() {
        base.Show();
        Player.LocalInstance.GetComponent<PlayerController>().DisableControls();
    }

    public override void Hide() {
        base.Hide();
        if(Player.LocalInstance != null)
            Player.LocalInstance.GetComponent<PlayerController>().EnableControls();
    }
}
