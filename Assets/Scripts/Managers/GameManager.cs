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
    Finish,
    Fail,
    Clear
}

//GameManager의 역할
//1. 게임의 state를 관리한다.
//2. 게임의 중요 데이터를 관리한다.
//3. event를 호출하는 inspector를 제공한다. (관찰자 패턴)
//3-1. state변환에 따른 event 호출
//3-2. 플레이어 상태 변화에 따른 event 호출
//4. scene을 전환한다.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    [Header("In Game")]
    public int point;
    public int combo;
    public int maxCombo;
    public int totalHitNote;
    public int totalNoteCount;
    public float rhythmPoint;
    public int hp;
    public int maxHP;
    public bool fullCombo;
    public bool noHit;

    [Header("Balance")]
    public int enemyPoint;
    [Header("UI")]
    public int selectedCharacter = 0;
    public Music selectedMusic;

    public static event Action<GameState> OnGameStateChange;
    public static event Action OnPlayerHit;
    public static event Action OnNoteMiss;
    public static event Action OnNoteHit; 
    public static event Action OnGameOver;
    public static event Action OnClear;

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
                maxCombo = 0;
                totalHitNote = 0;
                rhythmPoint = 0;
                hp = maxHP;
                fullCombo = true;
                noHit = true;
                break;
            case GameState.Finish:
                break;
            case GameState.Fail:
                break;
            case GameState.Clear:
                break;
            default:
                break;
        }
        OnGameStateChange?.Invoke(State);
    }

    //inspector역할
    public void playerHit() {
        if (State != GameState.Play) return;
        hp -= 1;
        combo = 0;
        noHit = false;
        OnPlayerHit?.Invoke();
        if (hp <= 0) {
            OnGameOver?.Invoke();
            State = GameState.Finish;
            UpdateGameState();
            MusicPlayer.Instance.GameOver();
        }
    }

    public void NoteMiss() {
        fullCombo = false;
        combo = 0;
        OnNoteMiss?.Invoke();
    }
    
    public void NoteHit() {
        combo += 1;
        totalHitNote += 1;
        rhythmPoint = totalHitNote * 100f / totalNoteCount;
        if (combo > maxCombo) maxCombo = combo;
        OnNoteHit?.Invoke();
    }
    
    public void MusicFinished() {
        OnClear?.Invoke();
        State = GameState.Finish;
        UpdateGameState();
    }

    //scene전환
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
    
    public void ToFail() {
        SceneManager.LoadScene("Fail");
        State = GameState.Fail;
    }
    
    public void ToClear() {
        SceneManager.LoadScene("Clear");
        State = GameState.Clear;
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
