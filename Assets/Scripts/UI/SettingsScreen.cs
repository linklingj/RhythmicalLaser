using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//타이틀 화면의 애니메이션과 버튼 처리를 담당한다
public class SettingsScreen : ScreenManager2D
{
    public Animator transition;
    public float transitionTime;
    Save_Settings currentSettings;
    public int minNoteSpeed;
    
    [SerializeField] TextMeshProUGUI inputDelayText, visualDelayText, noteSpeedText, volumeText;

    public override void Initialize() {
        readyToStart = false;
        currentSettings = DataManager.Instance.playerData.playerSettings;
        UpdateText();
    }
    
    public override void Button(int r, int c) {
        if (r == 0) {
            //input delay
            if (c == 0) currentSettings.inputDelay -= 10;
            else if (c == 1) currentSettings.inputDelay -= 1;
            else if (c == 2) currentSettings.inputDelay += 1;
            else if (c == 3) currentSettings.inputDelay += 10;
        } if (r == 1) {
            //visual delay
            if (c == 0) currentSettings.visualDelay -= 5;
            else if (c == 1) currentSettings.visualDelay -= 1;
            else if (c == 2) currentSettings.visualDelay += 1;
            else if (c == 3) currentSettings.visualDelay += 5;
        } if (r == 2) {
            //note speed
            if (c == 0) currentSettings.noteSpeed -= 10;
            else if (c == 1) currentSettings.noteSpeed -= 1;
            else if (c == 2) currentSettings.noteSpeed += 1;
            else if (c == 3) currentSettings.noteSpeed += 10;
            if (currentSettings.noteSpeed < minNoteSpeed) currentSettings.noteSpeed = minNoteSpeed;
        } if (r == 3) {
            //volume
            if (c == 0) currentSettings.volume = 0f;
            else if (c == 1) currentSettings.volume -= 0.1f;
            else if (c == 2) currentSettings.volume += 0.1f;
            else if (c == 3) currentSettings.volume = 1f;
            currentSettings.volume = Mathf.Clamp(currentSettings.volume, 0f, 1f);
        }if (r == 4) {
            //reset
            DataManager.Instance.ResetSettings();
            StartCoroutine(nameof(Transition3));
        }if (r == 5) {
            //back
            DataManager.Instance.UpdateSettingsData(currentSettings);
            StartCoroutine(nameof(Transition3));
        }
        UpdateText();
    }

    private void UpdateText() {
        inputDelayText.text = currentSettings.inputDelay.ToString();
        visualDelayText.text = currentSettings.visualDelay.ToString();
        noteSpeedText.text = currentSettings.noteSpeed.ToString();
        volumeText.text = (currentSettings.volume * 100).ToString("F0") + "%";
    }

    public override void CheckForChange(float horizontal) {
        if (index_r == 4 || index_r == 5) return;
    }

    public override void StartGame() {
        throw new System.NotImplementedException();
    }

    public override void Cancel() {
        throw new System.NotImplementedException();
    }


    
    IEnumerator Transition3() {
        transition.Play("transition3");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.ToCharacterSelect();
    }
}
