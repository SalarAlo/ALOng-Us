using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : Singleton<OutlineManager>
{
    private Player lastPlayerOutlined;
    private Func<bool> conditionToUnoutline;

    public void OutlinePlayer(Player player, Func<bool> conditionToUnoutline){
        player.GetComponent<Outline>().enabled = true;
        lastPlayerOutlined = player;
        this.conditionToUnoutline = conditionToUnoutline;
    }

    public bool IsPlayerOutlined(Player check = null) => check == null ? lastPlayerOutlined != null : lastPlayerOutlined == check;
    private void DisableLastPlayerOutline(){
        lastPlayerOutlined.GetComponent<Outline>().enabled = false;
        lastPlayerOutlined = null;
        conditionToUnoutline = null;
    }

    private void Update(){
        if(!IsPlayerOutlined()) return;
        if(conditionToUnoutline()) {
            Debug.Log("UNOUTLINE");
            DisableLastPlayerOutline();
        }
    }
}
