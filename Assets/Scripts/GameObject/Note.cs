using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteManager noteManager;
    int noteSpeed;
    float destroyY;
    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
        noteSpeed = noteManager.noteSpeed;
        destroyY = noteManager.destroyPos.localPosition.y;
    }

    void Update() {
        transform.localPosition += Vector3.down * noteManager.noteSpeed * Time.deltaTime;
        if (transform.localPosition.y <= destroyY);
    }
}
