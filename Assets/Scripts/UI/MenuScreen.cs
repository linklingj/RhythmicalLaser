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
        LeanTween.alphaCanvas(fade, 0, 0.5f).setDelay(0.3f);
    }

    public override void CheckForChange(float dir) {
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
            //Debug.Log("card num: "+ i +"pos_after: "+ pos_after);
            characterCards[i].SetPos(cardPositions[pos_before + 2].position, cardPositions[pos_after + 2].position);
        }
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
    }

    IEnumerator Transition1() {
        transition.Play("transition1");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.ToMusicSelect(index_c);
    }

    public override void StartGame() {
        Debug.Log("Error");
    }

    public override void Cancel() {
        Debug.Log("Error");
    }

}
