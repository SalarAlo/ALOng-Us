using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameActionsUI : MonoBehaviour
{
    [SerializeField] private Image primaryActionImage;
    [SerializeField] private ActionButtonUI primaryAction; 
    [SerializeField] private ActionButtonUI alternateAction; 
    [SerializeField] private ActionButtonUI mysteryItemAction; 
    [SerializeField] private Sprite taskSprite;
    [SerializeField] private Sprite killSprite;

    private void Start() {
        PlayerController.OnInitialised += PlayerController_OnInitialised;
    }

    private void PlayerController_OnInitialised(){
        UpdateRole();
    }
    private void UpdateRole(){
    }

    private void Update() {
        // Temporarily
        if (Input.GetKeyDown(KeyCode.T)){
            UpdateRole();
        }
    }
}
