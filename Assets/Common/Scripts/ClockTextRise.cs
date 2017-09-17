using UnityEngine;
using System.Collections;
using System;

public class ClockTextRise : MonoBehaviour
{
    public bool runOnStart = false;
    public int timeValue = 0;
    public bool showHour = false;
    public bool showMinute = true;
    public bool showSecond = true;

    private bool isRunning;

    private void Start()
    {
        UpdateText();
        if (runOnStart)
        {
            Run();
        }
    }

    public void Run()
    {
        isRunning = true;
        StartCoroutine(UpdateClockText());
    }

    private IEnumerator UpdateClockText()
    {
        while (isRunning)
        {
            UpdateText();
            yield return new WaitForSeconds(1);
            timeValue ++;
        }
    }

    private void UpdateText()
    {
        TimeSpan t = TimeSpan.FromSeconds(timeValue);

        string text;
        if (showHour && showMinute && showSecond)
        {
            text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        }
        else if (showHour && showMinute)
        {
            text = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
        }
        else
        {
            text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }
        gameObject.SetText(text);
    }

    public void Stop()
    {
        isRunning = false;
    }
}
