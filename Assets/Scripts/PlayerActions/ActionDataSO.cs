using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/new Action")]
public class ActionDataSO : ScriptableObject {
    public PlayerAction action;
    public Sprite sprite;
    public int cooldown;
}
