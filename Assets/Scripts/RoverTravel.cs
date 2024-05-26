using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverTravel : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    private Transform destinationPatrolPoint;
    private int currentPatrolIndex = 0;

    private void Awake() {
        if (patrolPoints.Length <= 1) {
            Debug.LogError("Make sure to add more than 1 patrol point!");
            enabled = false; // Disable the script if there are not enough patrol points
            return;
        }

        destinationPatrolPoint = patrolPoints[currentPatrolIndex];
    }

    private void Update() {
        float patrolPointReachDistance = 1f;
        bool reachedTargetDestination = Vector3.Distance(transform.position, destinationPatrolPoint.position) < patrolPointReachDistance;
        if (reachedTargetDestination) {
            destinationPatrolPoint = GetNewPatrolPoint();
        } else {
            Vector3 moveDir = (destinationPatrolPoint.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            transform.position += movementSpeed * Time.deltaTime * moveDir;
        }
    }

    private Transform GetNewPatrolPoint() {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        return patrolPoints[currentPatrolIndex];
    }
}
