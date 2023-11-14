using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastToGazeTarget : MonoBehaviour
{
    [SerializeField] LeadingCar LC;
    [SerializeField] DemoCarController DC;
    public Transform DC_Transform, LC1_Transform, LC2_Transform;
    public Transform GazeTarget;
    public float EOR_Time;
    public int NumberOfGlances;
    public bool DrivingDirection;
    int PlaceOfGazeTarget;
    int CheckIfGlanceChanged;
    bool StopCheckingGlance;
    LayerMask Mask_CMS_Left;
    LayerMask Mask_FrontMirror;
    LayerMask Mask_CMS_Right;

    private void Start()
    {
        Mask_CMS_Left = LayerMask.GetMask("CMS_L");
        Mask_CMS_Right = LayerMask.GetMask("CMS_R");
        Mask_FrontMirror = LayerMask.GetMask("FrontMirror");
    }

    void FixedUpdate()
    {
        if (LC.LC_StoppingTime == 1)
        {
            transform.LookAt(GazeTarget);

            if (!Physics.Raycast(transform.position, transform.forward, 500.0f, Mask_FrontMirror))
                EOR_Time += Time.deltaTime;

            if (DrivingDirection && (DC_Transform.position.z > LC1_Transform.position.z))
                StopCheckingGlance = true;
            else if (!DrivingDirection && (DC_Transform.position.z < LC2_Transform.position.z))
                StopCheckingGlance = true;

            if (Physics.Raycast(transform.position, transform.forward, 500.0f, Mask_CMS_Left))
            {
                PlaceOfGazeTarget = 1;
            }
            if (Physics.Raycast(transform.position, transform.forward, 500.0f, Mask_CMS_Right))
            {
                PlaceOfGazeTarget = 2;
            }
                
            if (Physics.Raycast(transform.position, transform.forward, 500.0f, Mask_FrontMirror))
                PlaceOfGazeTarget = 0;

            if (!StopCheckingGlance && (CheckIfGlanceChanged != PlaceOfGazeTarget))
            {
                NumberOfGlances++;
                CheckIfGlanceChanged = PlaceOfGazeTarget;
            }
        }
    }

    public void Reset()
    {
        StopCheckingGlance = false;
        EOR_Time = 0;
        NumberOfGlances = 0;
        CheckIfGlanceChanged = 0;
    }
}
