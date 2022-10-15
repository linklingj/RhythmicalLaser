using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    public PlayerController playerController;
    [Header("Notes")]
    public int noteSpeed;
    [Header("Transform")]
    public Transform noteSpawnPos1, noteSpawnPos2, destroyPos;
    int currentBPM;
    double currentTime;
    void Start() {
        currentBPM = musicPlayer.currentMusic.bpm;
        currentTime = 0;
    }

    void Update() {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / currentBPM) {
            GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
            t_note.transform.position = noteSpawnPos1.position;
            t_note.SetActive(true);
            t_note.GetComponent<Note>().noteActive = true;
            playerController.Shoot();
            currentTime -= 60d / currentBPM;
        }
    }
}
