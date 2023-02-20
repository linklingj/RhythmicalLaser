using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class hitSound {
    public string name;
    public AudioClip kick;
    public AudioClip snare;
}

public class SFXPlayer : MonoBehaviour {
    public static SFXPlayer Instance;
    
    [Header("gameplay")] public List<hitSound> hitSounds = new List<hitSound>();
    public int sfxSet;

    [Header("UI")] 
    [SerializeField] private AudioClip[] uiSound;
    
    [Header("Result")]
    public AudioClip clearSound;
    
    AudioSource audioSource;
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }
    void Start() {
        sfxSet = 0;
    }
    
    
    public void HitSound(int n) {
        if (n == 0)
            audioSource.PlayOneShot(hitSounds[sfxSet].kick);
        else 
            audioSource.PlayOneShot(hitSounds[sfxSet].snare);
    }

    public void UISOund(int n) {
        audioSource.PlayOneShot(uiSound[n]);
    }
    
    public void PlayClip(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}
