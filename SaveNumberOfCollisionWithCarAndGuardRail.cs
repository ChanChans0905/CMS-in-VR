using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class SaveNumberOfCollisionWithCarAndGuardRail : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] CSV_Save CSV;

    private string csvSeparator = ",";
    private string csvFileName = "NumberOfCollisionWIthCarAndGuardRail.csv";
    private string[] csvHeaders = new string[5] { "Time", "TaskNumber", "UsedCMSCombination", "Car", "GuardRail" };
    private string csvDirectoryName = "Data";
    float Timer;

    void FixedUpdate()
    {
        if (DriverCar.TaskEndBool)
        {
            float[] SaveNum = new float[4];
            SaveNum[0] = TimeLogger.TimeNumber;
            SaveNum[1] = DriverCar.CMScombination[DriverCar.CMSchangeCount];
            SaveNum[2] = DriverCar.taskCount;
            SaveNum[3] = DriverCar.NumOfCollisionWithCar;
            SaveNum[4] = DriverCar.NumOfCollisionWithGuardRail;

            CSV.AppendToCsv(SaveNum, csvFileName, csvHeaders);

            Timer += Time.deltaTime;

            if(Timer > 2)
            {
                DriverCar.TaskEndBool = false;
                DriverCar.NumOfCollisionWithCar = 0;
                DriverCar.NumOfCollisionWithGuardRail = 0;
            }
        }
    }
}
