using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsButtonManagerUI : Singleton<ActionsButtonManagerUI>
{
    [SerializeField] private List<ActionButtonUI> actionButtonUIs;

    private void Start(){
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised() {
        PlayerActionsManager localPlayerActionsManager = Player.LocalInstance.GetComponent<PlayerActionsManager>();
        localPlayerActionsManager.OnPlayerActionsListChanged += PlayerActionsManager_OnPlayerActionsListChanged;
    }

    private void PlayerActionsManager_OnPlayerActionsListChanged(List<PlayerAction> newPlayerActionsList) {
        UpdateButtons(newPlayerActionsList);
        // playerCooldownManager
    }

    public void UpdateButtons(List<PlayerAction> newPlayerActionsList){
        for(int i = 0; i < newPlayerActionsList.Count; i++) {
            var actionButtonUI = actionButtonUIs[i];
            actionButtonUI.SetAction(newPlayerActionsList[i]);
        }

        foreach(ActionButtonUI actionButtonUI in actionButtonUIs){
            if(!actionButtonUI.IsActionSet()) actionButtonUI.Hide();
        }
    }
}
