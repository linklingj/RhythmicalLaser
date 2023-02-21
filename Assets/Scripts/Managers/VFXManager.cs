using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class VFXManager : MonoBehaviour
{
    //32fps기준
    public float[] effectFrames;
    [SerializeField] UIController ui;
    [SerializeField] Camera playerCam;
    [SerializeField] Transform player;
    
    public float deathTransitionTime;
    [SerializeField] GameObject playerDeath, mask, cover;

    public float clearTransitionTime;
    [SerializeField] GameObject clearEffect;
    [SerializeField] TextMeshProUGUI[] clearTexts;
    [SerializeField] Image[] boxTypeA, boxTypeB;

    public float transitionTime;
    [SerializeField] private Animator transition;

    private void Awake() {
        GameManager.OnGameOver += PlayerDeath;
        GameManager.OnClear += MusicClear;
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

    private void MusicClear() {
        Color yellowColor = new Color(250/255f,230/255f,30/255f);
        Color redColor = new Color(231/255f,0,87/255f);
        if (GameManager.Instance.fullCombo && GameManager.Instance.noHit) {
            ClearAnimation("PERFECT PLAY", yellowColor, redColor);
        }
        else if (GameManager.Instance.fullCombo) {
            ClearAnimation("FULL COMBO",redColor,redColor);
        }
        else if (GameManager.Instance.noHit) {
            ClearAnimation("NO HIT",yellowColor,yellowColor);
        }
        else {
            Transition1();
        }
    }

    void ClearAnimation(string text, Color color1, Color color2) {
        clearEffect.SetActive(true);
        clearEffect.GetComponent<Animator>().Play("transition5");
        clearTexts[0].color = new Color(color1.r * 0.8f, color1.g * 0.8f, color1.b * 0.8f);
        clearTexts[1].color = color1;
        foreach (var t in clearTexts) {
            t.text = text;
        }
        foreach (var box in boxTypeA) {
            box.color = color1;
        }
        foreach (var box in boxTypeB) {
            box.color = color2;
        }
        Invoke(nameof(Transition1), clearTransitionTime);
    }

    
    void Transition1() {
        transition.Play("transition1");
        GameManager.Instance.Invoke("ToClear",transitionTime);
    }
    
    public void EnemyDeath (Vector3 pos, int maxHP) {
        // StartCoroutine(Effect1(pos + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f),0), new Vector3(1.6f,1.6f,1)));
        // StartCoroutine(Effect1(pos + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.3f,0.3f),0), new Vector3(1.2f,1.2f,1)));
        // StartCoroutine(Effect1(pos + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.4f,0.4f),0), new Vector3(1f,1f,1)));
        if (maxHP > 1)
            StartCoroutine(Blast2(pos));
        else
            StartCoroutine(Blast3(pos));
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

    IEnumerator Blast1 (Vector2 pos) {
        GameObject e = ObjectPool.instance.blast1Queue.Dequeue();
        e.transform.position = pos;
        e.SetActive(true);
        e.GetComponent<Animator>().Play("blast_1");
        yield return new WaitForSeconds(effectFrames[1] / 60);
        e.SetActive(false);
        ObjectPool.instance.blast1Queue.Enqueue(e);
    }
    
    IEnumerator Blast2 (Vector2 pos) {
        GameObject e = ObjectPool.instance.blast1Queue.Dequeue();
        e.transform.position = pos;
        e.SetActive(true);
        e.GetComponent<Animator>().Play("blast_2");
        yield return new WaitForSeconds(effectFrames[1] / 60);
        e.SetActive(false);
        ObjectPool.instance.blast1Queue.Enqueue(e);
    }
    
    IEnumerator Blast3 (Vector2 pos) {
        GameObject e = ObjectPool.instance.blast1Queue.Dequeue();
        e.transform.position = pos;
        e.SetActive(true);
        e.GetComponent<Animator>().Play("blast_3");
        yield return new WaitForSeconds(effectFrames[1] / 60);
        e.SetActive(false);
        ObjectPool.instance.blast1Queue.Enqueue(e);
    }

    private void OnDestroy() {
        GameManager.OnGameOver -= PlayerDeath;
        GameManager.OnClear -= MusicClear;
    }
}
