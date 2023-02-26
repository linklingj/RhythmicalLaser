using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Camera cam;
    public CameraController cameraController;
    public VFXManager vfxManager;
    public GameObject laser;
    [Header("Movement")]
    public float turnSmoothTime;
    public float drag;
    public float maxVel;
    public float shootForce;
    [Header("Laser")]
    public float laserForce;

    public float laserDurationBeat;
    public float laserTime;
    [Header("hp")]
    public int maxHP;

    Rigidbody2D rb;
    Animator anim;
    Animator laserAnim;
    Vector2 mousePos;
    Vector2 lookDir;
    float turnSmoothVelocity;
    bool freezeDir;
    float timer;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        laserAnim = laser.GetComponent<Animator>();
    }
    void Start() {
        rb.drag = drag;
        freezeDir = false;
        timer = 0;
        laserTime = 1 / ((float)GameManager.Instance.selectedMusic.bpm / (60 / laserDurationBeat));
        laserAnim.speed = 1 / laserTime;
    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        timer -= Time.deltaTime;
        if (timer > 0) {
            freezeDir = true;
            laser.SetActive(true);
        }
        else {
            freezeDir = false;
            laser.SetActive(false);
        }
        //temporary
         /*if (Input.GetKeyDown(KeyCode.Q)) {
             Kick();
         }
         if (Input.GetKeyDown(KeyCode.W)) {
             Snare();
         }*/
    }
    void FixedUpdate() {
        if (!freezeDir)
            LookMouse(turnSmoothTime);
    }
    void LookMouse(float tsTime) {
        lookDir = mousePos - rb.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * tsTime);
        rb.rotation = angle;
    }
    public void Kick() {
        cameraController.Shake(1);
        //rb.velocity = -transform.right * shootForce;
        rb.AddForce(-lookDir.normalized * shootForce, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
        anim.Play("Dash");
        vfxManager.PlayerDash(transform.position + transform.right * 0.4f, rb.rotation);
    }
    public void Snare() {
        cameraController.Shake(2);
        LookMouse(0f);
        laser.SetActive(true);
        laserAnim.Play("Laser");
        //rb.velocity = -transform.right * laserForce;
        rb.AddForce(-lookDir.normalized * laserForce, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
        anim.Play("Squish");
        timer = laserTime;
    }
}
