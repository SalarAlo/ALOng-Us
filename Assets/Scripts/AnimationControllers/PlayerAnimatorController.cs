using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerAnimatorController : NetworkBehaviour
{
    [SerializeField] private Animator anim;

    private void Update() {
        if (PlayerController.LocalInstance == null) return;
        if (NetworkManager.Singleton.LocalClientId != OwnerClientId) return;

        anim.SetBool("Running", PlayerController.LocalInstance.IsMoving());
    }
}
