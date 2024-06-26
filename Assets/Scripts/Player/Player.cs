using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform cameraPos;
    public static Action OnLocalInstanceInitialised;
    public static Action OnAllInstancesInitialised;
    public static Player LocalInstance;
    private PlayerData playerData;


    public override void OnNetworkSpawn() {
        if (!(OwnerClientId == NetworkManager.Singleton.LocalClientId)) return;
        StartCoroutine(TriggerInitialization());
    }

    public static Player GetPlayerWithId(ulong clientId) { 
        return FindObjectsOfType<Player>().First(p => p.OwnerClientId == clientId);
    }

    private IEnumerator TriggerInitialization(){ 
        yield return new WaitUntil(() => PlayerMoveCam.Instance != null && PlayerCam.Instance != null);

        Initialize();
    }

    public static List<Player> GetAllPlayers(){
        return FindObjectsOfType<Player>().ToList();
    }

    public Vector3 GetPos() => transform.position;

    public void Initialize() {
        if (Player.LocalInstance != null) {
            Destroy(gameObject);
            Debug.LogError("There are more then one Local Player Instances");
        }

        SetPlayerDataServerRpc(playerData);
        LocalInstance = this;

        PlayerVisuals playerVisuals = GetComponent<PlayerVisuals>();

        PlayerMoveCam.Instance.SetCameraPosition(cameraPos);
        PlayerCam.Instance.SetOrientation(GetComponent<PlayerController>().GetOrientation());
        playerVisuals.DeactivateLocalVisuals();

        OnLocalInstanceInitialised?.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerDataServerRpc(PlayerData data){
        SetPlayerDataClientRpc(data);
    }

    [ClientRpc]
    public void SetPlayerDataClientRpc(PlayerData data){
        playerData = data;
    }

    public PlayerData GetPlayerData(){
        return playerData;
    }
}
