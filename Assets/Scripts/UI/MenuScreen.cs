using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenManager2D
{
    [SerializeField] Music[] musics;


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
