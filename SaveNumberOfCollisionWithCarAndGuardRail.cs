using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveNumberOfCollisionWithCarAndGuardRail : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] TimeLogger TimeLogger;

    private string csvSeparator = ",";
    private string csvFileName = "NumberOfCollisionWIthCarAndGuardRail.csv";
    private string[] csvHeaders = new string[5] { "Time", "TaskNumber", "UsedCMSCombination", "Car", "GuardRail" };
    private string csvDirectoryName = "Data";

    void FixedUpdate()
    {
        if (DriverCar.TaskEndBool)
        {
            float[] SaveNum = new float[4];
            SaveNum[0] = TimeLogger.TimeNumber;
            SaveNum[1] = DriverCar.taskCount;
            SaveNum[2] = DriverCar.CMSchangeCount;
            SaveNum[3] = DriverCar.NumOfCollisionWithCar;
            SaveNum[4] = DriverCar.NumOfCollisionWithGuardRail;

            AppendToCsv(SaveNum);
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
        DriverCar.TaskEndBool = false;
        DriverCar.NumOfCollisionWithCar = 0;
        DriverCar.NumOfCollisionWithGuardRail = 0;
    }
}
