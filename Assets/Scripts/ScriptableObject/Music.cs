using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty {
    Normal,
    Hard,
    Expert,
    Master
}

[CreateAssetMenu(fileName = "Music Data", menuName = "Scriptable Object/Music Data")]
public class Music : ScriptableObject
{
    public string title;
    public string artist;
    public int bpm;
    public Difficulty difficulty;
    public int difficultyLvl;
    public AudioClip audio;
    public string midiFileLocation;
}
