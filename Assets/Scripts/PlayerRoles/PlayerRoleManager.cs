using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRoleManager : MonoBehaviour {
    [SerializeField] private PlayerRole role;

    public PlayerRole GetRole() => role; 
    public void SetRole(PlayerRole role) => this.role = role; 
}