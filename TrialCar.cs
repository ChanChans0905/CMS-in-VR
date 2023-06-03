using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCar : MonoBehaviour
{
    public PathCreator pathCreator;
    float distanceTravelled;
    [SerializeField] DemoCarController DriverCar;
    public float trialTime;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (DriverCar.TrialBool)
        {
            distanceTravelled += Time.deltaTime * 15;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            trialTime += Time.deltaTime;
        }

        if (trialTime <= 10 && trialTime > 0 )
        {
            // ���� ���� �ȳ� ���� ����
            // ���ݺ��� 3�а� ���� ������ �����մϴ�.
        }

        if( trialTime >= 180)
        {
            DriverCar.TrialBool = false;
        }

        if(DriverCar.TrialBool == false)
        {
            trialTime = 0;
            gameObject.SetActive(false);
        }
    }
}
