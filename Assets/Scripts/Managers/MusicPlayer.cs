using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public static MusicPlayer Instance;
    public Music currentMusic;
    public AudioClip bgm;
    AudioSource audioSource;
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
            PlayBgm();
        }
    }
    
    public void StartMusic(Music music) {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = music.audio;
        audioSource.volume = music.volume;
        currentMusic = music;
        audioSource.Play();
    }

    public void GameOver() {
        float v = audioSource.volume;
        LeanTween.value(gameObject, 1, 0, 2).setOnUpdate((float val) => {
            audioSource.pitch = val;
            audioSource.volume = v * val;
        }).setOnComplete(() => {
            audioSource.Stop();
            audioSource.pitch = 1;
            audioSource.volume = v;
        });
    }

    public void PlayBgm() {
        audioSource.loop = true;
        audioSource.PlayOneShot(bgm);
    }
}
