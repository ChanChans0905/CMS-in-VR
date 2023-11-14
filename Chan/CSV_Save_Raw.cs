using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class CSV_Save_Raw : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] FollowingCar FC;

    public GameObject Volvocar, Gaze, Head;
    public GameObject LC1, LC2, FCL1, FCL2, FCR1, FCR2;

    public bool DataLoggingStart, DataLoggingEnd, Create_CSV_File;

    string FilePath;
    string csvDirectoryName = "Data";
    string[] csvHeaders = new string[] { "FrameNumber", /*"CMS_Combination", "TaskCount", "LC_LaneChangeStartingTime", "FC_Speed", */"LC_StoppingTime", "DC_LaneChangeCompleteTime", "FirstReactionTime", "Steering Wheel",
                                         "Pedal","NumberOfCollision", "Gaze_X", "Gaze_Y", "Gaze_Z", "Head_X","Head_Y","Head_Z","DC_Pos_X","DC_Pos_Y","DC_Pos_Z", "DC_Rot_X","DC_Rot_Y","DC_Rot_Z","DC_Rot_W",
                                         "LC_Pos_X","LC_Pos_Y","LC_Pos_Z", "FCL_Pos_X","FCL_Pos_Y","FCL_Pos_Z", "FCR_Pos_X", "FCR_Pos_Y", "FCR_Pos_Z" };

    float[] DataArray = new float[29];
    float FrameNumber;
    float EndLoggingTimer;
    public bool AddEndloggingTimer;

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
                New_CSV_File();

            FrameNumber += Time.fixedDeltaTime;
            SaveDataIntoArray();
            AppendToCsv(DataArray);
        }

        if (AddEndloggingTimer && EndLoggingTimer < 3)
            EndLoggingTimer += Time.deltaTime;

        if (DataLoggingEnd && EndLoggingTimer > 2f)
        {
            DataLoggingStart = false;
            DC.ResetTrigger = true;
            EndLoggingTimer = 0;
            AddEndloggingTimer = false;
            DataLoggingEnd = false;
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
                    csvFinalString += ",";
                }
                csvFinalString += data[i];
            }
            csvFinalString += ",";
            sw.WriteLine(csvFinalString);
        }
    }

    private void New_CSV_File()
    {
        string csvFileName = "CMS_RawData_SampleNumber_" + DC.SampleNumber + "_CMS_" + DC.CMScombination[DC.CMSchangeCount - 1] + "_TaskCount_" + (DC.taskCount + 1) + "_LC.LaneChangeStartTime_" + DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount]
                    + "_FC.Speed_" + DC.FollowingCarSpeed[DC.CMSchangeCount - 1, DC.taskCount] + ".csv";

        FilePath = Application.dataPath + "/" + csvDirectoryName + "/" + csvFileName;

        using (StreamWriter sw = File.CreateText(FilePath))
        {
            string finalString = "";
            for (int i = 0; i < csvHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += ",";
                }
                finalString += csvHeaders[i];
            }
            finalString += ",";
            sw.WriteLine(finalString);
        }
        Create_CSV_File = false;
        FrameNumber = 0;
    }

    private void SaveDataIntoArray()
    {
        DataArray[0] = FrameNumber;
        DataArray[1] = LC.LC_StoppingTime;
        DataArray[2] = DC.LaneChangeComplete;
        DataArray[3] = DC.TotalFirstReactionValue;
        DataArray[4] = DC.SteeringWheel_Data;
        DataArray[5] = DC.Pedal_Data;
        DataArray[6] = DC.NumOfCollision;
        DataArray[7] = Gaze.transform.localPosition.x;
        DataArray[8] = Gaze.transform.localPosition.y;
        DataArray[9] = Gaze.transform.localPosition.z;
        DataArray[10] = Head.transform.localPosition.x;
        DataArray[11] = Head.transform.localPosition.y;
        DataArray[12] = Head.transform.localPosition.z;

        DataArray[13] = Volvocar.transform.position.x;
        DataArray[14] = 0;
        DataArray[15] = Volvocar.transform.position.z;
        DataArray[16] = Volvocar.transform.rotation.x;
        DataArray[17] = Volvocar.transform.rotation.y;
        DataArray[18] = Volvocar.transform.rotation.z;
        DataArray[19] = Volvocar.transform.rotation.w;

        switch (LC.LC_Direction)
        {
            case 1:
                DataArray[20] = LC1.transform.position.x;
                DataArray[21] = 0;
                DataArray[22] = LC1.transform.position.z;
                DataArray[23] = FCL1.transform.position.x;
                DataArray[24] = 0;
                DataArray[25] = FCL1.transform.position.z;
                DataArray[26] = FCR1.transform.position.x;
                DataArray[27] = 0;
                DataArray[28] = FCR1.transform.position.z;
                break;

            case 2:
                DataArray[20] = LC2.transform.position.x;
                DataArray[21] = 0;
                DataArray[22] = LC2.transform.position.z;
                DataArray[23] = FCL2.transform.position.x;
                DataArray[24] = 0;
                DataArray[25] = FCL2.transform.position.z;
                DataArray[26] = FCR2.transform.position.x;
                DataArray[27] = 0;
                DataArray[28] = FCR2.transform.position.z;
                break;

            default:
                DataArray[20] = 0;
                DataArray[21] = 0;
                DataArray[22] = 0;
                DataArray[23] = 0;
                DataArray[24] = 0;
                DataArray[25] = 0;
                DataArray[26] = 0;
                DataArray[27] = 0;
                DataArray[28] = 0;
                break;
        }
    }
}
