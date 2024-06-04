using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRoleManager : MonoBehaviour {
    [SerializeField] private PlayerGameRole role;

    public PlayerGameRole GetRole() => role; 
    public void SetRole(PlayerGameRole role) => this.role = role; 
}

public enum PlayerGameRole {
    Crewmate,
    Imposter,
    Seer,
    Specter,
    Jester,
    Morpher,
}