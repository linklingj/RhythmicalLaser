using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Camera cam;
    public GameObject laser;
    [Header("Movement")]
    public float turnSmoothTime;
    public float drag;
    public float maxVel;
    public float shootForce;
    public float laserForce;
    [Header("hp")]
    public int maxHP;

    Rigidbody2D rb;
    Vector2 mousePos;
    float turnSmoothVelocity;
    bool freezeDir;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {
        rb.drag = drag;
        freezeDir = false;
    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //temporary
        if (Input.GetKeyDown(KeyCode.Q)) {
            Kick();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            Snare();
        }
        
    }
    void FixedUpdate() {
        if (!freezeDir)
            LookMouse();
    }
    void LookMouse() {
        Vector2 lookDir = mousePos - rb.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetAngle, ref turnSmoothVelocity, Time.smoothDeltaTime * turnSmoothTime);
        rb.rotation = angle;
    }
    public void Kick() {
        //rb.velocity = -transform.right * shootForce;
        rb.AddForce(-transform.right * shootForce, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
    }
    public void Snare() {
        //rb.velocity = -transform.right * laserForce;
        rb.AddForce(-transform.right * laserForce, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVel);
        //temporary script
        StartCoroutine("Laser");
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Enemy")) {
            GameObject contactEnemy = col.transform.gameObject;
            contactEnemy.GetComponent<Enemy>().Die();
            GameManager.Instance.point += GameManager.Instance.enemyPoint;
        }
    }
    IEnumerator Laser() {
        freezeDir = true;
        laser.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        freezeDir = false;
        laser.SetActive(false);
    }
}
