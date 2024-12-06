using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    private static string filePath = Application.persistentDataPath + "/timersave.save";
    public List<float> TimesList_Level_1 = new List<float>();
    public List<float> TimesList_Level_2 = new List<float>();
    public List<float> TimesList_Level_3 = new List<float>();

    public static string GetFilePath()
    {
        return filePath;
    }

}