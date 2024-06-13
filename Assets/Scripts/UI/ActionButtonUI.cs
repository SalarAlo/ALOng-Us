using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Image actionImage;
    [SerializeField] private TextMeshProUGUI coolDownText;
    [SerializeField] private Image buttonImage;
    private ActionDataSO actionSet;
    private bool activeAppearance = true;

    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised() {
        PlayerCooldownManager playerCooldownManager = Player.LocalInstance.GetComponent<PlayerCooldownManager>();
        playerCooldownManager.OnActionTimeChanged += PlayerCooldownManager_OnActionTimeChanged;
    }

    private void PlayerCooldownManager_OnActionTimeChanged(PlayerAction action, int seconds) {
        ApplyChangesForCooldown(action, seconds);
    }

    public void Hide(){
        transform.localScale = Vector3.zero;
        actionSet = null;
    }

    public void SetAction(PlayerAction action) {
        actionSet = GeneralActionsManager.Instance.GetDataForAction(action);

        gameObject.SetActive(true);
        actionImage.sprite = actionSet.sprite;
        actionText.text = actionSet.action.ToString();
    }

    public void ApplyChangesForCooldown(PlayerAction actionCooling, int amount){
        if(!IsActionSet()) return;
        if(actionSet.action != actionCooling) return;
        if(amount == 0) {
            coolDownText.text = "";
            buttonImage.color = Color.white;
            return;
        }
        coolDownText.text = amount.ToString();
        buttonImage.color = new Color32(255, 255, 255, 122);
    }

    public bool IsActionSet(){
        return actionSet != null;
    }

    private void Update() {
        if(!IsActionSet()) return;
        if (!activeAppearance && GeneralActionsManager.Instance.GetCheckForExecutionOfAction(actionSet.action)())
            DisplayActive();
        else if (activeAppearance && !GeneralActionsManager.Instance.GetCheckForExecutionOfAction(actionSet.action)())
            DisplayInactive();
    }

    private void DisplayActive() {
        buttonImage.color = new Color32(255, 255, 255, 255);
        activeAppearance = true;
    }
    private void DisplayInactive(){
        buttonImage.color = new Color32(255, 255, 255, 50);
        activeAppearance = false;
    }
}
