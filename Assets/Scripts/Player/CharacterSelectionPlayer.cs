using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectionPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameText;

    public void SetName(string name) {
        nameText.text = name;
    }
}
