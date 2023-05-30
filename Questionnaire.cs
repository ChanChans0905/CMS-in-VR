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
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] Savetrigger SaveTriggerFile;
    public int QuestionnaireNumber;
    [SerializeField] Slider AnswerSlider;
    private string csvSeparator = ",";
    public string csvFileName;
    private string[] csvHeaders = new string[2] { "Number", "Answer" };
    private string csvDirectoryName = "Questionnaire";
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool SaveTrigger = false;

    void Start()
    {
        csvFileName = "Questionnaire" + DriverCar.QuestionnaireCount +".csv";
        List<Transform> children = GetChildren(transform);

        foreach (Transform child in children)
        {
            Debug.Log(child.name);
        }

    }

    void Update()
    {
        List<Transform> children = GetChildren(transform);
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        rec = LogitechGSDK.LogiGetStateUnity(0);

        if (rec.rgbButtons[4] == 128 || Input.GetKeyDown(KeyCode.M))
        {

            children[QuestionnaireNumber].gameObject.SetActive(true);
            if(QuestionnaireNumber != 0)
            {
                children[QuestionnaireNumber - 1].gameObject.SetActive(false);
            }
            QuestionnaireNumber++;
            if (rec.rgbButtons[4] == 128 && SaveTriggerFile.SaveButton == true)
            {
                SaveTrigger= true;
            }
        }
        else if (rec.rgbButtons[5] == 128 || Input.GetKeyDown(KeyCode.N))
        {
            if(QuestionnaireNumber > 1)
            {
                QuestionnaireNumber--;
                children[QuestionnaireNumber].gameObject.SetActive(false);
                children[QuestionnaireNumber - 1].gameObject.SetActive(true);
            }
        }
        
        if (SaveTrigger == true) // save 확인 버튼 클릭 시
        {
            SaveToCSV();            

            DriverCar.respawnTrigger = false;
            DriverCar.QuestionnaireBool= false;
            DriverCar.TrialBool = true;
        }

        if (DriverCar.QuestionnaireCount == 7 && Input.GetKeyDown(KeyCode.M)) // 7번째 CMS 저장 버튼 누르면 최종 설문으로 이동
        {
            DriverCar.FinalQuestionnaireBool = true;
        }
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }

    public void SaveToCSV()
    {
        List<Transform> children = GetChildren(transform);
        for(int i = 0; i < children.Count; i++)
        {
            AnswerSlider = children[i].GetComponent<Slider>();
            float[] Data = new float[2];
            Data[0] = i+1;
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
            gameObject.SetActive(false);
        }
    }

}
