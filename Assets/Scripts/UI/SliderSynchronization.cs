using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSynchronization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToSync;
    private Slider slider;
    private void Awake() {
        slider = GetComponent<Slider>();
    }

    private void Start() {
        slider.onValueChanged.AddListener(Slider_OnValueChanged);
    }

    private void Slider_OnValueChanged(float newVal) {
        textToSync.text = ((int)newVal).ToString();
    }
}
