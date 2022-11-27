using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dia : Enemy
{
    public int maxHP;
    public float moveSpeed;
    public float turnSmoothTime;

    private void Start() {
        
    }

    public override void Spawn() {
        hp = maxHP;
        base.Spawned();
    }
    
    public override void Dequeue() {
        ObjectPool.instance.diaQueue.Enqueue(gameObject);
    }

    void Update() {
        TurnToward(turnSmoothTime);
        MoveToward(moveSpeed);
    }

    

    
    
}
