using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisuals : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] bodyParts;
    [SerializeField] private Transform visuals;
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private Image emoteImage; 
    private Material material;
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
        material.color = ColorSelectionManager.Instance.GetColorAtIndex(colorIndex);
    }

    public void SetPlayerName(string name) {
        nameTextField.text = name;
    }

    public void Emote(int emoteIndex){
        EmoteServerRpc(emoteIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void EmoteServerRpc(int emoteIndex) {
        EmoteClientRpc(emoteIndex);
    }

    [ClientRpc]
    private void EmoteClientRpc(int emoteIndex){
        emoteImage.sprite = EmoteWheelUI.Instance.GetEmoteSpriteByIndex(emoteIndex);
    }
}
