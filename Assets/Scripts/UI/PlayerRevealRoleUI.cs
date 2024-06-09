using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerRevealRoleUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI roleTextField;
    [SerializeField] private TextMeshProUGUI roleDescriptionTextField;
    [SerializeField] private RoleRevealBackground roleRevealBackground;
    [SerializeField] private int secondsWaiting;
    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised() {
        TriggerRoleRevealUI();
    }
    private void TriggerRoleRevealUI() {
        Show();
        var roleData = GameRoleManager.Instance.GetDataForRole(Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole());

        roleTextField.text = roleData.playerRole.ToString();
        roleTextField.color = roleData.color;
        roleDescriptionTextField.text = roleData.description.ToString();
        
        roleRevealBackground.SetColor(roleData.color);
        roleRevealBackground.StartAnim(secondsWaiting-1);

        Invoke(nameof(RoleRevealFinished), secondsWaiting);
    }

    private void RoleRevealFinished(){
        Hide();
        Player.LocalInstance.GetComponent<PlayerController>().SetCanMove(true);
        Destroy(gameObject);
    }
}
