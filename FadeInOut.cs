using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FadeInOut : MonoBehaviour
{
    public float alpha = 0;
    private Material _mat;
    public bool FadingEvent;
    [SerializeField] DemoCarController DriverCar;
    public float noticeTime;
    public GameObject QuestionnaireStartNotice;

    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void Update()
    {
        if (FadingEvent == true)
        {
            FadeIn(alpha);
        }
        else if (FadingEvent == false)
        {
            FadeOut(alpha);
        }
        Color nNew = new Color(0, 0, 0, alpha);
        _mat.SetColor("_BaseColor", nNew);

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

        if (DriverCar.taskCount == 0 && DriverCar.noticeBool == true)
        {
            noticeTime += Time.deltaTime;
            if (noticeTime <= 10 && noticeTime > 0)
            {
                // new combination has been activated. try it for 3minutes from now.
            }
            else if (noticeTime > 7)
            {
                noticeTime = 0;
                DriverCar.noticeBool = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car") || other.gameObject.CompareTag("OutOfRoad"))
        {
            FadingEvent = true;
            DriverCar.respawnTrigger = true;
        }

        if (other.gameObject.CompareTag("WayPoint") && DriverCar.taskCount == 1)
        {
            FadingEvent = true;
            DriverCar.respawnTrigger = true;
            QuestionnaireStartNotice.SetActive(true);
            DriverCar.taskCount = 0;
        }
    }
}