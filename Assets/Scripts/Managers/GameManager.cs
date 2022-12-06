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
    Result
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public int point;
    public int combo;
    public int hp;
    public int maxHP;

    [Header("Balance")]
    public int enemyPoint;

    public static event Action<GameState> OnGameStateChange;
    public static event Action OnPlayerHit;
    public static event Action OnGameOver;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    
    public void UpdateGameState(GameState newState) {
        State = newState;
        switch(newState) {
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
            case GameState.Result:
                break;
            default:
                break;
        }
        OnGameStateChange?.Invoke(newState);
    }

    public void playerHit() {
        hp -= 1;
        combo = 0;
        OnPlayerHit?.Invoke();
        if (hp <= 0)
            OnGameOver?.Invoke();
    }

    public void Play(Music music) {
        SceneManager.LoadScene("GamePlay");
        point = 0;
        combo = 0;
        hp = maxHP;
        UpdateGameState(GameState.Play);
    }
    public void ToTitle() {
        SceneManager.LoadScene("Title");
        UpdateGameState(GameState.Title);
    }

    public void ToCharacterSelect() {
        SceneManager.LoadScene("CharacterSelect");
        UpdateGameState(GameState.CharacterSelect);
    }

    public void ToMusicSelect() {
        SceneManager.LoadScene("MusicSelect");
        UpdateGameState(GameState.MusicSelect);
    }

    public void ToSettings() {
        SceneManager.LoadScene("Settings");
        UpdateGameState(GameState.Settings);
    }

    public void ToCredit() {
        SceneManager.LoadScene("Credit");
        UpdateGameState(GameState.Credit);
    }
    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
