using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportDriverCarCoor : MonoBehaviour
{
    private string csvSeparator = ",";
    private string csvFileName = "GazeBoxCoor.csv";
    private string[] csvHeaders = new string[3] {"X", "Y", "Z"};
    private string csvDirectoryName = "GazeBoxCoor";
    private string timeStampHeader = "CaptureTime";

    string GetDirectoryPath() {
        return Application.dataPath + "/" + csvDirectoryName;
    }

    string GetFilePath()
    {
        return GetDirectoryPath() + "/" + csvFileName;
    }

    void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    void VerifyFile()
    {
        string file = GetFilePath();
        if(!File.Exists(file))
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
            for (int i =0; i < csvHeaders.Length; i++)
            {
                if(finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += csvHeaders[i];
            }
            finalString += csvSeparator + timeStampHeader;
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
            for ( int i = 0; i < floats.Length; i++) { 
                if(finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += floats[i];
            }
            finalString += csvSeparator +  GetTimeStamp();
            sw.WriteLine(finalString);
        }

        string GetTimeStamp()
        {
            return System.DateTime.Now.ToString();
        }
    }

    private void Start()
    {
        CreateCsv();
    }

    void FixedUpdate()
    {
        // Add x,y,z coordinates to the list
        float[] GazeBoxCoor = new float[3];
        GazeBoxCoor[0] = transform.position.x;
        GazeBoxCoor[1] = transform.position.y;
        GazeBoxCoor[2] = transform.position.z;
        AppendToCsv(GazeBoxCoor);
        print(GazeBoxCoor[0] + "Y" + GazeBoxCoor[1] + "Z" + GazeBoxCoor[2]);
    }
}
