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
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform visuals;

    public static PlayerController LocalInstance = null;
    private Rigidbody rb;

    private Vector2 movementInput;
    private Vector3 movementDir;

    public static PlayerController GetLocalInstance() => LocalInstance;

    public override void OnNetworkSpawn() {
        StartCoroutine(TriggerInitialization());
    }

    private IEnumerator TriggerInitialization(){ 
        yield return new WaitUntil(() => PlayerMoveCam.Instance != null && PlayerCam.Instance != null);

        Initialize();
    }

    public void Initialize() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = false;

        if (OwnerClientId == NetworkManager.Singleton.LocalClientId) {
            if (PlayerController.LocalInstance != null) {
                Destroy(gameObject);
                Debug.LogError("There are more then one Local Player Instances");
            }

            LocalInstance = this;
            Debug.Log(name+" is the local instance");
            Debug.Log(PlayerMoveCam.Instance);

            PlayerMoveCam.Instance.SetCameraPosition(cameraPos);
            PlayerCam.Instance.SetOrientation(orientation);

            DeactivateLocalVisuals();
        }
    }

    private void DeactivateLocalVisuals(){
        foreach (Transform child in visuals){
            child.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
        }
    }

    public void SetMovementInput(Vector2 movementInput) {
        this.movementInput = movementInput;
    }

    private void LateUpdate() {
        MovePlayer();
    }

    private void MovePlayer() {
        Debug.Log(movementInput);
        int speedMultiplier = 10;
        movementDir = orientation.forward * movementInput.y + orientation.right * movementInput.x;
        rb.AddForce(movementSpeed * Time.deltaTime * speedMultiplier * movementDir, ForceMode.Force);
    }
}
