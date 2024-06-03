using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerSpawnManager))]
public class PlayerSpawnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerSpawnManager manager = (PlayerSpawnManager)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Ensure all roles are present and cannot be removed
        if (manager.playerRoles == null)
        {
            manager.playerRoles = new List<RoleCount>();
        }

        foreach (PlayerGameRole role in System.Enum.GetValues(typeof(PlayerGameRole)))
        {
            if (!manager.playerRoles.Exists(r => r.role == role))
            {
                manager.playerRoles.Add(new RoleCount { role = role, count = 0 });
            }
        }

        // Display fields for each role
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Player Roles", EditorStyles.boldLabel);

        int totalRoleCount = 0;
        foreach (var roleCount in manager.playerRoles) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(roleCount.role.ToString(), GUILayout.Width(100));
            roleCount.count = EditorGUILayout.IntField(roleCount.count);
            roleCount.count = Mathf.Clamp(roleCount.count, 0, AlongUsMultiplayer.MAX_PLAYERS_PER_LOBBY - totalRoleCount);
            totalRoleCount += roleCount.count;
            EditorGUILayout.EndHorizontal();
        }

        // Ensure the total role count does not exceed maxPlayerAmount
        if (totalRoleCount > AlongUsMultiplayer.MAX_PLAYERS_PER_LOBBY)
        {
            int excess = totalRoleCount - AlongUsMultiplayer.MAX_PLAYERS_PER_LOBBY;
            foreach (var roleCount in manager.playerRoles) {
                if (roleCount.count >= excess)
                {
                    roleCount.count -= excess;
                    break;
                }
                else
                {
                    excess -= roleCount.count;
                    roleCount.count = 0;
                }
            }
        } 

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }
    }
}
