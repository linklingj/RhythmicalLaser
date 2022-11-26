using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메뉴에서 버튼을 선택하는 스크립트
//모든 메뉴 메니저들이 상속한다
//TitleScreen, CreditScreen 에서 상속한다
public class MenuElements : MonoBehaviour
{
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public int index;
    public AudioSource audioSource;
    
    void Update() {
        if (Input.GetAxis("Vertical") != 0) {
            if (!keyDown) {
                if (Input.GetAxis("Vertical") < 0) {
                    if (index < maxIndex) {
                        index++;
                    } else {
                        index = 0;
                    }
                } else if (Input.GetAxis("Vertical") > 0) {
                    if (index > 0) {
                        index--;
                    } else {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        } else {
            keyDown = false;
        }
    }

}
