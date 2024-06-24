using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾��� �̵� �ӵ�

    private Rigidbody2D rb; // �÷��̾��� Rigidbody2D ������Ʈ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
    }

    void Update()
    {
        // �÷��̾� �̵� �Է� ó��
        float moveHorizontal = Input.GetAxis("Horizontal"); // �¿� �̵� �Է�
        float moveVertical = Input.GetAxis("Vertical"); // ���� �̵� �Է�

        // �̵� ���� ���� ���
        Vector2 moveDirection = new Vector2(moveHorizontal, moveVertical);

        // Rigidbody�� �̿��� �÷��̾� �̵�
        rb.velocity = moveDirection.normalized * moveSpeed;
    }
}