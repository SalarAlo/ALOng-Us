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
    [SerializeField] private int secondsWaiting;
    private void Start() {
        PlayerController.OnInitialised += PlayerController_OnInitialised;
    }

    private void PlayerController_OnInitialised() {
        TriggerRoleRevealUI();
    }
    private void TriggerRoleRevealUI() {
        Show();

        GameRoleData roleData = GameRoleManager.Instance.GetDataForRole(PlayerController.LocalInstance.GetComponent<PlayerRoleManager>().GetRole());

        roleTextField.text = roleData.role.ToString();
        roleTextField.color = roleData.color;
        roleDescriptionTextField.text = roleData.description.ToString();

        Invoke(nameof(RoleRevealFinished), secondsWaiting);
    }

    private void RoleRevealFinished(){
        Hide();
        PlayerController.LocalInstance.SetCanMove(true);
        Destroy(gameObject);
    }
}