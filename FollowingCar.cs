using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FollowingCar : MonoBehaviour
{
    Rigidbody _rb;
    public GameObject TargetCar;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    Vector3 velocityLeft = new Vector3(0, 0, 0);
    Vector3 velocityRight = new Vector3(0, 0, 0);
    float disableTime;
    Vector3 startPos;
    [SerializeField] DemoCarController DriverCar;
    public GameObject carLeft, carRight;
    public bool eventStartBool = false;
    public float accelTime;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        startPos= transform.position;
        carLeft.SetActive(false);
        carRight.SetActive(false);
    }

    void FixedUpdate()
    {
        velocityLeft = TargetCar.GetComponent<Rigidbody>().velocity;
        velocityRight = TargetCar.GetComponent<Rigidbody>().velocity;
        velocityLeft.y = 0;
        velocityLeft.x = 0;
        velocityRight.y = 0;
        velocityRight.x = 0;

        if(eventStartBool == false )
        {
            velocityLeft.z = 1;
            velocityRight.z = 1;
        }

        if(eventStartBool == true)
        {
            accelTime += Time.deltaTime;

            if(accelTime >= 8 + DriverCar.LaneChangeTime[DriverCar.taskCount])
            {
                if (DriverCar.FollowingCarSpeed[DriverCar.taskCount] == 1)
                {
                    velocityLeft.z *= 1.15f;
                    velocityRight.z *= 1.1f;
                }
                else
                {
                    velocityLeft.z *= 1.1f;
                    velocityRight.z *= 1.15f;
                }
            }

            if (wayPointTrigger == true)
            {
                distanceTravelled += Time.deltaTime * 8;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
                disableTime += Time.deltaTime;
                if (disableTime > 8)
                {
                    gameObject.transform.position = startPos;
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.SetActive(false);

                    wayPointTrigger = false;
                    eventStartBool= false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WayPoint"))
        {
            wayPointTrigger = true;
        }
        if (other.gameObject.CompareTag("EventStartPoint"))
        {
            eventStartBool= true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("StraightRoad"))
        {
            carLeft.GetComponent<Rigidbody>().velocity = velocityLeft;
            carRight.GetComponent<Rigidbody>().velocity = velocityRight;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("StraightRoad"))
        {
            velocityLeft.z= 0;
            velocityRight.z= 0;
            carLeft.GetComponent<Rigidbody>().velocity = velocityLeft;
            carRight.GetComponent<Rigidbody>().velocity = velocityRight;
        }
    }
}
