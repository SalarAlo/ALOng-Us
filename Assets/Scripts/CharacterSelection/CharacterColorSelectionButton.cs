using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectionButton : MonoBehaviour
{
    private Image image;
    private int colorIndex;
    private Button button;

    private void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Button_OnClick() {
        Debug.Log("Clicked me! Setting color index to " + colorIndex + "!");
        AlongUsMultiplayer.Instance.SetColorOfLocalClient(colorIndex);
    }

    public void SetColor(int colorIndex) {
        this.colorIndex = colorIndex;
        image.color = CharacterSelection.Instance.GetColorAtIndex(colorIndex);
        
        button.onClick.AddListener(Button_OnClick);
    }
}
