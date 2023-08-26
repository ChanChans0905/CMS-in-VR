using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCarRight : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    Rigidbody _rb;
    public PathCreator pathCreator;
    public float distanceTravelled;
    public bool wayPointTrigger = false;
    public float disableTime;
    Vector3 startPos;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void Update()
    {
        if (wayPointTrigger == true)
        {
            distanceTravelled += Time.deltaTime * 15;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            disableTime += Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WayPoint"))
        {
            wayPointTrigger = true;
        }
    }
}
