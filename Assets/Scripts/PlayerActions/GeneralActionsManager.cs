using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
        switch(playerAction){
            case PlayerAction.Kill:
                return () => {
                    // Kill the target
                };
            case PlayerAction.Invisible:
                return () => {
                    if(!GetCheckForExecutionOfAction(playerAction)()) return;
                    AlongUsMultiplayer.Instance.SetPlayerInvisibleServerRpc(Player.LocalInstance.NetworkObject);
                };
            case PlayerAction.Track:
                return () => {
                    if(!GetCheckForExecutionOfAction(playerAction)()) return;
                    Player playerToTrack = GetPlayerInFront();
                    playerToTrack.GetComponent<PlayerTracker>().Show();
                    Player.LocalInstance.GetComponent<PlayerActionsManager>().ReplaceAction(PlayerAction.Track, PlayerAction.TakeTrack);
                };
            case PlayerAction.TakeTrack:
                return () => {
                    if(!GetCheckForExecutionOfAction(playerAction)()) return;
                    Player playerToUntrack = GetPlayerInFront();
                    playerToUntrack.GetComponent<PlayerTracker>().Hide();
                    Player.LocalInstance.GetComponent<PlayerActionsManager>().ReplaceAction(PlayerAction.TakeTrack, PlayerAction.Track);
                };
            default:
                return null;
        }
        // return () => Debug.Log(playerAction.ToString());
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
