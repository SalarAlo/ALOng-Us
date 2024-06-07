using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerAnimatorController : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    private PlayerController localPlayerController;
    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised() {
        localPlayerController = Player.LocalInstance.GetComponent<PlayerController>();
    }

    private void Update() {
        if (localPlayerController == null) return;
        if (NetworkManager.Singleton.LocalClientId != OwnerClientId) return;

        anim.SetBool("Running", localPlayerController.IsMoving());
    }
}
