using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : Singleton<GameInput> {
    private InputActions inputActions;
    public override void Awake() {
        base.Awake();
        inputActions = new();
        inputActions.Enable();
    }

    
    private void Start() {
        inputActions.Movement.Move.performed += Movement_Performed;
    }

    private void Movement_Performed(InputAction.CallbackContext callbackContext) {
        if (PlayerController.GetLocalInstance() == null) return;

        PlayerController.GetLocalInstance().SetMovementInput(callbackContext.ReadValue<Vector2>());
    }
}
