using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour {   
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform orientation;
    private bool canMove;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector3 movementDir;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = false;
    }

    public Transform GetOrientation() => orientation;

    public void SetCanMove(bool canMove) => this.canMove = canMove;

    public bool IsMoving() {
        return movementInput != Vector2.zero;
    }  

    public void SetMovementInput(Vector2 movementInput) {
        this.movementInput = movementInput;
    }

    private void LateUpdate() {
        if (!canMove) return;
        MovePlayer();
    }

    private void MovePlayer() {
        int speedMultiplier = 10;
        movementDir = orientation.forward * movementInput.y + orientation.right * movementInput.x;
        rb.AddForce(movementSpeed * Time.deltaTime * speedMultiplier * movementDir, ForceMode.Force);
    }
}
