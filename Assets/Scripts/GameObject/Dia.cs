using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dia : Enemy
{
    public float moveSpeed;
    public float turnSmoothTime;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        rb = GetComponent<Rigidbody2D>();
        cameraController = FindObjectOfType<CameraController>();
        hp = 1;
    }

    void Update() {
        TurnToward(turnSmoothTime);
        MoveToward(moveSpeed);
    }
}
