using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FollowingCarLeft : MonoBehaviour
{
    Rigidbody _rb;
    public GameObject TargetCar;
    public GameObject ThisCar;
    public float laneChangeCoor;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    Vector3 velocity2 = new Vector3(0, 0, 0);
    float disableTime;
    Vector3 startPos;
    [SerializeField] DemoCarController DriverCar;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        startPos= transform.position;
        gameObject.SetActive(false);
        
    }

    void FixedUpdate()
    {

        velocity2 = TargetCar.GetComponent<Rigidbody>().velocity;
        velocity2.y= 0;
        velocity2.x= 0;
        laneChangeCoor += Time.deltaTime;

        if (DriverCar.FollowingCarSpeed[DriverCar.taskCount] == 0 )
        {
            velocity2.z *= 1.5f;
        }
        else
        {
            velocity2.z *= 1.2f;
        }

        if(wayPointTrigger == true)
        {
            distanceTravelled += Time.deltaTime*8;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            disableTime += Time.deltaTime;
            if(disableTime > 8)
            {
                gameObject.transform.position = startPos;
                gameObject.transform.rotation= Quaternion.identity;
                gameObject.SetActive(false);

                wayPointTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WayPoint"))
        {
            wayPointTrigger = true;
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("StraightRoad"))
        {
            ThisCar.GetComponent<Rigidbody>().velocity = velocity2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("StraightRoad"))
        {
            velocity2.z= 0;
            ThisCar.GetComponent<Rigidbody>().velocity = velocity2;
        }
    }
}
