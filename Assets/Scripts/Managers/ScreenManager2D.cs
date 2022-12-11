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
    public bool readyToStart;
    public AudioSource audioSource;

    bool changed;

    private void Start() {
        index_r = 0;
        index_c = 0;
        Initialize();
    }
    void Update() {
        if (readyToStart) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                StartGame();
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cancel();
            }
            return;
        }

        changed = false;
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
                changed = true;
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
                changed = true;
            }
        } else {
            keyDown_c = false;
        }
        if (changed && index_r == 0)
            CheckForChange(Input.GetAxisRaw("Horizontal"));
    }
    public abstract void Initialize();
    public abstract void CheckForChange(float horizontal);
    public abstract void Button(int r, int c);
    public abstract void StartGame();
    public abstract void Cancel();
}
