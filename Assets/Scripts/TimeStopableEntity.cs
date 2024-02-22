using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class TimeStoppableEntity : MonoBehaviour
{
    public bool isTimeStopped = false;
    public bool isTransitioningBehindFreeze = false;
    public int originalLayer;

    private void Start()
    {
        originalLayer = gameObject.layer;
    }

    public void StopTime()
    {
        isTimeStopped = true;
    }
    public void StartTime()
    {
        isTimeStopped = false;
    }
    public void ToggleTime()
    {
        isTimeStopped = !isTimeStopped;
    }
}
