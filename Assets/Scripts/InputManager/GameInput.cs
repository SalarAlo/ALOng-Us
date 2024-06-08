using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : Singleton<GameInput> {
    public Action<PlayerAction> OnLocalPlayerActionTriggered;
    private InputActions inputActions;
    private PlayerController localPlayerController;

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

    public void SetPrimaryAction(ActionData actionData){
        // inputActions.Actions.PrimaryAction.performed = null;
        inputActions.Actions.PrimaryAction.performed += _ => { 
            GameRoleManager.Instance.GetExecutableForAction(actionData.action)?.Invoke(); 
            OnLocalPlayerActionTriggered?.Invoke(actionData.action);
        };
    }

    public void SetAlternateAction(ActionData actionData){
        // inputActions.Actions.PrimaryAction.performed = null;
        inputActions.Actions.AlternateAction.performed += _ => {
            GameRoleManager.Instance.GetExecutableForAction(actionData.action)?.Invoke();
            OnLocalPlayerActionTriggered?.Invoke(actionData.action);
        };
    }
}
