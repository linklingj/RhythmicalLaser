using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dia : Enemy
{
    public int maxHP;
    public float moveSpeed;
    public float turnSmoothTime;


    public override void Spawn() {
        enemyData = GameManager.Instance.enemyData[0];
        hp = enemyData.maxHP;
    }

    public override void Move(Rigidbody2D rb, Transform transform, Transform player) {
        TurnToward(rb, transform, player);
        MoveToward(transform);
    }
    
    public override void Enqueue(GameObject gameObject) {
        ObjectPool.instance.diaQueue.Enqueue(gameObject);
    }
    
}
