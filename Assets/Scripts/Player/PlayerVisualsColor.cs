using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerVisualsColor : NetworkBehaviour
{
    
    [SerializeField] protected SkinnedMeshRenderer[] bodyParts;
    protected Material material;

    protected virtual void Awake(){
        material = new Material(bodyParts[0].material);
        foreach(var bodyPart in bodyParts) {
            bodyPart.material = material;
        }
    }
    
    public void SetColorTo(int colorIndex) {
        material.color = ColorSelectionManager.Instance.GetColorAtIndex(colorIndex);
    }
}
