using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public BoxCollider2D cameraBounds;

    // Transform of the GameObject you want to shake
    private Transform transform;

    private Vector3 shakeOffset;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    private Vector2 camPositionMax;
    private Vector2 camPositionMin;
    private Camera cam;

    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
        shakeOffset = Vector3.zero;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        camPositionMax = cameraBounds.bounds.max - new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize, 0.0f);
        camPositionMin = cameraBounds.bounds.min + new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize, 0.0f);
        transform.position = target.position;
    }

    private void FixedUpdate()
    {
        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.fixedDeltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            shakeOffset = Vector3.zero;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothPosition.x = Mathf.Clamp(smoothPosition.x, camPositionMin.x, camPositionMax.x);
        smoothPosition.y = Mathf.Clamp(smoothPosition.y, camPositionMin.y, camPositionMax.y);
        smoothPosition.z = -10.0f;
        transform.position = smoothPosition + shakeOffset;
    }

    public void TriggerShake()
    {
        shakeDuration = 0.5f;
    }
}
