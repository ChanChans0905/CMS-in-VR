//using Mono.Cecil.Cil;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LeadingCar : MonoBehaviour
{

    Rigidbody _rb;
    public GameObject TargetCar;
    public GameObject ThisCar;
    public float eventTimer, overtake, laneChangeTimer;
    [SerializeField] DemoCarController DriverCar;
    Vector3 startPos;
    float disableTime;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    public bool eventStartBool = false;
    public int LaneChangeDirection = 0;
    Vector3 CarSpeed = new Vector3(794.06f,0,0);
    public bool enterTrigger = false;
    public bool Respawn;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        startPos= gameObject.transform.position;
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        CarSpeed.z = TargetCar.transform.position.z + 80;
        
        if(enterTrigger == false)
        {
            gameObject.transform.position = CarSpeed;
        }

        if (eventStartBool) { EventStart(DriverCar.taskCount);}

        if (Respawn && DriverCar.respawnTrigger)
        {
            CarSpeed = new Vector3(794.06f, 0, 0);
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
            gameObject.transform.position = CarSpeed;
        }

        if (overtake > 5 && overtake <= 8)
        {
            //laneChangeTimer += (overtake-5) * 0.04f;
            CarSpeed.x += (overtake - 5) * 0.043f /*+ 794.06f*/;
            gameObject.transform.position = CarSpeed;
        }

        if (overtake > 8 && (DriverCar.LaneChangeTime[count] == 0))
        {
            gameObject.transform.position = CarSpeed;
        }
        else if (overtake < 8 + DriverCar.LaneChangeTime[count] && (DriverCar.LaneChangeTime[count] != 0))
        {
            gameObject.transform.position = CarSpeed;
        }

        if (overtake >= 8 + DriverCar.LaneChangeTime[count] && (DriverCar.LaneChangeTime[count] != 0))
        {
        }

        if (overtake >= 25 && (DriverCar.LaneChangeTime[count] != 0))
        {
            Respawn = true;
        }

        if(wayPointTrigger)
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
        if(other.gameObject.CompareTag("TaskStartPoint1"))
        {
            eventStartBool= true;
            enterTrigger = true;
        }

        if (other.gameObject.CompareTag("WayPoint"))
        {
            wayPointTrigger = true;
        }
    }
}
