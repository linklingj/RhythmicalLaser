using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyData {
    public int maxHP;
    public float moveSpeed;
    public int deathPoint;
    public float turnSmoothTime;
}

public abstract class Enemy
{
    public EnemyData enemyData;
    public int hp;
    
    float turnSmoothVelocity;

    public abstract void Spawn();
    public abstract void Move(Rigidbody2D rb, Transform transform, Transform player);


    public void TurnToward(Rigidbody2D rb, Transform transform, Transform player) {
        Vector2 lookDir = player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * enemyData.turnSmoothTime);

        rb.rotation = angle;
    }

    public void MoveToward(Transform transform) {
        transform.Translate(Vector3.right * enemyData.moveSpeed * Time.deltaTime);
    }

    
    public abstract void Enqueue(GameObject gameObject);

    
}
