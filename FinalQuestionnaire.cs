using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class FinalQuestionnaire : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    public int QuestionnaireNumber;
    [SerializeField] Slider AnswerSlider;
    private string csvSeparator = ",";
    public string csvFileName;
    private string[] csvHeaders = new string[2] { "Number", "Answer" };
    private string csvDirectoryName = "Questionnaire";
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool SaveTrigger;

    private void Start()
    {
        csvFileName = "FInalQuestionnaire.csv";
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
            if (QuestionnaireNumber != 0)
            {
                children[QuestionnaireNumber - 1].gameObject.SetActive(false);
            }
            QuestionnaireNumber++;
            if (Input.GetKeyDown(KeyCode.N)) // 마지막 저장 예 버튼 클릭시
            {
                SaveTrigger = true;
            }
        }
        else if (rec.rgbButtons[3] == 128 || Input.GetKeyDown(KeyCode.N))
        {
            if (QuestionnaireNumber > 1)
            {
                QuestionnaireNumber--;
                children[QuestionnaireNumber].gameObject.SetActive(false);
                children[QuestionnaireNumber - 1].gameObject.SetActive(true);
            }
        }

        if (SaveTrigger == true) // save 확인 버튼 클릭 시
        {
            SaveToCSV();

            // 리스폰트리거 제거, fade out

            DriverCar.respawnTrigger = false;
            DriverCar.QuestionnaireBool = false;
        }

        if (DriverCar.FinalQuestionnaireBool)
        {
            FQ();
        }
    }

    public void FQ()
    {
        // 최종 설문


    }

    public void GameEnd()
    {
        // 게임 종료 안내 문구 투영 ( 게임이 끝났습니다. 관리자에게 말씀해주세요 )
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
            gameObject.SetActive(false);
        }
    }
}
