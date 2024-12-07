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
        LoadHighScores(path);
    }
  
 
    void Update()
    {
        
    }

    public void LoadHighScores(string filePath)
    {
        if (filePath != null)
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                PlayerSave save = (PlayerSave)bf.Deserialize(file);
                file.Close();
                TimesListAddition = save.TimesList_Level_1;
                if (TimesListAddition.Count > 0)
                {
                    averageTimeAddition = GetAverageTime(TimesListAddition);
                    bestTimeAddition = GetBestTime(TimesListAddition);
                    gamesPlayedAddition = TimesListAddition.Count;
                }
                TimesListSubtraction = save.TimesList_Level_2;
                if (TimesListSubtraction.Count > 0)
                {
                    averageTimeSubtraction = GetAverageTime(TimesListSubtraction);
                    bestTimeSubtraction = GetBestTime(TimesListSubtraction);
                    gamesPlayedSubtraction = TimesListSubtraction.Count;
                }
                
                TimesListMultiplication= save.TimesList_Level_3;
                if (TimesListMultiplication.Count > 0)
                {
                    averageTimeMultiplication = GetAverageTime(TimesListMultiplication);
                    bestTimeMultiplication = GetBestTime(TimesListMultiplication);
                    gamesPlayedMultiplication = TimesListMultiplication.Count;
                }
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
        if (allData.Count > 0)
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
        else
        {
            return 0;
        }
    }

    private void SetTextFields()
    {
        averageTimeTextAddition.text = FormatTime(averageTimeAddition);
        averageTimeTextSubtraction.text = FormatTime(averageTimeSubtraction);
        averageTimeTextMultiplication.text = FormatTime(averageTimeMultiplication);
        bestTimeTextAddition.text = FormatTime(bestTimeAddition);
        bestTimeTextSubtraction.text = FormatTime(bestTimeSubtraction);
        bestTimeTextMultiplication.text = FormatTime(bestTimeMultiplication);
        if (gamesPlayedAddition == 0)
        {
            gamesPlayedTextAddition.text = "-";
        } 
        else
        {
            gamesPlayedTextAddition.text = gamesPlayedAddition.ToString();
        }
        if (gamesPlayedSubtraction == 0)
        {
            gamesPlayedTextSubtraction.text = "-";
        }
        else
        {
            gamesPlayedTextSubtraction.text = gamesPlayedSubtraction.ToString();
        }
        if (gamesPlayedMultiplication == 0)
        {
            gamesPlayedTextMultiplication.text = "-";
        }
        else
        {
            gamesPlayedTextMultiplication.text = gamesPlayedMultiplication.ToString();
        }
    }

    private string FormatTime(float time) {
        if (time <= 0)
        {
            return "-";
        }
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}