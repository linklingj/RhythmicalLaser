using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenManager2D : MonoBehaviour
{
    [SerializeField] bool keyDown_r;
    [SerializeField] bool keyDown_c;
    public int maxIndex_r;
    public int[] maxIndex_c;
    public int index_r;
    public int index_c;
    public int save_index;
    public AudioSource audioSource;

    private void Start() {
        index_r = 0;
        index_c = 0;
    }
    void Update() {
        if (Input.GetAxis("Vertical") != 0) {
            if (!keyDown_r) {
                if (Input.GetAxis("Vertical") < 0) {
                    if (index_r < maxIndex_r) {
                        index_r++;
                    } else {
                        index_r = 0;
                    }
                } else if (Input.GetAxis("Vertical") > 0) {
                    if (index_r > 0) {
                        index_r--;
                    } else {
                        index_r = maxIndex_r;
                    }
                }
                keyDown_r = true;
                if (index_r == 0) {
                    index_c = save_index;
                } else {
                    save_index = index_c;
                }
            }
        } else {
            keyDown_r = false;
        }
        if (Input.GetAxis("Horizontal") != 0) {
            if (!keyDown_c) {
                if (Input.GetAxis("Horizontal") > 0) {
                    if (index_c < maxIndex_c[index_r]) {
                        index_c++;
                    } else {
                        index_c = 0;
                    }
                } else if (Input.GetAxis("Horizontal") < 0) {
                    if (index_c > 0) {
                        index_c--;
                    } else {
                        index_c = maxIndex_c[index_r];
                    }
                }
                keyDown_c = true;
            }
        } else {
            keyDown_c = false;
        }
        CheckForChange();
    }
    public abstract void CheckForChange();
    public abstract void Button(int r, int c);
}
