using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FailScreen : ScreenManager
{
    public Animator transition;
    public RectTransform transitionObj;
    public float transitionTime;

    [SerializeField] private CanvasGroup fade;
    [SerializeField] RectTransform[] deathLetters;
    [SerializeField] CanvasGroup[] deathLettersCanvasGroup;

    void Start() {
        LeanTween.alphaCanvas(fade, 0, 0.5f);
        Lettering();
    }
    
    void Lettering() {
        for (int i = 0; i < deathLetters.Length; i++) {
            LeanTween.alphaCanvas(deathLettersCanvasGroup[i], 1, 0.3f).setDelay(i*0.1f);
            LeanTween.moveY(deathLetters[i], 190, 0.5f).setDelay(i*0.1f);
        }
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
        LeanTween.color(transitionObj, GameManager.Instance.selectedMusic.colorScheme.BG, 0.3f).setDelay(0.5f);
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.Play(GameManager.Instance.selectedMusic);
    }
}
