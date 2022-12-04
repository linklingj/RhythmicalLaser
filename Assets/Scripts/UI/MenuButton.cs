using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메뉴의 버튼에 붙는 스크립트
//선택 및 해제되었을 때 애니메이션을 담당한다
//모든 버튼은 애니메이터와 bool "selected", "pressed"를 요구한다
public class MenuButton : MonoBehaviour
{
    [SerializeField] ScreenManager screenManager;
    [SerializeField] Animator animator;
    //해당 버튼의 인덱스
    [SerializeField] int thisIndex;
    int currentIndex;

    private void Update() {
        currentIndex = screenManager.index;
        if (currentIndex == thisIndex) {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1) {
                animator.SetBool("pressed", true);
                screenManager.Button(thisIndex);
            } else if (animator.GetBool("pressed")) {
                animator.SetBool("pressed",false);
            }
        } else {
            animator.SetBool("selected",false);
        }
    }
    
}
