using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionHostOptions : NetworkBehaviour
{
    [SerializeField] private Button startGameButton;

    private void Start() {
        startGameButton.onClick.AddListener(StartGameButton_OnClick);
    }

    public override void OnNetworkSpawn() {
        if(!IsServer) {
            startGameButton.gameObject.SetActive(false);
        }
    }

    private void StartGameButton_OnClick() {
        // TODO: START THE GAME
    }
}
