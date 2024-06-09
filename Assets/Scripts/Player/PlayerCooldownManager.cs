using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldownManager : MonoBehaviour
{
    public Action<PlayerAction> OnActionCooldownFinished;
    private float counter;
    private Dictionary<PlayerAction, int> currentActionsOnCooldown;

    public void AddActionToCooldown(PlayerAction actionToCool, int amountToCool, int indexSpot){
        if(currentActionsOnCooldown.ContainsKey(actionToCool)) {
            Debug.LogError($"{actionToCool} Action already cooling");
            return;
        }

        currentActionsOnCooldown.Add(actionToCool, amountToCool);
    }

    private void Update() {
        counter += Time.deltaTime;
        if(counter >= 1) {
            counter = 0;
            foreach(PlayerAction playerAction in currentActionsOnCooldown.Keys) {
                currentActionsOnCooldown[playerAction]--;

                if(currentActionsOnCooldown[playerAction] == 0) {
                    currentActionsOnCooldown.Remove(playerAction);
                    OnActionCooldownFinished?.Invoke(playerAction);
                }
            }
        }
    }
}