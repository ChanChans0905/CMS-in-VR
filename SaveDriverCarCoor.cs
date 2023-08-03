using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDriverCarCoor : MonoBehaviour
{
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;

    public GameObject VolvoCar;
    private string csvSeparator = ",";
    private string csvFileName = "DriverCarCoor.csv";
    private string[] csvHeaders = new string[4] {"Time", "X", "Y", "Z" };
    private string csvDirectoryName = "Data";

    void FixedUpdate()
    {
        // Add x,y,z coordinates to the list
        float[] DriverCarCoor = new float[4];
        DriverCarCoor[0] = TimeLogger.TimeNumber;
        DriverCarCoor[1] = VolvoCar.transform.position.x;
        DriverCarCoor[2] = VolvoCar.transform.position.y;
        DriverCarCoor[3] = VolvoCar.transform.position.z;
        AppendToCsv(DriverCarCoor);
        //print("Time : " + GazeBoxCoor[0] + "X : " + GazeBoxCoor[1] + "Y : " + GazeBoxCoor[2] + "Z : " + GazeBoxCoor[3]);
    }

    string GetDirectoryPath()
    {
        return Application.dataPath + "/" + csvDirectoryName;
    }

    string GetFilePath()
    {
        return GetDirectoryPath() + "/" + csvFileName;
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
