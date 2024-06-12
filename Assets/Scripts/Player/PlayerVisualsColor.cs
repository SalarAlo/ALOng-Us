using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerVisualsColor : NetworkBehaviour
{
    
    [SerializeField] protected SkinnedMeshRenderer[] bodyParts;
    protected Material material;
    protected Color color;
    protected int colorIndex;

    protected virtual void Awake(){
        material = new Material(bodyParts[0].material);
        foreach(var bodyPart in bodyParts) {
            bodyPart.material = material;
        }
    }
    
    public virtual void SetColorTo(int colorIndex) {
        this.colorIndex = colorIndex;
        color = ColorSelectionManager.Instance.GetColorAtIndex(colorIndex);
        material.color = color;
    }

    public Color GetColor() => color;
    public int GetColorIndex() => colorIndex;
}
