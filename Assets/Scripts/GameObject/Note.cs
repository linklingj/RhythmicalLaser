using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour
{
    public NoteManager noteManager;

    public bool noteActive = false;
    public bool beforeHit;
    public int noteSpeed;
    public float noteMoveTime; //생성 후 히트 전까지 시간
    public double timeInstantiated;
    public float assignedTime;
    public float spawnY,hitY,despawnY;
    public float visualDelay;
    public int noteIdentity; //0 kick 1 snare

    double timeSinceInstantiated;

    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
    }

    void Update() {
        if (!SongManager.Instance.musicPlaying)
            return;

        if (noteActive) {
            timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / noteMoveTime);

            transform.localPosition = new Vector2(transform.localPosition.x, spawnY + (hitY - spawnY)*t + (visualDelay * noteSpeed * 0.01f));

            if (t > 1 && beforeHit) {
                //noteManager.NoteHit(noteIdentity);
                beforeHit = false;
                Debug.Log("hit");
            }

            if (transform.localPosition.y < despawnY) {
                ObjectPool.instance.noteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
                noteActive = false;
            }
        }

    }
}
