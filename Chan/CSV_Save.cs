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
    //bool Gaze_Untrackable;
    //float Gaze_Lost_Timer;

    string FilePath;
    string csvDirectoryName = "Data";
    string csvSeparator = ",";
    string[] csvHeaders = new string[] { "FrameNumber", /*"CMS_Combination", "TaskCount", "LC_LaneChangeStartingTime", "FC_Speed", */"LC_StoppingTime", "DC_LaneChangeCompleteTime", "FirstReactionTime", "Steering Wheel",
                                         "Pedal","NumberOfCollision", "Gaze_X", "Gaze_Y", "Gaze_Z", "Head_X","Head_Y","Head_Z","DC_Pos_X","DC_Pos_Y","DC_Pos_Z", "DC_Rot_X","DC_Rot_Y","DC_Rot_Z","DC_Rot_W", 
                                         "LC_Pos_X","LC_Pos_Y","LC_Pos_Z", "FCL_Pos_X","FCL_Pos_Y","FCL_Pos_Z", "FCR_Pos_X", "FCR_Pos_Y", "FCR_Pos_Z" };
           
    float[] SaveData = new float[29];
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
                string csvFileName = "SampleNumber_" + DC.SampleNumber + "_CMS_" + DC.CMScombination[DC.CMSchangeCount-1] + "_TaskCount_" + (DC.taskCount+1) + "_LC.LaneChangeStartTime_" + DC.LaneChangeTime[DC.CMSchangeCount-1,DC.taskCount] 
                                    + "_FC.Speed_" + DC.FollowingCarSpeed[DC.CMSchangeCount-1, DC.taskCount] + ".csv";

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
            //SaveData[1] = DC.CMScombination[DC.CMSchangeCount-1]; // CMS_Combination
            //SaveData[2] = DC.taskCount; // TaskCount
            //SaveData[3] = DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount]; // LC_laneChangeStartingTime
            //SaveData[4] = DC.FollowingCarSpeed[DC.CMSchangeCount - 1, DC.taskCount]; // FC_Speed
            SaveData[1] = LC.LC_StoppingTime; // LC_StoppingTime
            SaveData[2] = DC.LaneChangeComplete;
            SaveData[3] = DC.TotalFirstReactionValue; // FirstReactionTime
            SaveData[4] = DC.SteeringWheel_Data;
            SaveData[5] = DC.Pedal_Data;
            SaveData[6] = DC.NumOfCollision;
            SaveData[7] = Gaze.transform.localPosition.x;
            SaveData[8] = Gaze.transform.localPosition.y;
            SaveData[9] = Gaze.transform.localPosition.z;
            SaveData[10] = Head.transform.position.x;
            SaveData[11] = Head.transform.position.y;
            SaveData[12] = Head.transform.position.z;


            SaveData[13] = Volvocar.transform.position.x;
            SaveData[14] = 0;
            SaveData[15] = Volvocar.transform.position.z;
            SaveData[16] = Volvocar.transform.rotation.x;
            SaveData[17] = Volvocar.transform.rotation.y;
            SaveData[18] = Volvocar.transform.rotation.z;
            SaveData[19] = Volvocar.transform.rotation.w;
     
            switch (LC.LC_Direction)
            {
                case 1:
                    SaveData[20] = LC1.transform.position.x;
                    SaveData[21] = 0;
                    SaveData[22] = LC1.transform.position.z;
                    SaveData[23] = FCL1.transform.position.x;
                    SaveData[24] = 0;
                    SaveData[25] = FCL1.transform.position.z;
                    SaveData[26] = FCR1.transform.position.x;
                    SaveData[27] = 0;
                    SaveData[28] = FCR1.transform.position.z;
                    break;

                case 2:
                    SaveData[20] = LC2.transform.position.x;
                    SaveData[21] = 0;
                    SaveData[22] = LC2.transform.position.z;
                    SaveData[23] = FCL2.transform.position.x;
                    SaveData[24] = 0;
                    SaveData[25] = FCL2.transform.position.z;
                    SaveData[26] = FCR2.transform.position.x;
                    SaveData[27] = 0;
                    SaveData[28] = FCR2.transform.position.z;
                    break;

                default:
                    SaveData[20] = 0;
                    SaveData[21] = 0;
                    SaveData[22] = 0;
                    SaveData[23] = 0;
                    SaveData[24] = 0;
                    SaveData[25] = 0;
                    SaveData[26] = 0;
                    SaveData[27] = 0;
                    SaveData[28] = 0;
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
