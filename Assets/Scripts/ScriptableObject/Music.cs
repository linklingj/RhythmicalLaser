using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//123 Normal, 456 Hard, 789 Expert, 10+ Expert
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
    public float volume;
    public string midiFileLocation;
    public ColorScheme colorScheme;
    public int index;
}
