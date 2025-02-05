using UnityEngine;

public class AudioManager : MonoBehaviour
{
  [SerializeField] AudioSource Sounds;

  public AudioClip win;
  public AudioClip wrong;



    public void WinSound()
    {
        Sounds.clip = win;
        Sounds.Play();
    }
      public void Wrongound()
    {
        Sounds.clip = wrong;
        Sounds.Play();
    }
}

