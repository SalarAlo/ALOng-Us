using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameActionsUI : MonoBehaviour
{
    [SerializeField] private Image primaryActionImage;
    [SerializeField] private TextMeshProUGUI primaryActionText;
    [SerializeField] private Sprite taskSprite;
    [SerializeField] private Sprite killSprite;

    private void Start() {
        PlayerController.OnInitialised += PlayerController_OnInitialised;
    }

    private void PlayerController_OnInitialised(){
        PlayerGameRole role = PlayerController.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        GameRoleData gameRoleData = GameRoleManager.Instance.GetDataForRole(role);
        primaryActionImage.sprite = gameRoleData.canCompleteTasks ? taskSprite : killSprite;
        primaryActionText.text = gameRoleData.canCompleteTasks ? "use" : "kill";
    }
}
