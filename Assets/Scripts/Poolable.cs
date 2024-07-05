using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    public IObjectPool<GameObject> Pool {get; set;}
    private float test;
    public float Test {get{return test;} set{test = value;}}
    
    public void ReleaseObject(){
        Pool.Release(gameObject);
    }
}
