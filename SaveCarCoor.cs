using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveCarCoor : MonoBehaviour
{
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;

    public GameObject LC, FCL, FCR;

    private string csvSeparator = ",";
    private string csvFileName = "Coor.csv";
    private string[] csvHeaders = new string[10] { "Time", "LC_X", "LC_Y", "LC_Z", "FCL_X", "FCL_Y", "FCL_Z", "FCR_X", "FCR_Y", "FCR_Z" };
    private string csvDirectoryName = "Data";


    void FixedUpdate()
    {
        if (DriverCar.MainTask)
        {
            // Add x,y,z coordinates to the list
            float[] CarCoor = new float[10];
            CarCoor[0] = TimeLogger.TimeNumber;
            CarCoor[1] = LC.transform.position.x;
            CarCoor[2] = LC.transform.position.y;
            CarCoor[3] = LC.transform.position.z;
            CarCoor[4] = FCL.transform.position.x;
            CarCoor[5] = FCL.transform.position.y;
            CarCoor[6] = FCL.transform.position.z;
            CarCoor[7] = FCR.transform.position.x;
            CarCoor[8] = FCR.transform.position.y;
            CarCoor[9] = FCR.transform.position.z;
            AppendToCsv(CarCoor);
        }
    }

    string GetDirectoryPath()
    {
        return Application.dataPath + "/" + csvDirectoryName;
    }

    string GetFilePath()
    {
        return GetDirectoryPath() + "/" + LC.gameObject.name + "_" + FCL.gameObject.name + "_" + FCR.gameObject.name + "_" + csvFileName;
    }

    void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateCsv();
        }
    }

    public void CreateCsv()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < csvHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += csvHeaders[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);
        }
    }

    public void AppendToCsv(float[] floats)
    {
        VerifyDirectory();
        VerifyFile();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < floats.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += floats[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);
        }
    }
}
