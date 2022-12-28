using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타이틀 화면의 애니메이션과 버튼 처리를 담당한다
public class SettingsScreen : ScreenManager
{
    public Animator transition;
    public float transitionTime;

    public override void Button(int n) {
        if (n == 0) {
            //input delay
        } if (n == 1) {
            //visual delay
        } if (n == 2) {
            //volume
        } if (n == 3) {
            //Reset
        }if (n == 4) {
            StartCoroutine(nameof(Transition3));
        }
    }
    
    IEnumerator Transition3() {
        transition.Play("transition3");
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.ToTitle();
    }
}
