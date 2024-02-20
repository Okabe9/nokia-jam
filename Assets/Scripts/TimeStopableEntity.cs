using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class TimeStoppableEntity : MonoBehaviour
{
  public bool isTimeStopped = false;

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
