using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타이틀 화면의 애니메이션과 버튼 처리를 담당한다
public class TitleScreen : MenuElements
{
    [SerializeField] GameObject bg;
    

    void Start() {
        audioSource = GetComponent<AudioSource>();
        ShowBG();
    }

    

    
    void ShowBG() {
        bg.GetComponent<CanvasGroup>().alpha = 0;

        LeanTween.alphaCanvas(bg.GetComponent<CanvasGroup>(), 1, 0.3f).setDelay(3f);
    }

    public void Button(int n) {
        /*
        if (n == 0) {
            GameManager.Instance.GameStart();
        } if (n == 1) {
            GameManager.Instance.Credit();
        } else if (n == 2) {
            GameManager.Instance.Quit();
        }*/
    }
    
}
