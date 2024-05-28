using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSoundManager : SingletonPersistent<LocalSoundManager>
{
    [SerializeField] private SingleAudioSource audioSourcePrefab;

    public void PlaySound(AudioClip clip){
        SingleAudioSource audioSource = Instantiate(audioSourcePrefab, transform);
        audioSource.Play(clip);
    }
}
