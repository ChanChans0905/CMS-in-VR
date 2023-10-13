using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class CSV_Save : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] FollowingCar FC;

    public GameObject Volvocar, Gaze, Head;
    public GameObject LC1, LC2, FCL1, FCL2, FCR1, FCR2;
    //public GameObject Gaze_Untracked_Notice;

    public bool DataLoggingStart, DataLoggingEnd, Create_CSV_File;
    bool Gaze_Untrackable;
    float Gaze_Lost_Timer;

    string FilePath;
    string csvDirectoryName = "Data";
    string csvSeparator = ",";
    string[] csvHeaders = new string[] { "FrameNumber", "CMS_Combination", "TaskCount", "LC_LaneChangeStartingTime", "FC_Speed", "LC_StoppingTime", "DC_LaneChangeCompleteTime", "FirstReactionTime", "Steering Wheel","Pedal","NumberOfCollision", "Gaze_X", "Gaze_Y", "Gaze_Z",
                                         "Head_X","Head_Y","Head_Z","DC_Pos_X","DC_Pos_Y","DC_Pos_Z", "DC_Rot_X","DC_Rot_Y","DC_Rot_Z","DC_Rot_W", "LC_Pos_X","LC_Pos_Y","LC_Pos_Z", "FCL_Pos_X","FCL_Pos_Y","FCL_Pos_Z", "FCR_Pos_X", "FCR_Pos_Y", "FCR_Pos_Z" };
           
    float[] SaveData = new float[33];
    float FrameNumber;

    private void Start()
    {
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);
    }

    void FixedUpdate()
    {
        if (DataLoggingStart)
        {
            if (Create_CSV_File)
            {
                string csvFileName = "Data_SampleNumber_" + DC.SampleNumber + "_" + DC.CMScombination[DC.CMSchangeCount] + "_" + DC.taskCount + ".csv";

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
                Create_CSV_File = false;
                FrameNumber = 0;
            }

            FrameNumber += Time.fixedDeltaTime;

            SaveData[0] = FrameNumber; // FrameNumber
            SaveData[1] = DC.CMScombination[DC.CMSchangeCount-1]; // CMS_Combination
            SaveData[2] = DC.taskCount; // TaskCount
            SaveData[3] = DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount]; // LC_laneChangeStartingTime
            SaveData[4] = DC.FollowingCarSpeed[DC.CMSchangeCount - 1, DC.taskCount]; // FC_Speed
            SaveData[5] = LC.LC_StoppingTime; // LC_StoppingTime
            SaveData[6] = DC.LaneChangeComplete;
            SaveData[7] = DC.TotalFirstReactionValue; // FirstReactionTime
            SaveData[8] = DC.SteeringWheel_Data;
            SaveData[9] = DC.Pedal_Data;
            SaveData[10] = DC.NumOfCollision;
            SaveData[11] = Gaze.transform.localPosition.x;
            SaveData[12] = Gaze.transform.localPosition.y;
            SaveData[13] = Gaze.transform.localPosition.z;
            SaveData[14] = Head.transform.position.x;
            SaveData[15] = Head.transform.position.y;
            SaveData[16] = Head.transform.position.z;


            SaveData[17] = Volvocar.transform.position.x;
            SaveData[18] = 0;
            SaveData[19] = Volvocar.transform.position.z;
            SaveData[20] = Volvocar.transform.rotation.x;
            SaveData[21] = Volvocar.transform.rotation.y;
            SaveData[22] = Volvocar.transform.rotation.z;
            SaveData[23] = Volvocar.transform.rotation.w;
     
            switch (LC.LC_Direction)
            {
                case 1:
                    SaveData[24] = LC1.transform.position.x;
                    SaveData[25] = 0;
                    SaveData[26] = LC1.transform.position.z;
                    SaveData[27] = FCL1.transform.position.x;
                    SaveData[28] = 0;
                    SaveData[29] = FCL1.transform.position.z;
                    SaveData[30] = FCR1.transform.position.x;
                    SaveData[31] = 0;
                    SaveData[32] = FCR1.transform.position.z;
                    break;

                case 2:
                    SaveData[24] = LC2.transform.position.x;
                    SaveData[25] = 0;
                    SaveData[26] = LC2.transform.position.z;
                    SaveData[27] = FCL2.transform.position.x;
                    SaveData[28] = 0;
                    SaveData[29] = FCL2.transform.position.z;
                    SaveData[30] = FCR2.transform.position.x;
                    SaveData[31] = 0;
                    SaveData[32] = FCR2.transform.position.z;
                    break;

                default:
                    SaveData[24] = 0;
                    SaveData[25] = 0;
                    SaveData[26] = 0;
                    SaveData[27] = 0;
                    SaveData[28] = 0;
                    SaveData[29] = 0;
                    SaveData[30] = 0;
                    SaveData[31] = 0;
                    SaveData[32] = 0;
                    break;
            }

            //(SaveData[11] = 0 && SaveData[12] = 0 && SaveData[13] == 0) ? Gaze_Untrackable = true : Gaze_Untrackable = false;

            //if (Gaze_Untrackable)
            //{
            //    Gaze_Lost_Timer += Time.deltaTime;

            //    if (Gaze_Lost_Timer >= 60)
            //    {
            //        DC.respawnTrigger = true;
            //        Gaze_Untracked_Notice.SetActive(true);
            //    }
            //}
            //else
            //    Gaze_Lost_Timer = 0;

            AppendToCsv(SaveData);
        }
    }

    public void AppendToCsv(float[] data)
    {
        using (StreamWriter sw = File.AppendText(FilePath))
        {
            string csvFinalString = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (csvFinalString != "")
                {
                    csvFinalString += csvSeparator;
                }
                csvFinalString += data[i];
            }
            csvFinalString += csvSeparator;
            sw.WriteLine(csvFinalString);
        }

        if (DataLoggingEnd)
        {
            DataLoggingStart = false;
            DataLoggingEnd = false;
            DC.ResetData = true;
        }
    }
}
