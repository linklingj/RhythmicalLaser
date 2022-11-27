using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public UIController uIController;
    public VFXManager vfx;
    public bool active;
    public int hp;
    float turnSmoothVelocity;

    private void Start() {
        active = false;
        
    }

    private void Update() {
        if (hp <= 0) {
            GameManager.Instance.point += GameManager.Instance.enemyPoint;
            Die();
        }
    }

    public abstract void Spawn();

    public void Spawned() {
        //sr.color = uIController.currentColor.UI1;
        active = true;
    }

    public void TurnToward(float turnSmoothTime) {
        Vector2 lookDir = player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * turnSmoothTime);

        rb.rotation = angle;
    }

    public void MoveToward(float speed) {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void Die() {
        vfx.EnemyDeath(transform.position);
        Dequeue();
    }

    public abstract void Dequeue();

    private void OnTriggerEnter2D(Collider2D col) {
        //플레이어 접촉
        if (col.CompareTag("Player")) {
            GameManager.Instance.playerHit();
            Die();
        }
        //레이저 맞음
        if (col.CompareTag("Laser")) {
            hp -= 1;
        }
    }
}
