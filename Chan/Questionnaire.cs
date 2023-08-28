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
    public GameObject SaveTriggerObject;
    [SerializeField] DemoCarController DC;
    [SerializeField] TrialManager TM;
    public int QuestionnaireNumber;
    [SerializeField] Slider AnswerSlider;
    string csvSeparator = ",";
    string csvFileName;
    string[] csvHeaders = new string[2] { "Number", "Answer" };
    string csvDirectoryName = "Questionnaire";
    public bool SaveTrigger;
    [SerializeField] DC_Collidor DC_C;
    public GameObject FinalQuestionnaireStartNotice;
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool QuestionnairePhase;
    List<Transform> children;
    float ThresholdTimer;

    void Start()
    {
        List<Transform> children = GetChildren(transform);
    }

    void Update()
    {
        if (QuestionnairePhase)
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                LogitechGSDK.DIJOYSTATE2ENGINES rec;
                rec = LogitechGSDK.LogiGetStateUnity(0);

                if(ThresholdTimer < 3f)
                    ThresholdTimer += Time.deltaTime;

                // Get slider value from the steering wheel
                if (rec.lX < -7500) { AnswerSlider.value = 1; }
                else if (rec.lX < -4500 && rec.lX > -7500) { AnswerSlider.value = 2; }
                else if (rec.lX < -1500 && rec.lX > -4500) { AnswerSlider.value = 3; }
                else if (rec.lX < 1500 && rec.lX > -1500) { AnswerSlider.value = 4; }
                else if (rec.lX > 1500 && rec.lX < 4500) { AnswerSlider.value = 5; }
                else if (rec.lX > 4500 && rec.lX < 7500) { AnswerSlider.value = 6; }
                else if (rec.lX > 7500) { AnswerSlider.value = 7; }

                if (ThresholdTimer > 2)
                {
                    // when the right lever is pulled, move to the next question
                    if (rec.rgbButtons[4] == 128)
                    {
                        if (QuestionnaireNumber < 6)
                        {
                            QuestionnaireNumber++;
                            children[QuestionnaireNumber].gameObject.SetActive(true);
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            AnswerSlider = children[QuestionnaireNumber].GetComponent<Slider>();
                        }
                        else if (QuestionnaireNumber == 6)
                        {
                            QuestionnaireNumber++;
                        }

                        // if it's the last question, turn the questions off and turn on the save notive for saving the survey result
                        if (QuestionnaireNumber == 7)
                        {
                            children[QuestionnaireNumber - 1].gameObject.SetActive(false);
                            SaveTriggerObject.SetActive(true);
                        }
                        ThresholdTimer = 0;
                    }

                    // when the left leve is pulled, get back to the previous question
                    if (rec.rgbButtons[5] == 128)
                    {
                        if (QuestionnaireNumber > 1)
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

                    if (DC.QuestionnaireCount < 2)
                    {
                        DC.CMSchangeBool = true;
                        TM.TrialTask = true;
                        DC_C.FadingEvent = false;
                        DC.respawnTrigger = false;
                    }

                    if (DC.QuestionnaireCount == 2)
                    {
                        DC.FinalQuestionnaireBool = true;
                        FinalQuestionnaireStartNotice.SetActive(true);
                    }
                }
            }
        }
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent) { children.Add(child); }
        return children;
    }

    public void SaveToCSV()
    {
        DC.QuestionnaireCount++;
        csvFileName = "Questionnaire" + DC.QuestionnaireCount + ".csv";
        List<Transform> children = GetChildren(transform);
        for (int i = 0; i < children.Count; i++)
        {
            AnswerSlider = children[i].GetComponent<Slider>();
            string[] QuestionnaireSubject = { "1", "2" };
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
                { finalString += csvSeparator; }
                finalString += floats[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);

            SaveTrigger = false;
            QuestionnaireNumber = 0;
            QuestionnairePhase = false;
            gameObject.SetActive(false);
        }
    }

}
