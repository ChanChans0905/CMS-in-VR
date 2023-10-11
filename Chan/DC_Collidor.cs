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
    public GameObject QuestionnaireStartNotice, TaskFailureNotice;
    public float alpha = 0;
    public bool Activate_Fade, FadingEvent;
    private Material _mat;
    public bool DrivingIn2ndLane;
    float TaskCountThreshold, FadingTimer;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void FixedUpdate()
    {
        if (Activate_Fade)
        {
            FadingTimer += Time.deltaTime;

            if (FadingEvent && alpha <= 1)
                alpha += .01f;

            else if (!FadingEvent && alpha >= 0)
                alpha -= .01f;

            Color nNew = new Color(0, 0, 0, alpha);
            _mat.SetColor("_BaseColor", nNew);

            if (FadingTimer > 5)
            {
                FadingTimer = 0;
                Activate_Fade = false;
            }
        }

        if (TaskCountThreshold < 3)
            TaskCountThreshold += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LC") || other.gameObject.CompareTag("FC"))
        {
            DC.NumOfCollision = 1;
            CSV.DataLoggingEnd = true;


            if (DC.taskCount == 5 && LC.TaskStart) 
            {
                FadingEvent = true;
                DC.respawnTrigger = true;
                Activate_Fade = true;
                QuestionnaireStartNotice.SetActive(true);
                DC.taskCount = 0;
                DC.FirstTaskCountThreshold = false;
            }
            else
            {
                TaskFailureNotice.SetActive(true);
                DC.respawnTrigger = true;
                FadingEvent = true;
                Activate_Fade = true;
            }
        }

        if (other.gameObject.CompareTag("OutOfRoad"))
        {
            DC.NumOfCollision = 2;
            CSV.DataLoggingEnd = true;

            if (DC.taskCount == 5 && LC.TaskStart)
            {
                FadingEvent = true;
                DC.respawnTrigger = true;
                Activate_Fade = true;
                QuestionnaireStartNotice.SetActive(true);
                DC.taskCount = 0;
                DC.FirstTaskCountThreshold = false;
            }
            else
            {
                TaskFailureNotice.SetActive(true);
                //if (!LC.TaskStart && DC.taskCount != 0)
                //    DC.taskCount--;
                FadingEvent = true;
                DC.respawnTrigger = true;
                Activate_Fade = true;
            }
        }

        if (other.gameObject.CompareTag("TaskEndPoint"))
        {
            CSV.DataLoggingEnd = true;
            LC.RespawnTrigger = true;
            FC.RespawnTrigger = true;

            if (DC.taskCount == 5)
            {
                FadingEvent = true;
                DC.respawnTrigger = true;
                Activate_Fade = true;
                QuestionnaireStartNotice.SetActive(true);
                DC.taskCount = 0;
                DC.FirstTaskCountThreshold = false;
            }
        }

        if (other.gameObject.CompareTag("TaskStartPoint_1"))
        {
            if (DC.MainTask)
            {
                CSV.Create_CSV_File = true;
                CSV.DataLoggingStart = true;
                DC.LaneChangeComplete = 0;

                LC.TurnOn_LC_FC = 1;
                LC.LC_Direction = 1;
                LC.TaskStart = true;

                if (TaskCountThreshold > 2)
                {
                    if (DC.FirstTaskCountThreshold)
                        DC.taskCount++;
                    else
                        DC.FirstTaskCountThreshold = true;

                    TaskCountThreshold = 0;
                }
            }

            if (TM.TrialTask)
            {
                TM.TrialTaskTimer = 0;
                TM.TurnOnTrialCars = 1;
                TM.MoveTrialCar = 1;
                TM.ActivateTC_Speed = true;
            }
        }

        if (other.gameObject.CompareTag("TaskStartPoint_2"))
        {
            if (DC.MainTask)
            {
                CSV.Create_CSV_File = true;
                CSV.DataLoggingStart = true;
                DC.LaneChangeComplete = 0;

                LC.TurnOn_LC_FC = 2;
                LC.LC_Direction = 2;
                LC.TaskStart = true;

                if (TaskCountThreshold > 2)
                {
                    if (DC.FirstTaskCountThreshold)
                        DC.taskCount++;
                    else
                        DC.FirstTaskCountThreshold = true;

                    TaskCountThreshold = 0;
                }
            }

            if (TM.TrialTask)
            {
                TM.TrialTaskTimer = 0;
                TM.TurnOnTrialCars = 2;
                TM.MoveTrialCar = 2;
                TM.ActivateTC_Speed = true;
            }
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
}