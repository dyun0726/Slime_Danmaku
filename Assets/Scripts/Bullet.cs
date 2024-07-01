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

    private float xBound = 15f;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound){
            ReleaseObject();
        }

        transform.Translate(speed * Time.deltaTime * dir);
        if (dir.x != 0){
            spriteRenderer.flipX = dir.x < 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.layer == 8){
            if (other.gameObject.layer == 9){
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null){
                    enemy.TakeDamage(damage);
                } else {
                    Debug.Log("Fail to find Emeny component");
                }
                Destroy(this.gameObject);
            }
        }
        
    }
}
