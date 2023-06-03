using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FadeInOut : MonoBehaviour
{
    public float alpha = 0;
    private Material _mat;
    public bool FadingEvent;
    [SerializeField] DemoCarController DriverCar;


    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            FadingEvent = true;
            DriverCar.respawnTrigger = true;
        }

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


        if (Input.GetKeyDown(KeyCode.M))
        {
            DriverCar.respawnTrigger = false;
            FadingEvent = false;
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
            degree -= .05f;
            alpha = degree;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car") || other.gameObject.CompareTag("OutOfRoad"))
        {
            FadingEvent = true;
            DriverCar.respawnTrigger = true;
        }

        if (other.gameObject.CompareTag("WayPoint") ad DriverCar.taskCount == 8)
        {
            FadingEvent = true;
            DriverCar.respawnTrigger = true;
            DriverCar.CMSchangeCount++;
            DriverCar.CMSchangeBool = true;
            DriverCar.taskCount = 0;
        }


    }
}