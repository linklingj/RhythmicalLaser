using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Managers")]
    public MusicPlayer musicPlayer;
    public SongManager songManager;

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
    [SerializeField] SpriteRenderer playerR, playerDirR, bounderyR, laserR;
     public ColorScheme currentColor;
    
    List<GameObject> hearts = new List<GameObject>();

    private void Awake() {
        GameManager.OnGameStateChange += OnGameStateChange;
        GameManager.OnPlayerHit += PlayerHit;
    }

    void Start() {
        currentColor = GameManager.Instance.selectedMusic.colorScheme;

        title_t.text = musicPlayer.currentMusic.title;
        artist_t.text = musicPlayer.currentMusic.artist;

        bgR.color = currentColor.BG;
        foreach (Image item in bgs)
            item.color = currentColor.BG;

        playerR.color = currentColor.player1;
        playerDirR.color = currentColor.player2;
        bounderyR.color = currentColor.player3;
        laserR.color = currentColor.laser1;

        points_t.color = currentColor.UI1;
        combo_t.color = currentColor.UI1;
        title_t.color = currentColor.UI1;
        artist_t.color = currentColor.UI1;
        difficulty_t.color = currentColor.UI1;
        leftBar.color = currentColor.UI1;

        progressBarImg.color = currentColor.UI1;
        progressBarBg.color = currentColor.BG;

        rightBar.color = currentColor.UI1;

        fade.GetComponent<Image>().color = currentColor.BG;
        fade.GetComponent<CanvasGroup>().alpha = 1;
        LeanTween.alphaCanvas(fade.GetComponent<CanvasGroup>(),0,0.5f).setDelay(0.5f);
    }

    void Update() {
        progressBar.value = songManager.audioSource.time / songManager.audioSource.clip.length;
        points_t.text = GameManager.Instance.point.ToString();
        combo_t.text = "x" + GameManager.Instance.combo.ToString();
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
        int curHp = hearts.Count;
        if (curHp == GameManager.Instance.hp)
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
                GameObject h = hearts[hearts.Count-1];
                hearts.Remove(h);
                Destroy(h);
            }
        }
    }

}
