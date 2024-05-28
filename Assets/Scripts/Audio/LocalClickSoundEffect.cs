using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalClickSoundEffect : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip audioClip;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        LocalSoundManager.Instance.PlaySound(audioClip);
    }
}
