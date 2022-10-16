using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    public PlayerController playerController;
    [Header("Notes")]
    public int noteSpeed;
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    [Header("Transform")]
    public Transform noteSpawnPos1, noteSpawnPos2, destroyPos;

    public List<double> timeStamp = new List<double>();
    int currentBPM;
    float noteActiveTime;
    int spawnIndex;

    void Start() {
        currentBPM = musicPlayer.currentMusic.bpm;
        spawnIndex = 0;
        noteActiveTime = (noteSpawnPos1.localPosition.y - destroyPos.localPosition.y) / noteSpeed;
    }

    void Update() {
        if (spawnIndex < timeStamp.Count) {
            if (SongManager.GetAudioSourceTime() >= timeStamp[spawnIndex] - noteActiveTime) {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position = noteSpawnPos1.position;
                t_note.SetActive(true);

                Note n = t_note.GetComponent<Note>();
                n.noteActive = true;
                n.noteSpeed = noteSpeed;
                n.spawnY = noteSpawnPos1.localPosition.y;
                n.despawnY = destroyPos.localPosition.y;
                n.timeInstantiated = SongManager.GetAudioSourceTime();
                n.noteActiveTime = noteActiveTime;
                n.assignedTime = (float)timeStamp[spawnIndex];

                spawnIndex++;
            }
        }
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array) {
        foreach (var note in array) {
            if (note.NoteName == noteRestriction) {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamp.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
}
