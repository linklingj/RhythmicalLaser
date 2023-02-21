using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Color Data", menuName = "Scriptable Object/ColorScheme")]

public class ColorScheme : ScriptableObject
{
    public Color32 player1;
    public Color32 player2;
    public Color32 player3;
    public Color32 laser1;
    public Color32 laser2;
    public Color32 enemy1;
    public Color32 enemy2;
    public Color32 BG;
    public Color32 UI1;
    public Color32 UI2;
    public Color32 UI3;
    public Color32 noteL;
    public Color32 noteR;
}
