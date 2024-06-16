using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEditor;

public class GeneralActionsManager : Singleton<GeneralActionsManager>
{
    [SerializeField] private List<ActionDataSO> allActions;
    private const int reach = 5;

    public ActionDataSO GetDataForAction(PlayerAction playerAction) {
        return allActions.Find(actionData => actionData.action == playerAction);
    }

    public Func<bool> GetCheckForExecutionOfAction(PlayerAction playerAction) {
        switch(playerAction){
            case PlayerAction.Track:
                return () => TryGetPlayerInFront(out Player player) && !player.GetComponent<PlayerTracker>().IsTracked();
            case PlayerAction.TakeTrack:
                return () => TryGetPlayerInFront(out Player player) && player.GetComponent<PlayerTracker>().IsTracked();
            case PlayerAction.Use:
                return () => TryGetUsableInFront(out IUseable _);
        }
        return () => true;
    }

    public Action GetExecutableForActionUpdate(PlayerAction action){
        switch(action) {
            case PlayerAction.Track:
                return () => {
                    if(!TryGetPlayerInFront(out Player player)) {
                        return;
                    }
                    OutlineManager.Instance.OutlinePlayer(player, () => 
                        !TryGetPlayerInFront(out Player _) && 
                        Player.LocalInstance.GetComponent<PlayerActionsManager>().HasAction(action)
                    );
                };
            case PlayerAction.TakeTrack:
                return () => {
                    if(!TryGetPlayerInFront(out Player player)) {
                        return;
                    }
                    OutlineManager.Instance.OutlinePlayer(player, () => 
                        !TryGetPlayerInFront(out Player _) && 
                        Player.LocalInstance.GetComponent<PlayerActionsManager>().HasAction(action)
                    );
                };
        }

        return () => Debug.Log("No update exctable necessary");
    }

    public Action GetExecutableForAction(PlayerAction playerAction){
        if(!GetCheckForExecutionOfAction(playerAction)()) return () => Debug.Log("Cant execute action");
        switch(playerAction){
            case PlayerAction.Use:
                return () => {
                    TryGetUsableInFront(out IUseable useable);
                    useable.Use();
                };
            case PlayerAction.Kill:
                return () => {
                    // Kill the target
                };
            case PlayerAction.Sabotage:
                return () => {
                    SabotageUI.Instance.Show();
                };
            case PlayerAction.Morph:
                return () => {
                    MorphUI.Instance.Show();
                };
            case PlayerAction.Invisible:
                return () => {
                    AlongUsMultiplayer.Instance.SetPlayerInvisibleServerRpc(Player.LocalInstance.NetworkObject);
                };
            case PlayerAction.Track:
                return () => {
                    Player playerToTrack = GetPlayerInFront();
                    playerToTrack.GetComponent<PlayerTracker>().Show();
                    Player.LocalInstance.GetComponent<PlayerActionsManager>().ReplaceAction(PlayerAction.Track, PlayerAction.TakeTrack);
                };
            case PlayerAction.TakeTrack:
                return () => {
                    Player playerToUntrack = GetPlayerInFront();
                    playerToUntrack.GetComponent<PlayerTracker>().Hide();
                    Player.LocalInstance.GetComponent<PlayerActionsManager>().ReplaceAction(PlayerAction.TakeTrack, PlayerAction.Track);
                };
            default:
                return null;
        }
        // return () => Debug.Log(playerAction.ToString());
    }

    private bool TryGetUsableInFront(out IUseable useable){
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, reach);
        bool hitUseable = false;
        useable = null;
        if(hitSomething)
            hitUseable = hit.transform.TryGetComponent(out useable);
        return hitSomething && hitUseable;
    }

    private Player GetPlayerInFront(){
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        Physics.Raycast(ray, out RaycastHit hit);
        
        return hit.transform.GetComponent<Player>();
    }
    private bool TryGetPlayerInFront(out Player player) {
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        
        if (!Physics.Raycast(ray, out RaycastHit hit, reach)) { 
            player = null;
            return false; 
        }

        return hit.transform.TryGetComponent(out player);
    }
}
