using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music Data", menuName = "Scriptable Object/Music Data")]
public class Music : ScriptableObject
{
    public string title;
    public string artist;
    public int bpm;
    public double duration;
}
