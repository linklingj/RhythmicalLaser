using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public UIController uIController;
    public GameObject player;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public int hp;
    float turnSmoothVelocity;

    public void Die() {
        Destroy(gameObject);
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

    private void OnTriggerEnter2D(Collider2D col) {
        //레이저 맞음
        if (col.CompareTag("Laser")) {
            GameManager.Instance.point += GameManager.Instance.enemyPoint;
            Die();
        }
        //플레이어 접촉
        if (col.CompareTag("Player")) {
            GameManager.Instance.playerHit();
            Die();
        }
    }
}
