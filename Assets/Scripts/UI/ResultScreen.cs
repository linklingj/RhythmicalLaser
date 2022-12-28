using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResultScreen : ScreenManager
{
    public Animator transition;
    public float transitionTime;

    private void Start() {
        SaveData();
    }

    private void SaveData() {
        Save_MusicData newData = DataManager.Instance.playerData.characterDatas[GameManager.Instance.selectedCharacter].musicDatas[GameManager.Instance.selectedMusic.index - 1];
        newData.clear = true;
        newData.highScore = Math.Max(newData.highScore, GameManager.Instance.point);
        newData.maxCombo = Math.Max(newData.maxCombo, GameManager.Instance.combo);
        DataManager.Instance.SaveData();
    }


    public override void Button(int n) {
        if (n == 0) {
            StartCoroutine(Transition4());
        } if (n == 1) {
            GameManager.Instance.ToMusicSelect(GameManager.Instance.selectedCharacter);
        }
    }
    
    IEnumerator Transition4() {
        transition.Play("transition4");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.Play(GameManager.Instance.selectedMusic);
    }
}
