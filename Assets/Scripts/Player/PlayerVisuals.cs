using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisuals : PlayerVisualsColor
{
    [SerializeField] private Transform visuals;
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private Image emoteImage; 
    [SerializeField] private SpriteRenderer miniMapIndicator; 
    protected override void Awake() {
        base.Awake();
        HideEmote();
    }
    public Transform GetVisualParent() => visuals;
    public override void SetColorTo(int colorIndex) {
        base.SetColorTo(colorIndex);
        miniMapIndicator.color = ColorSelectionManager.Instance.GetColorAtIndex(colorIndex);
    }
    public void DeactivateLocalVisuals() { 
        foreach (Transform child in visuals){
            child.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
        }
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
        int secondsToWait = 6;
        emoteImage.gameObject.SetActive(true);
        emoteImage.sprite = EmoteWheelUI.Instance.GetEmoteSpriteByIndex(emoteIndex);
        Invoke(nameof(HideEmote), secondsToWait);
    }

    private void HideEmote(){
        emoteImage.gameObject.SetActive(false);
    }
}
