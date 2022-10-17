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
    public Melanchall.DryWetMidi.MusicTheory.NoteName kickNoteRestriction;
    public Melanchall.DryWetMidi.MusicTheory.NoteName snareNoteRestriction;
    [Header("Transform")]
    public Transform noteSpawnPos1, noteSpawnPos2, hitPos, destroyPos;

    public List<double> kickTimeStamp = new List<double>();
    public List<double> snareTimeStamp = new List<double>();
    int currentBPM;
    float noteActiveTime,noteMoveTime;
    int k_spawnIndex, s_spawnIndex;

    void Start() {
        currentBPM = musicPlayer.currentMusic.bpm;
        k_spawnIndex = 0;
        s_spawnIndex = 0;
        noteMoveTime = (noteSpawnPos1.localPosition.y - hitPos.localPosition.y) / noteSpeed;
        noteActiveTime = (noteSpawnPos1.localPosition.y - destroyPos.localPosition.y) / noteSpeed;
    }

    void Update() {
        if (k_spawnIndex < kickTimeStamp.Count) {
            if (SongManager.GetAudioSourceTime() >= kickTimeStamp[k_spawnIndex] - noteMoveTime) {
                SpawnNote(0);
            }
        }
        if (s_spawnIndex < snareTimeStamp.Count) {
            if (SongManager.GetAudioSourceTime() >= snareTimeStamp[s_spawnIndex] - noteMoveTime) {
                SpawnNote(1);
            }
        }
    }

    private void SpawnNote(int identity) {

                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                if (identity == 0)
                    t_note.transform.position = noteSpawnPos1.position;
                else
                    t_note.transform.position = noteSpawnPos2.position;

                t_note.SetActive(true);

                Note n = t_note.GetComponent<Note>();
                n.noteActive = true;
                n.beforeHit = true;
                n.noteSpeed = noteSpeed;
                n.spawnY = noteSpawnPos1.localPosition.y;
                n.despawnY = destroyPos.localPosition.y;
                n.timeInstantiated = SongManager.GetAudioSourceTime();
                n.noteActiveTime = noteActiveTime;
                n.noteMoveTime = noteMoveTime;
                n.noteIdentity = identity;

                if (identity == 0) {
                    n.assignedTime = (float)kickTimeStamp[k_spawnIndex];
                    k_spawnIndex++;
                } else {
                    n.assignedTime = (float)snareTimeStamp[s_spawnIndex];
                    s_spawnIndex++;
                }
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array) {
        foreach (var note in array) {
            if (note.NoteName == kickNoteRestriction) {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                kickTimeStamp.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            } else if (note.NoteName == snareNoteRestriction) {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                snareTimeStamp.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    public void NoteHit(int identity) {
        if (identity == 0) {
            playerController.Kick();
        } else if (identity == 1) {
            playerController.Snare();
        }
    }
}
