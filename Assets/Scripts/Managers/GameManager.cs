using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Title,
    Settings,
    Credit,
    CharacterSelect,
    MusicSelect,
    Play,
    Fail,
    Clear
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    [Header("In Game")]
    public int point;
    public int combo;
    public int hp;
    public int maxHP;

    [Header("Balance")]
    public int enemyPoint;
    [Header("UI")]
    public int selectedCharacter = 0;
    public Music selectedMusic;

    public static event Action<GameState> OnGameStateChange;
    public static event Action OnPlayerHit;
    public static event Action OnGameOver;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start() { 
        //debug
        if (SceneManager.GetActiveScene().name == "GamePlay") {
            State = GameState.Play;
            UpdateGameState();
        }
    }


    
    public void UpdateGameState() {
        switch(State) {
            case GameState.Title:
                break;
            case GameState.Settings:
                break;
            case GameState.Credit:
                break;
            case GameState.CharacterSelect:
                break;
            case GameState.MusicSelect:
                break;
            case GameState.Play:
                point = 0;
                combo = 0;
                hp = maxHP;
                break;
            case GameState.Fail:
                break;
                
            default:
                break;
        }
        OnGameStateChange?.Invoke(State);
    }

    public void playerHit() {
        hp -= 1;
        combo = 0;
        OnPlayerHit?.Invoke();
        if (hp <= 0)
            OnGameOver?.Invoke();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        UpdateGameState();
    }

    public void Play(Music music) {
        selectedMusic = music;
        point = 0;
        combo = 0;
        hp = maxHP;
        SceneManager.LoadScene("GamePlay");
        State = GameState.Play;
    }
    public void ToTitle() {
        SceneManager.LoadScene("Title");
        State = GameState.Title;
    }

    public void ToCharacterSelect() {
        SceneManager.LoadScene("CharacterSelect");
        State = GameState.CharacterSelect;
    }

    public void ToMusicSelect(int characterIndex) {
        selectedCharacter = characterIndex;
        SceneManager.LoadScene("MusicSelect");
        State = GameState.MusicSelect;
    }

    public void ToSettings() {
        SceneManager.LoadScene("Settings");
        State = GameState.Settings;
    }

    public void ToCredit() {
        SceneManager.LoadScene("Credit");
        State = GameState.Credit;
    }
    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
