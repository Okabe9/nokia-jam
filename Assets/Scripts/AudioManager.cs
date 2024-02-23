using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;
  private AudioSource audioSource;
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
    audioSource = GetComponent<AudioSource>();

    // Assign each sound effect clip to its respective AudioSource
    foreach (Sound s in soundEffects)
    {
      s.source = gameObject.AddComponent<AudioSource>();
      s.source.clip = s.clip;
      s.source.volume = s.volume;
      s.source.pitch = s.pitch;
      s.source.loop = s.loop;
    }
  }


  // Play music with specified AudioClip
  public void PlayMusic(string name)
  {
    Sound music = tracks.Find(sound => sound.name == name);

    audioSource.clip = music.clip;
    audioSource.Play();
  }

  public void PlaySFX(string name)
  {
    Sound sfx = soundEffects.Find(sound => sound.name == name);
    if (audioSource.isPlaying && sfx != null)
    {
      audioSource.Pause();
      audioSource.PlayOneShot(sfx.clip);
      StartCoroutine(ResumeMusicAfterSFX());
    }
    else
    {
      audioSource.PlayOneShot(sfx.clip);
    }
  }

  private System.Collections.IEnumerator ResumeMusicAfterSFX()
  {
    yield return new WaitForSeconds(audioSource.clip.length);
    audioSource.Play();
  }

}
