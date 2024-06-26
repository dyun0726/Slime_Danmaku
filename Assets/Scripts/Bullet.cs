using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Poolable
{
    public float speed = 2f;
    private float xBound = 10f;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound){
            ReleaseObject();
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
