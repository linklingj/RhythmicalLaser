using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour
{
    public NoteManager noteManager;

    public bool noteActive = false;
    public int noteSpeed;
    public float noteActiveTime;
    public double timeInstantiated;
    public float assignedTime;
    public float spawnY,hitY,despawnY;

    double timeSinceInstantiated;

    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
    }

    void Update() {

        if (noteActive) {
            timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / noteActiveTime);

            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, spawnY, 0), new Vector3(transform.localPosition.x, despawnY, 0), t);
            if (t > 1) {
                ObjectPool.instance.noteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
                noteActive = false;
            }
        }

    }
}
