using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 참조
    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러운 정도
    public Vector3 offset; // 플레이어와 카메라 사이의 거리

    // 경계를 설정할 변수들
    public Transform leftBoundary;
    public Transform rightBoundary;

    public bool isInitialized = false;

    void Start()
    {
        if (player != null)
        {
            InitializeCameraPosition();
        }
    }

    void LateUpdate()
    {
        if (!isInitialized)
        {
            InitializeCameraPosition();
        }
        else
        {
            FollowPlayer();
        }
    }

    void InitializeCameraPosition()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned to CameraFollow script.");
            return;
        }

        Vector3 desiredPosition = player.position + offset;

        // 경계를 넘지 않도록 카메라 위치를 제한
        float clampedX = Mathf.Clamp(desiredPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        Vector3 clampedPosition = new Vector3(clampedX, desiredPosition.y, desiredPosition.z);

        transform.position = clampedPosition;
        isInitialized = true;
    }

    void FollowPlayer()
    {
        Vector3 desiredPosition = player.position + offset;

        // 경계를 넘지 않도록 카메라 위치를 제한
        float clampedX = Mathf.Clamp(desiredPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        Vector3 clampedPosition = new Vector3(clampedX, desiredPosition.y, desiredPosition.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
