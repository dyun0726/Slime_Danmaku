using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform�� ����
    public float smoothSpeed = 0.125f; // ī�޶� �̵��� �ε巯�� ����
    public Vector3 offset; // �÷��̾�� ī�޶� ������ �Ÿ�

    // ��踦 ������ ������
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

        // ��踦 ���� �ʵ��� ī�޶� ��ġ�� ����
        float clampedX = Mathf.Clamp(desiredPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        Vector3 clampedPosition = new Vector3(clampedX, desiredPosition.y, desiredPosition.z);

        transform.position = clampedPosition;
        isInitialized = true;
    }

    void FollowPlayer()
    {
        Vector3 desiredPosition = player.position + offset;

        // ��踦 ���� �ʵ��� ī�޶� ��ġ�� ����
        float clampedX = Mathf.Clamp(desiredPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        Vector3 clampedPosition = new Vector3(clampedX, desiredPosition.y, desiredPosition.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
