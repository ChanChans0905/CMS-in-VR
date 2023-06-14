using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Questionnaire : MonoBehaviour
{
    public bool SaveTriggerExit = false;
    public GameObject SaveTriggerObject;
    float x;
    int threshold_y;
    int threshold_z;
    [SerializeField] DemoCarController DriverCar;
    public int QuestionnaireNumber;
    [SerializeField] Slider AnswerSlider;
    private string csvSeparator = ",";
    public string csvFileName;
    private string[] csvHeaders = new string[2] { "Number", "Answer" };
    private string csvDirectoryName = "Questionnaire";
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool SaveTrigger;
    [SerializeField] FadeInOut FadeInOut;
    public GameObject FinalQuestionnaire;
    public bool ButtonActivation;


    void Start()
    {
        ButtonActivation = true;
        List<Transform> children = GetChildren(transform);

        foreach (Transform child in children)
        {
            Debug.Log(child.name);
        }
    }

    void Update()
    {
        List<Transform> children = GetChildren(transform);
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if (rec.lX > 0)
            {
                if (rec.lX < 1500)
                {
                    x = 0;
                }
                else if (rec.lX > 1500 && rec.lX < 4500)
                {
                    x = 1;
                }
                else if (rec.lX > 4500 && rec.lX < 7500)
                {
                    x = 2;
                }
                else if (rec.lX > 7500)
                {
                    x = 3;
                }
            }
            else
            {
                if (rec.lX > -1500)
                {
                    x = 0;
                }
                else if (rec.lX < -1500 && rec.lX > -4500)
                {
                    x = -1;
                }
                else if (rec.lX < -4500 && rec.lX > -7500)
                {
                    x = -2;
                }
                else if (rec.lX < -7500)
                {
                    x = -3;
                }
            }

            Chage_value(x);

            if (ButtonActivation)
            {
                if (rec.rgbButtons[4] == 128)
                {
                    threshold_y++;
                    if (threshold_y >= 25)
                    {
                        if (QuestionnaireNumber < 4)
                        {
                            QuestionnaireNumber++;
                            children[QuestionnaireNumber].gameObject.SetActive(true);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            AnswerSlider = children[QuestionnaireNumber].GetComponent<Slider>();
                        }
                        else if(QuestionnaireNumber == 4)
                        {
                            QuestionnaireNumber++;
                        }

                        threshold_y = 0;
                    }
                }

                if (QuestionnaireNumber == 5)
                {
                    children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                    SaveTriggerObject.SetActive(true);
                    ButtonActivation = false;
                }

                if (rec.rgbButtons[5] == 128)
                {
                    threshold_z++;
                    if (threshold_z >= 25)
                    {
                        if (QuestionnaireNumber > 1)
                        {
                            children[QuestionnaireNumber].gameObject.SetActive(false);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(true);
                            AnswerSlider = children[QuestionnaireNumber - 1].GetComponent<Slider>();
                            QuestionnaireNumber--;
                        }
                        threshold_z = 0;
                    }
                }
            }
            

            if (SaveTrigger == true)
            {
                SaveToCSV();

                if (DriverCar.QuestionnaireCount < 7)
                {
                    DriverCar.CMSchangeBool = true;
                    DriverCar.TrialBool = true;
                    DriverCar.TrialBoolFilter = true;
                    FadeInOut.FadingEvent = false;
                    DriverCar.respawnTrigger = false;
                }

                if (DriverCar.QuestionnaireCount == 7)
                {
                    FinalQuestionnaire.SetActive(true);
                }
            }
        }
    }

    void Chage_value(float x)
    {
        if (Mathf.Abs(x) > 0)
        {
            if (x == 1)
            {
                AnswerSlider.value = 5;
            }
            else if (x == 2)
            {
                AnswerSlider.value = 6;
            }
            else if (x == 3)
            {
                AnswerSlider.value = 7;
            }
            else if (x == -1)
            {
                AnswerSlider.value = 3;
            }
            else if (x == -2)
            {
                AnswerSlider.value = 2;
            }
            else if (x == -3)
            {
                AnswerSlider.value = 1;
            }
        }
        else
        {
            AnswerSlider.value = 4;

        }
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }

    public void SaveToCSV()
    {
        DriverCar.QuestionnaireCount++;
        csvFileName = "Questionnaire" + DriverCar.QuestionnaireCount + ".csv";
        List<Transform> children = GetChildren(transform);
        for (int i = 0; i < children.Count; i++)
        {
            AnswerSlider = children[i].GetComponent<Slider>();
            float[] Data = new float[2];
            Data[0] = i + 1;
            Data[1] = AnswerSlider.value;
            AppendToCsv(Data);
        }

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
            SaveTrigger = false;
            SaveTriggerExit = true;
            gameObject.SetActive(false);
            QuestionnaireNumber = 0;
        }
    }

}
