using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerActionsManager : NetworkBehaviour
{
    [SerializeField] private List<PlayerAction> playerActions;
    [SerializeField] private List<ActionButtonUI> actionsButtons;
    private Dictionary<PlayerAction, ActionButtonUI> actionButtonDict;

    private void Awake() {
        actionButtonDict = new();
    }

    public override void OnNetworkSpawn() {
        if(NetworkManager.Singleton.LocalClientId == OwnerClientId)
            Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void GameInput_OnLocalPlayerActionTriggered(PlayerAction action) {
        // implement cooldown logic
    }

    public void AddAction(PlayerAction actionToAdd) {
        ActionDataSO actionToAddData = GameRoleManager.Instance.GetDataForAction(actionToAdd);
        playerActions.Add(actionToAdd);

        switch(playerActions.Count) {
            case 1:
                GameInput.Instance.SetMainAction(actionToAddData);
                break;
            case 2:
                GameInput.Instance.SetPrimaryAction(actionToAddData);
                break;
            case 3:
                GameInput.Instance.SetAlternateAction(actionToAddData);
                break;
            case 4:
                GameInput.Instance.SetOptionalAction(actionToAddData);
                break;
            default:
                Debug.Log("WHY TF DO YOU HAVE MORE THEN 4 ACTIONS!");
                break;
        }

        ActionButtonUI actionButtonToSet = actionsButtons[playerActions.Count-1];
        actionButtonDict[actionToAdd] = actionButtonToSet;
        actionButtonToSet.SetAction(actionToAdd);
    }

    public void ReplaceAction(PlayerAction actionToRemove, PlayerAction actionToAdd) {
        RemoveAction(actionToRemove);
        AddAction(actionToAdd);
    }

    public void RemoveAction(PlayerAction actionToRemove){
        if(!actionButtonDict.ContainsKey(actionToRemove)) {
            Debug.LogError("Trying to remove an action which doesnt exist");
            return;
        }

        int indexOfActionToRemove = playerActions.IndexOf(actionToRemove);
        playerActions.RemoveAt(indexOfActionToRemove);
        actionButtonDict[actionToRemove].Hide();
    }

    private void Player_OnLocalInstanceInitialised(){
        GameInput.Instance.OnLocalPlayerActionTriggered += GameInput_OnLocalPlayerActionTriggered;

        actionsButtons = ActionButtonsParent.Instance.GetChildren().ToList();
        PlayerRole role = Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        var actions = GameRoleManager.Instance.GetDataForRole(role).actions.Select(actionData => actionData.action);
        
        foreach (var actionData in actions) {
            AddAction(actionData);
        }

        foreach(ActionButtonUI actionButton in actionsButtons){
            if(!actionButton.IsActionSet()) actionButton.Hide();
        }
    }

}

