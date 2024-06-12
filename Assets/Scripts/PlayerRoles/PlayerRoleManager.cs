using System;
using Unity.VisualScripting;
using UnityEngine;
using Unity;

public class PlayerRoleManager : MonoBehaviour {
    [SerializeField] private PlayerRole role;

    public PlayerRole GetRole() => role; 
    public void SetRole(PlayerRole role)  {
        this.role = role;
    }
}
