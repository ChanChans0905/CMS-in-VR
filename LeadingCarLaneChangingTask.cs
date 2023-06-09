using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadingCarLaneChangingTask : MonoBehaviour
{
    public GameObject TargetCar;
    public GameObject ThisCar;
    public float eventTimer, overtake, laneChangeTimer;
    Vector3 velocity2 = new Vector3(0, 0, 0);
    [SerializeField] DemoCarController DriverCar;
    Vector3 startPos;
    float disableTime;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    public bool eventStartBool = false;
    public int LaneChangeDirection = 0;
    Vector3 CarSpeed = new Vector3(-404, 0, 0);
    public bool enterTrigger = false;
    public bool Respawn;


    void Start()
    {
        startPos = gameObject.transform.position;
        gameObject.SetActive(false);
    }

    void Update()
    {
        CarSpeed.z = TargetCar.transform.localPosition.x + 80;

        if (enterTrigger == false)
        {
            gameObject.transform.localPosition = CarSpeed;
        }

        if (eventStartBool) { EventStart(DriverCar.taskCount); }

        if (Respawn && DriverCar.respawnTrigger)
        {
            overtake = 0;
            laneChangeTimer = 0;
            distanceTravelled = 0;
            disableTime = 0;
            wayPointTrigger = false;
            eventStartBool = false;
            enterTrigger = false;
            Respawn = false;
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }
    }

    private void EventStart(int count)
    {
        overtake += Time.deltaTime;
        if (overtake > 0 && overtake <= 8)
        {
            gameObject.transform.localPosition = CarSpeed;
        }

        if (overtake > 8 && overtake <= 10)
        {
            laneChangeTimer += Time.deltaTime * 1.4f;
            CarSpeed.x = laneChangeTimer - 404;
            gameObject.transform.localPosition = CarSpeed / laneChangeTimer;
        }

        if (overtake > 8 && (DriverCar.LaneChangeTime[count] == 0))
        {
            gameObject.transform.localPosition = CarSpeed / laneChangeTimer;
        }
        else if (overtake < 8 + DriverCar.LaneChangeTime[count] && (DriverCar.LaneChangeTime[count] != 0))
        {
            gameObject.transform.localPosition = CarSpeed / laneChangeTimer;
        }

        if (overtake >= 25 && (DriverCar.LaneChangeTime[count] != 0))
        {
            Respawn = true;

        }

        if (wayPointTrigger == true || DriverCar.respawnTrigger)
        {
            distanceTravelled += Time.deltaTime * 20;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            disableTime += Time.deltaTime;

            if (disableTime > 20) { Respawn = true; }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TaskStartPoint1"))
        {
            eventStartBool = true;
            enterTrigger = true;
        }

        if (other.gameObject.CompareTag("WayPoint"))
        {
            wayPointTrigger = true;
        }
    }
}
