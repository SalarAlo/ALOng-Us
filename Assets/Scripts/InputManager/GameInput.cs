using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : Singleton<GameInput> {
    public Action<PlayerAction> OnLocalPlayerActionTriggered;
    private InputActions inputActions;
    private PlayerController localPlayerController;

    private Action<InputAction.CallbackContext> currentMainAction;
    private Action<InputAction.CallbackContext> currentPrimaryAction;
    private Action<InputAction.CallbackContext> currentAlternateAction;
    private Action<InputAction.CallbackContext> currentOptionalAction;

    public override void Awake() {
        base.Awake();
        inputActions = new();
        inputActions.Enable();
    }

    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    public void SubscribeToEmote(Action action){
        inputActions.UI.Emote.performed += _ => action?.Invoke();
    }

    private void Player_OnLocalInstanceInitialised()
    {
        localPlayerController = Player.LocalInstance.GetComponent<PlayerController>();
        inputActions.Movement.Move.performed += Movement_Performed;
    }

    private void Movement_Performed(InputAction.CallbackContext callbackContext) {
        localPlayerController.SetMovementInput(callbackContext.ReadValue<Vector2>());
    }

    public void SetMainAction(ActionDataSO actionData){
        if(currentMainAction != null) {
            inputActions.Actions.MainAction.performed -= currentMainAction;
        }

        currentMainAction = GetActionForData(actionData); 

        inputActions.Actions.MainAction.performed += currentMainAction;
    }

    public void DisableMainAction() => inputActions.Actions.MainAction.performed -= currentMainAction;
    public void EnableMainAction() => inputActions.Actions.MainAction.performed += currentMainAction;

    public void SetPrimaryAction(ActionDataSO actionData){
        if(currentPrimaryAction != null) {
            inputActions.Actions.PrimaryAction.performed -= currentPrimaryAction;
        }

        currentPrimaryAction = GetActionForData(actionData); 

        inputActions.Actions.PrimaryAction.performed += currentPrimaryAction;
    }
    public void DisablePrimaryAction() => inputActions.Actions.PrimaryAction.performed -= currentPrimaryAction;
    public void EnablePrimaryAction() => inputActions.Actions.PrimaryAction.performed += currentPrimaryAction;

    public void SetAlternateAction(ActionDataSO actionData){
        if (currentAlternateAction != null) {
            inputActions.Actions.AlternateAction.performed -= currentAlternateAction;
        }

        currentAlternateAction = GetActionForData(actionData);

        inputActions.Actions.AlternateAction.performed += currentAlternateAction;
    }
    public void DisableAlternateAction() => inputActions.Actions.AlternateAction.performed -= currentAlternateAction;
    public void EnableAlternateAction() => inputActions.Actions.AlternateAction.performed += currentAlternateAction;


    public void SetOptionalAction(ActionDataSO actionData) {

        if (currentOptionalAction != null) {
            inputActions.Actions.OptionalAction.performed -= currentOptionalAction;
        }
        currentOptionalAction = GetActionForData(actionData);

        inputActions.Actions.OptionalAction.performed += currentOptionalAction;
    }

    public void DisableOptionalAction() => inputActions.Actions.OptionalAction.performed -= currentOptionalAction;
    public void EnableOptionalAction() => inputActions.Actions.OptionalAction.performed += currentOptionalAction;

    private Action<InputAction.CallbackContext> GetActionForData(ActionDataSO data){
        return  _ => {
            GameRoleManager.Instance.GetExecutableForAction(data.action)?.Invoke();
            OnLocalPlayerActionTriggered?.Invoke(data.action);
        };
    }
}
