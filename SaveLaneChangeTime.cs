using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLaneChangeTime : MonoBehaviour
{
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;

    private string csvSeparator = ",";
    private string csvFileName = "LaneChangeTime.csv";
    private string[] csvHeaders = new string[1] { "Time" };
    private string csvDirectoryName = "Data";

    void FixedUpdate()
    {
        // Add x,y,z coordinates to the list
        float[] LaneChangeTime = new float[1];
        LaneChangeTime[0] = TimeLogger.LeadingCarStopTime;

        if (TimeLogger.EventBool)
        {
            AppendToCsv(LaneChangeTime);
        }
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

            if (TimeLogger.LaneChangeComplete)
            {
                TimeLogger.EventBool = false;
                TimeLogger.LaneChangeComplete = false;
            }
        }
    }
}
