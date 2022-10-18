using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public List<Music> musicList;
    public Music currentMusic;
    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic() {
        audioSource.clip = currentMusic.audio;
        audioSource.Play();
    }
}
