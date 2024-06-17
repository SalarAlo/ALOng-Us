using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
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

    public List<PlayerAction> DisableAllActions(){
        List<PlayerAction> playerActionsCopy = new(playerActions);

        foreach(var action in playerActions){
            RemoveAction(action);
        }

        return playerActionsCopy;
    }

    public void ReplaceAction(PlayerAction actionToRemove, PlayerAction actionToAdd) {
        RemoveAction(actionToRemove);
        AddAction(actionToAdd);
    }

    public void RemoveAction(PlayerAction actionToRemove){
        int indexOfActionToRemove = playerActions.IndexOf(actionToRemove);
        DisableAction(actionToRemove);
        playerActions.RemoveAt(indexOfActionToRemove);

        OnPlayerActionsListChanged?.Invoke(playerActions);
    }

    public void DisableAction(PlayerAction action) {
        int index = playerActions.IndexOf(action);
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
                Debug.Log("Invalid index");
                break;
        }
    }

    private void Player_OnLocalInstanceInitialised(){
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

