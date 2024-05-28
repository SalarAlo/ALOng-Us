using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAudioSource : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Update() {
        if(audioSource.isPlaying) return;
        Destroy(gameObject, .1f);
    }
}
