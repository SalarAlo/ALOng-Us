using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonsParent : Singleton<ActionButtonsParent>
{
    public ActionButtonUI[] GetChildren(){
        return transform.GetComponentsInChildren<ActionButtonUI>();
    }
}
