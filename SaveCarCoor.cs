using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveCarCoor : MonoBehaviour
{
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] CSV_Save CSV;

    public GameObject LC, FCL, FCR;

    private string csvFileName = "Coor.csv";
    private string[] csvHeaders = new string[7] { "Time", "LC_X", "LC_Z", "FCL_X", "FCL_Z", "FCR_X", "FCR_Z" };


    void FixedUpdate()
    {
        if (DriverCar.MainTask)
        {
            // Add x,y,z coordinates to the list
            float[] CarCoor = new float[7];
            CarCoor[0] = TimeLogger.TimeNumber;
            CarCoor[1] = LC.transform.position.x;
            CarCoor[2] = LC.transform.position.z;
            CarCoor[3] = FCL.transform.position.x;
            CarCoor[4] = FCL.transform.position.z;
            CarCoor[5] = FCR.transform.position.x;
            CarCoor[6] = FCR.transform.position.z;
            CSV.AppendToCsv(CarCoor, csvFileName, csvHeaders);
        }
    }
}
