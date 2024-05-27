using UnityEngine;

public class SatelliteRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationPauseDuration = 1f;
    [SerializeField] private float angleThreshold = 5f; 
    private bool waiting;
    private float waitTimeCounter;
    private Quaternion targetRotation;

    private void Start()
    {
        SetTargetToRandomDirection();
    }

    private void SetTargetToRandomDirection()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;
        targetRotation = Quaternion.LookRotation(randomDirection, Vector3.up);
    }

    private void Update()
    {
        if (!waiting) {
            RotateTowardsTarget();
            
            if (Vector3.Angle(transform.forward, targetRotation * Vector3.forward) <= angleThreshold) {
                waiting = true;
                waitTimeCounter = rotationPauseDuration;
            }
        }
        else
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                SetTargetToRandomDirection();
                waiting = false;
            }
        }

    }

    private void RotateTowardsTarget() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
