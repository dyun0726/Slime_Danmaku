using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Poolable
{
    private float speed = 2f;
    public float Speed {get {return speed;} set {speed = value;}}
    
    private Vector2 dir = Vector2.left;
    public Vector2 Dir {get {return dir;} set {dir = value;}}

    private float damage = 5f;
    public float Damage {get {return damage;} set {damage = value;}}

    private float range = 100f;
    public float Range {get {return range;} set {range = value;}}

    private Vector2 startPos = Vector2.zero;
    public Vector2 StartPos {get {return startPos;} set {startPos = value;}}

    private int passCount = 0;
    public int PassCount {get {return passCount;} set {passCount = value;}}

    private float lifeSteel = 0;
    public float LifeSteel {get {return lifeSteel;} set {lifeSteel = value;}}


    private float xBound = 15f;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound || getDist() > range){
            ReleaseObject();
        }

        transform.Translate(speed * Time.deltaTime * dir);
        if (dir.x != 0){
            spriteRenderer.flipX = dir.x < 0;
        }
    }

    private float getDist(){
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        return (currPos - startPos).magnitude;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // layer 8: Player Bullet, 7: Enemy bullet
        if (gameObject.layer == 7){ // 적 탄환 일때
            if (other.gameObject.layer == 10) { // player 일때

                Vector2 dir = (other.transform.position - transform.position).normalized;
                PlayerManager.Instance.TakeDamage(damage, dir);
                ReleaseObject();

            }
        }

        else if (gameObject.layer == 8){
            if (other.gameObject.layer == 9){
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null){
                    enemy.TakeDamage(damage);
                    if (lifeSteel > 0){
                        PlayerManager.Instance.Heal(lifeSteel);
                    }
                } else {
                    Debug.Log("Fail to find Emeny component");
                }

                if (passCount == 0){
                    ReleaseObject();
                } else if (passCount > 0) { // 관통 가능하면
                    passCount--;
                } else { // 오류 발생
                    Debug.LogError("passCount cannot be negative");
                }
                
            }
        }
        
    }
}
