using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CSV_Save : MonoBehaviour
{
    // needed : FrameNumber, CMS_Combination, TaskCount, EventStartingTime, FirstReacionTIme, DC_LaneChangeTime, NumberOfCollision, GazeCoor, DC_Coor, LC_Coor, FC_Coor
    [SerializeField] DemoCarController DC;
    [SerializeField] LaneChangeCar LC;
    [SerializeField] FollowingCar_New FC;

    public GameObject Volvocar, LeadingCar, FollowingCarLeft,FollowingCarRight, Gaze;

    string csvFileName;
    string csvDirectoryName = "Data";
    string csvSeparator = ";";
    string[] csvHeaders = new string[] { "FrameNumber", "CMS_Combination", "TaskCount", "LC_LaneChangeStartingTime", "LC_StoppingTime", "FirstReactionTime", "FC_Speed", "NumberOfCollision", "DC_LaneChangeCompleteTime", "Gaze_X", "Gaze_Y", "Gaze_Z",
                                         "DC_Pos_X","DC_Pos_Y","DC_Pos_Z", "DC_Rot_X","DC_Rot_Y","DC_Rot_Z","DC_Rot_W", "LC_Pos_X","LC_Pos_Y","LC_Pos_Z", "FCL_Pos_X","FCL_Pos_Y","FCL_Pos_Z", "FCR_Pos_X", "FCR_Pos_Y", "FCR_Pos_Z" };

    public float[] SaveData = new float[28];
    float FrameNumber;

    void FixedUpdate()
    {
        if(DC.SampleSelection)
        {
            csvFileName = "Data_SampleNumber_" + DC.SampleNumber + ".csv";

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
        SaveData[13] = Volvocar.transform.position.y;
        SaveData[14] = Volvocar.transform.position.z;
        SaveData[15] = Volvocar.transform.rotation.x;
        SaveData[16] = Volvocar.transform.rotation.y;
        SaveData[17] = Volvocar.transform.rotation.z;
        SaveData[18] = Volvocar.transform.rotation.w;
        SaveData[19] = LeadingCar.transform.position.x;
        SaveData[20] = LeadingCar.transform.position.y;
        SaveData[21] = LeadingCar.transform.position.z;
        SaveData[22] = FollowingCarLeft.transform.position.x;
        SaveData[23] = FollowingCarLeft.transform.position.y;
        SaveData[24] = FollowingCarLeft.transform.position.z;
        SaveData[25] = FollowingCarRight.transform.position.x;
        SaveData[26] = FollowingCarRight.transform.position.y;
        SaveData[27] = FollowingCarRight.transform.position.z;

        AppendToCsv(SaveData);
    }

    public void AppendToCsv(float[] data)
    {
        string dir = Application.dataPath + "/" + csvDirectoryName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string FilePath = GetFilePath(csvFileName);
        if (!File.Exists(FilePath))
        {
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
        }

        using (StreamWriter sw = File.AppendText(FilePath))
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
