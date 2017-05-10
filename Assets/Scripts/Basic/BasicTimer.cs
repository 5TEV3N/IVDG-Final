using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTimer
{
    // TO BE REMOVED AND USING COROUTINES INSTEAD

    private float intialTimer = 0f;
    public float timerLeft;

    public void CountDownFrom(float t)
    {
		timerLeft = t;
        timerLeft -= Time.time;

        if (timerLeft <= 0)
        {
            timerLeft = 0;   
        }
    }

    public void CountUpTo(float t)
    {
        timerLeft += Time.time;
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
