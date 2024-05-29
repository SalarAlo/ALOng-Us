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
        SyncText();
        slider.onValueChanged.AddListener(Slider_OnValueChanged);
    }

    private void Slider_OnValueChanged(float _) {
        SyncText();
    }

    private void SyncText(){
        
        if (slider.wholeNumbers){
            textToSync.text = slider.value.ToString();
        } else {
            textToSync.text = slider.value.ToString("0.00");
        }
    }
}
