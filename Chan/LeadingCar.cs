using PathCreation;
using Unity.Mathematics;
using UnityEngine;
using VolvoCars.Data;

public class LeadingCar : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    public Transform TargetCar;
    public GameObject Obstacle_1, Obstacle_2;
    public GameObject LeadingCar_1, LeadingCar_2, LC_1_RearLight, LC_2_RearLight;
    public GameObject FC_Group_1, FC_Group_2;
    public GameObject TaskEndPoint_1, TaskEndPoint_2;
    GameObject LeadingCarVelocity;
    Vector3 TargetCarVelocity;
    Vector3 StartPos_LC_1, StartPos_LC_2;
    Vector3 LeadingCarPosition;
    quaternion StartRot_LC_1, StartRot_LC_2;
    float OvertakeTimer;
    float StoppingDistance;
    public bool TaskStart;
    //public bool WayPointTrigger;
    //float DistanceTravelled;

    public int LC_Direction;
    public int TurnOn_LC_FC;
    public bool RespawnTrigger;
    public bool StartScenario_LaneChangeThenStop, StartScenario_LaneChangeWithLowSpeed, StartScenario_Obstacle, StartScenario_None, StartTrial;
    public float DrivingDirection;

    public int TaskStartTime;
    public int LC_StoppingTime;

    private void Start()
    {
        GetStartPos();
    }

    private void FixedUpdate()
    {
        if (DC.DrivingPhase)
        {
            TurnOnCars();
            SetCarByDirection();

            TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

            GetDistance();

            if (TaskStart)
                SelectScenario();

            if (StartScenario_LaneChangeThenStop)
                LaneChangeThenStop();

            if (StartScenario_LaneChangeWithLowSpeed)
                LaneChangeWithLowSpeed();

            if (StartScenario_Obstacle)
                LaneChangeWithObstacle();

            if (StartScenario_None)
                None();
        }

        if (RespawnTrigger)
            Respawn();
    }

    void SelectScenario()
    {
        TaskStartTime = DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount];

        if(TaskStartTime != 0)
        {
            switch (DC.TaskScenario[DC.CMSchangeCount - 1, DC.taskCount])
            {
                case -1:
                    StartTrial = true;
                    break;
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
        else if (TaskStartTime == 0)
            StartScenario_None = true;


        TaskStart = false;
    }

    void TurnOnCars()
    {
        if (TurnOn_LC_FC == 1)
        {
            LeadingCar_1.SetActive(true);
            FC_Group_1.SetActive(true);
            TaskEndPoint_1.SetActive(true);
            LC_1_RearLight.SetActive(false);
            TurnOn_LC_FC = 0;
        }
        else if (TurnOn_LC_FC == 2)
        {
            LeadingCar_2.SetActive(true);
            FC_Group_2.SetActive(true);
            TaskEndPoint_2.SetActive(true);
            LC_2_RearLight.SetActive(false);
            TurnOn_LC_FC = 0;
        }
    }

    void SetCarByDirection()
    {
        if (LC_Direction == 1)
        {
            DrivingDirection = 1;
            LeadingCarPosition = LeadingCar_1.transform.position;
            LeadingCarVelocity = LeadingCar_1;
        }
        else if (LC_Direction == 2)
        {
            DrivingDirection = -1;
            LeadingCarPosition = LeadingCar_2.transform.position;
            LeadingCarVelocity = LeadingCar_2;
        }
    }

    void GetStartPos()
    {
        StartPos_LC_1 = LeadingCar_1.transform.position;
        StartPos_LC_2 = LeadingCar_2.transform.position;
        StartRot_LC_1 = LeadingCar_1.transform.rotation;
        StartRot_LC_2 = LeadingCar_2.transform.rotation;
        LeadingCarVelocity = LeadingCar_1;
    }

    void GetDistance()
    {
        // increase the value ?
        // now it takes about 2 seconds to the crash
        // 3~6 seconds might be enough for checking the side mirrors
        if (TargetCarVelocity.z > 27 || TargetCarVelocity.z < -27)
            StoppingDistance = 80; 
        else if (TargetCarVelocity.z > 25 || TargetCarVelocity.z < -25)
            StoppingDistance = 75;
        else if (TargetCarVelocity.z > 22 || TargetCarVelocity.z < -22)
            StoppingDistance = 66;
        else if (TargetCarVelocity.z > 19 || TargetCarVelocity.z < -19)
            StoppingDistance = 57;
    }

    void LaneChangeThenStop()
    {
        Debug.Log("Stop");
        OvertakeTimer += Time.deltaTime;

        if (OvertakeTimer <= 8)
        {
            SetDistance(1);

            if (OvertakeTimer >= 4)
            {
                if (DrivingDirection == -1)
                    TargetCarVelocity.x = -1.5f;
                if (DrivingDirection == 1)
                    TargetCarVelocity.x = 1.5f;
            }
        }

        if (OvertakeTimer > 8)
            TargetCarVelocity.x = 0;

        if (OvertakeTimer > 8 + TaskStartTime)
        {
            LC_StoppingTime = 1;
            TargetCarVelocity.z = 0;
            LC_1_RearLight.SetActive(true);
            LC_2_RearLight.SetActive(true);
        }

        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    void LaneChangeWithLowSpeed()
    {
        Debug.Log("LowSpeed");
        OvertakeTimer += Time.deltaTime;

        if (OvertakeTimer <= 10 + TaskStartTime)
            SetDistance(2);

        if (OvertakeTimer >= 10 + TaskStartTime && OvertakeTimer <= 13 + TaskStartTime)
        {            
            TargetCarVelocity.z *= 0.5f;

            if (DrivingDirection == -1)
                TargetCarVelocity.x = -2f;
            if (DrivingDirection == 1)
                TargetCarVelocity.x = 2f;

            if (OvertakeTimer > 10.5f + TaskStartTime)
                LC_StoppingTime = 1;

            LC_1_RearLight.SetActive(true);
            LC_2_RearLight.SetActive(true);
        }

        if (OvertakeTimer > 13 + TaskStartTime)
        {

            TargetCarVelocity.z = 0;
            TargetCarVelocity.x = 0;
        }

        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    void LaneChangeWithObstacle()
    {
        Debug.Log("Obstacle");
        OvertakeTimer += Time.deltaTime;

        if (OvertakeTimer <= 8)
        {
            SetDistance(2);

            if (OvertakeTimer >= 4)
            {
                if (DrivingDirection == -1)
                    TargetCarVelocity.x = -1.5f;
                if (DrivingDirection == 1)
                    TargetCarVelocity.x = 1.5f;
            }
        }

        if (OvertakeTimer > 8 && OvertakeTimer <= 8 + TaskStartTime)
        {
            TargetCarVelocity.x = 0;
            if (DrivingDirection == 1)
                Obstacle_1.transform.position = new Vector3(LeadingCarPosition.x, 0, TargetCar.transform.position.z + StoppingDistance * DrivingDirection);

            else if (DrivingDirection == -1)
                Obstacle_2.transform.position = new Vector3(LeadingCarPosition.x, 0, TargetCar.transform.position.z + StoppingDistance * DrivingDirection);
        }

        if (OvertakeTimer > 8 + TaskStartTime && OvertakeTimer <= 12 + TaskStartTime)
        {
            TargetCarVelocity.z *= 1f;

            if (DrivingDirection == -1)
                TargetCarVelocity.x = -1.5f;
            if (DrivingDirection == 1)
                TargetCarVelocity.x = 1.5f;

            if (OvertakeTimer <= 10 + TaskStartTime)
            {
                LC_StoppingTime = 1;

                if (DrivingDirection == 1)
                    Obstacle_1.SetActive(true);
                else if(DrivingDirection == -1)
                    Obstacle_2.SetActive(true);
            }
        }

        if (OvertakeTimer > 11 + TaskStartTime)
        {
            TargetCarVelocity.z *= 1f;
            TargetCarVelocity.x = 0;
        }

        if(OvertakeTimer > 23)
        {
            TargetCarVelocity.z = 0;
            LC_1_RearLight.SetActive(true);
            LC_2_RearLight.SetActive(true);
        }

        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    void None()
    {
        Debug.Log("None");
        OvertakeTimer += Time.deltaTime;

        if (OvertakeTimer <= 8)
        {
            SetDistance(1);

            if (OvertakeTimer >= 4)
            {
                if (DrivingDirection == -1)
                    TargetCarVelocity.x = -1.5f;
                if (DrivingDirection == 1)
                    TargetCarVelocity.x = 1.5f;
            }
        }

        if (OvertakeTimer > 8 && OvertakeTimer < 20)
        {
            TargetCarVelocity.z *= 1f;
            TargetCarVelocity.x = 0;
        }

        if (OvertakeTimer >= 20 && OvertakeTimer <= 24)
        {
            TargetCarVelocity.z += 1f;

            if (DrivingDirection == -1)
                TargetCarVelocity.x = 1.5f;
            if (DrivingDirection == 1)
                TargetCarVelocity.x = -1.5f;
        }

        if (OvertakeTimer > 23)
        {
            TargetCarVelocity.z = 0;
            TargetCarVelocity.x = 0;
            LC_1_RearLight.SetActive(true);
            LC_2_RearLight.SetActive(true);
        }

        LeadingCarVelocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    void SetDistance(float Divide)
    {
        float DistanceBetween_DC_LC = LeadingCarPosition.z - TargetCar.transform.position.z;

        if (DistanceBetween_DC_LC * DrivingDirection < StoppingDistance / Divide)
            TargetCarVelocity.z *= 1.5f;
        else
            TargetCarVelocity.z *= 0.9f;
    }

    void Respawn()
    {
        TargetCarVelocity = Vector3.zero;
        LeadingCarVelocity.GetComponent <Rigidbody>().velocity = TargetCarVelocity ;
        LC_Direction = 0;
        OvertakeTimer = 0;
        //DistanceTravelled = 0;
        LC_StoppingTime = 0;
        //WayPointTrigger = false;
        TaskStart = false;
        StartScenario_LaneChangeThenStop = false;
        StartScenario_LaneChangeWithLowSpeed = false;
        StartScenario_Obstacle = false;
        StartScenario_None = false;
        StartTrial = false;
        LeadingCar_1.transform.position = StartPos_LC_1;
        LeadingCar_2.transform.position = StartPos_LC_2;
        LeadingCar_1.transform.rotation = StartRot_LC_1;
        LeadingCar_2.transform.rotation = StartRot_LC_2;
        LeadingCar_1.SetActive(false);
        LeadingCar_2.SetActive(false);
        TaskEndPoint_1.SetActive(false);
        TaskEndPoint_2.SetActive(false);
        Obstacle_1.SetActive(false);
        Obstacle_2.SetActive(false);
        RespawnTrigger = false;
    }


    //private void WayPointDriving()
    //{
    //    if (DrivingDirection == 1)
    //    {
    //        DistanceTravelled += Time.deltaTime * 10;
    //        LeadingCar_1.transform.position = PathCreator_1.path.GetPointAtDistance(DistanceTravelled);
    //        LeadingCar_1.transform.rotation = PathCreator_1.path.GetRotationAtDistance(DistanceTravelled);
    //        DisableTime += Time.deltaTime;

    //        if (DisableTime > 10)
    //            RespawnTrigger = true;
    //    }

    //    if (DrivingDirection == -1)
    //    {
    //        DistanceTravelled += Time.deltaTime * 10;
    //        LeadingCar_2.transform.position = PathCreator_2.path.GetPointAtDistance(DistanceTravelled);
    //        LeadingCar_2.transform.rotation = PathCreator_2.path.GetRotationAtDistance(DistanceTravelled);
    //        DisableTime += Time.deltaTime;

    //        if (DisableTime > 10)
    //            RespawnTrigger = true;
    //    }
    //}
}
