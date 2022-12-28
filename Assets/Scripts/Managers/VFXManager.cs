using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VFXManager : MonoBehaviour
{
    //32fps기준
    public float[] effectFrames;
    [SerializeField] UIController ui;
    [SerializeField] Camera playerCam;
    [SerializeField] Transform player;
    
    public float deathTransitionTime;
    [SerializeField] GameObject playerDeath, mask, cover;

    private void Awake() {
        GameManager.OnGameOver += PlayerDeath;
    }

    private void PlayerDeath () {
        Invoke( nameof(DeathAnimFin), deathTransitionTime);
        Vector3 screenPos = playerCam.WorldToScreenPoint(player.position);
        mask.transform.position = screenPos;
        mask.GetComponent<RectTransform>().localScale = new Vector3(500, 500, 1);
        playerDeath.SetActive(true);
        LeanTween.scale(mask, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutExpo).setOnComplete(() => {
            cover.transform.position = mask.transform.position;
            cover.SetActive(true);
        });
    }

    private void DeathAnimFin() {
        GameManager.Instance.ToFail();
    }

    
    public void EnemyDeath (Vector3 pos) {
        StartCoroutine(Effect1(pos + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f),0), new Vector3(1.6f,1.6f,1)));
        StartCoroutine(Effect1(pos + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.3f,0.3f),0), new Vector3(1.2f,1.2f,1)));
        StartCoroutine(Effect1(pos + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.4f,0.4f),0), new Vector3(1f,1f,1)));
    }

    IEnumerator Effect1(Vector3 pos, Vector3 size) {
        GameObject e = ObjectPool.instance.effect1Queue.Dequeue();
        e.transform.position = pos;
        e.transform.localScale = size;
        e.GetComponent<SpriteRenderer>().color = ui.currentColor.player2;
        e.SetActive(true);
        e.GetComponent<Animator>().Play("effect1");
        yield return new WaitForSeconds(effectFrames[0] / 32);
        e.SetActive(false);
        ObjectPool.instance.effect1Queue.Enqueue(e);
    }

    private void OnDestroy() {
        GameManager.OnGameOver -= PlayerDeath;
    }
}
