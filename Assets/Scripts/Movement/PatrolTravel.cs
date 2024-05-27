using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTravel : MonoBehaviour
{
    public event Action OnWait;
    public event Action OnMove;
    
    [SerializeField] private Transform transformToMove;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float waitingTime; // New serialized field for waiting time

    private Transform destinationPatrolPoint;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false; // Flag to track if the patrol is waiting
    private float waitTimer = 0f; // Timer for waiting

    private const float patrolPointReachDistance = 1f;

    private void Awake() {
        if (transformToMove == null) transformToMove = transform;
        if (patrolPoints.Length <= 1) {
            Debug.LogError("Make sure to add more than 1 patrol point!");
            enabled = false;
            return;
        }

        SetDestinationPatrolPoint();
    }

    private IEnumerator Start(){
        yield return null;
        OnMove?.Invoke();
    }

    private void Update() {
        if (HasReachedTargetDestination()) {
            if (!isWaiting) {
                StartWait();
            }
            else {
                Wait();
            }
        }
        else {
            MoveTowardsDestination();
        }
    }

    private void StartWait() {
        isWaiting = true;
        waitTimer = waitingTime;
        OnWait?.Invoke();
    }

    private void Wait() {
        waitTimer -= Time.deltaTime; 
        if (waitTimer <= 0f) {
            EndWait();
        }
    }

    private void EndWait() {
        isWaiting = false;
        SetDestinationPatrolPoint(); 
        OnMove?.Invoke();
    }

    private bool HasReachedTargetDestination() {
        return Vector3.Distance(transformToMove.position, destinationPatrolPoint.position) < patrolPointReachDistance;
    }

    private void MoveTowardsDestination() {
        Vector3 moveDir = (destinationPatrolPoint.position - transformToMove.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(moveDir, Vector3.up);
        transformToMove.rotation = Quaternion.Lerp(transformToMove.rotation, rotation, Time.deltaTime * rotationSpeed);
        transformToMove.position += movementSpeed * Time.deltaTime * moveDir;
    }

    private void SetDestinationPatrolPoint() {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        destinationPatrolPoint = patrolPoints[currentPatrolIndex];
    }
}
