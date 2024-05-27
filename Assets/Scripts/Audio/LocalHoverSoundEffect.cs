using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalHoverSoundEffect : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip audioClip;

    public void OnPointerEnter(PointerEventData eventData) {
        LocalSoundManager.Instance.PlaySound(audioClip);
    }
}
