using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float shakeDecay = 1.0f;

    private Vector3 initialPosition;
    private float currentShakeDuration = 0f;

    void Awake()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * shakeDecay;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.position = initialPosition;
        }
    }

    public void Shake(float duration, float magnitude, float decay = 1.0f)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeDecay = decay;
        currentShakeDuration = duration;
    }
}
