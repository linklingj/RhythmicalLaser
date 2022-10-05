using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Camera cam;
    public float turnSmoothTime;
    Rigidbody2D rb;
    Vector2 mousePos;
    float turnSmoothVelocity;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {
    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    void FixedUpdate() {
        LookMouse();
    }
    void LookMouse() {
        Vector2 lookDir = mousePos - rb.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * turnSmoothTime);
        rb.rotation = angle;
    }
}
