using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCar : MonoBehaviour
{
    public PathCreator pathCreator;
    float TC1Distance, TC2Distance, TC3Distance, TC4Distance;
    public bool ActivateTC;

    public GameObject TC1, TC2, TC3, TC4;

    private void Start()
    {
        TC1.gameObject.SetActive(false);
        TC2.gameObject.SetActive(false);
        TC3.gameObject.SetActive(false);
        TC4.gameObject.SetActive(false);
    }

    void Update()
    {
        TC1Distance += Time.deltaTime * 50;
        //TC2Distance += Time.deltaTime * 45;
        //TC3Distance += Time.deltaTime * 40;
        //TC4Distance += Time.deltaTime * 35;
        TC1.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance);
        TC1.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance);        
        //TC2.transform.position = pathCreator.path.GetPointAtDistance(TC2Distance);
        //TC2.transform.rotation = pathCreator.path.GetRotationAtDistance(TC2Distance);
        //TC3.transform.position = pathCreator.path.GetPointAtDistance(TC3Distance);
        //TC3.transform.rotation = pathCreator.path.GetRotationAtDistance(TC3Distance);
        //TC4.transform.position = pathCreator.path.GetPointAtDistance(TC4Distance);
        //TC4.transform.rotation = pathCreator.path.GetRotationAtDistance(TC4Distance);

        TC2.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance*0.9f);
        TC2.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance*0.9f);
        TC3.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance * 0.8f);
        TC3.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance * 0.8f);
        TC4.transform.position = pathCreator.path.GetPointAtDistance(TC1Distance * 0.7f);
        TC4.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance * 0.7f);

        if (ActivateTC)
        {
            TC1.gameObject.SetActive(true);
            TC2.gameObject.SetActive(true);
            TC3.gameObject.SetActive(true);
            TC4.gameObject.SetActive(true);
        }
        else
        {
            TC1.gameObject.SetActive(false);
            TC2.gameObject.SetActive(false);
            TC3.gameObject.SetActive(false);
            TC4.gameObject.SetActive(false);
        }

    }
}
