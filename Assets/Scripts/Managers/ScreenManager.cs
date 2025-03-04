using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenManager : MonoBehaviour
{
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public int index;

    private void Start() {
        index = 0;
    }
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
    public abstract void Button(int n);
}
