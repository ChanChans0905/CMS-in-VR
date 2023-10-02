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
    LogitechGSDK.LogiControllerPropertiesData properties;
    [SerializeField] DemoCarController DC;
    [SerializeField] FinalQuestionnaire FQ;
    [SerializeField] Slider AnswerSlider;
    List<Transform> children;
    public GameObject SaveTriggerObject, FinalQuestionnaireStartNotice, TrialStartNotice;
    public bool SaveTrigger, QuestionnairePhase, FirstSlider;
    public int QuestionnaireNumber;
    string csvSeparator = ",";
    string csvFileName;
    string csvDirectoryName = "Questionnaire";
    string[] csvHeaders = new string[2] { "Questionnaire", "Answer" };
    float ThresholdTimer;

    void Update()
    {
        if (QuestionnairePhase)
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                LogitechGSDK.DIJOYSTATE2ENGINES rec;
                rec = LogitechGSDK.LogiGetStateUnity(0);

                List<Transform> children = GetChildren(transform);

                if (ThresholdTimer < 3f)
                    ThresholdTimer += Time.deltaTime;

                if (FirstSlider)
                {
                    AnswerSlider = children[QuestionnaireNumber].GetComponent<Slider>();
                    FirstSlider = false;
                }

                // Get slider value from the steering wheel

                if(QuestionnaireNumber <= 12)
                {
                    if (rec.lX < -7500) { AnswerSlider.value = 1; }
                    else if (rec.lX < -4500 && rec.lX > -7500) { AnswerSlider.value = 2; }
                    else if (rec.lX < -1500 && rec.lX > -4500) { AnswerSlider.value = 3; }
                    else if (rec.lX < 1500 && rec.lX > -1500) { AnswerSlider.value = 4; }
                    else if (rec.lX > 1500 && rec.lX < 4500) { AnswerSlider.value = 5; }
                    else if (rec.lX > 4500 && rec.lX < 7500) { AnswerSlider.value = 6; }
                    else if (rec.lX > 7500) { AnswerSlider.value = 7; }
                }


                if (ThresholdTimer > 1)
                {
                    // when the right lever is pulled, move to the next question
                    if (rec.rgbButtons[4] == 128)
                    {
                        if (QuestionnaireNumber < 12)
                        {
                            QuestionnaireNumber++;
                            children[QuestionnaireNumber].gameObject.SetActive(true);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            AnswerSlider = children[QuestionnaireNumber].GetComponent<Slider>();
                        }
                        else if (QuestionnaireNumber == 12)
                        {
                            QuestionnaireNumber++;
                            children[QuestionnaireNumber - 2].gameObject.SetActive(false);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            SaveTriggerObject.SetActive(true);
                            
                        }
                        Debug.Log(QuestionnaireNumber);
                        ThresholdTimer = 0;
                    }

                    // when the left leve is pulled, get back to the previous question
                    if (rec.rgbButtons[5] == 128)
                    {
                        if (QuestionnaireNumber > 1 && QuestionnaireNumber != 13)
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
                    SaveToCSV();
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
        DC.QuestionnaireCount++;
        csvFileName = "Questionnaire" + DC.CMScombination[DC.CMSchangeCount-1] + ".csv";
        List<Transform> children = GetChildren(transform);
        for (int i = 0; i < children.Count; i++)
        {
            AnswerSlider = children[i].GetComponent<Slider>();
            string[] QuestionnaireSubject = { "Quickly", "Acuurately", "Safely", "Conveniently", "Effort", "meets expectation", "like this layout", "mentally", "physically", "hurries or rushed", "successful", "hard", "insecure" };
            string[] Data = new string[2];
            Data[0] = QuestionnaireSubject[i];
            Data[1] = AnswerSlider.value.ToString();
            AppendToCsv(Data);
        }

        if (DC.QuestionnaireCount < 2)
        {
            DC.CMSchangeBool = true;
            TrialStartNotice.SetActive(true);
        }

        if (DC.QuestionnaireCount == 2)
        {
            DC.FinalQuestionnaireBool = true;
            FQ.FinalQuestionnairePhase = true;
            FinalQuestionnaireStartNotice.SetActive(true);
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
                if (finalString != "")
                    finalString += csvSeparator;
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
                    finalString += csvSeparator;
                finalString += floats[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);

            QuestionnairePhase = false;
            ThresholdTimer = 0;
            FirstSlider = true;
            SaveTrigger = false;
            QuestionnaireNumber = 0;
        }
    }
}
