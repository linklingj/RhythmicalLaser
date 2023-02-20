using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
//5. 그 외의 전역 함수들을 보관한다.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    [Header("In Game")]
    public int point;
    
    public int combo;
    public int maxCombo;
    public int totalPerfectNote;
    public int totalGoodNote;
    public int comboPoint;
    
    public float rhythmPoint;
    
    public int hp;
    public int maxHP;
    
    public bool fullCombo;
    public bool noHit;
    
    public int rank;
    
    [Header("Music Info")]
    public int totalNoteCount;
    public int totalEnemyCount;

    [Header("Balancing")] 
    public EnemyData[] enemyData;
    [Range(0,1)]
    public float cutC, cutCp, cutB, cutBp, cutA, cutAp, cutS, cutSS, cutSSS;

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
                comboPoint = 0;  
                totalPerfectNote = 0;
                totalGoodNote = 0;
                rhythmPoint = 0;
                rank = -1;
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
        comboPoint += SumToN(combo-1);
        combo = 0;
        OnNoteMiss?.Invoke();
    }
    
    public void NoteHit() {
        combo += 1;
        totalPerfectNote += 1;
        if (combo > maxCombo) maxCombo = combo;
        OnNoteHit?.Invoke();
        CaculateRhythmPoint();
    }
    
    public void MusicFinished() {
        comboPoint += SumToN(combo-1);
        rhythmPoint = CaculateRhythmPoint();
        rank = CaculateRank();
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
        SFXPlayer.Instance.PlayClip(SFXPlayer.Instance.clearSound);
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
    
    //함수
    
    //cytus2의 점수 체계를 따른다
    //https://namu.wiki/w/Cytus%20II#s-2.3
    public float CaculateRhythmPoint() {
        float p1 = (totalPerfectNote * 1.0f + totalGoodNote * 0.7f) / totalNoteCount;
        float p2 = (float)comboPoint/SumToN(totalNoteCount-1);
        Debug.Log(p1);
        Debug.Log(p2);
        Debug.Log(p1 * 0.9f + p2 * 0.1f);
        return p1 * 0.9f + p2 * 0.1f;
    }
    int SumToN(int n) {
        return n * (n + 1) / 2;
    }
    public int CaculateRank() {
        if (rhythmPoint < cutC) return -1;
        if (rhythmPoint < cutCp) return 0;
        if (rhythmPoint < cutB) return 1;
        if (rhythmPoint < cutBp) return 2;
        if (rhythmPoint < cutA) return 3;
        if (rhythmPoint < cutAp) return 4;
        if (rhythmPoint < cutS) return 5;
        if (rhythmPoint < cutSS) return 6;
        if (rhythmPoint < cutSSS) return 7;
        return 8;
    }
    //unranked -10, F -1, C 0, C+ 1, B 2, B+ 3, A 4, A+ 5, S 6, SS 7, SSS 8
    public string GetRankFromNum(int rankNum) {
        if (rankNum < -10)
            rankNum = rank;
        switch (rankNum) {
            case -1:
                return "F";
            case 0:
                return "C";
            case 1:
                return "C+";
            case 2:
                return "B";
            case 3:
                return "B+";
            case 4:
                return "A";
            case 5:
                return "A+";
            case 6:
                return "S";
            case 7:
                return "SS";
            case 8:
                return "SSS";
            default:
                return "Unranked";
        }
    }
    
    
}
