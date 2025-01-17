using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScreen : ScreenManager
{
    public Animator transition;
    public float transitionTime;
    [SerializeField] private CanvasGroup[] alphaElements;
    [SerializeField] private TextMeshProUGUI titleText, difficultyText, pointText, rpText, comboText, rankText;
    [SerializeField] private RectTransform highScore1, highScore2;
    [SerializeField] private GameObject medal;
    [SerializeField] private Sprite[] medalSprites;
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
            LeanTween.scale(highScore1, new Vector3(1, 1), 0.3f).setDelay(2f);
        }

        //rhythm point 3 ~ 6.3
        LeanTween.alphaCanvas(alphaElements[2], 1, 0.3f).setDelay(3f);
        LeanTween.value(0, GameManager.Instance.rhythmPoint, 1f).setDelay(3.3f).setEase(LeanTweenType.easeOutQuart).setOnUpdate((float val) => {
            rpText.text = (Mathf.Round(val*1000)/10).ToString()+ "%";
        });
        LeanTween.value(0, GameManager.Instance.maxCombo, 1f).setDelay(5f).setEase(LeanTweenType.easeOutQuart).setOnUpdate((float val) => {
            comboText.text = Mathf.Round(val).ToString();
        });
        if (rpHighScore) {
            LeanTween.alphaCanvas(alphaElements[6], 1, 0.3f).setDelay(6f);
            LeanTween.scale(highScore2, new Vector3(1, 1), 0.3f).setDelay(6f);
        }
        
        //rank medal 7~8
        rankText.text = GameManager.Instance.GetRankFromNum(-11);
        LeanTween.alphaCanvas(alphaElements[4], 1, 0.5f).setDelay(7f);
        int medalIndex = 0;
        switch (GameManager.Instance.rank / 2) {
            case 0:
                medalIndex = 0;
                break;
            case 1:
                medalIndex = 1;
                break;
            case 2:
                medalIndex = 2;
                break;
            default:
                medalIndex = 3;
                break;
        }
        medal.GetComponent<Image>().sprite = medalSprites[medalIndex];
        medal.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 1f);
        LeanTween.rotate(medal, new Vector3(0, 360 * 4, 0), 1f).setDelay(7f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.scale(medal.GetComponent<RectTransform>(), new Vector2(1, 1), 1f).setDelay(7f).setEase(LeanTweenType.easeOutBack);

    }

    void Initialize() {
        foreach (var el in alphaElements)
            el.alpha = 0;
        titleText.text = GameManager.Instance.selectedMusic.title;
        difficultyText.text = GameManager.Instance.selectedMusic.difficulty.ToString();
        pointText.text = "0";
        rpText.text = "0%";
    }
    
    void SaveData() {
        Save_MusicData newData = DataManager.Instance.playerData.characterDatas[GameManager.Instance.selectedCharacter].musicDatas[GameManager.Instance.selectedMusic.index - 1];
        //check if high score
        if (newData.highScore < GameManager.Instance.point)
            spHighScore = true;
        if (newData.maxRP < GameManager.Instance.rhythmPoint)
            rpHighScore = true;
        //save data
        newData.clear = true;
        newData.highScore = Math.Max(newData.highScore, GameManager.Instance.point);
        newData.maxCombo = Math.Max(newData.maxCombo, GameManager.Instance.maxCombo);
        newData.maxRP = Math.Max(newData.maxRP, GameManager.Instance.rhythmPoint);
        newData.maxGrade = Math.Max(newData.maxGrade, GameManager.Instance.rank);
        DataManager.Instance.UpdateMusicData(GameManager.Instance.selectedCharacter,newData.musicNum, newData);
    }


    public override void Button(int n) {
        if (n == 0) {
            StartCoroutine(Transition4());
        } if (n == 1) {
            GameManager.Instance.ToMusicSelect(GameManager.Instance.selectedCharacter);
            MusicPlayer.Instance.PlayBgm();
        }
        SFXPlayer.Instance.UISound(0);
    }
    
    IEnumerator Transition4() {
        transition.Play("transition4");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.Play(GameManager.Instance.selectedMusic);
    }
}
