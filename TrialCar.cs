using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCar : MonoBehaviour
{
    public PathCreator pathCreator;
    float distanceTravelled;
    [SerializeField] DemoCarController DriverCar;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (DriverCar.TrialBool)
        {
            distanceTravelled += Time.deltaTime * 8;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
    }
}
