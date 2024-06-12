using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralActionsManager : Singleton<GeneralActionsManager>
{
    [SerializeField] private List<ActionDataSO> allActions;
    private const int reach = 5;

    public ActionDataSO GetDataForAction(PlayerAction playerAction) {
        return allActions.Find(actionData => actionData.action == playerAction);
    }

    public Func<bool> GetCheckForExecution(PlayerAction playerAction) {
        switch(playerAction){
            case PlayerAction.Track:
                break;
        }
        return () => true;
    }

    public Action GetExecutableForAction(PlayerAction playerAction){
        if(!GetCheckForExecution(playerAction)()) return () => { Debug.Log("couldnt execue function"); };
        switch(playerAction){
            case PlayerAction.Kill:
                return () => {
                    // Kill the target
                };
            case PlayerAction.Invisible:
                return () => {
                    AlongUsMultiplayer.Instance.SetPlayerInvisibleServerRpc(Player.LocalInstance.NetworkObject);
                };
            case PlayerAction.Track:
                return () => {
                    if(!TryGetPlayerInFront(out Player player)) return;
                    player.GetComponent<PlayerTracker>().Show();
                    Player.LocalInstance.GetComponent<PlayerActionsManager>().ReplaceAction(PlayerAction.Track, PlayerAction.TakeTrack);
                };
            default:
                return null;
        }
        // return () => Debug.Log(playerAction.ToString());
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
