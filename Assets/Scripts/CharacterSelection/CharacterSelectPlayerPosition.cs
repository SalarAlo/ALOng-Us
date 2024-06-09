using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterSelectPlayerPosition : MonoBehaviour
{
    [SerializeField] private PlayerVisualsColor playerPrefab;
    private PlayerData playerData;

    public void PopulateWithPlayer(PlayerData playerData) {
        this.playerData = playerData;
        var player = Instantiate(playerPrefab, transform);
        player.SetColorTo(playerData.colorIndex);
        player.GetComponent<CharacterSelectionPlayer>().SetClient(playerData);
    }
    
    public void Clear(){
        if (transform.childCount >= 1)
            Destroy(transform.GetChild(0).gameObject);
    }
    
    public bool IsEmpty() => transform.childCount == 0;

    public bool IsOccupiedBy(ulong clientId) => !IsEmpty() && playerData.clientId == clientId;
}
