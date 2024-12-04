using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    public TMP_Text averageTimeText;
    [SerializeField]
    public TMP_Text bestTimeText;
    [SerializeField]
    public TMP_Text gamesPlayedText;
    
    private float averageTime;
    private float bestTime;
    private int gamesPlayed;
    private List<float> ListOfTimesPast = new List<float>();
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
                ListOfTimesPast = save.ListOfTimesPast_Level1;
                averageTime = GetAverageTime(ListOfTimesPast);
                bestTime = GetBestTime(ListOfTimesPast);
                gamesPlayed = ListOfTimesPast.Count;
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
        if (averageTime == 0 || bestTime == 0 || gamesPlayed == 0)
        {
            Debug.LogError("Text fields not set in PlayerData");
            averageTimeText.text = "0:00";
            bestTimeText.text = "0:00";
            gamesPlayedText.text = "0";
            return;
        } 
        else
        {   
        averageTimeText.text = FormatTime(averageTime);
        bestTimeText.text = FormatTime(bestTime);
        gamesPlayedText.text = gamesPlayed.ToString();
        }
    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}