using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadingCarForObstacleEvent : MonoBehaviour
{
    public GameObject Obstacle;
    public Transform targetCar;

    private Rigidbody rb;
    public Vector3 Velocity2;
    public float Overtake;
    int StoppingDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StoppingDistance = 30;
    }

    private void FixedUpdate()
    {
        Velocity2 = targetCar.GetComponent<Rigidbody>().velocity;
        Velocity2.x = 0;
        Overtake += Time.deltaTime;

        // overtake
        if (Overtake <= 5) Velocity2.z *= 1.5f;

        // slows down and change the lane to the 2nd
        if (Overtake >= 5 && Overtake <= 8)
        {
            // slow down
            if (Mathf.Abs(targetCar.transform.position.z - gameObject.transform.position.z) > StoppingDistance) Velocity2.z *= 0.3f;
            else Velocity2.z *= 2f;

            // lane changing
            Velocity2.x = 2f;
        }

        // stop
        if (Overtake >= 10 && Overtake <= 13)
        {
            Velocity2.z *= 1.3f;
            Velocity2.x = 2f;
            if (Overtake <= 10.5)
            {
                Obstacle.transform.position = targetCar.transform.position + new Vector3(0, 1, 60);
                Obstacle.SetActive(true);
            }
        }

        if (Overtake >= 13) Velocity2.z *= 3f;
        
        // disable
        if (Overtake >= 20)
        {
            Obstacle.SetActive(false);
            gameObject.SetActive(false);
        }
        // apply the velocity to the car
        gameObject.GetComponent<Rigidbody>().velocity = Velocity2;
    }
}
