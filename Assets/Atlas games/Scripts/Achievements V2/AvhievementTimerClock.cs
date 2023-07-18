using System;
using System.Collections;
using UnityEngine;
using TMPro;

public unsafe class AvhievementTimerClock : MonoBehaviour
{
    public TextMeshProUGUI ClockText;
    private Coroutine disPacher;
    public void StartTheClock(DateTime expireTime){
        disPacher = StartCoroutine(StartTheClockEnumerator(expireTime));
    }
    IEnumerator StartTheClockEnumerator(DateTime expireTime){
        while (true)
        {
            yield return new WaitForSeconds(1);
            TimeSpan timeSpan = expireTime - DateTime.Now;
            ClockText.text = $"{timeSpan.Days}:{timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
        }
    }
    void StopTheClock(){
        if (disPacher != null) StopCoroutine(disPacher);
    }
    ~AvhievementTimerClock(){
        StopTheClock();
    }
    void OnApplicationQuit(){
        StopTheClock();
    }
}
