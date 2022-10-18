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
    public MusicPlayer musicPlayer;
    public AudioSource audioSource;
    public NoteManager noteManager;
    public float songDelayInSeconds;
    public double marginOfError;

    public int inputDelayInMilliseconds;

    public static MidiFile midiFile;
    void Start() {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://")) {
            StartCoroutine(ReadFromWebsite());
        }
        else {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite() {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + '/' + musicPlayer.currentMusic.midiFileLocation)) {
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
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + musicPlayer.currentMusic.midiFileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi() {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array,0);

        noteManager.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public static double GetAudioSourceTime() {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    public void StartSong() {
        musicPlayer.StartMusic();
    }

}
