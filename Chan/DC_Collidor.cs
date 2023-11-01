using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DC_Collidor : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] TrialManager TM;
    [SerializeField] CSV_Save CSV;
    [SerializeField] FollowingCar FC;
    public GameObject QuestionnaireStartNotice, TaskFailureNotice, DoNotMoveNotice, TaskStartNotice;
    public float alpha = 0;
    public bool Activate_Fade, FadingEvent;
    private Material _mat;
    public bool DrivingIn2ndLane;
    float TimerForTaskCountThresholding, FadingTimer, TimerForTriggerThresholding;
    int TaskCountNum = 6;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void FixedUpdate()
    {
        if (Activate_Fade)
            FadeInOut();

        if (TimerForTriggerThresholding < 3)
            TimerForTriggerThresholding += Time.deltaTime;

        if (TimerForTaskCountThresholding < 3)
            TimerForTaskCountThresholding += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LC") || other.gameObject.CompareTag("FC"))
        {
            if (TimerForTriggerThresholding > 2)
            {
                Debug.Log("A activated" + Time.time);
                DC.NumOfCollision = 1;
                RespawnOtherCars();
                CheckTaskCount();
                TimerForTriggerThresholding = 0;
            }
        }

        if (other.gameObject.CompareTag("OutOfRoad"))
        {
            if (TimerForTriggerThresholding > 2)
            {
                Debug.Log("B activated" + Time.time);
                DC.NumOfCollision = 2;
                RespawnOtherCars();
                CheckTaskCount();
                TimerForTriggerThresholding = 0;
            }
        }

        if (other.gameObject.CompareTag("TaskEndPoint"))
        {
            if (TimerForTriggerThresholding > 2)
            {
                Debug.Log("C activated" + Time.time);
                RespawnOtherCars();
                CheckTaskCount();
                TimerForTriggerThresholding = 0;
            }
        }

        if (other.gameObject.CompareTag("TaskStartPoint_1"))
        {
            if (TimerForTriggerThresholding > 2)
            {
                IncreaseTaskCount();
                CheckScenario(1);
                TimerForTriggerThresholding = 0;
            }
        }

        if (other.gameObject.CompareTag("TaskStartPoint_2"))
        {
            if (TimerForTriggerThresholding > 2)
            {
                IncreaseTaskCount();
                CheckScenario(2);
                TimerForTriggerThresholding = 0;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!DrivingIn2ndLane && other.tag == "LaneChangeTimeCalculator" && LC.LC_StoppingTime == 1)
            DrivingIn2ndLane = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (DrivingIn2ndLane && other.tag == ("LaneChangeTimeCalculator"))
            DC.LaneChangeComplete = 1;
    }

    void RespawnOtherCars()
    {
        CSV.DataLoggingEnd = true;
        FC.RespawnTrigger = true;
        LC.RespawnTrigger = true;
        TM.RespawnTrigger = true;
    }
    void CheckTaskCount()
    {
        if ((DC.taskCount != TaskCountNum) && (DC.taskCount != 0))
        {
            Debug.Log("Entered");
            if (DC.NumOfCollision > 0)
            {
                DC.RespawnTrigger = true;
                TaskFailureNotice.SetActive(true);
            }
        }
        else if (DC.taskCount == TaskCountNum)
        {
            DC.RespawnTrigger = true;
            QuestionnaireStartNotice.SetActive(true);
            DC.taskCount = 0;
            DC.FirstTaskCountThreshold = false;
        }
        else if (DC.taskCount == 0)
        {
            DC.RespawnTrigger = true;
            TaskStartNotice.SetActive(true);
        }

        CSV.AddEndloggingTimer = true;
    }
    void IncreaseTaskCount()
    {
        if (TimerForTaskCountThresholding > 2)
        {
            if (DC.FirstTaskCountThreshold)
                DC.taskCount++;
            else
                DC.FirstTaskCountThreshold = true;

            TimerForTaskCountThresholding = 0;
        }
    }
    void CheckScenario(int Direction)
    {
        CSV.Create_CSV_File = true;
        CSV.DataLoggingStart = true;
        DC.LaneChangeComplete = 0;

        if (DC.TaskScenario[DC.CMSchangeCount - 1, DC.taskCount] > 0)
        {
            LC.TurnOn_LC_FC = Direction;
            LC.LC_Direction = Direction;

        }
        else if (DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount] < 0)
        {
            TM.TurnOnTrialCars = Direction;
            TM.MoveTrialCar = Direction;
        }

        LC.TaskStart = true;
    }

    void FadeInOut()
    {
        FadingTimer += Time.deltaTime;
        //DoNotMoveNotice.SetActive(true);

        if (FadingEvent && alpha <= 1.01f)
            alpha += .01f;
        else if (!FadingEvent && alpha >= -0.1f)
            alpha -= .01f;

        Color nNew = new Color(0, 0, 0, alpha);
        _mat.SetColor("_BaseColor", nNew);

        if (FadingTimer > 3)
        {
            //DoNotMoveNotice.SetActive(false);
            FadingTimer = 0;
            Activate_Fade = false;
        }
    }
}