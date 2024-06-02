using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCam : Singleton<PlayerMoveCam>
{
    [SerializeField] private Transform cameraPosition;

    // Update is called once per frame
    void LateUpdate() {
        if (cameraPosition == null) return;
        transform.position = cameraPosition.position;
    }

    public void SetCameraPosition(Transform cameraPos) {
        cameraPosition = cameraPos;
    }
}
