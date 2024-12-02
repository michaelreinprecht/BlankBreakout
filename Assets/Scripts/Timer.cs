using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;
    public float timer;
    public bool timeStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update in Timer.cs called.");
        if(timeStarted)
        {
            Debug.Log("timeStarted is TRUE in update()");
            timer += Time.deltaTime;
        }
        UpdateUi();
    }

    public void StartTimer()
    {
        Debug.Log("StartTimer called in Timer.cs");
        timer = 0;
        timeStarted = true;
        UpdateUi();
    }

    private void UpdateUi()
    {
        Debug.Log("UpdateUi called in Timer.cs");
        timerText.text = FormatTimeTextField();
    }

    public void StopTimer()
    {
        Debug.Log("StopTimer called in Timer.cs");
        timeStarted = false;
    }

    private string FormatTimeTextField()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
