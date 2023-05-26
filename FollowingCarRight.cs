using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCarRight : MonoBehaviour
{
    Rigidbody _rb;
    public PathCreator pathCreator;
    float distanceTravelled;
    public bool wayPointTrigger = false;
    float disableTime;
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
}
