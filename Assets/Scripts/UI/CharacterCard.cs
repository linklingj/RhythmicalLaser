using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CharacterCard : MenuButton2D {
    public string characterName;
    public string genre;
    public string description;
    //상대값, 0: 중앙, 1: 오른쪽, -1: 왼쪽
    public int posSave;

    [SerializeField] TextMeshProUGUI nameText1, nameText2;

    private void Start() {
        nameText1.text = characterName;
        nameText2.text = characterName;
    }


    //pos1에서 pos2로 이동
    public void SetPos(Vector2 pos1, Vector2 pos2) {
        transform.position = pos1;
        LeanTween.move(gameObject, pos2, 0.3f).setEase(LeanTweenType.easeOutQuad);
    }
}
