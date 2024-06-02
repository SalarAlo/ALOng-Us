using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{   
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform visuals;

    public static PlayerController LocalInstance;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector3 movementDir;

    private void Awake() {
        LocalInstance = this;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        DeactivateLocalVisuals();
    }
    private void DeactivateLocalVisuals(){
        if(LocalInstance == this) {
            foreach (Transform child in visuals){
                child.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
            }
        }
    }

    // public override void OnNetworkSpawn() {
        // if (LocalInstance != null) {
            // Destroy(gameObject);
            // Debug.LogError("There are more then one Local Player Instances");
        // }
// 
        // if (OwnerClientId == NetworkManager.Singleton.LocalClientId) {
            // LocalInstance = this;
        // }
    // }

    public void SetMovementInput(Vector2 movementInput) {
        this.movementInput = movementInput;
    }

    private void Update() {
        MovePlayer();
    }

    private void MovePlayer() {
        Debug.Log(movementInput);
        int speedMultiplier = 10;
        movementDir = orientation.forward * movementInput.y + orientation.right * movementInput.x;
        rb.AddForce(movementSpeed * Time.deltaTime * speedMultiplier * movementDir, ForceMode.Force);
    }
}
