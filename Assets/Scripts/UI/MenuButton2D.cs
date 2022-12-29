using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuButton2D : MonoBehaviour
{
    public ScreenManager2D screenManager;
    [SerializeField] Animator animator;
    //해당 버튼의 인덱스
    public int thisIndex_r;
    public int thisIndex_c;
    int currentIndex_r;
    int currentIndex_c;
    private bool pressed;


    private void Update() {
        currentIndex_r = screenManager.index_r;
        currentIndex_c = screenManager.index_c;
        if (currentIndex_r != thisIndex_r) {
            animator.SetBool("selected", false);
            return;
        }
        if (currentIndex_c == thisIndex_c) {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1 && !pressed) {
                animator.SetBool("pressed", true);
                screenManager.Button(thisIndex_r, thisIndex_c);
                pressed = true;
            } else if (Input.GetAxis("Submit") != 1) {
                pressed = false;
                animator.SetBool("pressed", false);
            }
        } else {
            animator.SetBool("selected",false);
        }
    }
    
}
