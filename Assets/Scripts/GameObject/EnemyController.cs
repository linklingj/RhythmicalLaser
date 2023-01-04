using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy myEnemy;
    public GameObject player;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public UIController uIController;
    public VFXManager vfx;
    public bool active;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        active = false;
        GameManager.OnGameStateChange += HandleOnGameStateChange;
    }


    void Update() {
        if (!active) return;
        myEnemy.Move(rb, this.transform, player.transform);
        if (myEnemy.hp <= 0) {
            GameManager.Instance.point += myEnemy.data.deathPoint;
            Die();
        }
    }

    public void Spawn(Enemy enemy) {
        //임시
        myEnemy = enemy;
        sr.color = uIController.currentColor.UI1;
        gameObject.SetActive(true);
        active = true;
        myEnemy.Spawn();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (!active) return;
        //플레이어 접촉
        if (col.CompareTag("Player")) {
            GameManager.Instance.playerHit();
            Die();
        }
        //레이저 맞음
        if (col.CompareTag("Laser")) {
            myEnemy.hp -= 1;
        }
    }

    public void Die() {
        gameObject.SetActive(false);
        vfx.EnemyDeath(transform.position);
        myEnemy.Enqueue(gameObject);
        active = false;
    }
    
    private void HandleOnGameStateChange(GameState state) {
        if (state == GameState.Finish) {
            active = false;
        }
    }
}
