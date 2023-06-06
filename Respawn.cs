using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    float timeCount = 0;

   
    void Start()
    {
        
    }

    
    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > 5)
        {
            gameObject.transform.position = new Vector3(6035, 54, -1672);
        }
    }
}
