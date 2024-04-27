// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour
{
    [Header("Player Sounds")]
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] AudioClip dropSound;
    [SerializeField] AudioClip equipSound;

    private AudioSource audioSource;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() {
         GameEventsManager.instance.pickUpEvents.onItemPickedUp += ItemPickedUp;
         GameEventsManager.instance.pickUpEvents.onItemDropped += ItemDropped;
         GameEventsManager.instance.pickUpEvents.onItemEquipped += ItemEquipped;
    }

    private void OnDisable() {
         GameEventsManager.instance.pickUpEvents.onItemPickedUp -= ItemPickedUp;
         GameEventsManager.instance.pickUpEvents.onItemDropped -= ItemDropped;
         GameEventsManager.instance.pickUpEvents.onItemEquipped -= ItemEquipped;
    }

    private void ItemPickedUp(Item item) {
        Play(pickUpSound);
    }

    private void ItemDropped() {
        Play(dropSound);
    }

    private void ItemEquipped() {
        Play(equipSound);
    }


    
    private void Play(AudioClip sound) {
        
        audioSource.PlayOneShot(sound, 1);
        
    }
   
}
