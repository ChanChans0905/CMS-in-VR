using PathCreation;
using Unity.Mathematics;
using UnityEngine;
using VolvoCars.Data;

public class LaneChangeCar : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] FadeInOut FadeInOut;
    public Transform TargetCar;
    public GameObject Obstacle;
    public GameObject LeadingCar_1, LeadingCar_2;
    public PathCreator PathCreator_1, PathCreator_2;
    GameObject LeadingCarVelocity;
    Vector3 TargetCarVelocity;
    Vector3 StartPos_LC_1, StartPos_LC_2;
    Vector3 LeadingCarPosition;
    quaternion StartRot_LC_1, StartRot_LC_2;
    float OvertakeTimer;
    float StoppingDistance;
    public bool TaskStart;
    public bool WayPointTrigger;
    float DistanceTravelled;

    float DisableTime;
    bool RespawnTrigger;
    bool StartScenario_LaneChangeThenStop, StartScenario_LaneChangeWithLowSpeed, StartScenario_Obstacle, StartScenario_None;
    public float DrivingDirection;

    public int TaskStartTime;
    public int LC_StoppingTime;

    private void Start()
    {
        // Set distance between DC and LC
        StoppingDistance = 60;

        // Get original position and rotation of LC_1 and LC_2
        StartPos_LC_1 = LeadingCar_1.transform.localPosition;
        StartPos_LC_2 = LeadingCar_2.transform.localPosition;
        StartRot_LC_1 = LeadingCar_1.transform.localRotation;
        StartRot_LC_2 = LeadingCar_2.transform.localRotation;
    }

    private void FixedUpdate()
    {
        if (FadeInOut.GetLeadingCarDirection)
        {
            DrivingDirection = 1;
            LeadingCarPosition = LeadingCar_1.transform.position;
            LeadingCarVelocity = LeadingCar_1;
            Debug.Log(LeadingCarVelocity);
        }
        else if (!FadeInOut.GetLeadingCarDirection)
        {
            Debug.Log(LeadingCarVelocity);
            DrivingDirection = -1;
            LeadingCarPosition = LeadingCar_2.transform.position;
            LeadingCarVelocity = LeadingCar_2;
        }

        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        if (TargetCarVelocity.z > 27)
            StoppingDistance = 80;
        else if (TargetCarVelocity.z > 25)
            StoppingDistance = 55;
        else if (TargetCarVelocity.z > 22)
            StoppingDistance = 40;
        else if (TargetCarVelocity.z > 19)
            StoppingDistance = 30;

        if (TaskStart)
        {
            TaskStartTime = DriverCar.LaneChangeTime[DriverCar.taskCount];

            if (TaskStartTime != 0)
            {
                switch (DriverCar.TaskScenario[DriverCar.taskCount])
                {
                    case 1:
                        StartScenario_LaneChangeThenStop = true;
                        break;
                    case 2:
                        StartScenario_LaneChangeWithLowSpeed = true;
                        break;
                    case 3:
                        StartScenario_Obstacle = true;
                        break;
                }
            }
            else
                StartScenario_None = true;

            TaskStart = false;
        }
        else if (!WayPointTrigger)
            LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;

        if (StartScenario_LaneChangeThenStop)
            LaneChangeThenStop();

        if (StartScenario_LaneChangeWithLowSpeed)
            LaneChangeWithLowSpeed();

        if (StartScenario_Obstacle)
            LaneChangeWithObstacle();

        if (StartScenario_None)
            None();

        if (WayPointTrigger)
            WayPointDriving();

        if (DriverCar.respawnTrigger)
            RespawnTrigger = true;

        if (RespawnTrigger)
            Respawn();
    }

    private void LaneChangeThenStop()
    {
        Debug.Log("Stop");
        OvertakeTimer += Time.deltaTime;

        // overtake, slows down and change the lane to the 2nd
        if (OvertakeTimer <= 8)
        {
            // slow down
            if (TargetCar.transform.position.z - LeadingCarPosition.z < -1 * StoppingDistance)
                TargetCarVelocity.z *= 0.9f;
            else
                TargetCarVelocity.z *= 1.5f;
            // lane changing
            if (OvertakeTimer >= 4)
                TargetCarVelocity.x = 1.5f;
        }

        // delete lane changing velocity
        if (OvertakeTimer > 8)
            TargetCarVelocity.x = 0;

        // stop
        if (OvertakeTimer > 8 + TaskStartTime)
        {
            LC_StoppingTime = 1;
            TargetCarVelocity.z = 0;
        }

        // disable
        if (OvertakeTimer >= 20)
            RespawnTrigger = true;

        // apply the velocity to the car
        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * DrivingDirection;
    }

    private void LaneChangeWithLowSpeed()
    {
        Debug.Log("LowSpeed");
        OvertakeTimer += Time.deltaTime;

        // overtake
        if (OvertakeTimer <= 10 + TaskStartTime)
        {
            float DistanceBetween_DC_LC = TargetCar.transform.position.z - LeadingCarPosition.z;

            if (LeadingCarPosition.x < 1000)
            {
                if (DistanceBetween_DC_LC > -38)
                    TargetCarVelocity.z *= 1.2f;
                else
                    TargetCarVelocity.z *= 0.9f;
            }
            else
            {
                if (DistanceBetween_DC_LC < -38)
                    TargetCarVelocity.z *= 1.2f;
                else
                    TargetCarVelocity.z *= 0.9f;
            }

            //if (Mathf.Abs(TargetCar.transform.position.z - LeadingCarPosition.z) < 40) 
            //    TargetCarVelocity.z *= 0.3f;
            //else 
            //    TargetCarVelocity.z *= 2f;

            //Debug.Log(Mathf.Abs(TargetCar.transform.position.z - LeadingCarPosition.z));
        }

        // slows down and change the lane to the 2nd
        if (OvertakeTimer > 10 + TaskStartTime && OvertakeTimer <= 12 + TaskStartTime)
        {
            TargetCarVelocity.z *= 0.4f;
            TargetCarVelocity.x = 3f;
        }

        if (OvertakeTimer > 12 + TaskStartTime)
        {
            LC_StoppingTime = 1;
            TargetCarVelocity.z = 0;
            TargetCarVelocity.x = 0;
        }

        if (OvertakeTimer >= 20)
            RespawnTrigger = true;

        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * DrivingDirection;
    }

    private void LaneChangeWithObstacle()
    {
        Debug.Log("Obstacle");
        OvertakeTimer += Time.deltaTime;

        // over slows down and change the lane to the 2nd
        if (OvertakeTimer <= 8)
        {
            // slow down
            if (Mathf.Abs(TargetCar.transform.position.z - LeadingCarPosition.z) > StoppingDistance)
                TargetCarVelocity.z *= 0.6f;
            else
                TargetCarVelocity.z *= 1.4f;
            // lane changing
            if (OvertakeTimer >= 4)
                TargetCarVelocity.x = 1.5f;
        }

        if (OvertakeTimer > 8 && OvertakeTimer <= 8 + TaskStartTime)
        {
            TargetCarVelocity.x = 0;
            Obstacle.transform.position = LeadingCarPosition + new Vector3(0, 1, 60);
        }

        // stop
        if (OvertakeTimer > 8 + TaskStartTime && OvertakeTimer <= 11 + TaskStartTime)
        {
            TargetCarVelocity.z *= 1.3f;
            TargetCarVelocity.x = 2f;
            if (OvertakeTimer <= 10 + TaskStartTime)
            {

                Obstacle.SetActive(true);
            }
        }

        if (OvertakeTimer > 11 + TaskStartTime)
        {
            LC_StoppingTime = 1;
            TargetCarVelocity.x = 0;
        }
            

        // disable
        if (OvertakeTimer >= 20)
            RespawnTrigger = true;

        // apply the velocity to the car
        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * DrivingDirection;
    }

    private void None()
    {
        Debug.Log("None");
        OvertakeTimer += Time.deltaTime;

        // overtake
        if (OvertakeTimer <= 5)
            TargetCarVelocity.z *= 1.5f;

        // slows down and change the lane to the 2nd
        if (OvertakeTimer >= 5 && OvertakeTimer <= 8)
        {
            // slow down
            if (Mathf.Abs(TargetCar.transform.position.z - LeadingCarPosition.z) > StoppingDistance)
                TargetCarVelocity.z *= 0.3f;
            else
                TargetCarVelocity.z *= 2f;

            // lane changing
            TargetCarVelocity.x = 2f;
        }

        if (OvertakeTimer > 8)
        {
            TargetCarVelocity.z *= 1f;
            TargetCarVelocity.x = 0;
        }

        if (WayPointTrigger)
            TargetCarVelocity.z = 0;

        // apply the velocity to the car
        gameObject.GetComponent<Rigidbody>().velocity = TargetCarVelocity * DrivingDirection;
    }

    private void WayPointDriving()
    {
        if (DrivingDirection == 1)
        {
            DistanceTravelled += Time.deltaTime * 10;
            LeadingCar_1.transform.position = PathCreator_1.path.GetPointAtDistance(DistanceTravelled);
            LeadingCar_1.transform.rotation = PathCreator_1.path.GetRotationAtDistance(DistanceTravelled);
            DisableTime += Time.deltaTime;

            if (DisableTime > 10)
                RespawnTrigger = true;
        }

        if (DrivingDirection == -1)
        {
            DistanceTravelled += Time.deltaTime * 10;
            LeadingCar_2.transform.position = PathCreator_2.path.GetPointAtDistance(DistanceTravelled);
            LeadingCar_2.transform.rotation = PathCreator_2.path.GetRotationAtDistance(DistanceTravelled);
            DisableTime += Time.deltaTime;

            if (DisableTime > 10)
                RespawnTrigger = true;
        }
    }

    private void Respawn()
    {
        OvertakeTimer = 0;
        DistanceTravelled = 0;
        DisableTime = 0;
        LC_StoppingTime = 0;
        WayPointTrigger = false;
        TaskStart = false;
        RespawnTrigger = false;
        StartScenario_LaneChangeThenStop = false;
        StartScenario_LaneChangeWithLowSpeed = false;
        StartScenario_Obstacle = false;
        StartScenario_None = false;
        LeadingCar_1.transform.localPosition = StartPos_LC_1;
        LeadingCar_2.transform.localPosition = StartPos_LC_2;
        LeadingCar_1.transform.localRotation = StartRot_LC_1;
        LeadingCar_2.transform.localRotation = StartRot_LC_2;
        LeadingCar_1.SetActive(false);
        LeadingCar_2.SetActive(false);
        RespawnTrigger = false;
    }
}
