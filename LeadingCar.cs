using Mono.Cecil.Cil;
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
    public float eventTimer;
    public float overtake;
    Vector3 velocity2 = new Vector3(0, 0, 0);
    [SerializeField] DemoCarController DriverCar;
    Vector3 startPos;
    float disableTime;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    bool eventStartBool = false;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        startPos= transform.position;
        gameObject.SetActive(false);
        
    }

    void FixedUpdate()
    {

        velocity2 = TargetCar.GetComponent<Rigidbody>().velocity;
        velocity2.y = 0;
        velocity2.x = 0;
        eventTimer += Time.deltaTime;

        if(eventStartBool == true)
        {
            EventStart(DriverCar.taskCount);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("StraightRoad"))
        {
            ThisCar.GetComponent<Rigidbody>().velocity = velocity2;
        }
    }

    private void EventStart(int count)
    {
        overtake += Time.deltaTime;

        if (overtake >= 5 && overtake <= 8)
        {
            velocity2.x = 1.3f;
        }

        if (overtake >= 8 + DriverCar.LaneChangeTime[count - 1] && (DriverCar.LaneChangeTime[count - 1] != 0))
        {
            velocity2.z = 0;
        }

        if (velocity2.z > 0)
        {
            velocity2.z *= 1.3f;
        }

        if (overtake >= 25 && (DriverCar.LaneChangeTime[count - 1] != 0))
        {
            gameObject.SetActive(false);
        }
        
        if(wayPointTrigger == true)
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

                wayPointTrigger= false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("TaskStartPoint"))
        {
            eventStartBool= true;
        }

        if (other.gameObject.CompareTag("WayPoint"))
        {
            wayPointTrigger = true;
        }
    }
}
