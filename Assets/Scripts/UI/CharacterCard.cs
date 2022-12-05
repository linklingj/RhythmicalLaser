using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterCard : MenuButton2D {
    public string characterName;
    public string genre;
    public string description;

    //상대값, 0: 중앙, 1: 오른쪽, -1: 왼쪽
    public int currentPos;

    public void SetPos(int n) {
        currentPos = n;
    }
}
