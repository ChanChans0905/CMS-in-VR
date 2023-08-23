using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CSV_Save : MonoBehaviour
{
    private string csvDirectoryName = "Data";
    private string csvSeparator = ",";

    public void AppendToCsv(float[] data, string csvFileName, string[] csvHeaders)
    {
        string dir = Application.dataPath + "/" + csvDirectoryName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string file = GetFilePath(csvFileName);
        if (!File.Exists(file))
        {
            using (StreamWriter sw = File.CreateText(GetFilePath(csvFileName)))
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

        using (StreamWriter sw = File.AppendText(GetFilePath(csvFileName)))
        {
            string finalString = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += data[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);
        }
    }

    string GetFilePath(string csvFileName)
    {
        return Application.dataPath + "/" + csvDirectoryName + "/" + csvFileName;
    }
}
