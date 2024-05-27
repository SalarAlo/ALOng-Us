using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PatrolTravel))]
public class PatrolAnimator : MonoBehaviour
{
    private const string MOVING_BOOL_ANIM = "Moving";

    [SerializeField] private Animator anim;
    private PatrolTravel patrolTravel;

    private void Awake() {
        if (anim == null) anim = GetComponent<Animator>();
        patrolTravel = GetComponent<PatrolTravel>();
    }

    private void Start() {
        patrolTravel.OnMove += PatrolTravel_OnMove;    
        patrolTravel.OnWait += PatrolTravel_OnWait; 
    }

    private void PatrolTravel_OnWait() {
        anim.SetBool(MOVING_BOOL_ANIM, false);
    }

    private void PatrolTravel_OnMove() {
        anim.SetBool(MOVING_BOOL_ANIM, true);
    }
}
