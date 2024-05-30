using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPlayerPosition : MonoBehaviour
{
    private PlayerData childPlayerData;
    [SerializeField] private Transform playerPrefab;

    public void PopulateWithPlayer(PlayerData playerData) {
        childPlayerData = playerData;
        Instantiate(playerPrefab, transform);
    }
    
    public void Clear(){
        if (transform.childCount >= 1)
            Destroy(transform.GetChild(0).gameObject);
        childPlayerData = new();
    }
    public bool IsEmpty() => transform.childCount == 0;
}
