using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 originalPosition;
    [Header("Shake")]
    public bool screenShakeOn;
    [System.Serializable]
    public class shakeValue {
        public string name;
        public float power, duration;
        public List <AnimationCurve> curves;
        public float rotationPower;
        public AnimationCurve rotationCurve;
    }
    public List<shakeValue> shakeValues;
    float timer;
    float shakePower, shakeDuration, rotationPower;
    AnimationCurve currentCurveX,currentCurveY,currentRotCurve;
    int negativeMultiply = 1;
    bool shaking;
    void Start() {
        timer = 0;
        shaking = false;
    }

    void LateUpdate() {
        if (screenShakeOn)
            CameraShake();
    }
    
    void CameraShake() {
        if (!shaking)
            return;

        if (timer > shakeDuration) {
            shaking = false;
            timer = 0;
            return;
        }
        
        timer += Time.deltaTime;
        float moveX = (currentCurveX.Evaluate(timer / shakeDuration) - 1) * shakePower;
        float moveY = (currentCurveY.Evaluate(timer / shakeDuration) - 1) * shakePower;
        float rotation = (currentRotCurve.Evaluate(timer / shakeDuration) - 1) * rotationPower * negativeMultiply;
        transform.position = new Vector3(originalPosition.x + moveX, originalPosition.y + moveY, originalPosition.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }

    public void Shake(int n) {
        negativeMultiply *= -1;
        if (n > shakeValues.Count)
            return;

        if (shaking && shakeValues[n].power < shakePower)
            return;
        shakePower = shakeValues[n].power;
        shakeDuration = shakeValues[n].duration;
        rotationPower = shakeValues[n].rotationPower;
        int r = Random.Range(0,shakeValues[n].curves.Count-1);
        currentCurveX = shakeValues[n].curves[r];
        currentCurveY = shakeValues[n].curves[(r+Random.Range(1,shakeValues[n].curves.Count)) % shakeValues[n].curves.Count];
        currentRotCurve = shakeValues[n].rotationCurve;
        timer = 0;
        shaking = true;
    }
}
