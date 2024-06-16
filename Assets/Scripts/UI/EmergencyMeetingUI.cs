using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmergencyMeetingUI : BaseUISingleton<EmergencyMeetingUI>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button alertButton;

    public override void Awake() {
        base.Awake();
        closeButton.onClick.AddListener(Hide);
        alertButton.onClick.AddListener(AlertButton_OnClick);
    }

    public override void Show() {
        base.Show();
        Player.LocalInstance.GetComponent<PlayerController>().DisableControls();
    }

    public override void Hide() {
        base.Hide();
        if(Player.LocalInstance != null)
            Player.LocalInstance.GetComponent<PlayerController>().EnableControls();
    }

    private void AlertButton_OnClick(){
        Hide();
        EmergencyMeetingManager.Instance.TriggerEmergency(AlongUsMultiplayer.Instance.GetLocalPlayerData());
    }
}
