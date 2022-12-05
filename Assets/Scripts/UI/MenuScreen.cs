using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenManager2D
{
    [SerializeField] Music[] musics;
    [SerializeField] CharacterCard[] characterCards;
    int prevIndex_r, prevIndex_c;
    /*
    private void Update() {
        //row 변화 있을 때
        if (prevIndex_r != index_r) {
            if (index_r == 0) {
                HighlightCharacterRow();
            } else if (index_r == 1) {
                UnHighlightCharacterRow();
            }
            prevIndex_r = index_r;
        //col 변화 있을 때
        } else if (prevIndex_c != index_c) {
            if (index_r == 0) {
                if (index_c > prevIndex_c) {
                    //오른쪽 카드

                } else if (index_c < prevIndex_c) {
                    //왼쪽 카드

                }
            }
            prevIndex_c = index_c;
        }
    }*/

    public override void CheckForChange() {
        for (int i = 0; i < characterCards.Length; i++) {
            int pos = i - index_c;
            if (pos == maxIndex_c[0]) {
                pos = -1;
            } else if (pos == -maxIndex_c[0]) {
                pos = 1;
            }
            characterCards[i].SetPos(pos);
        }
    }


    public override void Button(int r, int c) {
        if (r == 0) {
            GameManager.Instance.Play(musics[c]);
        } else if (r == 1) {
            if  (c == 0) {
                GameManager.Instance.ToTitle();
            } else if (c == 1) {
                GameManager.Instance.ToSettings();
            }
        }
    }
}
