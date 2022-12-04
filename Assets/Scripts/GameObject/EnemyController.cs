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
    }

    void Update() {
        if (!active) return;
        myEnemy.Move(rb, this.transform, player.transform);
        if (myEnemy.hp <= 0) {
            GameManager.Instance.point += GameManager.Instance.enemyPoint;
            Die();
        }
    }

    public void Spawn() {
        //임시
        myEnemy = new Dia();
        sr.color = uIController.currentColor.UI1;
        gameObject.SetActive(true);
        active = true;
        myEnemy.Spawn();
    }

    private void OnTriggerEnter2D(Collider2D col) {
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
}
