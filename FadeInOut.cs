using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FadeInOut : MonoBehaviour
{
    public float alpha = 0;
    private Material _mat;
    public bool Event;
    [SerializeField] DemoCarController DriverCar;


    void Start()
    {
        Renderer nRend = GetComponent<Renderer>();
        _mat = nRend.material;
    }

    void Update()
    {
        if (Event == true) 
        {
            FadeIn(alpha);
        }
        else if (Event == false)
        {
            FadeOut(alpha);
        }
            Color nNew = new Color(0, 0, 0, alpha);
            _mat.SetColor("_BaseColor", nNew);


        if (Input.GetKeyDown(KeyCode.M)) // ������ ���� �� fade out�� trigger
        {
            DriverCar.respawnTrigger = false;
            Event= false;
        }
    }

    public void FadeIn(float degree)
    {
            if(alpha <= 1)
            {
                degree += .05f;
                alpha = degree;
            }
    }

    public void FadeOut(float degree)
    {
            if(alpha >= 0)
            {
                degree -= .05f;
                alpha = degree;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car") || other.gameObject.CompareTag("OutOfRoad"))
        {
            Event= true;
            DriverCar.respawnTrigger = true;
        }

        if (other.gameObject.CompareTag("WayPoint") && DriverCar.taskCount == 8)
        {
            Event = true;
            DriverCar.respawnTrigger = true;
            DriverCar.CMSchangeCount++;
            DriverCar.CMSchangeBool = true;
            DriverCar.taskCount = 0;
        }
        

    }
}