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
    public float noteActiveTime; //생성 후 파괴 전까지 시간
    public double timeInstantiated;
    public float assignedTime;
    public float spawnY,hitY,despawnY;
    public int noteIdentity; //0 kick 1 snare

    double timeSinceInstantiated;

    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
    }

    void Update() {

        if (noteActive) {
            timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / noteActiveTime);

            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, spawnY, 0), new Vector3(transform.localPosition.x, despawnY, 0), t);

            if (t > noteMoveTime / noteActiveTime && beforeHit) {
                //noteManager.NoteHit(noteIdentity);
                beforeHit = false;
            }

            if (t > 1) {
                ObjectPool.instance.noteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
                noteActive = false;
            }
        }

    }
}
