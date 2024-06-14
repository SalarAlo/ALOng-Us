using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            // Handle morph logic
        });
    }
}
