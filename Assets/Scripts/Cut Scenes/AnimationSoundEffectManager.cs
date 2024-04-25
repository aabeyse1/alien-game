using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class AnimationSoundEffectManager : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] AudioClip[] sounds;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(int soundEffectIndex) {
        AudioClip clipToPlay = sounds[soundEffectIndex];
        audioSource.PlayOneShot(clipToPlay, 1);
        
    }
}
