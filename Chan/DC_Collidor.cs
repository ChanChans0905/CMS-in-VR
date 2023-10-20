using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DC_Collidor : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] TrialManager TM;
    [SerializeField] CSV_Save CSV;
    [SerializeField] FollowingCar FC;
    public GameObject QuestionnaireStartNotice, TaskFailureNotice, DoNotMoveNotice;
    public float alpha = 0;
    public bool Activate_Fade, FadingEvent;
    private Material _mat;
    public bool DrivingIn2ndLane;
    float TimerForTaskCountThresholding, FadingTimer;
    int TaskCountNum = 7;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void FixedUpdate()
    {
        if (Activate_Fade)
            FadeInOut();

        if (TimerForTaskCountThresholding < 3)
            TimerForTaskCountThresholding += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AR_Signal_L") || other.gameObject.CompareTag("AR_Signal_R") || other.gameObject.CompareTag("FC"))
        {
            DC.NumOfCollision = 1;
            DC.RespawnTrigger = true;
            RespawnOtherCars();
            CheckTaskCount();
        }

        if (other.gameObject.CompareTag("OutOfRoad"))
        {
            DC.NumOfCollision = 2;
            DC.RespawnTrigger = true;
            RespawnOtherCars();
            CheckTaskCount();
        }

        if (other.gameObject.CompareTag("TaskEndPoint"))
        {
            RespawnOtherCars();

            if (DC.taskCount == TaskCountNum)
            {
                DC.RespawnTrigger = true;
                QuestionnaireStartNotice.SetActive(true);
                DC.taskCount = 0;
                DC.FirstTaskCountThreshold = false;
            }
        }

        if (other.gameObject.CompareTag("TaskStartPoint_1"))
        {
            IncreaseTaskCount();
            CheckScenario(1);
        }

        if (other.gameObject.CompareTag("TaskStartPoint_2"))
        {
            IncreaseTaskCount();
            CheckScenario(2);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!DrivingIn2ndLane && other.tag == "LaneChangeTimeCalculator")
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
        if (DC.taskCount == TaskCountNum)
        {
            QuestionnaireStartNotice.SetActive(true);
            DC.taskCount = 0;
            DC.FirstTaskCountThreshold = false;
        }
        else if (DC.taskCount < TaskCountNum)
            TaskFailureNotice.SetActive(true);
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
        if (DC.TaskScenario[DC.CMSchangeCount - 1, DC.taskCount] > 0)
        {
            CSV.Create_CSV_File = true;
            CSV.DataLoggingStart = true;
            DC.LaneChangeComplete = 0;

            LC.TurnOn_LC_FC = Direction;
            LC.LC_Direction = Direction;
            LC.TaskStart = true;
        }
        else if (DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount] < 0)
        {
            TM.TurnOnTrialCars = Direction;
            TM.MoveTrialCar = Direction;
        }
    }

    void FadeInOut()
    {
        FadingTimer += Time.deltaTime;
        DoNotMoveNotice.SetActive(true);

        if (FadingEvent && alpha <= 1.05f)
            alpha += .05f;
        else if (!FadingEvent && alpha >= -0.5)
            alpha -= .05f;

        Color nNew = new Color(0, 0, 0, alpha);
        _mat.SetColor("_BaseColor", nNew);

        if (FadingTimer > 3)
        {
            DoNotMoveNotice.SetActive(false);
            FadingTimer = 0;
            Activate_Fade = false;
        }
    }
}