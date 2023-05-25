using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class RoadCollision : MonoBehaviour
{
    public GameObject targetCar;

    void Update()
    {
        float targetX = targetCar.transform.position.z;
        float carX = transform.position.z;
        float distance = Mathf.Abs(carX - targetX);
        //Debug.Log("Distance : " + distance);
        if(distance <= 50)
        {
            //AR alert
        }
    }

    

}
