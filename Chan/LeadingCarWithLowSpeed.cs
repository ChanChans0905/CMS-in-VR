//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LeadingCarWithLowSpeed : MonoBehaviour
//{
//    public Transform targetCar;

//    private Rigidbody rb;
//    public Vector3 Velocity2;
//    public float Overtake;
//    int StoppingDistance;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        StoppingDistance = 60;
//    }

//    private void FixedUpdate()
//    {
//        Velocity2 = targetCar.GetComponent<Rigidbody>().velocity;
//        Velocity2.x = 0;
//        Overtake += Time.deltaTime;

//        // overtake
//        if (Overtake <= 5)
//        {
//            if (Mathf.Abs(targetCar.transform.position.z - gameObject.transform.position.z) > 40) Velocity2.z *= 0.3f;
//            else Velocity2.z *= 2f;
//        }


//        // slows down and change the lane to the 2nd
//        if (Overtake >= 5 && Overtake <= 8)
//        {
//            Velocity2.z *= 0.6f;
//            Velocity2.x = 3f;
//        }

//        // stop
//        if (Overtake >= 7) Velocity2.z = 0;

//        // disable
//        if (Overtake >= 20) gameObject.SetActive(false);

//        // apply the velocity to the car
//        gameObject.GetComponent<Rigidbody>().velocity = Velocity2;
//    }
//}
