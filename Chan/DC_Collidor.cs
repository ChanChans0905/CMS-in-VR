using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DC_Collidor : MonoBehaviour
{
    public float alpha = 0;
    private Material _mat;
    public bool FadingEvent;
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] TrialManager TM;
    [SerializeField] CSV_Save CSV;
    public GameObject QuestionnaireStartNotice, TaskFailureNotice/*,KeepLaneNotice*/;
    //float OutofLaneTime;

    bool DrivingIn2ndLane;
    float TaskCountThreshold;
    public bool Activate_Fade;
    float FadingTimer;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void FixedUpdate()
    {
        //if(OutofLaneTime >= 3) KeepLaneNotice.gameObject.SetActive(true);
        //if (OutofLaneTime == 0) KeepLaneNotice.gameObject.SetActive(false);

        if (Activate_Fade)
        {
            FadingTimer += Time.deltaTime;

            if (FadingEvent && alpha <= 1)
                alpha += .01f;
                
            else if (!FadingEvent && alpha >= 0)
                alpha -= .01f;
                
            Color nNew = new Color(0, 0, 0, alpha);
            _mat.SetColor("_BaseColor", nNew);
            //OutofLaneTime = 0;
            //DrivingIn2ndLane = false;

            if(FadingTimer > 5)
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
            FadingEvent = true;
            DC.respawnTrigger = true;
            Activate_Fade = true;
            TaskFailureNotice.SetActive(true);
            CSV.DataLoggingEnd = true;
        }

        if (other.gameObject.CompareTag("OutOfRoad"))
        {
            DC.NumOfCollision = 2;
            FadingEvent = true;
            DC.respawnTrigger = true;
            Activate_Fade = true;
            TaskFailureNotice.SetActive(true);
            if (!LC.TaskStart && DC.taskCount != 0)
                DC.taskCount--;
            CSV.DataLoggingEnd = true;
        }

        if (other.gameObject.CompareTag("TaskEndPoint"))
        {
            CSV.DataLoggingEnd = true;
            LC.RespawnTrigger = true;

            if (DC.taskCount == 0)
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

                LC.TurnOn_LC_FC = 1;
                LC.LC_Direction = 1;
                LC.TaskStart = true;
                
                if(TaskCountThreshold > 2)
                {
                    if(DC.FirstTaskCountThreshold)
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
        //if (other.gameObject.CompareTag("KeepLaneNoticeArea"))
        //{
        //    OutofLaneTime += Time.deltaTime;
        //}

        if(other.tag == "LaneChangeTimeCalculator" && !DrivingIn2ndLane)
            DrivingIn2ndLane = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("KeepLaneNoticeArea"))
        //{
        //    OutofLaneTime = 0;
        //    KeepLaneNotice.SetActive(false);
        //}

        if (other.tag == ("LaneChangeTimeCalculator") && DrivingIn2ndLane)
        {
                DC.LaneChangeComplete = 1;
        }
    }
}