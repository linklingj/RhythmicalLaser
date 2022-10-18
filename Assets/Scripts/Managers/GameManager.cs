using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Title,
    Menu,
    Play,
    Result
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public int point;
    public int combo;
    [Header("Balance")]
    public int enemyPoint;

    private void Start() {
        point = 0;
        combo = 0;
    }


    public static event Action<GameState> OnGameStateChange;
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
            case GameState.Menu:
                break;
            case GameState.Play:
                break;
            case GameState.Result:
                break;
            default:
                break;
        }
        OnGameStateChange?.Invoke(newState);
    }
}
