using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerActionsManager : NetworkBehaviour
{
    [SerializeField] private List<PlayerAction> playerActions;
    [SerializeField] private ActionButtonUI actionButtonUIPrefab;
    [SerializeField] private Dictionary<PlayerAction, ActionButtonUI> activeActionButtons;

    private void Awake() {
        activeActionButtons = new();
    }

    public override void OnNetworkSpawn() {
        if(NetworkManager.Singleton.LocalClientId == OwnerClientId)
            Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void GameInput_OnLocalPlayerActionTriggered(PlayerAction action) {
        // implement cooldown logic
    }

    public void AddAction(ActionData action) {
        playerActions.Add(action.action);

        if (playerActions.Count == 1) {
            GameInput.Instance.SetPrimaryAction(action);
        } else if (playerActions.Count == 2) {
            GameInput.Instance.SetAlternateAction(action);
        }


        ActionButtonUI instantiatedActionButton = Instantiate(actionButtonUIPrefab);
        ActionButtonParent.Instance.AddChild(instantiatedActionButton);
        instantiatedActionButton.SetAction(action);
        activeActionButtons[action.action] = instantiatedActionButton;
    }



    private void Player_OnLocalInstanceInitialised(){
        GameInput.Instance.OnLocalPlayerActionTriggered += GameInput_OnLocalPlayerActionTriggered;

        PlayerRole role = Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        GameRoleData gameRoleData = GameRoleManager.Instance.GetDataForRole(role);
        
        foreach(ActionData actionData in gameRoleData.actions) {
            AddAction(actionData);
        }
    }

}

