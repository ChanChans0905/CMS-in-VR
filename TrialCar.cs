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
        distanceTravelled += Time.deltaTime * 15;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}
