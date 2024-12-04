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
        if(timeStarted)
        {
            timer += Time.deltaTime;
        }
        UpdateUi();
    }

    public void StartTimer()
    {
        timer = 0;
        timeStarted = true;
        UpdateUi();
    }

    private void UpdateUi()
    {
        timerText.text = FormatTimeTextField();
    }

    public void StopTimer()
    {
        timeStarted = false;
    }

    private string FormatTimeTextField()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public float GetTime()
    {
        return timer;
    }
}
