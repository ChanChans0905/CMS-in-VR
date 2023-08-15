using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveReactionTime : MonoBehaviour
{

    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;

    private string csvSeparator = ",";
    private string csvFileName = "ReactionTIme.csv";
    private string[] csvHeaders = new string[1] { "Time" };
    private string csvDirectoryName = "Data";
    bool FirstReact;

    void FixedUpdate()
    {
        float[] ReactionTime = new float[1];

        if (TimeLogger.EventBool )
        {
            ReactionTime[0] = TimeLogger.LeadingCarStopTime;

            if ((DriverCar.Br >= -0.1 || DriverCar.SteeringInput < 3000)) FirstReact = true; // not fixed
            if (FirstReact)
            {
                AppendToCsv(ReactionTime);
            }
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
        }
    }
}
