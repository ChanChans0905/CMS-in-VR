using Mono.Cecil.Cil;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LeadingCar2 : MonoBehaviour
{
    Rigidbody _rb;
    public GameObject TargetCar;
    public GameObject ThisCar;
    public float eventTimer;
    public float overtake, laneChangeTimer;
    Vector3 velocity2 = new Vector3(0, 0, 0);
    [SerializeField] DemoCarController DriverCar;
    Vector3 startPos;
    float disableTime;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    public bool eventStartBool = false;
    public int LaneChangeDirection = 0;
    Vector3 CarSpeed = new Vector3(-95.6f, 0, 1097);
    public bool enterTrigger = false;
    public bool Respawn;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        startPos = gameObject.transform.position;
        gameObject.SetActive(false);
    }

    void Update()
    {
        CarSpeed.z = TargetCar.transform.localPosition.x - 80;

        if (enterTrigger == false)
        {
            gameObject.transform.localPosition = CarSpeed;
        }

        if (eventStartBool == true)
        {
            EventStart(DriverCar.taskCount);
        }

        if (Respawn && DriverCar.respawnTrigger)
        {
            Debug.Log(Respawn);
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
        if (overtake > 0 && overtake <= 5)
        {
            gameObject.transform.localPosition = CarSpeed;
        }

        if (overtake > 5 && overtake <= 8)
        {
            laneChangeTimer += Time.deltaTime * 1.4f;
            CarSpeed.x = laneChangeTimer - 95.5f;
            gameObject.transform.localPosition = CarSpeed;
        }

        if (overtake > 8 && (DriverCar.LaneChangeTime[count] == 0))
        {
            gameObject.transform.localPosition = CarSpeed;
        }
        else if (overtake < 8 + DriverCar.LaneChangeTime[count] && (DriverCar.LaneChangeTime[count] != 0))
        {
            gameObject.transform.localPosition = CarSpeed;
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
