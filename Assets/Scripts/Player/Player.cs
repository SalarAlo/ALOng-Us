using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Dictionary<ulong, Player> players = new();
    [SerializeField] private Transform cameraPos;
    public static Action OnLocalInstanceInitialised;
    public static Action OnAllInstancesInitialised;
    public static Player LocalInstance;
    
    public override void OnNetworkSpawn() {
        if (!(OwnerClientId == NetworkManager.Singleton.LocalClientId)) return;
        StartCoroutine(TriggerInitialization());
    }

    private IEnumerator TriggerInitialization(){ 
        yield return new WaitUntil(() => PlayerMoveCam.Instance != null && PlayerCam.Instance != null);

        Initialize();
    }

    public void Initialize() {
        if (Player.LocalInstance != null) {
            Destroy(gameObject);
            Debug.LogError("There are more then one Local Player Instances");
        }

        LocalInstance = this;

        PlayerVisuals playerVisuals = GetComponent<PlayerVisuals>();

        PlayerMoveCam.Instance.SetCameraPosition(cameraPos);
        PlayerCam.Instance.SetOrientation(GetComponent<PlayerController>().GetOrientation());
        playerVisuals.DeactivateLocalVisuals();

        players[OwnerClientId] = LocalInstance;
        
        OnLocalInstanceInitialised?.Invoke();
    }
}
