using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타이틀 화면의 애니메이션과 버튼 처리를 담당한다
public class TitleScreen : ScreenManager
{
    public Animator transition;
    public float transitionTime;

    public override void Button(int n) {
        if (n == 0) {
            StartCoroutine(Transition1());
        } if (n == 1) {
            GameManager.Instance.ToSettings();
        } else if (n == 2) {
            GameManager.Instance.ToCredit();
        } else if (n == 3) {
            GameManager.Instance.Quit();
        }
    }
    
    IEnumerator Transition1() {
        transition.Play("transition1");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.ToCharacterSelect();
    }
}
