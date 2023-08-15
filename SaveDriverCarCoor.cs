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
    private string[] csvHeaders = new string[8] {"Time", "Position_X", "Position_Y", "Position_Z", "Rotation_X", "Rotation_Y", "Rotation_Z", "Rotation_W" };
    private string csvDirectoryName = "Data";

    void FixedUpdate()
    {
        float[] DriverCarCoor = new float[8];
        DriverCarCoor[0] = TimeLogger.TimeNumber;
        DriverCarCoor[1] = VolvoCar.transform.position.x;
        DriverCarCoor[2] = VolvoCar.transform.position.y;
        DriverCarCoor[3] = VolvoCar.transform.position.z;
        DriverCarCoor[4] = VolvoCar.transform.rotation.x;
        DriverCarCoor[5] = VolvoCar.transform.rotation.y;
        DriverCarCoor[6] = VolvoCar.transform.rotation.z;
        DriverCarCoor[7] = VolvoCar.transform.rotation.w;
        AppendToCsv(DriverCarCoor);
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
