using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    [Header("Notes")]
    public GameObject note;
    public int noteSpeed;
    [Header("Transform")]
    public Transform noteParent;
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
            GameObject n = Instantiate(note, noteSpawnPos1.position, Quaternion.identity);
            n.transform.SetParent(noteParent);
            currentTime -= 60d / currentBPM;
        }
    }
}
