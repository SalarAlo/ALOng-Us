using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleOptionWheelUI : MonoBehaviour
{
    public Action OnTransitionBackFinished;
    [SerializeField] private Image optionImage;
    [SerializeField] private TextMeshProUGUI numberText;
    float destinationRotation;
    bool shouldTransition;
    private bool transitioningBack = false;
    float transitionTime = 2f;
    float transitionProgress;
    float startRotation;

    public void SetOptionSprite(Sprite sprite) => optionImage.sprite = sprite;

    public void Place(int spotIndex, bool back) {
        numberText.text = !back ? (spotIndex+1).ToString() : "";
        float targetRotation = spotIndex * 45;
        startRotation = transform.localEulerAngles.z;

        // Ensure that the rotation always increments
        if (targetRotation < startRotation) {
            targetRotation += 360;
        }

        transitioningBack = back;

        destinationRotation = targetRotation;
        shouldTransition = true;
        transitionProgress = 0f; // Reset the transition progress
    }

    private void Update() {
        if (shouldTransition) {
            transitionProgress += Time.deltaTime * transitionTime;

            if (transitionProgress >= 1f) {
                transitionProgress = 1f;
                shouldTransition = false;
                if(transitioningBack)
                    OnTransitionBackFinished?.Invoke();
            }

            float currentRotation = Mathf.Lerp(startRotation, destinationRotation, transitionProgress);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentRotation % 360);
        }
    }
}
