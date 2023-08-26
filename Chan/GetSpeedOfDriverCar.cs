using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpeedOfDriverCar : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (DriverCar.respawnTrigger) { rb.velocity = new Vector3(0, 0, 0); }
    }
}
