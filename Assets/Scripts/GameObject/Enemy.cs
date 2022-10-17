using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    public int hp;

    private void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
    public void Die() {
        Destroy(gameObject);
    }
    public void TurnToward(float turnSmoothTime) {
        Vector2 lookDir = player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    }

    public void MoveToward(float speed) {

    }
}
