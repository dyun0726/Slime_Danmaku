using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : BossBullet
{
    private Rigidbody2D rb;
    private float angleRange = 2f;
    public float deathTime = 0f;
    // Start is called before the first frame update
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // live 체크 함수
        if (!GameManager.Instance.isLive)
        {  
            return;
        }
        
        if (transform.position.x < GameManager.Instance.leftBound || transform.position.x > GameManager.Instance.rightBound ||
            transform.position.y < GameManager.Instance.lowerBound || transform.position.y > GameManager.Instance.upperBound || Time.time > deathTime)
        {
            // if (Time.time > deathTime)
             ReleaseObject();
        }
    }

    private void FixedUpdate() {
        Vector2 playerDir = GetPlayerDirection();
        float playerAngle = GetRotationAngle(playerDir);
        float curAngle = transform.eulerAngles.z;

        // 현재 각도와 회전해야할 각도 차이가 0.05도 사이이면 그만큼 회전
        if (Mathf.Abs(curAngle - playerAngle) < angleRange)
        {
            Dir = playerDir;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerAngle));
        }
        else
        {
            // 사이각 계산
            float includeAngle = playerAngle - curAngle;
            if (includeAngle < 0) includeAngle += 360;
            float newAngle = (includeAngle < 180) ? curAngle + angleRange : curAngle - angleRange;
            Dir = AngleToVector2(newAngle);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
        }

        rb.velocity = Speed * Dir;
    }

    private Vector2 GetPlayerDirection(){
        Vector3 locDiff = Player.Instance.GetPlayerLoc() - transform.position;
        return ((Vector2)locDiff).normalized;
    }
    private float GetRotationAngle(Vector2 dir){
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        return angle > 0 ? angle : angle + 360;
    }
    private Vector2 AngleToVector2(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleInRadians);
        float y = Mathf.Sin(angleInRadians);
        return new Vector2(x, y);
    }
}
