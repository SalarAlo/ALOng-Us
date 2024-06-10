using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntireMapUI : BaseUISingleton<EntireMapUI>
{
    [SerializeField] private float openSpeed;
    private bool animating;
    private Vector3 destinationSize;

    private void Start() {
        GameInput.Instance.SubscribeToOpenMap(() => {
            Player.LocalInstance.GetComponent<PlayerController>().SetCanMove(isOpen);
            PlayerCam.Instance.SetCanLookAround(isOpen);
            if(isOpen) {
                Hide();
                MainUI.Instance.Show();
            } else {
                Show();
                MainUI.Instance.Hide();
            }
        });
    }

    public override void Hide() {
        animating = true;
        destinationSize = Vector3.zero;
    }

    public override void Show() {
        base.Show();
        animating = true;
        destinationSize = Vector3.one;
    }

    private void Update() {
        if(!animating) return;
        Transform transform = ownWindow.transform;

        transform.localScale = Vector3.Lerp(transform.localScale, destinationSize, Time.deltaTime * openSpeed);
        float openThreshold = 0.01f;

        if(Vector3.Distance(transform.localScale, destinationSize) < openThreshold) {
            animating = false;
            if(destinationSize == Vector3.zero) {
                base.Hide();
            }
        }
    }
}
