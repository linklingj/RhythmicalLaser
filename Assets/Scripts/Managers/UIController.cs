using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    [Header("UI Elements")]
    //top
    [SerializeField] Slider progressBar;
    //left
    [SerializeField] TextMeshProUGUI points_t,combo_t,title_t,artist_t,difficulty_t;
    [SerializeField] GameObject heartParent, fade;
    [SerializeField] Image leftBar,rightBar,progressBarImg,progressBarBg;
    [SerializeField] SpriteRenderer bgR;
    [SerializeField] Image[] bgs;
    [SerializeField] GameObject heartPrefab;
    [SerializeField]  GameObject[] heartObjs;
    [SerializeField] SpriteRenderer playerR, playerDirR, bounderyR, laserR;
    [SerializeField] Material blastMat, flashMat;
    [SerializeField] Transform feedbackPos;
    [SerializeField] GameObject feedbackPrefab;
    public ColorScheme currentColor;

    private List<GameObject> hearts;

    private void Awake() {
        GameManager.OnGameStateChange += OnGameStateChange;
        GameManager.OnPlayerHit += PlayerHit;
        GameManager.OnNoteMiss += NoteMiss;
        GameManager.OnNoteHit += NoteHit;
        hearts = new List<GameObject>();
    }

    void Start() {
        Music music = GameManager.Instance.selectedMusic;

        currentColor = music.colorScheme;

        title_t.text = music.title;
        artist_t.text = music.artist;
        difficulty_t.text = music.difficulty.ToString();

        bgR.color = currentColor.BG;
        foreach (Image item in bgs)
            item.color = currentColor.BG;

        playerR.color = currentColor.player1;
        playerDirR.color = currentColor.player2;
        bounderyR.color = currentColor.player3;
        laserR.color = currentColor.laser1;

        points_t.color = currentColor.UI1;
        combo_t.color = currentColor.UI2;
        title_t.color = currentColor.UI1;
        artist_t.color = currentColor.UI1;
        difficulty_t.color = currentColor.UI1;
        leftBar.color = currentColor.UI1;

        progressBarImg.color = currentColor.UI1;
        progressBarBg.color = currentColor.UI3;
        foreach (GameObject h in heartObjs)
            h.GetComponent<Image>().color = currentColor.UI1;

        rightBar.color = currentColor.UI1;
        
        blastMat.SetColor("_Color", currentColor.enemy2);
        flashMat.SetColor("_Color", currentColor.player2);


        fade.GetComponent<Image>().color = currentColor.BG;
        fade.GetComponent<CanvasGroup>().alpha = 1;
        LeanTween.alphaCanvas(fade.GetComponent<CanvasGroup>(),0,0.5f).setDelay(0.5f);
    }

    void Update() {
        if (SongManager.Instance.audioSource.isPlaying) progressBar.value = SongManager.Instance.audioSource.time / SongManager.Instance.audioSource.clip.length;
        points_t.text = GameManager.Instance.point.ToString();
        combo_t.text = "x" + GameManager.Instance.combo;
    }

    void OnGameStateChange(GameState state) {
        if (state == GameState.Play) {
            UpdateHearts();
        }
    }

    void PlayerHit() {
        UpdateHearts();
    }

    void UpdateHearts() {
        /*
        int curHp = hearts.Count;
        if (curHp == GameManager.Instance.hp)
            return;
        if (GameManager.Instance.State == GameState.Finish)
            return;
        //하트 추가
        if (curHp < GameManager.Instance.hp) {
            for (int i = 0; i < GameManager.Instance.hp - curHp; i++) {
                GameObject h = Instantiate(heartPrefab,Vector3.zero,Quaternion.identity);
                h.GetComponent<Image>().color = currentColor.UI1;
                h.transform.SetParent(heartParent.transform);
                hearts.Add(h);
            }
        //하트 삭제
        } else {
            for (int i = 0; i < curHp - GameManager.Instance.hp; i++) {
                if (hearts.Count >= 1) {
                    GameObject h = hearts[hearts.Count-1];
                    hearts.Remove(h);
                    Destroy(h);
                }
            }
        }*/
        for (int q = 0; q < heartObjs.Length; q++)
        {
            if (GameManager.Instance.hp > q)
                heartObjs[q].SetActive(true);
            else
                heartObjs[q].SetActive(false);
        }
    }
    
    private void NoteMiss() {
        combo_t.color = currentColor.UI1;
        GenerateFeedback(1);
    }
    
    private void NoteHit() {
        GenerateFeedback(0);
    }

    void GenerateFeedback(int index)
    {
        GameObject feedback = Instantiate(feedbackPrefab, feedbackPos.position, Quaternion.identity);
        feedback.transform.SetParent(feedbackPos);
        if (index == 0)
        {
            feedback.GetComponent<TextMeshProUGUI>().text = "Perfect";
            feedback.GetComponent<TextMeshProUGUI>().color = currentColor.UI2;
        }
        else
        {
            feedback.GetComponent<TextMeshProUGUI>().text = "Miss";
            feedback.GetComponent<TextMeshProUGUI>().color = Color.red;
        }

        LeanTween.alphaCanvas(feedback.GetComponent<CanvasGroup>(), 0, 0.5f);
        LeanTween.moveLocal(feedback, new Vector3(0, 30, 0), 0.5f).setEase(LeanTweenType.easeOutQuad);
        Destroy(feedback, 0.5f);
    }
    
    private void OnDestroy() {
        GameManager.OnGameStateChange -= OnGameStateChange;
        GameManager.OnPlayerHit -= PlayerHit;
        GameManager.OnNoteMiss -= NoteMiss;
        GameManager.OnNoteHit -= NoteHit;
    }

}
