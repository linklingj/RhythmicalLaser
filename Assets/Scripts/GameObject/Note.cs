using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public int index;

    UIController uIController;
    Image img;
    double timeSinceInstantiated;

    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
        img = GetComponent<Image>();
        uIController = FindObjectOfType<UIController>();

        img.color = uIController.currentColor.UI1;
    }

    void Update() {
        if (!SongManager.Instance.musicPlaying || GameManager.Instance.State != GameState.Play)
            return;

        if (noteActive) {
            timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / noteMoveTime);

            transform.localPosition = new Vector2(transform.localPosition.x, spawnY + (hitY - spawnY)*t + (visualDelay * noteSpeed * 0.01f));

            if (t > 1 && beforeHit) {
                //noteManager.NoteHit(noteIdentity);
                beforeHit = false;
            }

            if (transform.localPosition.y < despawnY) {
                bool hitCheck;
                if (noteIdentity == 0)
                    hitCheck = noteManager.k_hit[index];
                else
                    hitCheck = noteManager.s_hit[index];
                if (!hitCheck) {
                    //Debug.Log("nohit"+index.ToString() + noteIdentity.ToString());
                    noteManager.NoHit();
                    transform.Translate(Vector3.left*200);
                }
                ObjectPool.instance.noteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
                noteActive = false;
            }
        }

    }
}
