using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameActionsUI : MonoBehaviour
{
    [SerializeField] private Image primaryActionImage;
    [SerializeField] private ActionButtonUI primaryAction; 
    [SerializeField] private ActionButtonUI alternateAction; 
    [SerializeField] private ActionButtonUI mysteryItemAction; 
    [SerializeField] private Sprite taskSprite;
    [SerializeField] private Sprite killSprite;

    private void Start() {
        PlayerController.OnInitialised += PlayerController_OnInitialised;
    }

    private void PlayerController_OnInitialised(){
        PlayerRole role = PlayerController.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        GameRoleData gameRoleData = GameRoleManager.Instance.GetDataForRole(role);
        int i = 0;
        foreach(ActionData actionData in gameRoleData.actions) {
            if (i == 0){
                primaryAction.SetAction(actionData);
            } else if (i == 1) {
                alternateAction.SetAction(actionData);
            }
            i++;
        }
    }
}
