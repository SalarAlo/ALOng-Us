using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationEffectUI : MonoBehaviour
{
    private Button button;
    [SerializeField] private int rotations;
    [SerializeField] private Transform transformToRot;
    private float rotationAmount = 360f;
    private float rotationSpeed = 720f; // Degrees per second
    private float totalRotation = 0f;
    private bool isRotating = false;

    private void Awake() {
        button = GetComponent<Button>();
        if (transformToRot == null) {
            transformToRot = this.transform;
        }
        button.onClick.AddListener(StartRotation);
    }

    private void StartRotation() {
        if (!isRotating) {
            isRotating = true;
            totalRotation = 0f;
        }
    }

    private void Update() {
        if (isRotating) {
            float rotationStep = rotationSpeed * Time.deltaTime;
            transformToRot.Rotate(0, 0, rotationStep);
            totalRotation += rotationStep;

            if (totalRotation >= rotationAmount * rotations) {
                isRotating = false;
            }
        }
    }
}
