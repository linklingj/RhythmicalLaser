using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Camera cam;
    public CameraController cameraController;
    public GameObject laser;
    [Header("Movement")]
    public float turnSmoothTime;
    public float drag;
    public float maxVel;
    public float shootForce;
    public float laserForce;
    public float laserTime;
    [Header("hp")]
    public int maxHP;

    Rigidbody2D rb;
    Animator anim;
    Vector2 mousePos;
    Vector2 lookDir;
    float turnSmoothVelocity;
    bool freezeDir;
    float timer;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start() {
        rb.drag = drag;
        freezeDir = false;
        timer = 0;
        laserTime = 1 / ((float)GameManager.Instance.selectedMusic.bpm / 15);
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
        // if (Input.GetKeyDown(KeyCode.Q)) {
        //     Kick();
        // }
        // if (Input.GetKeyDown(KeyCode.W)) {
        //     Snare();
        // }
    }
    void FixedUpdate() {
        if (!freezeDir)
            LookMouse();
    }
    void LookMouse() {
        lookDir = mousePos - rb.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * turnSmoothTime);
        rb.rotation = angle;
    }
    public void Kick() {
        cameraController.Shake(1);
        //rb.velocity = -transform.right * shootForce;
        rb.AddForce(-lookDir.normalized * shootForce, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
        anim.Play("Dash");
    }
    public void Snare() {
        cameraController.Shake(2);
        //rb.velocity = -transform.right * laserForce;
        rb.AddForce(-lookDir.normalized * laserForce, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
        timer = laserTime;
        //temporary script
        // StartCoroutine("Laser");
    }

    
    IEnumerator Laser() {
        freezeDir = true;
        laser.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        freezeDir = false;
        laser.SetActive(false);
    }
}
