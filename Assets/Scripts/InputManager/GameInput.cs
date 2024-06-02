using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : Singleton<GameInput> {
    private InputActions inputActions;

    private void OnEnable() {
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    public override void Awake() {
        base.Awake();
        inputActions = new();
    }

    
    private void Start() {
        inputActions.Movement.Move.performed += Movement_Performed;
    }

    private void Movement_Performed(InputAction.CallbackContext callbackContext){
        PlayerController.LocalInstance.SetMovementInput(callbackContext.ReadValue<Vector2>());
    }
}
