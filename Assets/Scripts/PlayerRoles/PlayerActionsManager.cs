using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    [SerializeField] private List<PlayerAction> playerActions;
    [SerializeField] private ActionButtonUI actionButtonUIPrefab;

    private void Awake() {
        PlayerController.OnInitialised += PlayerController_OnInitialised;
    }

    public void AddAction(ActionData action) {
        playerActions.Add(action.action);
    }



    private void PlayerController_OnInitialised(){
        PlayerRole role = PlayerController.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        GameRoleData gameRoleData = GameRoleManager.Instance.GetDataForRole(role);
        
        foreach(ActionData actionData in gameRoleData.actions) {
            AddAction(actionData);
        }
    }

}

