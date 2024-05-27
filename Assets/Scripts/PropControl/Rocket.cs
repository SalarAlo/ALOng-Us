using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rocketSpeed;
    [SerializeField] private float accelaration;
    [SerializeField] private float flyStartDelay;
    private bool flying = false;

    private void Start() {
        Invoke(nameof(TriggerFly), flyStartDelay);

        Destroy(gameObject, flyStartDelay + 10);
    }

    private void Update() {
        if (flying) {
            rocketSpeed += accelaration;
            transform.position += new Vector3(0, rocketSpeed * Time.deltaTime, 0);
        }
    }

    private void TriggerFly() {
        flying = true;
    }
}
