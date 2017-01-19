using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTimer
{
    public float intialTimer = 0f;
    public float timerLeft;

    public void CountDownFrom(float t)
    {
        intialTimer = t;
        timerLeft -= Time.deltaTime;

        if (timerLeft <= 0)
        {
            timerLeft = 0;   
        }
    }

    public void CountUpTo(float t)
    {
        timerLeft += Time.deltaTime;
        if (timerLeft >= t)
        {
            timerLeft = t;
        }
    }

    public void ResetTimer()
    {
        timerLeft = intialTimer;
    }
}
