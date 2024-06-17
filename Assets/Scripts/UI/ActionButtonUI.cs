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
    [SerializeField] private GameObject disabledImage;
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
        disabledImage.SetActive(false);
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
            foreach(Image image in transform.GetComponentsInChildren<Image>()){
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
            disabledImage.SetActive(false);
            return;
        }
        coolDownText.text = amount.ToString();
        foreach(Image image in transform.GetComponentsInChildren<Image>()){
            image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
        }
        disabledImage.SetActive(true);
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
        foreach(Image image in transform.GetComponentsInChildren<Image>()){
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
        disabledImage.SetActive(false);
        activeAppearance = true;
    }
    private void DisplayInactive(){
        foreach(Image image in transform.GetComponentsInChildren<Image>()){
            image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
        }
        disabledImage.SetActive(true);
        activeAppearance = false;
    }
}
