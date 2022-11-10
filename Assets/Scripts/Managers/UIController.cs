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
    [SerializeField] Slider progressBar;
    [SerializeField] TextMeshProUGUI points_t,combo_t,title_t,artist_t,difficulty_t;
    [SerializeField] Image leftBar,rightBar,progressBarImg;
    [SerializeField] SpriteRenderer bgR;
    [SerializeField] Image[] bgs;
    [Header("Color Elements")]
    public List <ColorScheme> colorSchemes;
    public ColorScheme currentColor;
    [SerializeField] SpriteRenderer playerR, playerDirR, bounderyR, laserR;
    void Start() {
        title_t.text = musicPlayer.currentMusic.title;
        artist_t.text = musicPlayer.currentMusic.artist;

        bgR.color = currentColor.BG;
        foreach (Image item in bgs)
            item.color = currentColor.BG;

        playerR.color = currentColor.player1;
        playerDirR.color = currentColor.player1;
        bounderyR.color = currentColor.player3;
        laserR.color = currentColor.player2;

        points_t.color = currentColor.UI1;
        combo_t.color = currentColor.UI1;
        title_t.color = currentColor.UI1;
        artist_t.color = currentColor.UI1;
        difficulty_t.color = currentColor.UI1;
        leftBar.color = currentColor.UI1;

        progressBarImg.color = currentColor.UI1;

        rightBar.color = currentColor.UI1;
    }

    // Update is called once per frame
    void Update() {
        progressBar.value = songManager.audioSource.time / songManager.audioSource.clip.length;
        points_t.text = GameManager.Instance.point.ToString();
        combo_t.text = "x" + GameManager.Instance.combo.ToString();
    }
}
