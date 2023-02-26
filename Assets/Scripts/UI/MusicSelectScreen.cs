using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {
    public string name;
    public List<MusicCard> musicCards;
}

public class MusicSelectScreen : ScreenManager2D
{
    public int characterNum;
    public List<Character> characters;
    [SerializeField] Transform[] cardPositions;
    [SerializeField] CanvasGroup fade;
    [SerializeField] GameObject colorBg;
    public Animator transition;
    public float transitionTime;
    [SerializeField] Color32[] difficultyColors;

    public override void Initialize() {
        readyToStart = false;
        characterNum = GameManager.Instance.selectedCharacter;
        for (int i = 0; i < characters.Count; i++) {
            if (i == characterNum) {
                int k = 0;
                foreach(MusicCard card in characters[i].musicCards) {
                    card.gameObject.SetActive(true);
                    string dif;
                    Color32 color;
                    dif = card.music.difficulty.ToString();
                    switch(card.music.difficulty) {
                        case Difficulty.Normal:
                            color = difficultyColors[0];
                            break;
                        case Difficulty.Hard:
                            color = difficultyColors[1];
                            break;
                        case Difficulty.Expert:
                            color = difficultyColors[2];
                            break;
                        case Difficulty.Master:
                            color = difficultyColors[3];
                            break;
                        default:
                            color = difficultyColors[0];
                            break;
                    }
                    card.SetColor(dif, color);
                    string t1 = DataManager.Instance.playerData.characterDatas[characterNum].musicDatas[k].highScore.ToString();
                    string t2 = DataManager.Instance.playerData.characterDatas[characterNum].musicDatas[k].maxCombo.ToString();
                    string t3 = (Mathf.RoundToInt(DataManager.Instance.playerData.characterDatas[characterNum].musicDatas[k].maxRP*100)/100).ToString();
                    string t4 = GameManager.Instance.GetRankFromNum(DataManager.Instance.playerData.characterDatas[characterNum].musicDatas[k].maxGrade);
                    card.SetText(t1, t2, t3, t4);
                    k++;
                }
            } else {
                foreach(MusicCard card in characters[i].musicCards)
                    card.gameObject.SetActive(false);
            }
        }
        CheckForChange(0);
        fade.alpha = 1;
        LeanTween.alphaCanvas(fade, 0, 0.5f).setDelay(0.3f);
    }

    public override void CheckForChange(float dir) {
        if (index_r != 0) return;
        for (int i = 0; i < characters[characterNum].musicCards.Count; i++) {
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
            characters[characterNum].musicCards[i].SetPos(cardPositions[pos_before + 2].position, cardPositions[pos_after + 2].position);
        }
        SFXPlayer.Instance.UISOund(1);
    }


    public override void Button(int r, int c) {
        if (readyToStart)
            return;
        if (r == 0) {
            transition.Play("showStartBtn");
            StartCoroutine(Wait());
        } else if (r == 1) {
            if  (c == 0) {
                GameManager.Instance.ToCharacterSelect();
            } else if (c == 1) {
                GameManager.Instance.ToSettings();
            }
        }
        SFXPlayer.Instance.UISOund(0);
    }

    IEnumerator Wait() {
        yield return new WaitForEndOfFrame();
        readyToStart = true;
    }


    public override void StartGame() {
        SFXPlayer.Instance.UISOund(3);
        StartCoroutine(Transition2());
        LeanTween.color(colorBg.GetComponent<RectTransform>(), characters[characterNum].musicCards[index_c].music.colorScheme.BG, transitionTime);
    }
    public override void Cancel() {
        SFXPlayer.Instance.UISOund(2);
        readyToStart = false;
        transition.Play("hideStartBtn");
    }

    IEnumerator Transition2() {
        transition.Play("pressStartBtn");
        yield return new WaitForSeconds(transitionTime+0.1f);
        GameManager.Instance.Play(characters[characterNum].musicCards[index_c].music);
    }
    
    public void Row0(int c) {
        Button(0, c);
    }
    public void Row1(int c) {
        Button(1, c);
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

    public void StartTest() {
        if (readyToStart)
            StartGame();
    }
}
