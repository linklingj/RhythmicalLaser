using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dia : Enemy
{
    public float moveSpeed;
    public float turnSmoothTime;

    void Start() {
        hp = 1;
    }

    void Update() {
        TurnToward(turnSmoothTime);
        MoveToward(moveSpeed);
    }
}
