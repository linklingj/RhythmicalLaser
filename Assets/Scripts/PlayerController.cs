using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Camera cam;
    public float turnSmoothTime;
    public float drag;
    public float maxVel;
    public float shootForce;
    public float laserForce;
    Rigidbody2D rb;
    Vector2 mousePos;
    float turnSmoothVelocity;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {
        rb.drag = drag;
    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Z)) {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            Laser();
        }
        
    }
    void FixedUpdate() {
        LookMouse();
    }
    void LookMouse() {
        Vector2 lookDir = mousePos - rb.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * turnSmoothTime);
        rb.rotation = angle;
    }
    void Shoot() {
        rb.AddForce(-transform.right * shootForce,ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
    }
    void Laser() {
        rb.velocity = -transform.right * laserForce;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
    }
}
