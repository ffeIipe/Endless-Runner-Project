using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : Timer.Timer
{
    public CountdownTimer(float value) : base(value) { }

    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time > 0)
        {
            Time -= deltaTime;
        }

        if (IsRunning && Time <= 0)
        {
            Stop();
        }
    }

    public bool IsFinished => Time <= 0;

    public void Reset() => Time = InitialTime;

    public void Reset(float newTime)
    {
        InitialTime = newTime;
        Reset();
    }
}
