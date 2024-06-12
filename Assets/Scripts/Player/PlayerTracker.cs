using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tracker;
    
    private void Awake() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised(){
        InitServerRpc(Player.LocalInstance.GetComponent<PlayerVisuals>().GetColorIndex());
    }

    [ServerRpc(RequireOwnership = false)]
    private void InitServerRpc(int colorIndex){
        InitClientRpc(colorIndex);
    }

    [ClientRpc]
    private void InitClientRpc(int colorIndex){
        tracker.color = ColorSelectionManager.Instance.GetColorAtIndex(colorIndex);
        Hide();
    }

    public void Show() => tracker.gameObject.SetActive(true);
    public void Hide() => tracker.gameObject.SetActive(false);
}
