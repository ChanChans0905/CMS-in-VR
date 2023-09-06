using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSampleNumber : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    public Text Sample;
    int Num;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Num++;
        }

    }
}
