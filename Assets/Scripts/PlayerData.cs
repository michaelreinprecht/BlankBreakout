using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    public TMP_Text averageTimeTextAddition;
    [SerializeField]
    public TMP_Text bestTimeTextAddition;
    [SerializeField]
    public TMP_Text gamesPlayedTextAddition;
    [SerializeField]
    public TMP_Text averageTimeTextSubtraction;
    [SerializeField]
    public TMP_Text bestTimeTextSubtraction;
    [SerializeField]
    public TMP_Text gamesPlayedTextSubtraction;
    [SerializeField]
    public TMP_Text averageTimeTextMultiplication;
    [SerializeField]
    public TMP_Text bestTimeTextMultiplication;
    [SerializeField]
    public TMP_Text gamesPlayedTextMultiplication;

    private float averageTimeAddition;
    private float bestTimeAddition;
    private int gamesPlayedAddition;
    private float averageTimeSubtraction;
    private float bestTimeSubtraction;
    private int gamesPlayedSubtraction;
    private float averageTimeMultiplication;
    private float bestTimeMultiplication;
    private int gamesPlayedMultiplication;
    private List<float> TimesListAddition = new List<float>();
    private List<float> TimesListSubtraction= new List<float>();
    private List<float> TimesListMultiplication= new List<float>();
    private string path;
    void Start()
    {
        path = Application.persistentDataPath + "/timersave.save";
        LoadHighscores(path);
    }
  
 
    void Update()
    {
        
    }

    public void LoadHighscores(string filePath)
    {
        if (filePath != null)
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                PlayerSave save = (PlayerSave)bf.Deserialize(file);
                file.Close();
                TimesListAddition = save.TimesList_Level1;
                averageTimeAddition = GetAverageTime(TimesListAddition);
                bestTimeAddition = GetBestTime(TimesListAddition);
                gamesPlayedAddition = TimesListAddition.Count;
                TimesListSubtraction = save.TimesList_Level2;
                averageTimeSubtraction = GetAverageTime(TimesListSubtraction);
                bestTimeSubtraction = GetBestTime(TimesListSubtraction);
                gamesPlayedSubtraction = TimesListSubtraction.Count;
                TimesListMultiplication= save.TimesList_Level3;
                averageTimeMultiplication = GetAverageTime(TimesListMultiplication);
                bestTimeMultiplication = GetBestTime(TimesListMultiplication);
                gamesPlayedMultiplication = TimesListMultiplication.Count;
            }
            else
            {
                Debug.Log("No save file found");
            }   
        }
        else
        {
            Debug.Log("No file path found");
        }
        SetTextFields();   
    }


    private float GetAverageTime(List<float> allData)
    {
        float sum = 0;
        foreach(float time in allData){
            sum += time;
        }
        return sum/allData.Count;
    }

    private float GetBestTime(List<float> allData)
    {
        float best = allData[0];
        foreach (float time in allData)
        {
            if (time < best)
            {
                best = time;
            }
        }
        return best;
    }

    private void SetTextFields()
    {
        if (averageTimeAddition == 0 || bestTimeAddition == 0 || gamesPlayedAddition == 0)
        {
            Debug.Log("Text fields not set in PlayerData");
            averageTimeTextAddition.text = "0:00";
            bestTimeTextAddition.text = "0:00";
            gamesPlayedTextAddition.text = "0";
            averageTimeTextSubtraction.text = "0:00";
            bestTimeTextSubtraction.text = "0:00";
            gamesPlayedTextSubtraction.text = "0";
            averageTimeTextMultiplication.text = "0:00";
            bestTimeTextMultiplication.text = "0:00";
            gamesPlayedTextMultiplication.text = "0";
            return;
        } 
        else
        {   
            averageTimeTextAddition.text = FormatTime(averageTimeAddition);
            bestTimeTextAddition.text = FormatTime(bestTimeAddition);
            gamesPlayedTextAddition.text = gamesPlayedAddition.ToString();
            averageTimeTextSubtraction.text = FormatTime(averageTimeSubtraction);
            bestTimeTextSubtraction.text = FormatTime(bestTimeSubtraction);
            gamesPlayedTextSubtraction.text = gamesPlayedSubtraction.ToString();
            averageTimeTextMultiplication.text = FormatTime(averageTimeMultiplication);
            bestTimeTextMultiplication.text = FormatTime(bestTimeMultiplication);
            gamesPlayedTextMultiplication.text = gamesPlayedMultiplication.ToString();
        }
    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}