using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleRevealBackground : MonoBehaviour
{
    private const int MAX_BACKGROUND_PX = 1920;
    private Image backgroundImage;
    private int animDuration;
    private float animTimeCounter;
    private bool animate;

    private void Awake() {
        backgroundImage = GetComponent<Image>();
    }
    public void SetColor(Color color){
        backgroundImage.color = color;
    }

    public void StartAnim(int animDuration){
        this.animDuration = animDuration;
        animate = true;
    }
    private void Update() {
        if (animate) {
            animTimeCounter += Time.deltaTime;
        
            float t = animTimeCounter / animDuration;
            t = EaseInOutQuad(t); // Apply easing function

            backgroundImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal, 
                Mathf.Lerp(0, MAX_BACKGROUND_PX, t)
            );

            if (animTimeCounter >= animDuration) {
                animate = false;
            }
        }
    }

    // Easing function for ease-in-out quadratic
    private float EaseInOutQuad(float t) {
        if (t < 0.5f) {
            return 2 * t * t;
        } else {
            return -1 + (4 - 2 * t) * t;
        }
    }

}
