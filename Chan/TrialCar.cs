using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCar : MonoBehaviour
{
    public PathCreator pathCreator;
    public float TC1Distance;

    public GameObject TC1, TC2, TC3, TC4;

    void Update()
    {
        TC1Distance += Time.deltaTime * 50;

        TC1.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance);
        //TC1.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance);        
        TC2.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance*0.9f);
        //TC2.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance*0.9f);
        TC3.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance * 0.8f);
        //TC3.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance * 0.8f);
        TC4.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance * 0.7f);
        //TC4.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance * 0.7f);
    }
}
