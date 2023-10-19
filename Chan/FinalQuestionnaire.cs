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
            string[] QuestionnaireSubject = { "Compared to a normal mirror, I would prefer a camera- monitor system in my own vehicle", "Which of the three camera positions experienced in this experiment would you personally prefer",
                                              "I prefer a defensive, cautious driving style", "I am willing to use CMS instead of the existing mirror system", "Using CMS instead of the existing mirror system will improve driving convenience",
                                              "Using CMS instead of the existing mirror system will help me comprehend the side and rear situations more quickly and accurately",
                                              "When purchasing a car with the same options/conditions, I will opt for CMS instead of the existing mirror system", "CMS is overall better than the existing mirror system",
                                              "How much were you able to control events", "How natural did your interactions with the environment seem", "How completely were all of your senses engaged",
                                              "How much did the visual aspects of the environment involve you", "How natural was the mechanism which controlled movement through the environment",
                                              "How inconsistent or disconnected was the information coming from your various senses", "How much did your experiences in the virtual environment seem consistent with your real world experiences",
                                              "How compelling was your sense of moving around inside the virtual environment", "To what degree did you feel confused or disoriented at the beginning of breaks or at the end of the experimental session",
                                              "How involved were you in the virtual environment experience", "How distracting was the control mechanism", "How much delay did you experience between your actions and expected outcomes",
                                              "How quickly did you adjust to the virtual environment experience", "How much did the visual display quality interfere or distract you from performing assigned tasks or required activities"};
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
