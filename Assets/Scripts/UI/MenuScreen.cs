using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenManager
{
    [SerializeField] Music[] musics;


    public override void Button(int n) {
        if (n == 0) {
            GameManager.Instance.Play(musics[0]);
        } if (n == 1) {
            GameManager.Instance.ToTitle();
        }
    }
}
