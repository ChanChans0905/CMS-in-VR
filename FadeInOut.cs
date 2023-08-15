using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FadeInOut : MonoBehaviour
{
    public float alpha = 0;
    private Material _mat;
    public bool FadingEvent;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] LeadingCar LC1;
    [SerializeField] LeadingCar2 LC2;
    [SerializeField] TimeLogger TimeLogger;
    public GameObject QuestionnaireStartNotice, TaskFailureNotice, KeepLaneNotice;
    float OutofLaneTime;
    public bool LaneChangeComplete;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void Update()
    {
        if (FadingEvent == true) { FadeIn(alpha);}
        else if (FadingEvent == false) { FadeOut(alpha);}
        Color nNew = new Color(0, 0, 0, alpha);
        _mat.SetColor("_BaseColor", nNew);

        if(OutofLaneTime >= 3) KeepLaneNotice.gameObject.SetActive(true);
        if (OutofLaneTime == 0) KeepLaneNotice.gameObject.SetActive(false);
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
        if (other.gameObject.CompareTag("Car") || other.gameObject.CompareTag("OutOfRoad"))
        {
            FadingEvent = true;
            DriverCar.respawnTrigger = true;
            TaskFailureNotice.SetActive(true);
            if(LC1.eventStartBool == false) { DriverCar.taskCount--; }
            if(LC2.eventStartBool == false) { DriverCar.taskCount--; }
            DriverCar.TaskEndBool = true;
            TimeLogger.EventBool = false;
        }

        if (other.gameObject.CompareTag("Car"))
        {
            DriverCar.NumOfCollisionWithCar = 1;
        }

        if (other.gameObject.CompareTag("OutOfRoad"))
        {
            DriverCar.NumOfCollisionWithGuardRail = 1;
        }
        if (other.gameObject.CompareTag("WayPoint"))
        {
            DriverCar.TaskEndBool = true;
            TimeLogger.EventBool = false;

            if(DriverCar.taskCount == 2)
            {
                FadingEvent = true;
                DriverCar.respawnTrigger = true;
                QuestionnaireStartNotice.SetActive(true);
                DriverCar.taskCount = 0;
                DriverCar.threshold = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("KeepLaneNoticeArea"))
        {
            OutofLaneTime += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("KeepLaneNoticeArea"))
        {
            OutofLaneTime = 0;
            KeepLaneNotice.SetActive(false);
        }

        if (other.gameObject.CompareTag("LaneChangeTimeCalculator"))
        {
            TimeLogger.LaneChangeComplete = true;
        }
    }
}