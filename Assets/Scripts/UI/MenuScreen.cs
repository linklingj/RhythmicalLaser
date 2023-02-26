using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenManager2D
{
    [SerializeField] CharacterCard[] characterCards;
    [SerializeField] Transform[] cardPositions;
    [SerializeField] CanvasGroup fade;
    public Animator transition;
    public float transitionTime;

    public override void Initialize() {
        readyToStart = false;
        CheckForChange(0);
        fade.alpha = 1;
        LeanTween.alphaCanvas(fade, 0, 0.8f).setDelay(0.1f);
    }

    public override void CheckForChange(float dir) {
        if (index_r != 0) return;
        for (int i = 0; i < characterCards.Length; i++) {
            int pos = i - index_c;
            if (pos == maxIndex_c[0]) {
                pos = -1;
            } else if (pos == -maxIndex_c[0]) {
                pos = 1;
            }

            int d = 0;
            if (dir > 0.2f)
                d = -1;
            else if (dir < -0.2f)
                d = 1;
            else
                d = 0;

            int pos_before = Mathf.Clamp(pos - d, -2, 2);
            int pos_after = Mathf.Clamp(pos, -2, 2);
            characterCards[i].SetPos(cardPositions[pos_before + 2].position, cardPositions[pos_after + 2].position);

            Save_CharacterData data = DataManager.Instance.playerData.characterDatas[i];
            int clearCnt = 0;
            foreach(Save_MusicData musicData in data.musicDatas) {
                if (musicData.clear)
                    clearCnt++;
            }
            characterCards[i].SetText(data.lvl.ToString(), clearCnt.ToString(), data.musicDatas.Length.ToString());
        }
        SFXPlayer.Instance.UISOund(1);
    }


    public override void Button(int r, int c) {
        if (r == 0) {
            StartCoroutine(Transition1());
        } else if (r == 1) {
            if  (c == 0) {
                GameManager.Instance.ToTitle();
            } else if (c == 1) {
                GameManager.Instance.ToSettings();
            }
        }
        SFXPlayer.Instance.UISOund(0);
    }

    IEnumerator Transition1() {
        transition.Play("transition3");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.ToMusicSelect(index_c);
    }

    public override void StartGame() {
        Debug.Log("Error");
    }

    public override void Cancel() {
        Debug.Log("Error");
    }
    
    //버튼의 onClick가 두개의 인자를 지원하지 않기 때문에 만든 함수
    public void Row0(int c) {
        Button(0, c);
    }
    public void Row1(int c) {
        Button(1, c);
        Debug.Log("VAR");
    }
    public void Swipe(int d) {
        if (d > 0) {
            if (index_c < maxIndex_c[index_r]) {
                index_c++;
            } else {
                index_c = 0;
            }
        }
        else {
            if (index_c > 0) {
                index_c--;
            } else {
                index_c = maxIndex_c[index_r];
            }
        }
        CheckForChange(d);
    }
}
