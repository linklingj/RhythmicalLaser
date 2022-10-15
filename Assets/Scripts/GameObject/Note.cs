using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteManager noteManager;
    public bool noteActive = false;
    int noteSpeed;
    float destroyY;
    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
        noteSpeed = noteManager.noteSpeed;
        destroyY = noteManager.destroyPos.localPosition.y;
    }

    void Update() {
        if (noteActive)
            transform.localPosition += Vector3.down * noteSpeed * Time.deltaTime;

        if (transform.localPosition.y <= destroyY) {
            ObjectPool.instance.noteQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
            noteActive = false;
        }
    }
}
