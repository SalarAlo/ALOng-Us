using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCooldownManager : MonoBehaviour
{
    public Action<PlayerAction, int> OnActionTimeChanged;
    public Action<PlayerAction> OnActionCooldownFinished;
    private float counter;
    private Dictionary<PlayerAction, int> currentActionsOnCooldown;

    private void Awake() {
        currentActionsOnCooldown = new();   
    }

    public void AddActionToCooldown(PlayerAction actionToCool, int amountToCool){
        if(currentActionsOnCooldown.ContainsKey(actionToCool)) {
            Debug.LogError($"{actionToCool} Action already cooling");
            return;
        }

        OnActionTimeChanged?.Invoke(actionToCool, amountToCool);
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