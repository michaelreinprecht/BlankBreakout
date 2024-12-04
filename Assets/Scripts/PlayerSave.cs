using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    private static string filePath = Application.persistentDataPath + "/timersave.save";
    public List<float> ListOfTimesPast_Level1 = new List<float>();

    public static string GetFilePath()
    {
        return filePath;
    }

}