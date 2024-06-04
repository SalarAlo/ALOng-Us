using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRoleUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI roleTextField;
    [SerializeField] private TextMeshProUGUI roleDescriptionTextField;
    private IEnumerator Start() {
        yield return new WaitUntil(() => PlayerController.LocalInstance != null);

        TriggerRoleRevealUI();
    }

    private void TriggerRoleRevealUI() {
        Show();

        GameRoleData roleData = GameRoleManager.Instance.GetDataForRole(PlayerController.LocalInstance.GetComponent<PlayerRoleManager>().GetRole());

        roleTextField.text = roleData.role.ToString();
        roleTextField.color = roleData.color;
        roleDescriptionTextField.text = roleData.description.ToString();

        Invoke(nameof(Hide), 2);
    }
}
