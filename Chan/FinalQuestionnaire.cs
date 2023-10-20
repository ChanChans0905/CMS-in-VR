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
    [SerializeField] Slider AnswerSlider;
    public GameObject SaveTriggerObject;
    int QuestionnaireNumber;
    string csvSeparator = ",";
    string csvFileName;
    string[] csvHeaders = new string[2] { "Questionnaire", "Answer" };
    string csvDirectoryName = "FinalQuestionnaire";
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool SaveTrigger;
    public GameObject GameEnd;
    List<Transform> children;
    public bool FinalQuestionnairePhase;
    float ThresholdTimer;
    public bool FirstSlider;

    void FixedUpdate()
    {
        if (FinalQuestionnairePhase) 
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                LogitechGSDK.DIJOYSTATE2ENGINES rec;
                rec = LogitechGSDK.LogiGetStateUnity(0);

                List<Transform> children = GetChildren(transform);

                if (ThresholdTimer < 3)
                    ThresholdTimer += Time.deltaTime;

                if (FirstSlider)
                {
                    AnswerSlider = children[QuestionnaireNumber].GetComponent<Slider>();
                    FirstSlider = false;
                }

                // Get slider value from the steering wheel
                if(QuestionnaireNumber <= 21)
                {
                    if (rec.lX < -5000) { AnswerSlider.value = 1; }
                    else if (rec.lX < -3000 && rec.lX > -5000) { AnswerSlider.value = 2; }
                    else if (rec.lX < -1000 && rec.lX > -3000) { AnswerSlider.value = 3; }
                    else if (rec.lX < 1000 && rec.lX > -1000) { AnswerSlider.value = 4; }
                    else if (rec.lX > 1000 && rec.lX < 3000) { AnswerSlider.value = 5; }
                    else if (rec.lX > 3000 && rec.lX < 5000) { AnswerSlider.value = 6; }
                    else if (rec.lX > 5000) { AnswerSlider.value = 7; }
                }

                if (ThresholdTimer > 2)
                {
                    // when the right lever is pulled, move to the next question
                    if (rec.rgbButtons[4] == 128)
                    {
                        if (QuestionnaireNumber < 21)
                        {
                            QuestionnaireNumber++;
                            children[QuestionnaireNumber].gameObject.SetActive(true);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            AnswerSlider = children[QuestionnaireNumber].GetComponent<Slider>();
                        }
                        else if (QuestionnaireNumber == 21)
                        {
                            QuestionnaireNumber++;
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            SaveTriggerObject.SetActive(true);
                        }
                        ThresholdTimer = 0;
                    }

                    // when the left leve is pulled, get back to the previous question
                    if (rec.rgbButtons[5] == 128)
                    {
                        if (QuestionnaireNumber > 1 && QuestionnaireNumber != 22)
                        {
                            children[QuestionnaireNumber].gameObject.SetActive(false);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(true);
                            AnswerSlider = children[QuestionnaireNumber - 1].GetComponent<Slider>();
                            QuestionnaireNumber--;
                        }
                        ThresholdTimer = 0;
                    }
                }

                // if the user pull the right lever when the save notice object is activated, the survey result will be saved to csv file
                if (SaveTrigger)
                {
                    SaveToCSV();
                    GameEnd.SetActive(true);
                }
            }
        }
    }


    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
            children.Add(child);
        return children;
    }

    public void SaveToCSV()
    {
        csvFileName = "FInalQuestionnaire.csv";
        List<Transform> children = GetChildren(transform);
        for (int i = 0; i < children.Count; i++)
        {
            AnswerSlider = children[i].GetComponent<Slider>();
            string[] QuestionnaireSubject = {"1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22"};
            string[] Data = new string[2];
            Data[0] = QuestionnaireSubject[i];
            Data[1] = AnswerSlider.value.ToString();
            AppendToCsv(Data);
        }

    }

    string GetDirectoryPath() { return Application.dataPath + "/" + csvDirectoryName; }

    string GetFilePath() { return GetDirectoryPath() + "/" + csvFileName; }

    void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
    }

    void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file)) { CreateCsv(); }
    }

    public void CreateCsv()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < csvHeaders.Length; i++)
            {
                if (finalString != "") { finalString += csvSeparator; }
                finalString += csvHeaders[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);
        }
    }

    public void AppendToCsv(string[] floats)
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

            FinalQuestionnairePhase = false;
            gameObject.SetActive(false);

        }
    }
}
