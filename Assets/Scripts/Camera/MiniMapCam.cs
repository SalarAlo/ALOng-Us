using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    private bool followPlayer;
    private float yOffset;
    private void Awake() {
        yOffset = transform.position.y;
    }

    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Player_OnLocalInstanceInitialised(){
        followPlayer = true;
    }

    private void Update() {
        if(!followPlayer) return;
        Vector3 playerPos = Player.LocalInstance.GetPos();
        transform.position = new Vector3(playerPos.x, yOffset, playerPos.z);
    }
}

