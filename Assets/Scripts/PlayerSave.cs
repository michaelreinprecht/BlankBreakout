using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    private static string filePath = Application.persistentDataPath + "/timersave.save";
    public List<float> TimesList_Level1 = new List<float>();
    public List<float> TimesList_Level2 = new List<float>();
    public List<float> TimesList_Level3 = new List<float>();

    public static string GetFilePath()
    {
        return filePath;
    }

}