using PathCreation;
using UnityEngine;
using VolvoCars.Data;

public class LaneChangeCar : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    public Transform TargetCar;
    public GameObject Obstacle;
    Vector3 TargetCarVelocity;
    Vector3 LeadingCarVelocity;
    float OvertakeTimer;
    int StoppingDistance;
    Vector3 StartPos;
    public bool TaskStart;
    bool WayPointTrigger;
    float DistanceTravelled;
    public PathCreator PathCreator;
    float DisableTime;
    bool RespawnTrigger;
    bool StartScenario_LaneChangeThenStop, StartScenario_LaneChangeWithLowSpeed, StartScenario_Obstacle;
    public bool BeforeTaskStart;

    private void Start()
    {
        StoppingDistance = 60;
        StartPos = transform.position;
    }

    private void FixedUpdate()
    {
        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        if (TaskStart)
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
            TaskStart = false;
            BeforeTaskStart = false;
        }
        else
            BeforeTaskStart = true;
        
        if(BeforeTaskStart)
            gameObject.GetComponent<Rigidbody>().velocity = TargetCarVelocity;

        if (StartScenario_LaneChangeThenStop)
            LaneChangeThenStop(DriverCar.taskCount);

        if (StartScenario_LaneChangeWithLowSpeed)
            LaneChangeWithLowSpeed(DriverCar.taskCount);

        if(StartScenario_Obstacle)
            LaneChangeWithObstacle(DriverCar.taskCount);

        if (WayPointTrigger)
            WayPointDriving();

        if (DriverCar.respawnTrigger)
            Respawn();
    }

    private void LaneChangeThenStop(int taskCount)
    {
        Debug.Log("Stop");
        OvertakeTimer += Time.deltaTime;

        int TaskStartTime = DriverCar.LaneChangeTime[taskCount];

        // overtake
        if (OvertakeTimer <= 5)
            TargetCarVelocity.z *= 2f;

        // slows down and change the lane to the 2nd
        if (OvertakeTimer >= 5 && OvertakeTimer <= 8)
        {
            Debug.Log(OvertakeTimer);
            // slow down
            if (Mathf.Abs(TargetCar.transform.position.z - gameObject.transform.position.z) > StoppingDistance) TargetCarVelocity.z *= 0.3f;
            else TargetCarVelocity.z *= 2f;

            // lane changing
            TargetCarVelocity.x = 2f;
        }

        // delete lane changing velocity
        if (OvertakeTimer > 8 && OvertakeTimer <= 8 + TaskStartTime)
            TargetCarVelocity.x = 0;

        // stop
        if (OvertakeTimer > 8 + TaskStartTime && TaskStartTime != 0)
        {
            TargetCarVelocity.z = 0;
        }
            
        

        // disable
        if (OvertakeTimer >= 20 && TaskStartTime != 0)
            RespawnTrigger = true;

        // apply the velocity to the car
        gameObject.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    private void LaneChangeWithLowSpeed(int taskCount)
    {
        Debug.Log("LowSpeed");
        OvertakeTimer += Time.deltaTime;

        int TaskStartTime = DriverCar.LaneChangeTime[taskCount];

        // overtake
        if (OvertakeTimer - TaskStartTime >= 5 && OvertakeTimer - TaskStartTime <= 10 && TaskStartTime != 0)
            TargetCarVelocity.z *= 2f;

        // slows down and change the lane to the 2nd
        if (OvertakeTimer - TaskStartTime > 10 && OvertakeTimer - TaskStartTime <= 13 && TaskStartTime != 0)
        {
            TargetCarVelocity.z *= 0.6f;
            TargetCarVelocity.x = 3f;
        }

        if (OvertakeTimer -TaskStartTime > 13)
            TargetCarVelocity.z = 0;

        if (OvertakeTimer >= 20 && TaskStartTime != 0)
            RespawnTrigger = true;

        gameObject.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    private void LaneChangeWithObstacle(int taskCount)
    {
        Debug.Log("Obstacle");
        OvertakeTimer += Time.deltaTime;

        int TaskStartTime = DriverCar.LaneChangeTime[taskCount];

        // overtake
        if (OvertakeTimer <= 5)
            TargetCarVelocity.z *= 1.5f;

        // slows down and change the lane to the 2nd
        if (OvertakeTimer >= 5 && OvertakeTimer <= 8)
        {
            // slow down
            if (Mathf.Abs(TargetCar.transform.position.z - gameObject.transform.position.z) > StoppingDistance) TargetCarVelocity.z *= 0.3f;
            else TargetCarVelocity.z *= 2f;

            // lane changing
            TargetCarVelocity.x = 2f;
        }

        // stop
        if (OvertakeTimer > 8 + TaskStartTime && OvertakeTimer <= 11 + TaskStartTime && TaskStartTime != 0)
        {
            TargetCarVelocity.z *= 1.3f;
            TargetCarVelocity.x = 2f;
            if (OvertakeTimer <= 10 + TaskStartTime)
            {
                Obstacle.transform.position = gameObject.transform.position + new Vector3(0, 1, 60);
                Obstacle.SetActive(true);
            }
        }

        // disable
        if (OvertakeTimer >= 20 && TaskStartTime != 0)
            RespawnTrigger = true;

        // apply the velocity to the car
        gameObject.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    private void WayPointDriving()
    {
        DistanceTravelled += Time.deltaTime * 20;
        transform.position = PathCreator.path.GetPointAtDistance(DistanceTravelled);
        transform.rotation = PathCreator.path.GetRotationAtDistance(DistanceTravelled);
        DisableTime += Time.deltaTime;

        if (DisableTime > 20)
            RespawnTrigger = true;
    }

    private void Respawn()
    {
        OvertakeTimer = 0;
        DistanceTravelled = 0;
        DisableTime = 0;
        WayPointTrigger = false;
        TaskStart = false;
        RespawnTrigger = false;
        BeforeTaskStart = false;
        StartScenario_LaneChangeThenStop = false;
        StartScenario_LaneChangeWithLowSpeed = false;
        StartScenario_Obstacle = false;
        gameObject.transform.position = StartPos;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
        Obstacle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TaskStartPoint1"))
            TaskStart = true;

        if (other.gameObject.CompareTag("WayPoint"))
        {
            WayPointTrigger = true;
            TaskStart = false;
        }
    }
}
