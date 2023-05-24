using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    float timecount = 0;

   
    void Start()
    {
        
    }

    
    void Update()
    {
        timecount += Time.deltaTime;
        if (timecount > 5)
        {
            gameObject.transform.position = new Vector3(6035, 54, -1672);
        }
    }
}
