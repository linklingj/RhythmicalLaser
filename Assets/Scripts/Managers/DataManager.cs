using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct Save_MusicData {
    public int musicNum;
    public string name;
    public bool clear;
    public int highScore;
    public int maxCombo;
    public Save_MusicData(int musicNum, string name, bool clear, int highScore, int maxCombo) {
        this.musicNum = musicNum;
        this.name = name;
        this.clear = clear;
        this.highScore = highScore;
        this.maxCombo = maxCombo;
    }
}

[System.Serializable]
public struct Save_CharacterData {
    public string name;
    public int lvl;
    public Save_MusicData[] musicDatas;
    public Save_CharacterData(string name, int lvl, Save_MusicData[] musicData) {
        this.name = name;
        this.lvl = lvl;
        this.musicDatas = musicData;
    }
}

[System.Serializable]
public class PlayerData {
    public string name;
    public int lvl;
    public Save_CharacterData[] characterDatas;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData playerData = new PlayerData();
    string path, fileName;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        path = Application.persistentDataPath + "/";
        fileName = "playerData.json";
    }

    void Start() {
        if (File.Exists(path + fileName)) {
            LoadData();
        } else {
            ResetData();
        }

    }

    public void SaveData() {
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData() {
        string data = File.ReadAllText(path + fileName);
        playerData = JsonUtility.FromJson<PlayerData>(data);
    }

    public void ResetData() {
        PlayerData defaultData = new PlayerData();

        defaultData.name = "DefaultPlayer";
        defaultData.lvl = 1;

        Save_MusicData[] harangMusics = new Save_MusicData[] {
            new Save_MusicData(1, "Attention", false, 0, 0),
            new Save_MusicData(2, "BOCA", false, 0, 0),
            new Save_MusicData(3, "LOONATIC", false, 0, 0)
        };

        Save_MusicData[] himeMusics = new Save_MusicData[] {
            new Save_MusicData(1, "Sugar Song To Bitter Step", false, 0, 0)
        };

        Save_MusicData[] jimmyMusics = new Save_MusicData[] {
            new Save_MusicData(1, "Blinding Lights", false, 0, 0)
        };

        Save_MusicData[] ismayaMusics = new Save_MusicData[] {
            new Save_MusicData(1, "Green Greens", false, 0, 0)
        };

        defaultData.characterDatas = new Save_CharacterData[] {
            new Save_CharacterData("Harang",1,harangMusics),
            new Save_CharacterData("Hime",1,himeMusics),
            new Save_CharacterData("Jimmy",1,jimmyMusics),
            new Save_CharacterData("Ismaya",1,ismayaMusics)
        };

        playerData = defaultData;

        SaveData();
    }

}

