using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public NoteManager noteManager;
    public EnemySpawner enemySpawner;
    public float songDelayInSeconds;
    public int inputDelayInMilliseconds;
    public bool musicPlaying;

    public static MidiFile midiFile;
    public static MidiFile epFile;
    string midiFileLocation, epFileLocation;
    void Awake() {
        
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        audioSource = MusicPlayer.Instance.GetComponent<AudioSource>();
        
        midiFileLocation = GameManager.Instance.selectedMusic.midiFileLocation;
        epFileLocation = GameManager.Instance.selectedMusic.epFileLocation;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://")) {
            StartCoroutine(ReadFromWebsite());
        }
        else {
            ReadFromFile();
        }

    }

    private void Start() {
        musicPlaying = false;
    }

    private IEnumerator ReadFromWebsite() {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + '/' + midiFileLocation)) {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(www.error);
            } else {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results)) {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile() {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiFileLocation);
        GetDataFromMidi();
        epFile = MidiFile.Read(Application.streamingAssetsPath + "/EP/" + epFileLocation);
        GetEPDataFromMidi();
    }

    public void GetDataFromMidi() {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array,0);

        noteManager.SetTimeStamps(array);
        
        //noteManager.timePerBar = (float)midiFile.GetTempoMap().GetTimeSpan(midiFile.GetTrackChunks()[0].GetNotes().GetTimeSpan()).TotalSeconds / (float)midiFile.GetTrackChunks()[0].GetNotes().GetTimeSpan().GetBars();
        var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(array[0].Time, midiFile.GetTempoMap());
        noteManager.barSpawnTime = (double)metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f;

        GameManager.Instance.totalNoteCount = array.Length;
        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void GetEPDataFromMidi() {
        var notes = epFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        enemySpawner.SetEPTimeStamps(array);
        GameManager.Instance.totalEnemyCount = array.Length;
    }

    public static double GetAudioSourceTime() {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    public void StartSong() {
        MusicPlayer.Instance.StartMusic(GameManager.Instance.selectedMusic);
        musicPlaying = true;
        StartCoroutine(nameof(WaitForSongFinish));
    }

    IEnumerator WaitForSongFinish() {
        yield return new WaitUntil(() => Instance.audioSource.isPlaying == false);
        GameManager.Instance.MusicFinished();
    }
}
