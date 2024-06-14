using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerActionsManager : NetworkBehaviour
{
    public Action<List<PlayerAction>> OnPlayerActionsListChanged;
    [SerializeField] private List<PlayerAction> playerActions;

    public bool HasAction(PlayerAction action) => playerActions.Contains(action);


    public override void OnNetworkSpawn() {
        if(NetworkManager.Singleton.LocalClientId == OwnerClientId)
            Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void GameInput_OnLocalPlayerActionTriggered(PlayerAction action) {
        ActionDataSO actionDataSO = GeneralActionsManager.Instance.GetDataForAction(action);

        if(actionDataSO.cooldown == 0) return;

        else if (actionDataSO.cooldown == -1) {
            RemoveAction(action);
            return;
        }

        PlayerCooldownManager playerCooldownManager = Player.LocalInstance.GetComponent<PlayerCooldownManager>();
        playerCooldownManager.AddActionToCooldown(action, actionDataSO.cooldown);
        DisableActionOnIndex(playerActions.IndexOf(action));
    }

    public void AddAction(PlayerAction actionToAdd) {
        ActionDataSO actionToAddData = GeneralActionsManager.Instance.GetDataForAction(actionToAdd);
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

        OnPlayerActionsListChanged?.Invoke(playerActions);
    }

    public void ReplaceAction(PlayerAction actionToRemove, PlayerAction actionToAdd) {
        RemoveAction(actionToRemove);
        AddAction(actionToAdd);
    }

    public void RemoveAction(PlayerAction actionToRemove){
        int indexOfActionToRemove = playerActions.IndexOf(actionToRemove);
        DisableActionOnIndex(indexOfActionToRemove);
        playerActions.RemoveAt(indexOfActionToRemove);

        OnPlayerActionsListChanged?.Invoke(playerActions);
    }

    private void DisableActionOnIndex(int index) {
        switch(index+1){
            case 1:
                GameInput.Instance.DisableMainAction();
                break;
            case 2:
                GameInput.Instance.DisablePrimaryAction();
                break;
            case 3:
                GameInput.Instance.DisableAlternateAction();
                break;
            case 4:
                GameInput.Instance.DisableOptionalAction();
                break;
            default:
                Debug.Log("WHY TF DO YOU HAVE MORE THEN 4 ACTIONS!");
                break;
        }       
    }

    private void Player_OnLocalInstanceInitialised(){
        GameInput.Instance.OnLocalPlayerActionTriggered += GameInput_OnLocalPlayerActionTriggered;
        Player.LocalInstance.GetComponent<PlayerCooldownManager>().OnActionCooldownFinished += PlayerCooldownManager_OnActionCooldownFinished;
        PlayerRole role = Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        var actions = GameRoleManager.Instance.GetDataForRole(role).actions.Select(actionData => actionData.action);
        
        foreach (var actionData in actions) {
            AddAction(actionData);
        }

        ActionsButtonManagerUI.Instance.UpdateButtons(playerActions);
    }

    private void PlayerCooldownManager_OnActionCooldownFinished(PlayerAction action) {
        int indexOfAction = playerActions.IndexOf(action);
        switch(indexOfAction+1) {
            case 1:
                GameInput.Instance.EnableMainAction();
                break;
            case 2:
                GameInput.Instance.EnablePrimaryAction();
                break;
            case 3:
                GameInput.Instance.EnableAlternateAction();
                break;
            case 4:
                GameInput.Instance.EnableOptionalAction();
                break;
            default:
                Debug.Log("WHY TF DO YOU HAVE MORE THEN 4 ACTIONS!");
                break;
        }
    }

    private void Update() {
        foreach(PlayerAction action in playerActions) {
            GeneralActionsManager.Instance.GetExecutableForActionUpdate(action)();
        }
    }
}

