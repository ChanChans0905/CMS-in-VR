using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FadeInOut : MonoBehaviour
{
    public float alpha = 0;
    private Material _mat;
    public bool FadingEvent;
    [SerializeField] DemoCarController DC;
    [SerializeField] LaneChangeCar LC;
    public GameObject QuestionnaireStartNotice, TaskFailureNotice/*,KeepLaneNotice*/;
    //float OutofLaneTime;
    public bool GetLeadingCarDirection;
    bool DrivingIn2ndLane;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;

    }

    void Update()
    {
        //if(OutofLaneTime >= 3) KeepLaneNotice.gameObject.SetActive(true);
        //if (OutofLaneTime == 0) KeepLaneNotice.gameObject.SetActive(false);

        if (DC.respawnTrigger)
        {
            if (FadingEvent == true) { FadeIn(alpha); }
            else if (FadingEvent == false) { FadeOut(alpha); }
            Color nNew = new Color(0, 0, 0, alpha);
            _mat.SetColor("_BaseColor", nNew);
            //OutofLaneTime = 0;
            DrivingIn2ndLane = false;
        }
    }

    public void FadeIn(float degree)
    {
        if (alpha <= 1)
        {
            degree += .01f;
            alpha = degree;
        }
    }

    public void FadeOut(float degree)
    {
        if (alpha >= 0)
        {
            degree -= .01f;
            alpha = degree;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FC") || other.gameObject.CompareTag("LC") || other.gameObject.CompareTag("OutOfRoad"))
        {
            FadingEvent = true;
            DC.respawnTrigger = true;
            TaskFailureNotice.SetActive(true);
            if(!LC.TaskStart && DC.taskCount != 0) 
                DC.taskCount--;
        }

        if (other.gameObject.CompareTag("LC") || other.gameObject.CompareTag("FC"))
        {
            DC.NumOfCollision = 1;
        }

        if (other.gameObject.CompareTag("OutOfRoad"))
        {
            DC.NumOfCollision = 2;
        }
        if (other.gameObject.CompareTag("WayPoint"))
        {

            if(DC.taskCount == 2)
            {
                FadingEvent = true;
                DC.respawnTrigger = true;
                QuestionnaireStartNotice.SetActive(true);
                DC.taskCount = 0;
                DC.threshold = false;
            }
        }

        if (other.gameObject.CompareTag("TaskCounter1"))
            GetLeadingCarDirection = true;

        if (other.gameObject.CompareTag("TaskCounter2"))
            GetLeadingCarDirection = false;
                
            

        if (other.gameObject.CompareTag("TaskStartPoint"))
            LC.TaskStart = true;
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