using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/new PlayerRole")]
public class PlayerRoleDataSO : ScriptableObject
{
    public PlayerRole playerRole;
    public Color color;
    public string description;
    public ActionDataSO[] actions;
    public bool canCompleteTasks = true;
}
