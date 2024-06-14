using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCooldownManager : NetworkBehaviour
{
    public Action<PlayerAction, int> OnActionTimeChanged;
    public Action<PlayerAction> OnActionCooldownFinished;
    private float counter;
    private Dictionary<PlayerAction, int> currentActionsOnCooldown;

    private void Awake() {
        currentActionsOnCooldown = new();   
    }


    public override void OnNetworkSpawn() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised(){
        GameInput.Instance.OnLocalPlayerActionTriggered += GameInput_OnLocalPlayerActionTriggered;
    }

    private void GameInput_OnLocalPlayerActionTriggered(PlayerAction action) {
        ActionDataSO actionDataSO = GeneralActionsManager.Instance.GetDataForAction(action);

        bool isCooldownAction = actionDataSO.cooldown == 0 || !actionDataSO.immeadiateCooldown;
        if (isCooldownAction) return;

        bool isDisposableAction = actionDataSO.cooldown == -1;
        if(isDisposableAction) {
            var localPlayerActionsManager = Player.LocalInstance.GetComponent<PlayerActionsManager>();
            localPlayerActionsManager.RemoveAction(action);
            return;
        }
        
        AddActionToCooldown(action, actionDataSO.cooldown);
    }

    public void AddActionToCooldown(PlayerAction actionToCool, int amountToCool){
        if(currentActionsOnCooldown.ContainsKey(actionToCool)) {
            Debug.LogError($"{actionToCool} Action already cooling");
            return;
        }

        OnActionTimeChanged?.Invoke(actionToCool, amountToCool);
        var localPlayerActionsManager = Player.LocalInstance.GetComponent<PlayerActionsManager>();
        localPlayerActionsManager.DisableAction(actionToCool);
        counter = 0;
        currentActionsOnCooldown.Add(actionToCool, amountToCool);
    }

    private void Update() {
        counter += Time.deltaTime;
        if(counter >= 1) {
            counter = 0;
            foreach(PlayerAction playerAction in currentActionsOnCooldown.Keys.ToList()) {
                currentActionsOnCooldown[playerAction] -= 1;
                OnActionTimeChanged?.Invoke(playerAction, currentActionsOnCooldown[playerAction]);

                if(currentActionsOnCooldown[playerAction] == 0) {
                    currentActionsOnCooldown.Remove(playerAction);
                    OnActionCooldownFinished?.Invoke(playerAction);
                }
            }
        }
    }
}