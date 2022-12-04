using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//크래딧 화면의 애니메이션과 버튼 처리를 담당한다
public class CreditScreen : ScreenManager
{
    [SerializeField] GameObject[] buttons;
    
    const string CJHLink = "https://github.com/linklingj";

    void Start() {
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
    
    public override void Button(int n) {
        
    }
}
