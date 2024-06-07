using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonParent : Singleton<ActionButtonParent>
{
    public void AddChild(ActionButtonUI actionButtonUI) {
        actionButtonUI.transform.SetParent(transform);
        actionButtonUI.transform.localScale = Vector3.one;
    }
}
