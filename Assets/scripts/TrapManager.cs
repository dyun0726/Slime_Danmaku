using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//맵에 있는 트랩 관리

public class TrapManager : MonoBehaviour
{
    Trap trap;

    // Start is called before the first frame update
    void Start()
    {
        trap = GetComponent<Trap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
