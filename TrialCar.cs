using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCar : MonoBehaviour
{
    public PathCreator pathCreator;
    float distanceTravelled;
    [SerializeField] DemoCarController DriverCar;
    public float trialTime;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        trialTime += Time.deltaTime;

        if (DriverCar.TrialBool)
        {
            distanceTravelled += Time.deltaTime * 15;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        }

        if( trialTime >= 120)
        {
            DriverCar.TrialBool = false;
        }

        if(DriverCar.TrialBool == false)
        {
            trialTime = 0;
            gameObject.SetActive(false );
        }
    }
}
