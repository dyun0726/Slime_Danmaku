using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // Update is called once per frame
    void Update()
    {
        // live 체크 함수
        if (!GameManager.Instance.isLive)
        {  
            return;
        }
        
        if (transform.position.x < GameManager.Instance.leftBound || transform.position.x > GameManager.Instance.rightBound ||
            transform.position.y < GameManager.Instance.lowerBound || transform.position.y > GameManager.Instance.upperBound || getDist() > range)
        {
            ReleaseObject();
        }

        transform.Translate(speed * Time.deltaTime * dir, Space.World);
    }

    protected float getDist(){
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        return (currPos - startPos).magnitude;
    }

}
