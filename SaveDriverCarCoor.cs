using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDriverCarCoor : MonoBehaviour
{
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] CSV_Save CSV;

    public GameObject VolvoCar;
    private string csvFileName = "DriverCarCoor.csv";
    private string[] csvHeaders = new string[7] {"Time", "Position_X", "Position_Z", "Rotation_X", "Rotation_Y", "Rotation_Z", "Rotation_W" };

    void FixedUpdate()
    {
        float[] DriverCarCoor = new float[8];
        DriverCarCoor[0] = TimeLogger.TimeNumber;
        DriverCarCoor[1] = VolvoCar.transform.position.x;
        DriverCarCoor[2] = VolvoCar.transform.position.z;
        DriverCarCoor[3] = VolvoCar.transform.rotation.x;
        DriverCarCoor[4] = VolvoCar.transform.rotation.y;
        DriverCarCoor[5] = VolvoCar.transform.rotation.z;
        DriverCarCoor[6] = VolvoCar.transform.rotation.w;
        CSV.AppendToCsv(DriverCarCoor, csvFileName, csvHeaders);
    }
}
