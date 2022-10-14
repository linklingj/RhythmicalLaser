using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    public GameObject note;
    public Transform noteParent;
    public int noteSpeed;
    int currentBPM;
    double currentTime;
    void Start() {
        currentBPM = musicPlayer.currentMusic.bpm;
    }

    void Update() {
        currentTime += Time.deltaTime;
    }
}
