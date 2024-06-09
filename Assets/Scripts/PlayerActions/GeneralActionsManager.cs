using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralActionsManager : Singleton<GeneralActionsManager>
{
    [SerializeField] private List<ActionDataSO> allActions;

    public ActionDataSO GetDataForAction(PlayerAction playerAction) {
        return allActions.Find(actionData => actionData.action == playerAction);
    }
}
