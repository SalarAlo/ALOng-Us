using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSoundManager : SingletonPersistent<LocalSoundManager>
{
    private AudioSource audioSource;

    public override void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }
}
