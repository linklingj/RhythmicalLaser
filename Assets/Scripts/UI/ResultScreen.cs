using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResultScreen : ScreenManager
{
    public Animator transition;
    public float transitionTime;
    
    

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
