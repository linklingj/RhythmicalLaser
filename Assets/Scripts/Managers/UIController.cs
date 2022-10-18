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
    public Slider progressBar;
    public TextMeshProUGUI points_t,combo_t,title_t,artist_t,difficulty_t;
    void Start() {
        title_t.text = musicPlayer.currentMusic.title;
        artist_t.text = musicPlayer.currentMusic.artist;
    }

    // Update is called once per frame
    void Update() {
        progressBar.value = songManager.audioSource.time / songManager.audioSource.clip.length;
        points_t.text = GameManager.Instance.point.ToString();
        combo_t.text = "x" + GameManager.Instance.combo.ToString();
    }
}
