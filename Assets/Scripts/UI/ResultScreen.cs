using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ResultScreen : ScreenManager
{
    public Animator transition;
    public float transitionTime;
    [SerializeField] private CanvasGroup[] alphaElements;
    [SerializeField] private TextMeshProUGUI titleText, difficultyText, pointText, rpText, comboText;
    [SerializeField] private RectTransform hightScore1, highScore2;
    private bool spHighScore, rpHighScore;

    private void Start() {
        spHighScore = false;
        rpHighScore = false;
        if (FindObjectOfType<DataManager>() != null)
            SaveData();
        
        Initialize();
        
        //title, artist
        LeanTween.alphaCanvas(alphaElements[0], 1, 0.3f).setDelay(0.3f);
        
        //point 1 ~ 2.3
        LeanTween.alphaCanvas(alphaElements[1], 1, 0.3f).setDelay(1f);
        LeanTween.value(0, GameManager.Instance.point, 1f).setDelay(1.3f).setEase(LeanTweenType.easeOutQuart).setOnUpdate((float val) => {
            pointText.text = Mathf.Round(val).ToString();
        });
        if (spHighScore) {
            LeanTween.alphaCanvas(alphaElements[5], 1, 0.3f).setDelay(2f);
            LeanTween.scale(hightScore1, new Vector3(1, 1), 0.3f).setDelay(2f);
        }

        //rhythm point 3 ~ 6
        LeanTween.alphaCanvas(alphaElements[2], 1, 0.3f).setDelay(3f);
        LeanTween.value(0, GameManager.Instance.rhythmPoint, 1f).setDelay(3.3f).setEase(LeanTweenType.easeOutQuart).setOnUpdate((float val) => {
            rpText.text = (Mathf.Round(val*10)/10).ToString()+ "%";
        });
        LeanTween.value(0, GameManager.Instance.maxCombo, 1f).setDelay(5f).setEase(LeanTweenType.easeOutQuart).setOnUpdate((float val) => {
            comboText.text = Mathf.Round(val).ToString();
        });
    }

    void Initialize() {
        foreach (var el in alphaElements)
            el.alpha = 0;
        titleText.text = GameManager.Instance.selectedMusic.title;
        difficultyText.text = GameManager.Instance.selectedMusic.difficulty.ToString();
        pointText.text = "0";
        rpText.text = "0%";
    }

    private void SaveData() {
        Save_MusicData newData = DataManager.Instance.playerData.characterDatas[GameManager.Instance.selectedCharacter].musicDatas[GameManager.Instance.selectedMusic.index - 1];
        //check if high score
        if (newData.highScore < GameManager.Instance.point)
            spHighScore = true;
        if (newData.maxCombo < GameManager.Instance.maxCombo)
            rpHighScore = true;
        //save data
        newData.clear = true;
        newData.highScore = Math.Max(newData.highScore, GameManager.Instance.point);
        newData.maxCombo = Math.Max(newData.maxCombo, GameManager.Instance.maxCombo);
        DataManager.Instance.UpdateMusicData(GameManager.Instance.selectedCharacter,newData.musicNum, newData);
    }


    public override void Button(int n) {
        if (n == 0) {
            StartCoroutine(Transition4());
        } if (n == 1) {
            GameManager.Instance.ToMusicSelect(GameManager.Instance.selectedCharacter);
        }
    }
    
    IEnumerator Transition4() {
        transition.Play("transition4");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.Play(GameManager.Instance.selectedMusic);
    }
}
