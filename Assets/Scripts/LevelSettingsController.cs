using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSettingsController : MonoBehaviour
{
    private TMPro.TMP_InputField maxTargetInput;

    private void Start()
    {
        maxTargetInput = gameObject.GetComponent<TMP_InputField>();
        SharedData.MaxTargetValue = 20;
    }

    public void SetSharedMaxTargetValue(string newValue)
    {
        if (int.TryParse(newValue, out int parsedValue))
        {
            if (Mathf.Abs(parsedValue) >= 20)
            {
                SharedData.MaxTargetValue = Mathf.Abs(parsedValue);
                Debug.Log($"SharedData.MaxTargetValue updated to: {SharedData.MaxTargetValue}");
            }
            else
            {
                maxTargetInput.text = "20";
                SharedData.MaxTargetValue = 20;
                Debug.Log($"MaxTargetValue to small, set to minimum of 20");
            }
        }
    }
}
