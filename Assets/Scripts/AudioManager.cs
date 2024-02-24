using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;
  [SerializeField] private AudioSource musicSource;
  [SerializeField] private AudioSource sfxSource;

  public List<Sound> soundEffects;
  public List<Sound> tracks;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }
  }
  void Start()
  {
    // Find the AudioSource for music on the same GameObject
    musicSource = GetComponent<AudioSource>();

  }


  // Play music with specified AudioClip
  public void PlayMusic(string name)
  {
    Sound music = tracks.Find(sound => sound.name == name);

    Debug.Log(music.clip);
    music.source.clip = music.clip;
    music.source.Play();
  }

  public void PlaySFX(string name)
  {
    Sound sfx = soundEffects.Find(sound => sound.name == name);
    if (musicSource.isPlaying && sfx != null)
    {
      musicSource.Pause();
      sfxSource.PlayOneShot(sfx.clip);
      StartCoroutine(ResumeMusicAfterSFX(sfx.clip.length));
    }

  }

  private IEnumerator ResumeMusicAfterSFX(float delay)
  {
    yield return new WaitForSeconds(delay + 1f);
    musicSource.UnPause();
  }
}
