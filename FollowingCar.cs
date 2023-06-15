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
    [SerializeField] LeadingCar LeadingCar;
    [SerializeField] FollowingCarRight FollowingCarRight;
    public GameObject carLeft, carRight;
    public bool eventStartBool = false;
    public float accelTime;
    public Vector3 CarSpeedLeft1 = new Vector3(-404, 0, 0);
    public Vector3 CarSpeedRight1 = new Vector3(-395, 0, 0);
    public float laneChangeTimer;
    public float distance = 25;
    public bool Respawn;


    void Start()
    {
        startPos= transform.position;
        carLeft.SetActive(false);
        carRight.SetActive(false);
    }

    void Update()
    {
            carLeft.transform.localPosition = CarSpeedLeft1;
            carRight.transform.localPosition = CarSpeedRight1;

            if (LeadingCar.eventStartBool)
            {
                accelTime += Time.deltaTime;
                if (DriverCar.LaneChangeTime[DriverCar.taskCount] != 0)
                {
                    if (accelTime >= 8 + DriverCar.LaneChangeTime[DriverCar.taskCount])
                    {
                        if (DriverCar.FollowingCarSpeed[DriverCar.taskCount] == 1)
                        {
                        laneChangeTimer += Time.deltaTime;
                        CarSpeedLeft1.z = (TargetCar.transform.localPosition.x - distance) + laneChangeTimer*2f;
                        CarSpeedRight1.z = (TargetCar.transform.localPosition.x - distance) + laneChangeTimer*3f;
                        }
                        else
                        {
                        laneChangeTimer += Time.deltaTime;
                        CarSpeedLeft1.z = (TargetCar.transform.localPosition.x - distance) + laneChangeTimer * 3f;
                            CarSpeedRight1.z = (TargetCar.transform.localPosition.x - distance) + laneChangeTimer * 2f;
                        }
                    }
                    else if (accelTime <= 8 + DriverCar.LaneChangeTime[DriverCar.taskCount])
                    {
                        CarSpeedLeft1.z = TargetCar.transform.localPosition.x - distance;
                        CarSpeedRight1.z = TargetCar.transform.localPosition.x - distance;
                    }
                }
                else if (DriverCar.LaneChangeTime[DriverCar.taskCount] == 0)
                {

                CarSpeedLeft1.z = TargetCar.transform.localPosition.x - distance;
                CarSpeedRight1.z = TargetCar.transform.localPosition.x - distance;
            }
            }
            else if (eventStartBool == false)
            {
                CarSpeedLeft1.z = TargetCar.transform.localPosition.x - distance;
                CarSpeedRight1.z = TargetCar.transform.localPosition.x - distance;
            }

        if (wayPointTrigger == true || DriverCar.respawnTrigger)
        {
            distanceTravelled += Time.deltaTime * 15;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            disableTime += Time.deltaTime;

            if(disableTime > 20) { Respawn = true; }

        }

        if (Respawn && DriverCar.respawnTrigger)
        {
            FollowingCarRight.transform.position = startPos;
            FollowingCarRight.transform.rotation = Quaternion.identity;
            FollowingCarRight.wayPointTrigger = false;
            FollowingCarRight.disableTime = 0;
            FollowingCarRight.distanceTravelled = 0;
            FollowingCarRight.gameObject.SetActive(false);

            laneChangeTimer = 0;
            accelTime = 0;
            distanceTravelled = 0;
            disableTime = 0;
            wayPointTrigger = false;
            eventStartBool = false;
            Respawn = false;
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
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
