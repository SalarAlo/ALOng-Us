using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform cameraPos;
    public static Action OnLocalInstanceInitialised;
    public static Player LocalInstance;
    private PlayerData data;

    public void SetPlayerData(PlayerData data) => this.data = data;

    public static Player GetLocalInstance() => LocalInstance;
    public override void OnNetworkSpawn() {
        StartCoroutine(TriggerInitialization());
    }

    private IEnumerator TriggerInitialization(){ 
        yield return new WaitUntil(() => PlayerMoveCam.Instance != null && PlayerCam.Instance != null);

        Initialize();
    }

    public void Initialize() {
        if (OwnerClientId == NetworkManager.Singleton.LocalClientId) {
            if (Player.LocalInstance != null) {
                Destroy(gameObject);
                Debug.LogError("There are more then one Local Player Instances");
            }

            LocalInstance = this;

            PlayerMoveCam.Instance.SetCameraPosition(cameraPos);
            PlayerCam.Instance.SetOrientation(GetComponent<PlayerController>().GetOrientation());
            GetComponent<PlayerVisuals>().DeactivateLocalVisuals();
            
            OnLocalInstanceInitialised?.Invoke();
        }
    }

}
