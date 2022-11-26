using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//크래딧 화면의 애니메이션과 버튼 처리를 담당한다
public class CreditScreen : MenuElements
{
    [SerializeField] GameObject[] buttons;
    
    const string CJHLink = "https://github.com/linklingj";

    void Start() {
        audioSource = GetComponent<AudioSource>();
        ShowButtons();
    }

    void ShowButtons() {
        float delay = 0f;
        foreach (GameObject item in buttons) {
            item.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 1f);
            item.GetComponent<CanvasGroup>().alpha = 0;

            LeanTween.alphaCanvas(item.GetComponent<CanvasGroup>(), 1, 0.3f).setDelay(delay);
            LeanTween.scale(item, new Vector3(1f,1f,1f),0.3f).setEase(LeanTweenType.easeOutBack).setDelay(delay);
            delay += 0.2f;
        }
    }
    
    public void Button(int n) {
        /*
        if (n == 0) {
            //다울
            Application.OpenURL(KDULink);
        } if (n == 1) {
            //건민
            Application.OpenURL(KGMLink);
        } else if (n == 2) {
            //소영
            Application.OpenURL(KSYLink);
        } else if (n == 3) {
            //재현
            Application.OpenURL(CJHLink);
        } else if (n == 4) {
            GameManager.Instance.ToTitile();
        }*/
    }
}
