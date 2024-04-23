// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void SwitchMusic(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            Stop();
            SetAudioClip(audioClip);
            Play();
        }
    }

    public void SetAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }


}
