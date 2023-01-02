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
    public float songDelayInSeconds;
    public int inputDelayInMilliseconds;
    public bool musicPlaying;

    public static MidiFile midiFile;
    string midiFileLocation;
    void Awake() {
        
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        midiFileLocation = GameManager.Instance.selectedMusic.midiFileLocation;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://")) {
            StartCoroutine(ReadFromWebsite());
        }
        else {
            ReadFromFile();
        }

        audioSource = MusicPlayer.Instance.GetComponent<AudioSource>();
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
    }

    public void GetDataFromMidi() {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array,0);

        noteManager.SetTimeStamps(array);
        GameManager.Instance.totalNoteCount = array.Length;

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public static double GetAudioSourceTime() {
        //if (Instance.audioSource == null) return 0;
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
