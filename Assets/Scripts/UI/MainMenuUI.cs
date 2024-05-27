using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;

    private void Start() {
        quitGameButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
