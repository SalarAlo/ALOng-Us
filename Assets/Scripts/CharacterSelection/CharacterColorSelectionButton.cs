using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectionButton : MonoBehaviour
{
    private Image image;
    private int colorIndex;
    private Button button;
    [SerializeField] private GameObject unavaibleGameObject;
    [SerializeField] private GameObject selectedGameObject;

    private void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    public void SetColor(int colorIndex) {
        this.colorIndex = colorIndex;
        image.color = CharacterSelection.Instance.GetColorAtIndex(colorIndex);
        button.onClick.AddListener(Button_OnClick);
    }

    public void UpdateButtonAvaiability()
    {
        var localPlayerData = AlongUsMultiplayer.Instance.GetLocalPlayerData();

        if (localPlayerData.colorIndex == colorIndex) {
            SetSelected();
            SetAvaible();
            button.enabled = false;
            return;
        } else {
            SetUnselected();
        }

        if (AlongUsMultiplayer.Instance.IsColorAvaible(colorIndex)) {
            SetAvaible();
        } else {
            SetUnavaible();
            button.enabled = false;
            return;
        }
        button.enabled = true;
    }

    public void SetUnavaible() => unavaibleGameObject.SetActive(true);
    public void SetAvaible() => unavaibleGameObject.SetActive(false);
    public void SetSelected() => selectedGameObject.SetActive(true);
    public void SetUnselected() => selectedGameObject.SetActive(false);
    public int GetColorIndex() => colorIndex;
    private void Button_OnClick() {
        AlongUsMultiplayer.Instance.SetColorOfLocalClient(colorIndex);
    }
}
