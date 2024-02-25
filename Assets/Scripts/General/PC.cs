using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public void StartLevel()
    {
        GameManager.instance.StartLevel();
    }
    public void PlaySparkle()
    {
        AudioManager.instance.PlaySFX("Sparkle");
    }
    public void PlayCD()
    {
        AudioManager.instance.PlaySFX("InsertCD");

    }
    public void PlayPCStart()
    {
        AudioManager.instance.PlaySFX("PcInit");

    }
    public void PlayDots()
    {
        AudioManager.instance.PlaySFX("PCPipuPipu");

    }
}
