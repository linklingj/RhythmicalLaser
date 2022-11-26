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

    private void Start() {
        //to fix
        point = 0;
        combo = 0;
        hp = maxHP;
        UpdateGameState(GameState.Play);
    }

    public static event Action<GameState> OnGameStateChange;
    public static event Action OnPlayerHit;
    public static event Action OnGameOver;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
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
}
