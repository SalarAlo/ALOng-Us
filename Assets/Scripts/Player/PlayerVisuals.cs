using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] bodyParts;
    private Material material;
    private void Awake() {
        material = new Material(bodyParts[0].material);
        foreach(var bodyPart in bodyParts) {
            bodyPart.material = material;
        }
    }

    public void SetColorTo(Color color) {
        material.color = color;
    }
}
