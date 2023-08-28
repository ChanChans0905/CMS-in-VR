using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class CSV_Save : MonoBehaviour
{
    // needed : FrameNumber, CMS_Combination, TaskCount, EventStartingTime, FirstReacionTIme, DC_LaneChangeTime, NumberOfCollision, GazeCoor, DC_Coor, LC_Coor, FC_Coor
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] FollowingCar FC;

    public GameObject Volvocar, Gaze;
    public GameObject LC1, LC2, FCL1, FCL2, FCR1, FCR2;

    string csvFileName;
    string csvDirectoryName = "Data";
    string csvSeparator = ",";
    string[] csvHeaders = new string[] { "FrameNumber", "CMS_Combination", "TaskCount", "LC_LaneChangeStartingTime", "LC_StoppingTime", "FirstReactionTime", "FC_Speed", "NumberOfCollision", "DC_LaneChangeCompleteTime", "Gaze_X", "Gaze_Y", "Gaze_Z",
                                         "DC_Pos_X","DC_Pos_Y","DC_Pos_Z", "DC_Rot_X","DC_Rot_Y","DC_Rot_Z","DC_Rot_W", "LC_Pos_X","LC_Pos_Y","LC_Pos_Z", "FCL_Pos_X","FCL_Pos_Y","FCL_Pos_Z", "FCR_Pos_X", "FCR_Pos_Y", "FCR_Pos_Z" };

    public float[] SaveData = new float[28];
    float FrameNumber;
    string FilePath;

    private void Start()
    {
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);
        DC.SampleSelection = true;
    }

    void FixedUpdate()
    {
        if(DC.SampleSelection)
        {
            csvFileName = "Data_SampleNumber_" + DC.SampleNumber + ".csv";

            FilePath = Application.dataPath + "/" + csvDirectoryName + "/" + csvFileName;

            using (StreamWriter sw = File.CreateText(FilePath))
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

            DC.SampleSelection = false;
        }

        FrameNumber += Time.fixedDeltaTime;

        SaveData[0] = FrameNumber; // FrameNumber
        SaveData[1] = DC.CMScombination[DC.CMSchangeCount]; // CMS_Combination
        SaveData[2] = DC.taskCount; // TaskCount
        SaveData[3] = DC.LaneChangeTime[DC.taskCount]; // LC_laneChangeStartingTime
        SaveData[4] = LC.LC_StoppingTime; // LC_StoppingTime
        SaveData[5] = DC.TotalFirstReactionValue; // FirstReactionTime
        SaveData[6] = DC.FollowingCarSpeed[DC.taskCount]; // FC_Speed
        SaveData[7] = DC.NumOfCollision;
        SaveData[8] = DC.LaneChangeComplete;
        SaveData[9] = Gaze.transform.localPosition.x;
        SaveData[10] = Gaze.transform.localPosition.y;
        SaveData[11] = Gaze.transform.localPosition.z;
        SaveData[12] = Volvocar.transform.position.x;
        SaveData[13] = 0;
        SaveData[14] = Volvocar.transform.position.z;
        SaveData[15] = Volvocar.transform.rotation.x;
        SaveData[16] = Volvocar.transform.rotation.y;
        SaveData[17] = Volvocar.transform.rotation.z;
        SaveData[18] = Volvocar.transform.rotation.w;

        if (DC.MainTask)
        {
            if(LC.LC_Direction == 1)
            {
                SaveData[19] = LC1.transform.position.x;
                SaveData[20] = 0;
                SaveData[21] = LC1.transform.position.z;
                SaveData[22] = FCL1.transform.position.x;
                SaveData[23] = 0;
                SaveData[24] = FCL1.transform.position.z;
                SaveData[25] = FCR1.transform.position.x;
                SaveData[26] = 0;
                SaveData[27] = FCR1.transform.position.z;
            }
            else if (LC.LC_Direction == 2)
            {
                SaveData[19] = LC2.transform.position.x;
                SaveData[20] = 0;
                SaveData[21] = LC2.transform.position.z;
                SaveData[22] = FCL2.transform.position.x;
                SaveData[23] = 0;
                SaveData[24] = FCL2.transform.position.z;
                SaveData[25] = FCR2.transform.position.x;
                SaveData[26] = 0;
                SaveData[27] = FCR2.transform.position.z;
            }
        }
        else
        {
            SaveData[19] = 0;
            SaveData[20] = 0;
            SaveData[21] = 0;
            SaveData[22] = 0;
            SaveData[23] = 0;
            SaveData[24] = 0;
            SaveData[25] = 0;
            SaveData[26] = 0;
            SaveData[27] = 0;
        }


        AppendToCsv(SaveData);
    }

    public void AppendToCsv(float[] data)
    {
        using (StreamWriter sw = File.AppendText(FilePath))
        {
            string csvFinalString = "";
            for (int i = 0; i < data.Length; i++)
            {
                Debug.Log(data.Length);
                if (csvFinalString != "")
                {
                    csvFinalString += csvSeparator;
                }
                csvFinalString += data[i];
            }
            csvFinalString += csvSeparator;
            sw.WriteLine(csvFinalString);
        }
    }
}
