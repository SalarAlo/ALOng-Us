using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] bodyParts;
    [SerializeField] private Transform visuals;
    private Material material;
    private int colorIndex;
    private void Awake() {
        material = new Material(bodyParts[0].material);
        foreach(var bodyPart in bodyParts) {
            bodyPart.material = material;
        }
    }
    public Transform GetVisualParent() => visuals;

    public void DeactivateLocalVisuals(){
        foreach (Transform child in visuals){
            child.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
        }
    }
    public void SetColorTo(int colorIndex) {
        this.colorIndex = colorIndex;
        material.color = ColorSelectionManager.Instance.GetColorAtIndex(colorIndex);
    }
}
