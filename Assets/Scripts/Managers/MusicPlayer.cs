using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public Music currentMusic;
    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        currentMusic = GameManager.Instance.selectedMusic;
    }

    public void StartMusic() {
        audioSource.clip = currentMusic.audio;
        audioSource.Play();
    }
}
