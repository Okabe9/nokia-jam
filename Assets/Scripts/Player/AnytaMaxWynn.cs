using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnytaMaxWynn : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            gameObject.GetComponent<Animator>().SetTrigger("FinalVictory");
            AudioManager.instance.PlayMusic("VictorySong");
            AudioManager.instance.StopMusicLoop(); 
            // On Anim End Call Win
        }
    }
    public void LoadNextLevel()
    {
        SceneChanger.instance.ChangeScene(nextLevelName);
    }
    public void PlaySparkle()

    {
        AudioManager.instance.PlaySFX("Freeze"); 
    }
}
