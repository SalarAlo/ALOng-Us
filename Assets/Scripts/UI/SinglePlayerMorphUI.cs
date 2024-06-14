using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerMorphUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image playerImage;
    [SerializeField] private Button swapButton;


    public void SetPlayerMorphUI(PlayerData data) {
        playerNameText.text = data.playerName.ToString();
        playerImage.color = ColorSelectionManager.Instance.GetColorAtIndex(data.colorIndex);
        swapButton.onClick.AddListener(() => {
            var playerCooldownManager = Player.LocalInstance.GetComponent<PlayerCooldownManager>();
            playerCooldownManager.AddActionToCooldown(PlayerAction.Morph, GeneralActionsManager.Instance.GetDataForAction(PlayerAction.Morph).cooldown);
            MorphUI.Instance.Hide();

            AlongUsMultiplayer.Instance.ChangePlayerAppearanceTo(NetworkManager.Singleton.LocalClientId, data, 10);
        });
    }
}
