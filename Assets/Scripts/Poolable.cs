using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    public IObjectPool<GameObject> Pool {get; set;}
    
    public void ReleaseObject(){
        if (gameObject.activeInHierarchy)
        {
            Pool.Release(gameObject);
        }
    }
}
