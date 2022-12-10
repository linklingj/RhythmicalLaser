using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MusicCard : MenuButton2D {
    public Music music;
    public string description;
    //상대값, 0: 중앙, 1: 오른쪽, -1: 왼쪽
    public int posSave;

    [SerializeField] TextMeshProUGUI nameText1, nameText2, artistText, levelText, maxScoreText, maxComboText;
    [SerializeField] GameObject levelImg;

    private void Start() {
        nameText1.text = music.title;
        nameText2.text = music.title;
        artistText.text = music.artist;
        levelText.text = music.difficultyLvl.ToString();
    }

    public void SetColor(string dif, Color32 color) {
        levelImg.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dif;
        levelImg.GetComponent<Image>().color = color;
    }


    //pos1에서 pos2로 이동
    public void SetPos(Vector2 pos1, Vector2 pos2) {
        transform.position = pos1;
        LeanTween.move(gameObject, pos2, 0.3f).setEase(LeanTweenType.easeOutQuad);
    }
}