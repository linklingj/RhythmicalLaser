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
                foreach(MusicCard card in characters[i].musicCards) {
                    card.gameObject.SetActive(true);
                    string dif;
                    Color32 color;
                    switch(card.music.difficulty) {
                        case Difficulty.Normal:
                            dif = "Normal";
                            color = difficultyColors[0];
                            break;
                        case Difficulty.Hard:
                            dif = "Hard";
                            color = difficultyColors[1];
                            break;
                        case Difficulty.Expert:
                            dif = "Expert";
                            color = difficultyColors[2];
                            break;
                        case Difficulty.Master:
                            dif = "Master";
                            color = difficultyColors[3];
                            break;
                        default:
                            dif = "Normal";
                            color = difficultyColors[0];
                            break;
                    }
                    card.SetColor(dif, color);
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
    }


    public override void Button(int r, int c) {
        if (readyToStart)
            return;
        if (r == 0) {
            readyToStart = true;
            transition.Play("showStartBtn");
        } else if (r == 1) {
            if  (c == 0) {
                GameManager.Instance.ToCharacterSelect();
            } else if (c == 1) {
                GameManager.Instance.ToSettings();
            }
        }
    }

    public override void StartGame() {
        StartCoroutine(Transition2());
        LeanTween.color(colorBg.GetComponent<RectTransform>(), characters[characterNum].musicCards[index_c].music.colorScheme.BG, transitionTime);
    }
    public override void Cancel() {
        readyToStart = false;
        transition.Play("hideStartBtn");
    }

    IEnumerator Transition2() {
        transition.Play("pressStartBtn");
        yield return new WaitForSeconds(transitionTime+0.1f);
        GameManager.Instance.Play(characters[characterNum].musicCards[index_c].music);
    }
}
