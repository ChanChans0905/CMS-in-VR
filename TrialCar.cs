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
    [SerializeField] FadeInOut FadeInOut;

    private void Start()
    {
    }

    void Update()
    {
        distanceTravelled += Time.deltaTime * 15;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        trialTime += Time.deltaTime;

        if (trialTime >= 120)
        {
            DriverCar.TrialBool = false;
            FadeInOut.FadingEvent = true;
            DriverCar.respawnTrigger = true;
        }

        if (trialTime >= 125)
        {
            FadeInOut.FadingEvent = false;
            DriverCar.respawnTrigger = false;
            trialTime = 0;
            distanceTravelled = 0;
            gameObject.SetActive(false);
        }
    }
}
