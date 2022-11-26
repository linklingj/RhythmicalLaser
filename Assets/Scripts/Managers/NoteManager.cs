using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    public PlayerController playerController;
    public CameraController cam;
    [Header("Notes")]
    public int noteSpeed;
    public float visualDelay;
    public Melanchall.DryWetMidi.MusicTheory.NoteName kickNoteRestriction;
    public Melanchall.DryWetMidi.MusicTheory.NoteName snareNoteRestriction;
    [Header("Transform")]
    public Transform noteSpawnPos1, noteSpawnPos2, hitPos, destroyPos;
    [Header("Input")]
    public KeyCode[] kickInput;
    public KeyCode[] snareInput;
    //판정 범위 시간
    public float marginOfError;
    //보정값: 높을수록 노트의 판단에서 앞노트에 우선순위 둠
    public float correctionVal;
    [Header("")]
    public List<double> kickTimeStamp = new List<double>();
    public List<double> snareTimeStamp = new List<double>();
    public List <bool> k_hit = new List<bool>();
    public List <bool> s_hit = new List<bool>();
    
    int currentBPM;
    float noteActiveTime,noteMoveTime;
    int k_spawnIndex, s_spawnIndex;
    int k_inputIndex, s_inputIndex;

    void Start() {
        currentBPM = musicPlayer.currentMusic.bpm;
        k_spawnIndex = 0;
        s_spawnIndex = 0;
        k_inputIndex = 0;
        s_inputIndex = 0;
        noteMoveTime = (noteSpawnPos1.localPosition.y - hitPos.localPosition.y) / noteSpeed;
    }

    void Update() {
        CheckNote();
        CheckInput();
    }

    void CheckNote() {
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
    

    void CheckInput() {
        if (Input.GetButtonDown("Kick")) {
            k_inputIndex = BtnHit(0, kickTimeStamp, k_inputIndex, k_hit);
        }
        if (Input.GetButtonDown("Snare")) {
            s_inputIndex = BtnHit(1, snareTimeStamp, s_inputIndex, s_hit);
        }
    }

    int BtnHit(int identity, List<double> timeStampList, int inputIndex, List<bool> hitList) {
        /*
        inputIndex는 항상 마지막 히트한 input의 인덱스 +1 를 가리킴
        만약 이번 timeStamp와 현재시간의 차이가 다음 timeStamp와 현재시간의 차이보다 크면 inputIndex를 증가시킴
        즉 격차가 최소가 되는 index를 찾음
        */
        double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
        float curTimeDif = Mathf.Abs((float)(timeStampList[inputIndex] - audioTime));
        float nextTimeDif = (inputIndex < timeStampList.Count)? Mathf.Abs((float)(timeStampList[inputIndex+1] - audioTime)) : -1;
        while(curTimeDif > nextTimeDif && nextTimeDif + correctionVal >= 0) {
            inputIndex += 1;
            curTimeDif = nextTimeDif;
            nextTimeDif = Mathf.Abs((float)(timeStampList[inputIndex+1] - audioTime));
        }
        double timeStamp = timeStampList[inputIndex];
        if (Mathf.Abs((float)(audioTime - timeStamp)) < marginOfError) {
            print($"{identity}Hit index:{inputIndex} delay:{(float)(audioTime - timeStamp)}");
            hitList[inputIndex] = true;
            inputIndex++;
            NoteHit(identity);
        } else {
            print($"{identity}Miss index:{inputIndex} delay:{(float)(audioTime - timeStamp)}");
            MissHit();
        }
        return inputIndex;
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
        n.hitY = hitPos.localPosition.y;
        n.timeInstantiated = SongManager.GetAudioSourceTime();
        n.noteMoveTime = noteMoveTime;
        n.visualDelay = visualDelay;
        n.noteIdentity = identity;

        if (identity == 0) {
            n.assignedTime = (float)kickTimeStamp[k_spawnIndex];
            n.index = k_spawnIndex;
            k_spawnIndex++;
        } else {
            n.assignedTime = (float)snareTimeStamp[s_spawnIndex];
            n.index = s_spawnIndex;
            s_spawnIndex++;
        }
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array) {
        foreach (var note in array) {
            if (note.NoteName == kickNoteRestriction) {
                k_hit.Add(false);
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                kickTimeStamp.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            } else if (note.NoteName == snareNoteRestriction) {
                s_hit.Add(false);
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
        GameManager.Instance.combo += 1;
        cam.CameraZoom();
    }

    public void NoHit() {
        ComboBreak();
    }
    
    public void MissHit() {
        ComboBreak();
    }

    public void ComboBreak() {
        GameManager.Instance.combo = 0;
        cam.ResetZoom();
    }
}
