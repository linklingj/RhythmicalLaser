using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public int hp;
    float turnSmoothVelocity;

    private void Start() {
        
    }
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
        if (col.CompareTag("Laser")) {
            Die();
        }
    }
}
