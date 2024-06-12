using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCam : SingletonNetwork<PlayerMoveCam>
{
    [SerializeField] private Transform cameraPosition;

    void LateUpdate() {
        if (cameraPosition == null) return;
        transform.position = cameraPosition.position;
    }

    public void SetCameraPosition(Transform cameraPos) {
        cameraPosition = cameraPos;
    }
}
