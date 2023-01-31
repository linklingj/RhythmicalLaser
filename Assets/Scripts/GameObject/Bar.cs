using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public NoteManager noteManager;

    public bool barActive = false;
    public int noteSpeed;
    public float noteMoveTime; //생성 후 히트 전까지 시간
    public double timeInstantiated;
    public float spawnX, hitX;
    public float visualDelay;
    public int barIdentity; //0 from left 1 from right

    UIController uIController;
    Image img;
    double timeSinceInstantiated;
    
    void Start() {
        noteManager = FindObjectOfType<NoteManager>();
        img = GetComponent<Image>();
        uIController = FindObjectOfType<UIController>();
        
        img.color = uIController.currentColor.UI2;
    }
    
    void Update() {
        if (!SongManager.Instance.musicPlaying || GameManager.Instance.State != GameState.Play)
            return;

        if (barActive) {
            timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / noteMoveTime);

            transform.localPosition = new Vector2( spawnX + (hitX - spawnX)*t + (visualDelay * noteSpeed * 0.01f)*(barIdentity*2-1), transform.localPosition.y);

            if (transform.localPosition.x > hitX && barIdentity == 0 || transform.localPosition.x < hitX && barIdentity == 1) {
                ObjectPool.instance.barQueue.Enqueue(gameObject);
                barActive = false;
                gameObject.SetActive(false);
            }
        }
    }
    
}
